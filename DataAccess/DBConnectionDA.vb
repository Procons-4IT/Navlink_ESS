Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports System.Xml
Imports System.Configuration
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports EN
Imports System.Web.UI.WebControls
Imports System.Data.Odbc
Imports System.Net.Mail
Imports System.Collections.Specialized
Imports System.Security.Cryptography
Imports System.Text
Imports System.Management
Imports System.Globalization

Public Class DBConnectionDA

    Dim SmtpServer As New Net.Mail.SmtpClient()
    Dim mail As New Net.Mail.MailMessage
    Dim mailServer As String
    Dim mailPort As String
    Dim mailId As String
    Dim mailUser As String
    Dim mailPwd As String
    Dim mailSSL As String
    Dim toID As String
    Dim ccID As String
    Dim mType As String
    Dim path As String
    Dim sQuery As String
    Dim strEmpName As String
    Dim objen As DBConnectionEN = New DBConnectionEN()
    Public oCompany, objMainCompany As New SAPbobsCOM.Company
    Public ConnectionString As String
    Public strQuery, strQuery1, strQuery2 As String
    Dim strError As String
    Public con As SqlConnection
    Public cmd As SqlCommand
    Public sqlda As SqlDataAdapter
    Public ds As DataSet = New DataSet()
    Public ds1 As DataSet = New DataSet()
    Public dss As DataSet = New DataSet()
    Public dss1 As DataSet = New DataSet()
    Public dss2 As DataSet = New DataSet()
    Public dss3 As DataSet = New DataSet()
    Public dss4 As DataSet = New DataSet()
    Public ds2 As DataSet = New DataSet()
    Public ds5 As DataSet = New DataSet()
    Public ds4 As DataSet = New DataSet()
    Public ds3 As DataSet = New DataSet()
    Public dt As DataTable = New DataTable()
    Dim oRecordSet As SAPbobsCOM.Recordset
    Public strmsg As String
    Public sqlreader As SqlDataReader
    Dim HANAConnection As OdbcConnection = New OdbcConnection()
    Public key As String = "!@#$%^*()"
    Public Sub New()
        'readxml()
        objen.DBConnection = "data source=" & ConfigurationManager.AppSettings("SAPServer") & ";Integrated Security=SSPI;database=" & ConfigurationManager.AppSettings("CompanyDB") & ";User id=" & ConfigurationManager.AppSettings("DbUserName") & "; password=" & ConfigurationManager.AppSettings("DbPassword")

        ' objen.DBConnection = "data source=SSRSAS-PC;Integrated Security=SSPI;database=dbName;User id=sa; password=sql"

        ' objen.DBConnection = "ServerNode=" & objen.HANAServerName & ";UID=" & objen.HANALoginName & ";PWD=" & objen.HANAPassword
    End Sub
    Public Function GetConnection() As String
        Return objen.DBConnection
    End Function
    Public Function GetDate(ByVal strDate As String) As DateTime
        Try
            Dim stdate As String = strDate.Trim().Replace("-", "/").Replace(".", "/")
            stdate = stdate.Trim().Replace("/", "")
            Dim stda As String = stdate.Substring(0, 2)
            Dim stmon As String = stdate.Substring(2, 2)
            Dim styear As String = stdate.Substring(4, 4)
            Dim dttime As New DateTime(CInt(styear), CInt(stmon), CInt(stda))
            Return dttime.Date
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message & strDate)
        End Try
    End Function

    Public Shared Sub WriteError(ByVal errorMessage As String)

        Try

            Dim path As String = "~/Error/" & DateTime.Today.ToString("dd-MM-yy") & ".txt"

            If (Not File.Exists(System.Web.HttpContext.Current.Server.MapPath(path))) Then

                File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close()

            End If

            Using w As StreamWriter = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path))

                w.WriteLine(Constants.vbCrLf & "Log Entry : ")

                w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture))

                Dim err As String = "Error in: " & System.Web.HttpContext.Current.Request.Url.ToString() & ". Error Message:" & errorMessage

                w.WriteLine(err)

                w.WriteLine("__________________________")

                w.Flush()

                w.Close()

            End Using

        Catch ex As Exception
            WriteError(ex.Message)
        End Try
    End Sub
    Public Function Encrypt(ByVal strText As String, ByVal strEncrKey _
       As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Strings.Left(strEncrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Return ex.Message
        End Try
    End Function

    Public Function Decrypt(ByVal strText As String, ByVal sDecrKey _
               As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte
        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Strings.Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Return ex.Message
        End Try
    End Function

    Public Function getLoginPassword(ByVal strLicenseText As String) As String
        Dim fields() As String
        Dim strPwd As String = ""
        Try
            If strLicenseText = "" Then
                Return ""
            End If
            Try

                Dim strDecryptText As String = Decrypt(strLicenseText, key)
                fields = strDecryptText.Split("$")

                If fields.Length > 0 Then
                    strPwd = fields(0)
                Else
                    strPwd = ""
                End If
            Catch ex As Exception
                DBConnectionDA.WriteError(ex.Message)
                strPwd = strLicenseText
            End Try
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Return ex.Message
        End Try
        Return strPwd
    End Function
    Public Function Connection() As String
        Try
            'readxml()
            oCompany = New SAPbobsCOM.Company
            objMainCompany = New SAPbobsCOM.Company
            oCompany.Server = ConfigurationManager.AppSettings("SAPServer") ' objen.ServerName
            Select Case ConfigurationManager.AppSettings("DbServerType")
                Case "2005"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                Case "2008"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                Case "2012"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                Case "2014"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
            End Select
            oCompany.DbUserName = ConfigurationManager.AppSettings("DbUserName") ' objen.UID
            oCompany.DbPassword = ConfigurationManager.AppSettings("DbPassword") ' objen.PWD
            oCompany.CompanyDB = ConfigurationManager.AppSettings("CompanyDB") ' objen.SQLCompany
            oCompany.UserName = ConfigurationManager.AppSettings("SAPuserName") ' objen.CUID
            oCompany.Password = ConfigurationManager.AppSettings("SAPpassword") ' objen.CPWD
            oCompany.LicenseServer = ConfigurationManager.AppSettings("SAPlicense") ' objen.License
            oCompany.UseTrusted = ConfigurationManager.AppSettings("SAPtursted") ' False
            If oCompany.Connect <> 0 Then
                strError = oCompany.GetLastErrorDescription()
                DBConnectionDA.WriteError(strError)
                Return strError
            Else
                objMainCompany = oCompany
                Return "Success"
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Connection(ByVal UserID As String, ByVal Pwd As String) As String
        Try

            Pwd = Decrypt(Pwd, key)
            oCompany = New SAPbobsCOM.Company
            objMainCompany = New SAPbobsCOM.Company
            oCompany.Server = ConfigurationManager.AppSettings("SAPServer") ' objen.ServerName
            Select Case ConfigurationManager.AppSettings("DbServerType")
                Case "2005"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                Case "2008"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                Case "2012"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                Case "2014"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
            End Select
            oCompany.DbUserName = ConfigurationManager.AppSettings("DbUserName") ' objen.UID
            oCompany.DbPassword = ConfigurationManager.AppSettings("DbPassword") ' objen.PWD
            oCompany.CompanyDB = ConfigurationManager.AppSettings("CompanyDB") ' objen.SQLCompany
            oCompany.UserName = UserID ' ConfigurationManager.AppSettings("SAPuserName") ' objen.CUID
            oCompany.Password = Pwd ' ConfigurationManager.AppSettings("SAPpassword") ' objen.CPWD
            oCompany.LicenseServer = ConfigurationManager.AppSettings("SAPlicense") ' objen.License
            oCompany.UseTrusted = ConfigurationManager.AppSettings("SAPtursted") ' False
            If oCompany.Connect <> 0 Then
                strError = oCompany.GetLastErrorDescription()
                DBConnectionDA.WriteError(strError)
                Return strError
            Else
                objMainCompany = oCompany
                Return "Success"
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConnectSAP() As Boolean
        Try
            'readxml()
            oCompany = New SAPbobsCOM.Company
            objMainCompany = New SAPbobsCOM.Company
            oCompany.Server = ConfigurationManager.AppSettings("SAPServer") ' objen.ServerName
            Select Case ConfigurationManager.AppSettings("DbServerType")
                Case "2005"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                Case "2008"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                Case "2012"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                Case "2014"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
            End Select
            oCompany.DbUserName = ConfigurationManager.AppSettings("DbUserName") ' objen.UID
            oCompany.DbPassword = ConfigurationManager.AppSettings("DbPassword") ' objen.PWD
            oCompany.CompanyDB = ConfigurationManager.AppSettings("CompanyDB") ' objen.SQLCompany
            oCompany.UserName = ConfigurationManager.AppSettings("SAPuserName") ' objen.CUID
            oCompany.Password = ConfigurationManager.AppSettings("SAPpassword") ' objen.CPWD
            oCompany.LicenseServer = ConfigurationManager.AppSettings("SAPlicense") ' objen.License
            oCompany.UseTrusted = ConfigurationManager.AppSettings("SAPtursted") ' False
            If oCompany.Connect <> 0 Then
                strError = oCompany.GetLastErrorDescription()
                DBConnectionDA.WriteError(strError)
                Return False
            Else
                objMainCompany = oCompany
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function readxml()
        Dim doc As New XmlDocument()
        Try
            If File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Login.xml")) Then
                doc.Load(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Login.xml"))
                Dim root As XmlNode = doc.DocumentElement
                objen.ServerName = root.SelectSingleNode("ServerName").ChildNodes(0).Value
                objen.ServerType = root.SelectSingleNode("ServerType").ChildNodes(0).Value
                objen.UID = root.SelectSingleNode("UID").ChildNodes(0).Value
                objen.PWD = root.SelectSingleNode("PWD").ChildNodes(0).Value
                objen.CUID = root.SelectSingleNode("CUID").ChildNodes(0).Value
                objen.CPWD = root.SelectSingleNode("CPWD").ChildNodes(0).Value
                objen.AdminUid = root.SelectSingleNode("ADMINUID").ChildNodes(0).Value
                objen.AdminPwd = root.SelectSingleNode("ADMINPWD").ChildNodes(0).Value
                objen.SQLCompany = root.SelectSingleNode("SQLCompany").ChildNodes(0).Value
                objen.License = root.SelectSingleNode("License").ChildNodes(0).Value
                'objen.HANAServerName = root.SelectSingleNode("HANAServerName").ChildNodes(0).Value
                'objen.HANALoginName = root.SelectSingleNode("HANALoginName").ChildNodes(0).Value
                'objen.HANAPassword = root.SelectSingleNode("HANAPassword").ChildNodes(0).Value
            End If
            Return objen
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function AdminUserAuthentication(ByVal objLogEn As LoginEN) As Boolean
        ' readxml()
        Dim status As Boolean
        status = (objLogEn.Userid = ConfigurationManager.AppSettings("ADMINUID") And objLogEn.Password = ConfigurationManager.AppSettings("ADMINPWD"))
        If status = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Dropdown1(ByVal query As String, ByVal valcode As String, ByVal valname As String, ByVal ddl As DropDownList)
        'HANAConnection = New OdbcConnection(GetConnection)
        Dim con As SqlConnection = New SqlConnection(GetConnection)
        Dim da As SqlDataAdapter = New SqlDataAdapter(query, con)
        Dim ds As DataSet = New DataSet()
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            ddl.DataTextField = valname
            ddl.DataValueField = valcode
            ddl.DataSource = ds
            ddl.DataBind()
            ddl.Items.Insert(0, "")
        Else
            ddl.DataBind()
            ddl.Items.Insert(0, "")
        End If
    End Sub
    Public Function ESSLink() As String
        Dim strESSLink As String = ""
        Try
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            con.Open()
            cmd = New SqlCommand("Select U_Z_WebPath from [@Z_HR_OWEB]", con)
            cmd.CommandType = CommandType.Text
            strESSLink = Convert.ToString(cmd.ExecuteScalar())
            con.Close()
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
        End Try
        Return strESSLink
    End Function
    Public Function Getmaxcode(ByVal sTable As String, ByVal sColumn As String) As String
        Dim MaxCode As Integer
        Dim sCode As String
        Dim con As SqlConnection = New SqlConnection(GetConnection)
        con.Open()
        cmd = New SqlCommand("SELECT isnull(max(isnull(CAST(isnull(" & sColumn & ",'0') AS Numeric),0)),0) FROM " & sTable & "", con)
        cmd.CommandType = CommandType.Text
        MaxCode = Convert.ToString(cmd.ExecuteScalar())
        If MaxCode >= 0 Then
            MaxCode = MaxCode + 1
        Else
            MaxCode = 1
        End If
        sCode = Format(MaxCode, "00000000")
        con.Close()
        Return sCode
    End Function

    Public Sub UpdateTimeStamp(ByVal objen As SelfAppraisalEN)
        Try
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            If objen.Status = "SF" Then
                objen.StrQry = " Update [@Z_HR_OSEAPP] Set U_Z_SFUserID = '" & objen.EmpId & "',U_Z_SFUDate = GetDate() Where DocEntry = '" & objen.AppraisalNumber & "'"
            ElseIf objen.Status = "SFA" Then
                objen.StrQry = " Update [@Z_HR_OSEAPP] Set U_Z_SFAUserID = '" & objen.EmpId & "',U_Z_SFAUDate = GetDate() Where DocEntry = '" & objen.AppraisalNumber & "'"
            ElseIf objen.Status = "FL" Then
                objen.StrQry = " Update [@Z_HR_OSEAPP] Set U_Z_FUserID = '" & objen.EmpId & "',U_Z_FUDate = GetDate() Where DocEntry = '" & objen.AppraisalNumber & "'"
            ElseIf objen.Status = "SL" Then
                objen.StrQry = " Update [@Z_HR_OSEAPP] Set U_Z_SCUserID = '" & objen.EmpId & "',U_Z_SCUDate = GetDate() Where DocEntry = '" & objen.AppraisalNumber & "'"
            ElseIf objen.Status = "HR" Then
                objen.StrQry = " Update [@Z_HR_OSEAPP] Set U_Z_HRUserID = '" & objen.EmpId & "',U_Z_HRDate = GetDate() Where DocEntry = '" & objen.AppraisalNumber & "'"
            End If
            cmd = New SqlCommand(objen.StrQry, con)
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
        End Try
    End Sub

    Public Function PageloadBindHrAdmin() As DataSet
        Try
            strQuery = "SELECT ""DocEntry"", ""U_Z_EmpId"", ""U_Z_EmpName"", CAST(""U_Z_Date"" AS varchar(11)) AS ""U_Z_Date"", ""U_Z_Period"", CASE ""U_Z_Status"" WHEN 'D' THEN 'Draft' WHEN 'F' THEN 'Approved' WHEN 'S' THEN '2nd Level Approval' WHEN 'L' THEN 'Closed' ELSE 'Canceled' END AS ""U_Z_Status"", CASE ""U_Z_WStatus"" WHEN 'DR' THEN 'Draft' WHEN 'HR' THEN 'HR Approved' WHEN 'SM' THEN 'Sr.Manager Approved' WHEN 'LM' THEN 'LineManager Approved' WHEN 'SE' THEN 'SelfApproved' END AS ""U_Z_WStatus"" FROM ""@Z_HR_OSEAPP"" WHERE ""U_Z_WStatus"" = 'LM' Order by ""DocEntry"" Desc;"
            strQuery += "SELECT ""DocEntry"", ""U_Z_EmpId"", ""U_Z_EmpName"", CAST(""U_Z_Date"" AS varchar(11)) AS ""U_Z_Date"", ""U_Z_Period"", CAST(""U_Z_FDate"" AS varchar(11)) AS ""U_Z_FDate"", CAST(""U_Z_TDate"" AS varchar(11)) AS ""U_Z_TDate"", CASE ""U_Z_Status"" WHEN 'D' THEN 'Draft' WHEN 'F' THEN 'Approved' WHEN 'S' THEN '2nd Level Approval' WHEN 'L' THEN 'Closed' ELSE 'Canceled' END AS ""U_Z_Status"", CASE ""U_Z_WStatus"" WHEN 'DR' THEN 'Draft' WHEN 'HR' THEN 'HR Approved' WHEN 'SM' THEN 'Sr.Manager Approved' WHEN 'LM' THEN 'LineManager Approved' WHEN 'SE' THEN 'SelfApproved' END AS ""U_Z_WStatus"", (SELECT  Top 1 T1.""Name"" AS ""Department"" FROM OHEM T0 INNER JOIN OUDP T1 ON T0.""dept"" = T1.""Code"" WHERE T0.""empID"" = ""U_Z_EmpId"") AS ""Department"" FROM ""@Z_HR_OSEAPP"" WHERE ""U_Z_GStatus"" = 'G' AND ISNULL(""U_Z_GRef"", 0) = 0 ORDER BY ""DocEntry"" DESC;"
            strQuery += "SELECT ""DocEntry"", CAST(""U_Z_ReqDate"" AS varchar(11)) AS ""U_Z_ReqDate"", ""U_Z_HREmpID"", ""U_Z_HREmpName"", ""U_Z_CourseName"", ""U_Z_DeptName"", ""U_Z_PosiName"", CASE ""U_Z_AppStatus"" WHEN 'P' THEN 'Pending' WHEN 'A' THEN 'Approved' WHEN 'R' THEN 'Rejected' END AS ""U_Z_ReqStatus"" FROM ""@Z_HR_ONTREQ"" ORDER BY ""DocEntry"" Desc;"
            strQuery += "SELECT ""DocEntry"", CAST(""U_Z_ReqDate"" AS varchar(11)) AS ""U_Z_ReqDate"", ""U_Z_EmpCode"", ""U_Z_EmpName"", ""U_Z_DeptCode"", ""U_Z_DeptName"", ""U_Z_PosName"", ""U_Z_ExpMin"", ""U_Z_ExpMax"", ""U_Z_EmpstDate"", ""U_Z_IntAppDead"", ""U_Z_ExtAppDead"", ""U_Z_EmpPosi"", ""U_Z_Vacancy"", CASE ""U_Z_AppStatus"" WHEN 'P' THEN 'Pending' WHEN 'A' THEN 'Approved' WHEN 'R' THEN 'Rejected' WHEN 'C' THEN 'Closed' WHEN 'L' THEN 'Canceled' END AS ""U_Z_MgrStatus"" FROM ""@Z_HR_ORMPREQ"" WHERE ""U_Z_AppStatus"" = 'A' ORDER BY ""DocEntry"" Desc;"
            strQuery += "SELECT T0.""DocEntry"", T0.""U_Z_HRAppID"", T0.""U_Z_HRAppName"", T0.""U_Z_DeptName"", T0.""U_Z_JobPosi"", T0.""U_Z_ReqNo"", T0.""U_Z_Email"", T0.""U_Z_Mobile"", T0.""U_Z_ApplStatus"", T0.""U_Z_Skills"", T0.""U_Z_YrExp"", CASE T0.""U_Z_IPHODSta"" WHEN 'S' THEN 'Selected' WHEN 'R' THEN 'Rejected' ELSE 'Pending' END AS ""U_Z_IPHODSta"", CASE T0.""U_Z_AppStatus"" WHEN 'P' THEN 'Pending' WHEN 'R' THEN 'Rejected' WHEN 'A' THEN 'Approved' ELSE 'Pending' END AS ""U_Z_AppStatus"" FROM ""@Z_HR_OHEM1"" T0 INNER JOIN ""@Z_HR_ORMPREQ"" T1 ON T1.""DocEntry"" = T0.""U_Z_ReqNo"" WHERE T0.""U_Z_ApplStatus"" = 'A' ORDER BY T0.""DocEntry"" Desc;"
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(ds)
            Return ds
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function PageLoadbindContactUs() As DataSet
        Try
            strQuery = "Select ""U_Empname"",""U_Position"",""U_Email"" as ""mail"",""U_phone"" from ""U_CONTACTUS"""
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(ds)
            Return ds
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function

    Function Company() As Object
        Throw New NotImplementedException
    End Function
    Public Function validateAuthorization(ByVal objen As ChangePwdEN) As Boolean
        Try
            Dim struserid, st As String
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            strQuery = "Select isnull(UserId,0) as UserId from OHEM where empID='" & objen.EmpId & "'"
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                struserid = ds.Tables(0).Rows(0)("UserId").ToString()
                strQuery1 = "select * from UPT1 where FormId='" & objen.Formid & "'"
                sqlda = New SqlDataAdapter(strQuery1, con)
                sqlda.Fill(ds1)
                If ds1.Tables(0).Rows.Count <= 0 Then
                    Return True
                Else
                    st = ds1.Tables(0).Rows(0)("PermId").ToString
                    strQuery2 = "Select * from USR3 where PermId='" & st & "' and UserLink=" & struserid
                    sqlda = New SqlDataAdapter(strQuery2, con)
                    sqlda.Fill(ds2)
                    If ds2.Tables(0).Rows.Count > 0 Then
                        If ds2.Tables(0).Rows(0)("Permission").ToString = "N" Then
                            Return False
                        End If
                        Return True
                    Else
                        Return True
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function DocApproval(ByVal DocType As String, ByVal Empid As String, Optional ByVal LeaveType As String = "") As String
        Try
            Dim strQuery As String = ""
            Dim Status As String = ""
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            Select Case DocType
                Case "EmpLife", "Rec"
                    strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT3"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_DeptCode""='" & Empid & "' "
                Case "ExpCli", "Train", "TraReq", "LveReq", "LoanReq"
                    If DocType = "LveReq" Then
                        If LeaveType <> "" Then
                            strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT1"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_LveType""='" & LeaveType & "' and  T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_OUser""='" & Empid & "' "
                        Else
                            strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT1"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_OUser""='" & Empid & "' "
                        End If
                    Else
                        strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT1"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_OUser""='" & Empid & "' "

                    End If
            End Select
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(ds2)
            If ds2.Tables(0).Rows.Count > 0 Then
                Status = "P"
            Else
                Status = "A"
            End If
            Return Status
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
            Return False
        End Try
    End Function

    Public Function GetTemplateID(ByVal DocType As String, ByVal Empid As String, Optional ByVal LeaveType As String = "") As String
        Try
            Dim strQuery As String = ""
            Dim Status As String = ""
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            Select Case DocType
                Case "EmpLife", "Rec"
                    strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT3"" T1 on T0.""DocEntry""=T1.""DocEntry"" where isnull(T0.""U_Z_Active"",'N')='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_DeptCode""='" & Empid & "' "
                Case "ExpCli", "Train", "TraReq", "LveReq", "LoanReq"
                    If DocType = "LveReq" Then
                        If LeaveType <> "" Then
                            strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT1"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_LveType""='" & LeaveType & "' and  isnull(T0.""U_Z_Active"",'N')='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_OUser""='" & Empid & "' "
                        Else
                            strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT1"" T1 on T0.""DocEntry""=T1.""DocEntry"" where isnull(T0.""U_Z_Active"",'N')='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_OUser""='" & Empid & "' "
                        End If
                    Else
                        strQuery = "Select * from ""@Z_HR_OAPPT"" T0 left join ""@Z_HR_APPT1"" T1 on T0.""DocEntry""=T1.""DocEntry"" where isnull(T0.""U_Z_Active"",'N')='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_OUser""='" & Empid & "' "

                    End If
            End Select
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(ds4)
            If ds4.Tables(0).Rows.Count > 0 Then
                Status = ds4.Tables(0).Rows(0)("DocEntry").ToString()
            Else
                Status = "0"
            End If
            Return Status
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function

    Public Sub UpdateApprovalRequired(ByVal strTable As String, ByVal sColumn As String, ByVal StrCode As String, ByVal ReqValue As String, ByVal AppTempId As String)
        Try
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            strQuery = "Update [" & strTable & "] set U_Z_AppRequired='" & ReqValue & "',U_Z_AppReqDate=getdate(),U_Z_ApproveId='" & AppTempId & "',"
            strQuery += " U_Z_ReqTime='" & Now.TimeOfDay.ToString() & "' where " & sColumn & "='" & StrCode & "'"
            cmd = New SqlCommand(strQuery, con)
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Sub

    Public Sub InitialMessage(ByVal strReqType As String, ByVal strReqNo As String, ByVal strAppStatus As String _
           , ByVal strTemplateNo As String, ByVal strOrginator As String, ByVal enDocType As String, ByVal objMainCompany As SAPbobsCOM.Company, Optional ByVal strExpNo As String = "", Optional ByVal ESSlink As String = "", Optional ByVal ESSUserID As String = "")
        Try
            'If ConnectSAP() = True Then
            Dim strQuery As String
            Dim strEmailMessage As String
            Dim strMessageUser, strExpReqNo1 As String
            Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
            Dim oCmpSrv As SAPbobsCOM.CompanyService
            Dim oMessageService As SAPbobsCOM.MessagesService
            Dim oMessage As SAPbobsCOM.Message
            Dim pMessageDataColumns As SAPbobsCOM.MessageDataColumns
            Dim pMessageDataColumn As SAPbobsCOM.MessageDataColumn
            Dim oLines As SAPbobsCOM.MessageDataLines
            Dim oLine As SAPbobsCOM.MessageDataLine
            Dim oRecipientCollection As SAPbobsCOM.RecipientCollection
            oCmpSrv = objMainCompany.GetCompanyService()
            oMessageService = oCmpSrv.GetBusinessService(SAPbobsCOM.ServiceTypes.MessagesService)
            oMessage = oMessageService.GetDataInterface(SAPbobsCOM.MessagesServiceDataInterfaces.msdiMessage)
            oRecordSet = objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            strQuery = "Select Top 1 U_Z_AUser From [@Z_HR_APPT2] Where DocEntry = '" + strTemplateNo + "'  and isnull(U_Z_AMan,'')='Y' Order By LineId Asc "
            oRecordSet.DoQuery(strQuery)
            If Not oRecordSet.EoF Then
                strMessageUser = oRecordSet.Fields.Item(0).Value
                oMessage.Subject = strReqType + " " + " is awaiting Your Approval "
                Dim strMessage As String = ""
                Select Case enDocType
                    Case "BankTime" 'Leave Request"
                        strQuery = "Select * from  [@Z_PAY_OLADJTRANS1] where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Z_EMPNAME").Value & " Leave Name  " & oTemp.Fields.Item("U_Z_LeaveName").Value
                        strOrginator = strMessage
                    Case "PerHour"
                        strQuery = "Select * from  [@Z_PAY_OLETRANS1] where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Z_EMPNAME").Value
                        strOrginator = strMessage
                    Case "LveReq", "RetLve" 'Leave Request"
                        strQuery = "Select * from  [@Z_PAY_OLETRANS1] where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Z_EMPNAME").Value & " Leave Name  " & oTemp.Fields.Item("U_Z_LeaveName").Value
                        strOrginator = strMessage
                    Case "ExpCli" 'Expense Claim"
                        strQuery = "Select * from  [@Z_HR_OEXPCL]  where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = "requested by  " & oTemp.Fields.Item("U_Z_EmpName").Value
                        strOrginator = strMessage
                    Case "RegTra"
                        strQuery = "Select * from  [@Z_HR_TRIN1]  where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Z_HREmpName").Value
                        strOrginator = strMessage
                    Case "NewTra"
                        strQuery = "Select * from  [@Z_HR_ONTREQ]  where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = "requested by  " & oTemp.Fields.Item("U_Z_HREmpName").Value
                        strOrginator = strMessage
                    Case "EmpPos"
                        strQuery = "Select * from  [@Z_HR_HEM4]  where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " for Employee  " & oTemp.Fields.Item("U_Z_FirstName").Value & " " & oTemp.Fields.Item("U_Z_LastName").Value
                        strOrginator = strMessage
                    Case "EmpPro"
                        strQuery = "Select * from  [@Z_HR_HEM2]  where Code='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " for Employee  " & oTemp.Fields.Item("U_Z_FirstName").Value & " " & oTemp.Fields.Item("U_Z_LastName").Value
                        strOrginator = strMessage
                    Case "Rec"
                        strQuery = "Select * from [@Z_HR_ORMPREQ]  where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " Recruited by   " & oTemp.Fields.Item("U_Z_EmpName").Value & " for   " & oTemp.Fields.Item("U_Z_PosName").Value & " Position "
                        strOrginator = strMessage
                    Case "AppShort"
                        strQuery = "Select * from  [@Z_HR_OHEM1]  where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = "  Candidate Name  " & oTemp.Fields.Item("U_Z_HRAPPName").Value & " Applied Position  " & oTemp.Fields.Item("U_Z_JobPosi").Value
                        strOrginator = strMessage
                    Case "Final"
                        strQuery = "Select * from  [@Z_HR_OHEM1]  where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " Candidate Name  " & oTemp.Fields.Item("U_Z_HRAPPName").Value & " Applied Position  " & oTemp.Fields.Item("U_Z_JobPosi").Value
                        strOrginator = strMessage
                    Case "TraReq"
                        strQuery = "Select * from  [@Z_HR_OTRAREQ]  where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Z_EmpName").Value
                        strOrginator = strMessage
                    Case "IntAppReq"
                        strQuery = "Select * from  [U_VACPOSITION]  where U_DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Empname").Value
                        strOrginator = strMessage
                    Case "PerObj"
                        strQuery = "Select * from  [U_PEOPLEOBJ]  where U_DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Empname").Value
                        strOrginator = strMessage
                    Case "LoanReq"
                        strQuery = "Select * from  [U_LOANREQ]  where U_DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " requested by  " & oTemp.Fields.Item("U_Empname").Value
                        strOrginator = strMessage
                End Select
                Select Case enDocType
                    Case "BankTime"
                        strQuery = "Update [@Z_PAY_OLADJTRANS1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where Code='" & strReqNo & "'"
                    Case "LveReq", "RetLve"
                        strQuery = "Update [@Z_PAY_OLETRANS1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where Code='" & strReqNo & "'"
                    Case "ExpCli"
                        strQuery = "Update [@Z_HR_EXPCL] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where Code in (" & strExpNo & ")"
                    Case "RegTra"
                        strQuery = "Update [@Z_HR_TRIN1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where Code='" & strReqNo & "'"
                    Case "NewTra"
                        strQuery = "Update [@Z_HR_ONTREQ] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & strReqNo & "'"
                    Case "EmpPos"
                        strQuery = "Update [@Z_HR_HEM4] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where Code='" & strReqNo & "'"
                    Case "EmpPro"
                        strQuery = "Update [@Z_HR_HEM2] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where Code='" & strReqNo & "'"
                    Case "Rec"
                        strQuery = "Update [@Z_HR_ORMPREQ] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & strReqNo & "'"
                    Case "AppShort"
                        strQuery = "Update [@Z_HR_OHEM1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "',U_Z_CurApprover1='" & strMessageUser & "',U_Z_NxtApprover1='" & strMessageUser & "' where DocEntry='" & strReqNo & "'"
                    Case "Final"
                        strQuery = "Update [@Z_HR_OHEM1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & strReqNo & "'"
                    Case "TraReq"
                        strQuery = "Update [@Z_HR_OTRAREQ] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & strReqNo & "'"
                    Case "IntAppReq"
                        strQuery = "Update [U_VACPOSITION] set U_CurApprover='" & strMessageUser & "',U_NxtApprover='" & strMessageUser & "' where U_DocEntry='" & strReqNo & "'"
                    Case "PerObj"
                        strQuery = "Update [U_PEOPLEOBJ] set U_CurApprover='" & strMessageUser & "',U_NxtApprover='" & strMessageUser & "' where U_DocEntry='" & strReqNo & "'"
                    Case "LoanReq"
                        strQuery = "Update [U_LOANREQ] set U_CurApprover='" & strMessageUser & "',U_NxtApprover='" & strMessageUser & "' where U_DocEntry='" & strReqNo & "'"
                End Select
                oTemp.DoQuery(strQuery)

                Dim IntReqNo As Integer = Integer.Parse(strReqNo)
                Dim strExpReqNo As String = IntReqNo.ToString()
                If enDocType = "ExpCli" Then
                    Dim IntReqNo1 As String = strExpNo
                    strExpReqNo1 = IntReqNo1.ToString()
                    ' oMessage.Text = strReqType + "  " + strExpReqNo + " with Expenses :  " + strExpNo + " " + strOrginator + " Needs Your Approval "
                    oMessage.Text = "Expense Claim " & strExpReqNo & " " & strOrginator & " is awaiting your approval"
                ElseIf enDocType = "RetLve" Then
                    oMessage.Text = strReqType + "  is awaiting your approval "
                Else
                    oMessage.Text = strReqType + "  " + strExpReqNo + " " + strOrginator + " is awaiting your approval "
                End If
                oRecipientCollection = oMessage.RecipientCollection
                oRecipientCollection.Add()
                oRecipientCollection.Item(0).SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                oRecipientCollection.Item(0).UserCode = strMessageUser
                pMessageDataColumns = oMessage.MessageDataColumns

                pMessageDataColumn = pMessageDataColumns.Add()
                pMessageDataColumn.ColumnName = "Request No"
                oLines = pMessageDataColumn.MessageDataLines()
                oLine = oLines.Add()
                If enDocType = "ExpCli" Then
                    oLine.Value = strExpReqNo1
                Else
                    oLine.Value = strExpReqNo
                End If
                oMessageService.SendMessage(oMessage)
                If enDocType = "ExpCli" Then
                    ' Dim IntReqNo1 As Integer = strExpNo
                    ' strExpReqNo1 = IntReqNo1.ToString()
                    strEmailMessage = "Expense Claim " & strExpReqNo & " " & strOrginator & " is awaiting your approval.Please login to ESS  " & ESSlink & ". "'ESS UserID : " & ESSUserID
                    ' strEmailMessage = strReqType + "  " + strExpReqNo + " with Expenses :  " + strExpReqNo1 + " " + strOrginator + " Needs Your Approval "
                ElseIf enDocType = "RetLve" Then
                    strEmailMessage = strReqType + " is awaiting your approval "
                Else
                    strEmailMessage = strReqType + "  " + strExpReqNo + " " + strOrginator + " is awaiting your approval"
                End If
                SendMail_Approval(strEmailMessage, strMessageUser, strMessageUser, objMainCompany, strExpReqNo1)


            End If
            ' End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Sub

    Public Sub SendMail_Approval(ByVal aMessage As String, ByVal aMail As String, ByVal aUser As String, ByVal aCompany As SAPbobsCOM.Company, Optional ByVal SerialNo As String = "", Optional ByVal ReqNo As String = "")
        Try
            Dim oRecordset As SAPbobsCOM.Recordset
            oRecordset = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery("Select U_Z_SMTPSERV,U_Z_SMTPPORT,U_Z_SMTPUSER,U_Z_SMTPPWD,U_Z_SSL From [@Z_HR_OMAIL]")
            If Not oRecordset.EoF Then
                mailServer = oRecordset.Fields.Item("U_Z_SMTPSERV").Value
                mailPort = oRecordset.Fields.Item("U_Z_SMTPPORT").Value
                mailId = oRecordset.Fields.Item("U_Z_SMTPUSER").Value
                mailPwd = oRecordset.Fields.Item("U_Z_SMTPPWD").Value
                mailSSL = oRecordset.Fields.Item("U_Z_SSL").Value
                If mailServer <> "" And mailId <> "" And mailPwd <> "" Then
                    oRecordset.DoQuery("Select * from OUSR where USER_CODE='" & aUser & "'")
                    aMail = oRecordset.Fields.Item("E_Mail").Value
                    If aMail <> "" Then
                        Dim oTest As SAPbobsCOM.Recordset
                        oTest = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oTest.DoQuery("Select * from [@Z_HR_OWEB]")
                        Dim strESSLink As String = ""
                        If oTest.RecordCount > 0 Then
                            strESSLink = oTest.Fields.Item("U_Z_WebPath").Value
                        End If
                        SendMailforApproval(mailServer, mailPort, mailId, mailPwd, mailSSL, aMail, aMail, "Approval", aMessage, aCompany, strESSLink, SerialNo, aUser)
                    End If
                Else
                    ' oApplication.Utilities.Message("Mail Server Details Not Configured...", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                End If
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
        End Try
    End Sub

    Private Sub SendMailforApproval(ByVal mailServer As String, ByVal mailPort As String, ByVal mailId As String, ByVal mailpwd As String, ByVal mailSSL As String, ByVal toId As String, ByVal ccId As String, ByVal mType As String, ByVal Message As String, ByVal aCompany As SAPbobsCOM.Company, Optional ByVal strESSLink As String = "", Optional ByVal SerialNo As String = "", Optional ByVal aUser As String = "", Optional ByVal aEmpId As String = "")
        Try
            'Dim strRptPath As String = System.Windows.Forms.Application.StartupPath.Trim() & "\Report.pdf"
            SmtpServer.Credentials = New Net.NetworkCredential(mailId, mailpwd)
            SmtpServer.Port = mailPort
            SmtpServer.EnableSsl = mailSSL
            SmtpServer.Host = mailServer
            mail = New Net.Mail.MailMessage()
            mail.From = New Net.Mail.MailAddress(mailId, "HRMS")
            mail.To.Add(toId)
            '  mail.CC.Add(ccId)
            mail.IsBodyHtml = True
            mail.Priority = MailPriority.High
            mail.Subject = Message
            ' mail.Body = Message & "  <a href=" & strESSLink & " >Click Here to Login to ESS</a>"
            If SerialNo <> "" Then
                mail.Body = BuildHtmBody(SerialNo, aUser, "ExpClaim", mType, aCompany, Message, aEmpId)
            Else
                mail.Body = Message
            End If
            SmtpServer.Send(mail)
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
        Finally
            mail.Dispose()
        End Try
    End Sub
    Public Function BuildHtmBody(ByVal DocEntry As String, ByVal Name As String, ByVal type As String, ByVal mtype As String, ByVal aCompany As SAPbobsCOM.Company, Optional ByVal strMessage As String = "", Optional ByVal aEmpId As String = "")
        Dim oHTML, oHTML1, oHtml2, strPath As String
        Dim strCompany As String
        Dim strName As String
        Dim Address1, Address2, Mail, empid, empName, client, project, total, UserID As String
        Dim CourseCode, CourseName, StDate, EdDate, StTime, EndTime, TotalHours, Instrutor, AppED As String
        Dim strCode, SerialNo, ExpType, TransAmut, LocAmt, tobeRem, Notes, RejRemarks, trancur, ExcRate, ReimAmt As String

        If type = "Appraisal" Then
            oHTML = GetFileContents("Appraisal.htm")
        ElseIf type = "Agenda" Then
            oHTML = GetFileContents("Agenda.htm")
        ElseIf type = "ExpClaim" Then
            strPath = System.Web.HttpContext.Current.Server.MapPath("ExpClaim.htm")
            oHTML = GetFileContents(strPath)
        End If

        sQuery = " Select CompnyName,CompnyAddr,Country,PrintHeadr,Phone1,E_Mail From OADM"
        oRecordSet = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oRecordSet.DoQuery(sQuery)

        If Not oRecordSet.EoF Then
            strCompany = oRecordSet.Fields.Item("CompnyName").Value
            strName = oRecordSet.Fields.Item("PrintHeadr").Value
            Address1 = oRecordSet.Fields.Item("CompnyAddr").Value
            Address2 = oRecordSet.Fields.Item("Country").Value
            Mail = oRecordSet.Fields.Item("E_Mail").Value
        End If
        If type = "ExpClaim" Then
            sQuery = "select SUM(cast(isnull(replace(U_Z_UsdAmt,left(U_Z_UsdAmt,3),''),0) as decimal(12,2))) AS U_Z_CurAmt,U_Z_EmpID,U_Z_EmpName,U_Z_Client,U_Z_Project,U_Z_DocRefNo from [@Z_HR_EXPCL] "
            sQuery += " where Code in (" & DocEntry & ") group by U_Z_DocRefNo,U_Z_EmpID,U_Z_EmpName,U_Z_Client,U_Z_Project"
            oRecordSet.DoQuery(sQuery)
            If Not oRecordSet.EoF Then
                empid = oRecordSet.Fields.Item("U_Z_EmpID").Value
                empName = oRecordSet.Fields.Item("U_Z_EmpName").Value
                client = oRecordSet.Fields.Item("U_Z_Client").Value
                project = oRecordSet.Fields.Item("U_Z_Project").Value
                total = oRecordSet.Fields.Item("U_Z_CurAmt").Value
                sQuery = "select U_Z_UID  from [@Z_HR_LOGIN] where U_Z_EMPID='" & empid & "'"
                oRecordSet.DoQuery(sQuery)
                If oRecordSet.RecordCount > 0 Then
                    UserID = oRecordSet.Fields.Item("U_Z_UID").Value
                End If
            End If


            sQuery = "select SUM(cast(isnull(replace(U_Z_UsdAmt,left(U_Z_UsdAmt,3),''),0) as decimal(12,2))) AS U_Z_CurAmt,U_Z_EmpID,U_Z_EmpName,U_Z_Client,U_Z_Project,U_Z_DocRefNo from [@Z_HR_EXPCL] "
            sQuery += " where Code in (" & DocEntry & ") and U_Z_Reimburse='Y' group by U_Z_DocRefNo,U_Z_EmpID,U_Z_EmpName,U_Z_Client,U_Z_Project"
            oRecordSet.DoQuery(sQuery)
            If Not oRecordSet.EoF Then
                ReimAmt = oRecordSet.Fields.Item("U_Z_CurAmt").Value
            End If
            If Not IsDBNull(strMessage) Then
                oHTML = oHTML.Replace("$$Messages$$", strMessage)
            Else
                oHTML = oHTML.Replace("$$Messages$$", "")
            End If
            If Not IsDBNull(UserID) Then
                oHTML = oHTML.Replace("$$ESSUserID$$", UserID)
            Else
                oHTML = oHTML.Replace("$$ESSUserID$$", "")
            End If
            If Not IsDBNull(empid) Then
                oHTML = oHTML.Replace("$$EmpCode$$", empid)
            Else
                oHTML = oHTML.Replace("$$EmpCode$$", "")
            End If

            If Not IsDBNull(empName) Then
                oHTML = oHTML.Replace("$$ReqEmpName1$$", empName)
            Else
                oHTML = oHTML.Replace("$$ReqEmpName1$$", "")
            End If

            If Not IsDBNull(client) Then
                oHTML = oHTML.Replace("$$Client$$", client)
            Else
                oHTML = oHTML.Replace("$$Client$$", "")
            End If

            If Not IsDBNull(project) Then
                oHTML = oHTML.Replace("$$Project$$", project)
            Else
                oHTML = oHTML.Replace("$$Project$$", "")
            End If
            If Not IsDBNull(total) Then
                oHTML = oHTML.Replace("$$TransAmt$$", total)
            Else
                oHTML = oHTML.Replace("$$TransAmt$$", "")
            End If
            If Not IsDBNull(ReimAmt) Then
                oHTML = oHTML.Replace("$$Reimbused$$", ReimAmt)
            Else
                oHTML = oHTML.Replace("$$Reimbused$$", "")
            End If
            If aEmpId = "" Then
                If Not IsDBNull(empName) Then
                    oHTML = oHTML.Replace("$$ReqEmpName$$", empName)
                Else
                    oHTML = oHTML.Replace("$$ReqEmpName$$", "")
                End If
            Else
                If Not IsDBNull(aEmpId) Then
                    oHTML = oHTML.Replace("$$ReqEmpName$$", aEmpId)
                Else
                    oHTML = oHTML.Replace("$$ReqEmpName$$", "")
                End If
            End If
            sQuery = "select Code,U_Z_ExpType,U_Z_CurAmt,U_Z_UsdAmt,U_Z_RejRemark,U_Z_Currency,U_Z_ExcRate, CASE U_Z_Reimburse when 'Y' then 'Yes' else 'NO' end AS U_Z_Reimburse,U_Z_Notes,U_Z_DocRefNo from [@Z_HR_EXPCL] "
            sQuery += " where Code in (" & DocEntry & ") "
            oRecordSet.DoQuery(sQuery)
            If Not oRecordSet.EoF Then
                For intRow As Integer = 0 To oRecordSet.RecordCount - 1
                    strPath = System.Web.HttpContext.Current.Server.MapPath("ExpDetails.htm")
                    oHTML1 = GetFileContents(strPath)
                    SerialNo = oRecordSet.Fields.Item("Code").Value
                    strCode = oRecordSet.Fields.Item("U_Z_DocRefNo").Value
                    ExpType = oRecordSet.Fields.Item("U_Z_ExpType").Value
                    TransAmut = oRecordSet.Fields.Item("U_Z_CurAmt").Value
                    LocAmt = oRecordSet.Fields.Item("U_Z_UsdAmt").Value
                    tobeRem = oRecordSet.Fields.Item("U_Z_Reimburse").Value
                    Notes = oRecordSet.Fields.Item("U_Z_Notes").Value
                    RejRemarks = oRecordSet.Fields.Item("U_Z_RejRemark").Value
                    trancur = oRecordSet.Fields.Item("U_Z_Currency").Value
                    ExcRate = oRecordSet.Fields.Item("U_Z_ExcRate").Value
                    If Not IsDBNull(strCode) Then
                        oHTML1 = oHTML1.Replace("$$ExpCode$$", strCode)
                    Else
                        oHTML1 = oHTML1.Replace("$$ExpCode$$", "")
                    End If
                    If Not IsDBNull(SerialNo) Then
                        oHTML1 = oHTML1.Replace("$$SerialNo$$", SerialNo)
                    Else
                        oHTML1 = oHTML1.Replace("$$SerialNo$$", "")
                    End If
                    If Not IsDBNull(ExpType) Then
                        oHTML1 = oHTML1.Replace("$$ExpType$$", ExpType)
                    Else
                        oHTML1 = oHTML1.Replace("$$ExpType$$", "")
                    End If
                    If Not IsDBNull(trancur) Then
                        oHTML1 = oHTML1.Replace("$$Currency$$", trancur)
                    Else
                        oHTML1 = oHTML1.Replace("$$Currency$$", "")
                    End If
                    If Not IsDBNull(ExcRate) Then
                        oHTML1 = oHTML1.Replace("$$ExcRate$$", ExcRate)
                    Else
                        oHTML1 = oHTML1.Replace("$$ExcRate$$", "")
                    End If
                    If Not IsDBNull(TransAmut) Then
                        oHTML1 = oHTML1.Replace("$$TransAmount$$", TransAmut)
                    Else
                        oHTML1 = oHTML1.Replace("$$TransAmount$$", "")
                    End If
                    If Not IsDBNull(LocAmt) Then
                        oHTML1 = oHTML1.Replace("$$LocAmt$$", LocAmt)
                    Else
                        oHTML1 = oHTML1.Replace("$$LocAmt$$", "")
                    End If
                    If Not IsDBNull(tobeRem) Then
                        oHTML1 = oHTML1.Replace("$$Tobereim$$", tobeRem)
                    Else
                        oHTML1 = oHTML1.Replace("$$Tobereim$$", "")
                    End If
                    If Not IsDBNull(Notes) Then
                        oHTML1 = oHTML1.Replace("$$Notes$$", Notes)
                    Else
                        oHTML1 = oHTML1.Replace("$$Notes$$", "")
                    End If
                    If Not IsDBNull(RejRemarks) Then
                        oHTML1 = oHTML1.Replace("$$Remarks$$", RejRemarks)
                    Else
                        oHTML1 = oHTML1.Replace("$$Remarks$$", "")
                    End If
                    oHtml2 += oHTML1
                    oRecordSet.MoveNext()
                Next
                oHTML = oHTML.Replace("$$$ROWS$$$", oHtml2)
            End If


        End If

        If type = "Agenda" Then
            sQuery = "Select U_Z_CourseCode,U_Z_CourseName,U_Z_Startdt,U_Z_EndDt,U_Z_AppStDt,U_Z_AppEndDt,U_Z_InsName,Convert(VarChar(5),Convert(DateTime ,Convert(Date,GetDate()))+ Convert(VarChar(5),SUBSTRING(RIGHT('0000' + CAST(U_Z_StartTime AS NVARCHAR),4),1,2) + ':' + SUBSTRING(RIGHT('0000' + CAST(U_Z_StartTime AS NVARCHAR),4),3,2)) ,108) as U_Z_StartTime,Convert(VarChar(5),Convert(DateTime ,Convert(Date,GetDate()))+ Convert(VarChar(5),SUBSTRING(RIGHT('0000' + CAST(U_Z_EndTime AS NVARCHAR),4),1,2) + ':' + SUBSTRING(RIGHT('0000' + CAST(U_Z_EndTime AS NVARCHAR),4),3,2)) ,108)As U_Z_EndTime,U_Z_Sunday,U_Z_Monday,U_Z_Tuesday,U_Z_Wednesday,U_Z_Thursday,U_Z_Friday,U_Z_Saturday,U_Z_NoOfHours From [@Z_HR_OTRIN] Where U_Z_CourseCode ='" & DocEntry & "'"
            oRecordSet.DoQuery(sQuery)
            If Not oRecordSet.EoF Then
                CourseCode = oRecordSet.Fields.Item("U_Z_CourseCode").Value
                CourseName = oRecordSet.Fields.Item("U_Z_CourseName").Value
                StDate = oRecordSet.Fields.Item("U_Z_Startdt").Value
                EdDate = oRecordSet.Fields.Item("U_Z_EndDt").Value
                StTime = oRecordSet.Fields.Item("U_Z_StartTime").Value
                EndTime = oRecordSet.Fields.Item("U_Z_EndTime").Value
                TotalHours = oRecordSet.Fields.Item("U_Z_NoOfHours").Value
                Instrutor = oRecordSet.Fields.Item("U_Z_InsName").Value
                AppED = oRecordSet.Fields.Item("U_Z_AppEndDt").Value
            End If
        End If
        If aEmpId = "" Then
            sQuery = " select isnull(firstName,'') +' '+ isnull(lastName,'') AS EmpName from OHEM T0 JOIN OUSR T1 On T0.userId=T1.USERID where T1.USER_CODE='" & Name & "'"
            oRecordSet.DoQuery(sQuery)
            If Not oRecordSet.EoF Then
                Name = oRecordSet.Fields.Item("EmpName").Value
            End If
        Else
            Name = Name
        End If

        If Not IsDBNull(Name) Then
            oHTML = oHTML.Replace("$$EmpName$$", Name)
        Else
            oHTML = oHTML.Replace("$$EmpName$$", "")
        End If


        If Not IsDBNull(strName) Then
            oHTML = oHTML.Replace("$$Company$$", strName)
        Else
            oHTML = oHTML.Replace("$$Company$$", "")
        End If

        If Not IsDBNull(Address1) Then
            oHTML = oHTML.Replace("$$Address1$$", Address1)
        Else
            oHTML = oHTML.Replace("$$Address1$$", "")
        End If

        If Not IsDBNull(Address2) Then
            oHTML = oHTML.Replace("$$Address2$$", Address2)
        Else
            oHTML = oHTML.Replace("$$Address2$$", "")
        End If

        If Not IsDBNull(Mail) Then
            oHTML = oHTML.Replace("$$Mail$$", Mail)
        Else
            oHTML = oHTML.Replace("$$Mail$$", "")
        End If


        If mtype = "AI" Then
            oHTML = oHTML.Replace("$$Comments$$", "Appraisal Process Initiated...")
            oHTML = oHTML.Replace("$$Messages$$", strMessage)
        ElseIf mtype = "SF" Then
            oHTML = oHTML.Replace("$$Comments$$", "First Level manager Appraisal Approval Notification")
            oHTML = oHTML.Replace("$$Messages$$", strMessage)
        ElseIf mtype = "LA" Then
            oHTML = oHTML.Replace("$$Comments$$", "Second Level manager Appraisal Approval Notification")
            oHTML = oHTML.Replace("$$Messages$$", strMessage)
        ElseIf mtype = "HA" Then
            oHTML = oHTML.Replace("$$Comments$$", "HR Appraisal Approval Notification...")
            oHTML = oHTML.Replace("$$Messages$$", strMessage)
        ElseIf mtype = "EN" Then
            oHTML = oHTML.Replace("$$Comments$$", "Appraisal Approval finished Notification...")
            oHTML = oHTML.Replace("$$Messages$$", strMessage)
        End If


        If Not IsDBNull(CourseCode) Then
            oHTML = oHTML.Replace("$$Course$$", CourseCode)
        Else
            oHTML = oHTML.Replace("$$Course$$", "")
        End If

        If Not IsDBNull(CourseName) Then
            oHTML = oHTML.Replace("$$Name$$", CourseName)
        Else
            oHTML = oHTML.Replace("$$Name$$", "")
        End If

        If Not IsDBNull(StDate) Then
            oHTML = oHTML.Replace("$$SD$$", StDate)
        Else
            oHTML = oHTML.Replace("$$SD$$", "")
        End If

        If Not IsDBNull(EdDate) Then
            oHTML = oHTML.Replace("$$ED$$", EdDate)
        Else
            oHTML = oHTML.Replace("$$ED$$", "")
        End If

        If Not IsDBNull(StTime) Then
            oHTML = oHTML.Replace("$$ST$$", StTime)
        Else
            oHTML = oHTML.Replace("$$ST$$", "")
        End If

        If Not IsDBNull(EndTime) Then
            oHTML = oHTML.Replace("$$ET$$", EndTime)
        Else
            oHTML = oHTML.Replace("$$ET$$", "")
        End If


        If Not IsDBNull(TotalHours) Then
            oHTML = oHTML.Replace("$$TH$$", TotalHours)
        Else
            oHTML = oHTML.Replace("$$TH$$", "")
        End If

        If Not IsDBNull(Instrutor) Then
            oHTML = oHTML.Replace("$$Instrutor$$", Instrutor)
        Else
            oHTML = oHTML.Replace("$$Instrutor$$", "")
        End If

        If Not IsDBNull(AppED) Then
            oHTML = oHTML.Replace("$$AED$$", AppED)
        Else
            oHTML = oHTML.Replace("$$AED$$", "")
        End If

        Return oHTML
    End Function
    Public Function GetFileContents(ByVal FullPath As String, _
      Optional ByRef ErrInfo As String = "") As String

        Dim strContents As String
        Dim objReader As StreamReader
        Try
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
    End Function
    Public Function WithDrawStatus(ByVal DocType As String, ByVal strCode As String) As Boolean
        Try
            dss4.Clear()
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            strQuery = "select * from [@Z_HR_APHIS] where U_Z_DocEntry='" & strCode.Trim() & "' and U_Z_DocType='" & DocType.Trim() & "' "
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(dss4)
            If dss4.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function LoanWithDrawStatus(ByVal DocType As String, ByVal strCode As String) As Boolean
        Try
            dss4.Clear()
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            strQuery = "select * from [@Z_HR_LOANHIS] where U_Z_DocEntry='" & strCode.Trim() & "' and U_Z_DocType='" & DocType.Trim() & "' "
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(dss4)
            If dss4.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function getEmpName(ByVal aEmpId As String) As String
        Dim con As SqlConnection = New SqlConnection(GetConnection)
        Dim strEmpName As String = ""
        Try
            strQuery = "Select isnull(firstName,'') + ' ' + isnull(middleName,'') +' ' + isnull(lastName,'') from OHEM where empid=" & aEmpId
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(dss4)
            If dss4.Tables(0).Rows.Count > 0 Then
                strEmpName = dss4.Tables(0).Rows(0)(0).ToString()
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
        Return strEmpName
    End Function

    Public Function ViewHistory(ByVal RefCode As String, ByVal DocType As String) As DataSet
        Try
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            sQuery = " Select DocEntry,U_Z_DocEntry,U_Z_DocType,U_Z_EmpId,U_Z_EmpName,U_Z_ApproveBy,Convert(Varchar(10),CreateDate,103) AS 'CreateDate',"
            sQuery += " LEFT(CONVERT(VARCHAR(5), CreateTime, 9),2) + ':' + RIGHT(CONVERT(VARCHAR(30), CreateTime, 9),2) AS CreateTime,Convert(Varchar(10),UpdateDate,103) AS UpdateDate,LEFT(CONVERT(VARCHAR(5), UpdateTime, 9),2) + ':' + RIGHT(CONVERT(VARCHAR(30), UpdateTime, 9),2) AS UpdateTime,Case U_Z_AppStatus when 'A' then 'Approved' when 'R' then 'Rejected' else 'Pending' end as U_Z_AppStatus,U_Z_Remarks From [@Z_HR_APHIS] "
            sQuery += " Where U_Z_DocType = '" + DocType.Trim() + "'"
            sQuery += " And U_Z_DocEntry = '" + RefCode.Trim() + "'"
            sqlda = New SqlDataAdapter(sQuery, con)
            sqlda.Fill(ds4)
            Return ds4
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function getDocumentQuantity(ByVal strQuantity As String, ByVal aCompany As SAPbobsCOM.Company) As Double
        Dim dblQuant As Double
        Dim strTemp, strTemp1 As String
        Dim oRec As SAPbobsCOM.Recordset
        Try
            oRec = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRec.DoQuery("Select CurrCode  from OCRN")
            For intRow As Integer = 0 To oRec.RecordCount - 1
                strQuantity = strQuantity.Replace(oRec.Fields.Item(0).Value, "")
                oRec.MoveNext()
            Next
            strTemp1 = strQuantity
            strTemp = "."
            If strQuantity = "" Then
                Return 0
            End If
            Try
                dblQuant = Convert.ToDouble(strQuantity)
            Catch ex As Exception
                dblQuant = Convert.ToDouble(strTemp1)
            End Try
            Return dblQuant
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
        End Try
        Return dblQuant
    End Function
    Public Function getCustAccNo(ByVal CardCode As String) As String
        Dim strQuery, FromatCode As String
        Dim con As SqlConnection = New SqlConnection(GetConnection)
        Try
            strQuery = "Select isnull(DebPayAcct ,0) from OCRD where cardCode='" & CardCode & "'"
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(ds4)
            If ds4.Tables(0).Rows.Count > 0 Then
                FromatCode = ds4.Tables(0).Rows(0)(0).ToString
            Else
                FromatCode = 0
            End If
            Return FromatCode
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function getSAPAccount(ByVal aCode As String, ByVal SAPCompany As SAPbobsCOM.Company) As String
        Dim oRS As SAPbobsCOM.Recordset
        oRS = SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oRS.DoQuery("Select isnull(AcctCode,'') from OACT where Formatcode='" & aCode & "'")
        Return oRS.Fields.Item(0).Value
    End Function

    Public Function checkmailconfiguration() As Boolean
        Dim con As SqlConnection = New SqlConnection(GetConnection)
        Try
            strQuery = "Select U_Z_SMTPSERV,U_Z_SMTPPORT,U_Z_SMTPUSER,U_Z_SMTPPWD,U_Z_SSL From [@Z_HR_OMAIL]"
            sqlda = New SqlDataAdapter(strQuery, con)
            sqlda.Fill(dss4)
            If dss4.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function

    Public Sub SendMail_RequestApproval(ByVal aMessage As String, ByVal Empid As String, ByVal aCompany As SAPbobsCOM.Company, Optional ByVal aMail As String = "", Optional ByVal SerialNo As String = "", Optional ByVal ReqNo As String = "", Optional ByVal CancelReq As String = "")
        Try
            Dim oRecordset As SAPbobsCOM.Recordset
            oRecordset = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery("Select U_Z_SMTPSERV,U_Z_SMTPPORT,U_Z_SMTPUSER,U_Z_SMTPPWD,U_Z_SSL From [@Z_HR_OMAIL]")
            If Not oRecordset.EoF Then
                mailServer = oRecordset.Fields.Item("U_Z_SMTPSERV").Value
                mailPort = oRecordset.Fields.Item("U_Z_SMTPPORT").Value
                mailId = oRecordset.Fields.Item("U_Z_SMTPUSER").Value
                mailPwd = oRecordset.Fields.Item("U_Z_SMTPPWD").Value
                mailSSL = oRecordset.Fields.Item("U_Z_SSL").Value
                If mailServer <> "" And mailId <> "" And mailPwd <> "" Then
                    oRecordset.DoQuery("Select * from OHEM where empID='" & Empid & "'")
                    If CancelReq = "" Then
                        aMail = oRecordset.Fields.Item("email").Value
                    Else
                        aMail = oRecordset.Fields.Item("U_Z_HRMail").Value
                    End If
                    If aMail <> "" Then
                        Dim oTest As SAPbobsCOM.Recordset
                        oTest = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oTest.DoQuery("Select * from [@Z_HR_OWEB]")
                        Dim strESSLink As String = ""
                        If oTest.RecordCount > 0 Then
                            strESSLink = oTest.Fields.Item("U_Z_WebPath").Value
                        End If
                        SendMailforApproval(mailServer, mailPort, mailId, mailPwd, mailSSL, aMail, aMail, "Approval", aMessage, aCompany, strESSLink, SerialNo, ReqNo, "HR")
                    End If
                Else
                End If
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
        End Try

    End Sub
    Public Function GetAttachment(ByVal acode As String, empID As String) As String
        Try
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            con.Open()
            cmd = New SqlCommand("SELECT ISNULL(U_Z_Attachment,'') AS U_Z_Attachment   FROM [@Z_HR_ONTREQ] WHERE U_Z_HREmpID='" & empID.Trim() & "' AND DocEntry='" & acode.Trim() & "'", con)
            cmd.CommandType = CommandType.Text
            Dim status As String
            status = cmd.ExecuteScalar()
            con.Close()
            If status Is Nothing Then
                status = ""
            End If
            Return status
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
End Class
