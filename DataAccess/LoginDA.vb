﻿Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports EN

Public Class LoginDA
    Dim objen As LoginEN = New LoginEN()
    Dim objDA As DBConnectionDA = New DBConnectionDA()
    Dim Password As String
    Public Sub New()
        objDA.con = New SqlConnection(objDA.GetConnection)
    End Sub
    Public Function GetCardName(ByVal objen As LoginEN) As String
        Try
            objDA.con.Open()
            objDA.cmd = New SqlCommand("SELECT isnull(firstName,'') +' '+ isnull(middleName,'') +' '+ isnull(lastName,'')   FROM OHEM WHERE empID='" & objen.SessionUid & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            Dim status As String
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function

    Public Function ValidateActiveEmployee(ByVal objen1 As String) As String
        Try
            objDA.con.Open()
            objDA.cmd = New SqlCommand("SELECT Active   FROM OHEM WHERE   empID='" & objen1 & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            Dim status As String
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function UserAuthentication(ByVal objen As LoginEN) As Boolean
        Try
            objDA.con.Open()
            Password = objDA.Encrypt(objen.Password, objDA.key)
            'objDA.cmd = New SqlCommand("select U_Z_EMPID  from [@Z_HR_LOGIN] WHERE U_Z_UID='" & objen.Userid & "' AND U_Z_PWD='" & Password & "' and isnull(U_Z_ESSAPPROVER,'E')='M'", objDA.con)
            objDA.cmd = New SqlCommand("select U_Z_EMPID  from [@Z_HR_LOGIN] WHERE U_Z_UID='" & objen.Userid & "' AND U_Z_PWD='" & Password & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            Dim status As String
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            If status <> "" Then
                If ValidateActiveEmployee(status) = "N" Then
                    Return False
                End If
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function GetCardCode(ByVal objen As LoginEN) As String
        Try
            objDA.con.Open()
            Password = objDA.Encrypt(objen.Password, objDA.key)
            objDA.cmd = New SqlCommand("SELECT isnull(U_Z_EMPID,'') FROM [@Z_HR_LOGIN] WHERE U_Z_UID='" & objen.Userid & "' AND U_Z_PWD='" & Password & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            Dim status As String
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function GetUserPwd(ByVal objen As LoginEN) As DataSet
        Try
            objDA.sqlda = New SqlDataAdapter("SELECT isnull(U_Z_EMPUID,''),isnull(U_Z_USERPWD,'') FROM [@Z_HR_LOGIN] WHERE U_Z_EMPID='" & objen.SessionUid & "'", objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            Return objDA.ds
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function ESSUsercheck(ByVal objen As LoginEN) As Boolean
        Try
            objDA.con.Open()
            Password = objDA.Encrypt(objen.Password, objDA.key)
            objDA.cmd = New SqlCommand("select U_Z_EMPID  from [@Z_HR_LOGIN] WHERE U_Z_UID='" & objen.Userid & "' AND U_Z_PWD='" & Password & "' and isnull(U_Z_ESSAPPROVER,'E')='E'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            Dim status As String
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            If status <> "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function CompanyAddress() As DataSet
        Try
            objDA.con.Open()
            objDA.sqlda = New SqlDataAdapter("select T0.[compnyname],isnull(T1.[Street],''),isnull(T1.[Block],'')+','+isnull(T1.[City],'')+','+isnull(T1.[Zipcode],''),isnull(T1.[State],'')+','+ isnull(T1.[Country],'') from OADM T0 inner join ADM1 T1 ON T0.[Country]=T1.[Country]", objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            If objDA.ds.Tables(0).Rows.Count > 0 Then
                Return objDA.ds
            End If
            Return objDA.ds
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function

    Public Function SessionDetails(ByVal CustCode As String) As Integer
        Try
            Dim exists As Integer = 0
            objDA.strQuery = "INSERT INTO U_HR_SESSION(U_EmpCode,U_LOGIN_DATE) VALUES('" & CustCode & "',GETDATE())"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.cmd.Connection.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.cmd.Connection.Close()
            objDA.con.Open()
            objDA.cmd = New SqlCommand("SELECT MAX(U_SESSIONID) FROM U_HR_SESSION", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            exists = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return exists
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function CheckFirstLogin(ByVal objen As LoginEN) As Boolean
        objDA.con.Open()
        objDA.cmd = New SqlCommand("select *  from ""U_HRSESSION"" WHERE ""empID""='" & objen.SessionUid & "'", objDA.con)
        objDA.cmd.CommandType = CommandType.Text
        Dim status As String
        status = objDA.cmd.ExecuteScalar()
        objDA.con.Close()
        If status <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub InsertFirstLogin(ByVal objen As ChangePwdEN)
        Dim exists As Integer = 0
        objDA.strQuery = "INSERT INTO ""U_HRSESSION""(""empID"",""empName"",""U_LOGIN_DATE"") VALUES('" & objen.EmpId & "','" & objen.Formid & "',getdate())"
        objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
        objDA.cmd.Connection.Open()
        objDA.cmd.ExecuteNonQuery()
        objDA.cmd.Connection.Close()
    End Sub
    Public Function GetEmployeeType(ByVal objen As LoginEN) As String
        Dim ReturnValue As String = ""
        Try
            objDA.con.Open()
            objDA.strQuery = "select ISNULL(U_Z_ESSAPPROVER,'E')  from [@Z_HR_LOGIN] where ""U_Z_EMPID""='" & objen.Userid & "'"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            ReturnValue = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return ReturnValue
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
End Class
