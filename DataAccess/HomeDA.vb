﻿Imports System
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports EN
Public Class HomeDA
    Dim objen As HomeEN = New HomeEN()
    Dim objDA As DBConnectionDA = New DBConnectionDA()
    Dim objCom As CommonFunctions = New CommonFunctions()
    Public Sub New()
        objDA.con = New SqlConnection(objDA.GetConnection)
    End Sub

    Public Function PopulateEmployeeInternal(ByVal objen As HomeEN) As DataSet
        Try
            objen.StrQry = "Select dept,empID,firstName,lastName, isnull(position,0) as position,descriptio  from OHEM T0 left join OHPS T1 on T0.position=t1.posID  where empID=" & objen.EmpId & ""
            objDA.sqlda = New SqlDataAdapter(objen.StrQry, objDA.con)
            objDA.sqlda.Fill(objDA.ds1)
            Return objDA.ds1
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function PopulateEmployee(ByVal objen As HomeEN) As DataSet
        Try
            objDA.strQuery = "SELECT isnull(U_Z_EmpID,'') as 'TAEmpID', ""empID"", ""firstName"", ""lastName"", ""middleName"", ""U_Z_HR_ThirdName"","
            objDA.strQuery += " T0.""position"", T1.""descriptio"" AS ""Positionname"", ""dept"", T2.""Remarks"" AS ""Deptname"", ""U_Z_HR_JobstCode"", T3.""Name"" AS ""BranchName"", ""officeExt"", ""U_Z_Rel_Name"", ""U_Z_Rel_Type"", ""U_Z_Rel_Phone"", ""officeTel"", ""mobile"", ""email"", ""fax"", ""homeTel"", ""pager"",""sex"", convert(varchar(10),""birthDate"",103) AS ""birthDate"", ""brthCountr"", ""martStatus"", ""nChildren"", ""govID"", ""citizenshp"", convert(varchar(10),""passportEx"",103) AS ""passportEx"", ""passportNo"", ""workBlock"", ""workCity"", ""workCountr"", ""workState"", ""workCounty"", ""workStreet"", ""workZip"", ""homeBlock"", ""homeCity"", ""homeCountr"", ""homeCounty"", ""homeState"", ""homeStreet"", ""homeZip"", ""U_Z_HR_OrgstCode"", ""U_Z_HR_OrgstName"", ""WorkBuild"", ""HomeBuild"", ""U_Z_LvlCode"", ""U_Z_LvlName"", ""U_Z_LocCode"", ""U_Z_LocName"", ""U_Z_HR_JobstCode"", ""U_Z_HR_JobstName"", ""U_Z_HR_SalaryCode"", ""U_Z_HR_ApplId"", ISNULL(""manager"", 0) AS ""Manager"" FROM OHEM T0 LEFT OUTER JOIN OHPS T1 ON T0.""position"" = T1.""posID"" LEFT OUTER JOIN OUDP T2 ON T0.""dept"" = T2.""Code"" LEFT OUTER JOIN OUBR T3 ON T0.""branch"" = T3.""Code"" where ""empID""='" & objen.EmpId & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss)
            Return objDA.dss
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function Department(ByVal objen As HomeEN) As String
        Try
            objDA.con.Open()
            objen.StrQry = "select Remarks from OUDP  where Code=" & objen.DeptCode & ""
            objDA.cmd = New SqlCommand(objen.StrQry, objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            objen.DeptName = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return objen.DeptName
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function EmpManager(ByVal objen As HomeEN) As String
        Try
            objDA.con.Open()
            objen.StrQry = "select isnull(""firstName"",'') +  ' ' + isnull(""middleName"",'') +  ' ' + isnull(""lastName"",'') as ManName from OHEM where empId=" & objen.EmpId & ""
            objDA.cmd = New SqlCommand(objen.StrQry, objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            objen.DeptName = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return objen.DeptName
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function mainGvbind(ByVal objen As HomeEN) As DataSet
        Try
            objDA.strQuery = "select DocEntry,U_Z_EmpId,U_Z_EmpName,convert(varchar(10),U_Z_Date,103) as U_Z_Date,U_Z_Period,case U_Z_Status when 'D' then 'Draft' when 'F' then 'Approved'"
            objDA.strQuery += " when 'S'then '2nd Level Approval' when 'L' then 'Closed' else 'Canceled' end as U_Z_Status,case U_Z_WStatus when 'DR' then 'Draft' when 'HR' then 'HR Approved' when 'SM'then 'Second Level Approved' when 'LM' then 'First Level Approved' when 'SE' then 'SelfApproved'  end as 'U_Z_WStatus',U_Z_GStatus,U_Z_GRemarks,convert(varchar(11),U_Z_GDate,101) as U_Z_GDate,U_Z_GNo  from [@Z_HR_OSEAPP] where U_Z_EmpId=" & objen.EmpId & " and ISNULL(U_Z_GStatus,'') = '-' and (U_Z_WStatus='DR' or U_Z_WStatus='LM') Order by DocEntry Desc"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            Return objDA.ds
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function LeaveBalance(ByVal objen As HomeEN) As DataSet
        Try
            objDA.strQuery = "SELECT T0.[U_Z_Year],T0.[U_Z_LeaveCode],T0.[U_Z_LeaveName],T0.[U_Z_Entile],T0.[U_Z_CAFWD],T0.[U_Z_CAFWDAMT],T0.[U_Z_ACCR],"
            objDA.strQuery += " T0.[U_Z_Trans],T0.[U_Z_Adjustment],T0.[U_Z_Balance],T0.[U_Z_BalanceAmt],T0.[U_Z_OB],T0.[U_Z_EnCash],T0.[U_Z_CashOut] "
            objDA.strQuery += " FROM [dbo].[@Z_EMP_LEAVE_BALANCE]  T0 where T0.U_Z_EmpID=" & objen.EmpId & " and T0.[U_Z_Year]='" & Now.Year & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds2)
            If objDA.ds2.Tables(0).Rows.Count > 0 Then
                Return objDA.ds2
            Else
                objDA.ds2.Clear()
                objDA.strQuery = "SELECT T0.[U_Z_Year],T0.[U_Z_LeaveCode],T0.[U_Z_LeaveName],T0.[U_Z_Entile],T0.[U_Z_CAFWD],T0.[U_Z_CAFWDAMT],T0.[U_Z_ACCR],"
                objDA.strQuery += " T0.[U_Z_Trans],T0.[U_Z_Adjustment],T0.[U_Z_Balance],T0.[U_Z_BalanceAmt],T0.[U_Z_OB],T0.[U_Z_EnCash],T0.[U_Z_CashOut] "
                objDA.strQuery += " FROM [dbo].[@Z_EMP_LEAVE_BALANCE]  T0 where T0.U_Z_EmpID=" & objen.EmpId & " and T0.[U_Z_Year]='" & Now.Year - 1 & "'"
                objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                objDA.sqlda.Fill(objDA.ds2)
                Return objDA.ds2
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function mainGvbind1(ByVal objen As HomeEN) As DataSet
        Try
            objDA.strQuery = "Select T1.""name"" as ""position"" from OHEM T0 left join OHPS T1 on T0.""position""=T1.""posID"" where ""empID""=" & objen.EmpId & ""
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss4)
            If objDA.dss4.Tables(0).Rows.Count <> 0 Then
                objDA.strQuery = "  select distinct(T0.U_Z_TrainCode),convert(varchar(10),T0.U_Z_DocDate,103) as U_Z_DocDate,T0.U_Z_CourseCode ,T0.U_Z_CourseName,T0.U_Z_CourseTypeDesc,convert(varchar(10),T0.U_Z_Startdt,103) as U_Z_Startdt,convert(varchar(10),T0.U_Z_Enddt,103) as U_Z_Enddt,T0.U_Z_MinAttendees,T0.U_Z_MaxAttendees,convert(varchar(10),T0.U_Z_AppStdt,103) as U_Z_AppStdt,convert(varchar(10),T0.U_Z_AppEnddt,103) as U_Z_AppEnddt,"
                objDA.strQuery += "T4. U_Z_FirstName +''+ T4.U_Z_LastName as U_Z_InsName,T0.U_Z_NoOfHours,T0.U_Z_StartTime,T0.U_Z_EndTime,isnull(T0.U_Z_Sunday,'N') 'U_Z_Sunday',isnull(T0.U_Z_Monday,'N') 'U_Z_Monday',isnull(T0.U_Z_Tuesday,'N') 'U_Z_Tuesday',isnull(T0.U_Z_Wednesday,'N') 'U_Z_Wednesday',isnull(T0.U_Z_Thursday,'N') 'U_Z_Thursday',isnull(T0.U_Z_Friday,'N') 'U_Z_Friday',isnull(T0.U_Z_Saturday,'N') 'U_Z_Saturday',T0.U_Z_AttCost,T0.U_Z_Active  from [@Z_HR_OTRIN] T0 left join [@Z_HR_OCOUR] T1 on T0.U_Z_CourseCode=T1.U_Z_CourseCode left join "
                objDA.strQuery += "  [@Z_HR_COUR4] T2  on T1.DocEntry=t2.DocEntry left join [@Z_HR_TRIN1] T3 on T3.U_Z_CourseCode<>T0.U_Z_CourseCode left join ""@Z_HR_TRRAPP"" T4 on T0.U_Z_InsName=T4.DocEntry where  (isnull(T1.U_Z_Allpos,'N')='Y' or  T2.U_Z_PosCode='" & objDA.dss4.Tables(0).Rows(0)("position").ToString() & "') and T0.U_Z_Active='Y' and isnull(T0.U_Z_Status,'O')='O' and  T0.U_Z_TrainCode not in( select U_Z_TrainCode from [@Z_HR_TRIN1] where U_Z_HREmpID='" & objen.EmpId & "') and GETDATE() between T0.U_Z_AppStdt and T0.U_Z_AppEnddt"
                objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                objDA.sqlda.Fill(objDA.dss1)
                Return objDA.dss1
            End If
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function AppPositions(ByVal objen As HomeEN) As DataSet
        Try
            objDA.strQuery = "Select U_Empid,U_Empname,U_EmpPosCode,U_EmpPosName,U_EmpdeptCode,U_EmpdeptName,U_ReqdeptCode,U_ReqdeptName,U_ReqPosCode,U_Remarks,"
            objDA.strQuery += " U_ReqPosName,U_RequestCode,U_ApplyDate,case U_Z_AppStatus when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end as U_Z_AppStatus from U_VACPOSITION where U_Empid='" & objen.EmpId & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss2)
            Return objDA.dss2
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function VacPositions(ByVal objen As HomeEN) As DataSet
        Try
            objDA.strQuery = "select DocEntry,U_Z_ReqDate,U_Z_DeptCode,U_Z_DeptName,isnull(U_Z_PosName,'') as Position,U_Z_ExpMin,U_Z_ExpMax, U_Z_Vacancy,U_Z_EmpPosi,"
            objDA.strQuery += "convert(varchar(10),U_Z_EmpstDate,103) as U_Z_EmpstDate,convert(varchar(10),U_Z_IntAppDead,103) as U_Z_IntAppDead,convert(varchar(10),U_Z_ExtAppDead,103) as U_Z_ExtAppDead  from [@Z_HR_ORMPREQ] where U_Z_AppStatus='A' and"
            objDA.strQuery += " DocEntry not in(select U_RequestCode from U_VACPOSITION where U_Empid='" & objen.EmpId & "') and U_Z_IntAppDead>=GETDATE()"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss3)
            Return objDA.dss3
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function ApplyPosition(ByVal objen As HomeEN) As Boolean
        Try
            objDA.strQuery = "Insert into U_VACPOSITION(U_Empid,U_Empname,U_EmpPosCode,U_EmpPosName,U_EmpdeptCode,U_EmpdeptName,U_ReqdeptCode,U_ReqdeptName,U_ReqPosCode,U_ReqPosName,U_RequestCode,U_ApplyDate,U_Z_AppStatus )"
            objDA.strQuery += " Values ('" & objen.EmpId & "','" & objen.EmpName & "','" & objen.EmpPosCode & "','" & objen.EmpPosName & "','" & objen.DeptCode & "','" & objen.DeptName & "',"
            objDA.strQuery += " '" & objen.ReqDeptCode & "','" & objen.ReqDeptName & "','" & objen.ReqposCode & "','" & objen.ReqPosName & "','" & objen.RequestNo & "',getdate(),'P')"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            Return True
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
            Return False
        End Try
    End Function
    Public Function LoadActivity(ByVal objEN As HomeEN) As DataSet
        Try
            objDA.strQuery1 = "Select ISNULL(""userId"",'0'),""empID"" from OHEM where ""empID""='" & objEN.EmpId & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery1, objDA.con)
            objDA.sqlda.Fill(objDA.ds3)
            If objDA.ds3.Tables(0).Rows(0)(0).ToString = 0 Then
                objDA.strQuery = "Select T0.""ClgCode"",T0.""U_Z_HREmpID"",T0.""U_Z_HREmpName"",T1.""Name"",case T0.""Action"" when 'T' then 'Task' else 'Other' end as ""Action"",T2.""Name"" as ""Subject"", T3.""firstName"" +' '+ ISNULL(T3.""middleName"",'') +' '+ T3.""lastName"" as ""EmpName"",convert(varchar(10),Recontact,103) as ""Recontact"",T0.""BeginTime"",convert(varchar(10),endDate,103) as ""endDate"",T0.""ENDTime"",T0.""Duration"",T0.""Details"""
                objDA.strQuery += " ,T4.""name"" as ""status"",T5.""U_NAME"" as ""UserName"",case T0.""Priority"" when '0' then 'Low' when '1' then 'Normal' when '2' then 'High' end as ""Priority"" from OCLG T0 left join OCLT T1 on "
                objDA.strQuery += " T0.""CntctType""=T1.""Code"" left join OCLS T2 on T0.""CntctSbjct""=T2.""Code"" left join OHEM T3"
                objDA.strQuery += " on T0.""AttendEmpl""=T3.""empID"" left join OUSR T5 on T0.""AttendUser""=T5.""INTERNAL_K""  left join OCLA T4 on T0.""status""=T4.""statusID"" where T0.""AttendEmpl""='" & objEN.EmpId & "' order by T0.""ClgCode"" desc"
            Else
                objDA.strQuery = "Select T0.""ClgCode"",T0.""U_Z_HREmpID"",T0.""U_Z_HREmpName"",T1.""Name"",case T0.""Action"" when 'T' then 'Task' else 'Other' end as ""Action"",T2.""Name"" as ""Subject"", T3.""firstName"" +' '+ ISNULL(T3.""middleName"",'') +' '+ T3.""lastName"" as ""EmpName"",convert(varchar(10),Recontact,103) as ""Recontact"",T0.""BeginTime"",convert(varchar(10),endDate,103) as ""endDate"",T0.""ENDTime"",T0.""Duration"",T0.""Details"""
                objDA.strQuery += " ,T4.""name"" as ""status"",T5.""U_NAME"" as ""UserName"",case T0.""Priority"" when '0' then 'Low' when '1' then 'Normal' when '2' then 'High' end as ""Priority"" from OCLG T0 left join OCLT T1 on "
                objDA.strQuery += " T0.""CntctType""=T1.""Code"" left join OCLS T2 on T0.""CntctSbjct""=T2.""Code"" left join OHEM T3"
                objDA.strQuery += " on T0.""AttendEmpl""=T3.""empID"" left join OUSR T5 on T0.""AttendUser""=T5.""INTERNAL_K""  left join OCLA T4 on T0.""status""=T4.""statusID"" where (T0.""AttendUser""='" & objDA.ds3.Tables(0).Rows(0)(0).ToString & "' or T0.""AttendEmpl""='" & objEN.EmpId & "') order by T0.""ClgCode"" desc"
            End If
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds4)
            Return objDA.ds4
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function ReturnDocEntry() As String
        Try
            objDA.con.Open()
            objen.StrQry = "select Top 1 U_DocEntry from [U_VACPOSITION] Order by U_DocEntry Desc "
            objDA.cmd = New SqlCommand(objen.StrQry, objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            objen.DeptName = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return objen.DeptName
        Catch ex As Exception
            DBConnectionDA.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
End Class
