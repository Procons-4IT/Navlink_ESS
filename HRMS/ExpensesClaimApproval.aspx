﻿<%@ Page Title="ExpensesClaim Approval" Language="vb" AutoEventWireup="false" MasterPageFile="~/HRMS.Master"
    CodeBehind="ExpensesClaimApproval.aspx.vb" Inherits="HRMS.ExpensesClaimApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Confirmation() {
            if (confirm("You are submitting expense claim for next approval level, do you want to continue?") == true) {
                return true;
            }
            else {
                return false;
            }
        }
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/Images/waiting.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajx:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                <tr>
                    <td height="30" align="left" colspan="2" valign="bottom" background="images/h_bg.png"
                        style="border-bottom: 1px dotted; border-color: #f45501; background-repeat: repeat-x">
                        <div>
                            &nbsp;
                            <asp:Label ID="Label3" runat="server" Text="Expenses Claim Approval" CssClass="subheader"
                                Style="float: left;"></asp:Label>
                            <span>
                                <asp:Label ID="lblNewTrip" runat="server" Text="" Visible="false"></asp:Label></span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                            <tr>
                                <td>
                                    <asp:Panel ID="panelhome" runat="server" Width="100%">
                                        <asp:ImageButton ID="btnhome" runat="server" ImageUrl="~/images/Homeicon.jpg" PostBackUrl="~/Home.aspx"
                                            ToolTip="Home" />
                                    </asp:Panel>
                                    <asp:Panel ID="PanelMain" runat="server" Width="100%" BorderColor="LightSteelBlue"
                                        BorderWidth="2">
                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                            <tr>
                                                <td valign="top">
                                                    <ajx:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                        Width="100%">
                                                        <ajx:TabPanel ID="TabPanel2" runat="server" HeaderText="Expenses Claim Approval">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="GrdLoadRequest" runat="server" CellPadding="4" AllowPaging="True"
                                                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                                AutoGenerateColumns="false" Width="100%" PageSize="15">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Request Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:LinkButton ID="lblRCode" runat="server" Text='<%#Bind("Code") %>' OnClick="lnbtnlblRCode_Click">
                                                                                                </asp:LinkButton>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Badge No" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblRtaEmpid" runat="server" Text='<%#Bind("U_Z_TAEmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Employee Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblREmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Emp.Name">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblREmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Submitted Date">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRSubDt" runat="server" Text='<%#Bind("U_Z_Subdt") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Client">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRClient" runat="server" Text='<%#Bind("U_Z_Client") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Project">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRProject" runat="server" Text='<%#Bind("U_Z_Project") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Status">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRDocStatus" runat="server" Text='<%#Bind("U_Z_DocStatus") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </ajx:TabPanel>
                                                        <ajx:TabPanel ID="TabPanel4" runat="server" HeaderText="Expenses Claim Summary">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                  <%--  <tr>
                                                                        <td valign="top" align="right">
                                                                            <div style="width:auto;">
                                                                                <div style="border:solid 0px red; height:30px;">
                                                                                   <a style="margin-right:50px; margin-top:20px;">Document Status     </a> 
                                                                                     <asp:DropDownList ID="ddlfildocStatus" runat="server" style="float:right;" CssClass="txtbox1" AutoPostBack="true">
                                                                                        <asp:ListItem Value="O">Open</asp:ListItem>
                                                                                        <asp:ListItem Value="C">Closed</asp:ListItem>                                                                                        
                                                                                    </asp:DropDownList>                                                                              
                                                                                </div>
                                                                              
                                                                            </div>
                                                                        </td>
                                                                    </tr>--%>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="grdSummaryLoad" runat="server" CellPadding="4" AllowPaging="True"
                                                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                                AutoGenerateColumns="false" Width="100%" PageSize="15">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Request Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:LinkButton ID="lblSCode" runat="server" Text='<%#Bind("Code") %>' OnClick="lnbtnlblSCode_Click">
                                                                                                </asp:LinkButton>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Badge No" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblStaEmpid" runat="server" Text='<%#Bind("U_Z_TAEmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Employee Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                <asp:Label ID="lblSEmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Emp.Name">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                <asp:Label ID="lblSEmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Submitted Date">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSSubDt" runat="server" Text='<%#Bind("U_Z_Subdt") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Client">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSClient" runat="server" Text='<%#Bind("U_Z_Client") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Project">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSProject" runat="server" Text='<%#Bind("U_Z_Project") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Status">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSDocStatus" runat="server" Text='<%#Bind("U_Z_DocStatus") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                     <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="imgSPrint" runat="server" Height="20" 
                                                                                                ImageUrl="~/Images/Print.png" OnClick="imgSPrint_Click" Text="Edit" 
                                                                                                ToolTip="Print" Width="20" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="lbtnHistory" runat="server" Text="Add New Expenses" Style="display: none;" />
                                                                            <ajx:ModalPopupExtender ID="ModalPopupExtender6" runat="server" DropShadow="false"
                                                                                PopupControlID="PanelItemEntry" TargetControlID="lbtnHistory" CancelControlID="btnclstech2"
                                                                                BackgroundCssClass="modalBackground">
                                                                            </ajx:ModalPopupExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a class="txtbox" style="text-decoration: underline; font-weight: bold;">Expenses Details</a>
                                                                            <br />
                                                                            <div style="overflow-x: scroll; width: 1100px;">
                                                                                <asp:GridView ID="grdSummary" runat="server" CellPadding="4" ShowHeaderWhenEmpty="true"
                                                                                    ShowFooter="true" EmptyDataText="No Records Found" CssClass="mGrid" HeaderStyle-CssClass="GridBG"
                                                                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false"
                                                                                    Width="100%">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Code">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:LinkButton ID="lblSCode" runat="server" Text='<%#Bind("Code") %>' OnClick="lbtnlblSCode_Click">
                                                                                                    </asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="T&A Emp.Code" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblStaEmpid" runat="server" Text='<%#Bind("TAEmpID") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Code" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblsEmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Name" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblsEmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Submitted Date" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsAgenda" runat="server" Text='<%#Bind("U_Z_SubDt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Client" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsstdate" runat="server" Text='<%#Bind("U_Z_Client") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Project" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblseddate" runat="server" Text='<%#Bind("U_Z_Project") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Trip Type" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblstriptype" runat="server" Text='<%#Bind("U_Z_TripType") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Travel Description" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsTraDesc" runat="server" Text='<%#Bind("U_Z_TraDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Transaction Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsCouCode" runat="server" Text='<%#Bind("U_Z_Claimdt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Expense Type">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsCouName" runat="server" Text='<%#Bind("U_Z_ExpType") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Transaction Currency">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblsCouType" runat="server" Text='<%#Bind("U_Z_Currency") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <div align="right">
                                                                                                    <asp:Label ID="lblSCur" runat="server" Text="Total :"></asp:Label>&nbsp;</div>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Transaction Amount">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblstraamt" runat="server" Text='<%#Bind("U_Z_CurAmt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <div align="right">
                                                                                                    <asp:Label ID="lblSCurTotal" runat="server"></asp:Label>&nbsp;</div>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Exchange Rate">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblsExcdamt" runat="server" Text='<%#Bind("U_Z_ExcRate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Local Currency Amount">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblsusdamt" runat="server" Text='<%#Bind("U_Z_UsdAmt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <div align="right">
                                                                                                    <asp:Label ID="lblsLocCurTotal" runat="server"></asp:Label>&nbsp;</div>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Redim Amount">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblssredim" runat="server" Text='<%#Bind("U_Z_ReimAmt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Notes">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblsnotes" runat="server" Text='<%#Bind("U_Z_Notes") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Attachments">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;
                                                                                                    <asp:LinkButton ID="SlnkDownload" Text="Download" CommandArgument='<%# Eval("U_Z_Attachment") %>'
                                                                                                        runat="server" OnClick="SlnkDownload_Click" Width="100px"></asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Payroll Month">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblspaymonth" runat="server" Text='<%#Bind("U_Z_Month") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Payroll Year">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblspayyear" runat="server" Text='<%#Bind("U_Z_Year") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsAppStatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Required" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsAppreq" runat="server" Text='<%#Bind("U_Z_AppRequired") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsreqdt" runat="server" Text='<%#Bind("U_Z_AppReqDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Time" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsreqtime" runat="server" Text='<%#Bind("U_Z_ReqTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Current Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblscurapp" runat="server" Text='<%#Bind("U_Z_CurApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Next Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsnxtapp" runat="server" Text='<%#Bind("U_Z_NxtApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        
                                                                                    </Columns>
                                                                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                                                                    <FooterStyle Height="25px" BackColor="#CCCCCC" Font-Bold="true" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </ajx:TabPanel>
                                                    </ajx:TabContainer>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panelview" runat="server" Width="100%" BorderColor="LightSteelBlue"
                                        BorderWidth="2">
                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                            <tr>
                                                <td>
                                                    <div id="Div1" runat="server" class="DivCorner" style="border: solid 2px LightSteelBlue;
                                                        width: 100%;">
                                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                                            <tr>
                                                                <td width="10%">
                                                                    Document Number
                                                                </td>
                                                                <td width="15%">
                                                                    <asp:Label ID="lbldocno" CssClass="txtbox" Width="200px" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Date Submitted
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblsubdt" CssClass="txtbox" Width="200px" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="10%">
                                                                    Employee Code
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblempNo" CssClass="txtbox" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Employee Name
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblempname" CssClass="txtbox" runat="server" Width="150px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Trip Type
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTripType" CssClass="txtbox" Width="150px" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Travel Description
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblTraDesc" CssClass="txtbox" Width="150px" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Client
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblClient" CssClass="txtbox" Width="150px" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Project
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblProject" CssClass="txtbox" Width="150px" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="10%">
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblTANo" CssClass="txtbox" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Document Status
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblDocStatus" CssClass="txtbox" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <ajx:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                        Width="100%">
                                                        <ajx:TabPanel ID="TabPanel3" runat="server" HeaderText="Expenses Claim Approval">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="overflow-x: scroll; width: 1100px;">
                                                                                <asp:GridView ID="grdRequestApproval" runat="server" CellPadding="4" ShowFooter="true"
                                                                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                    AutoGenerateColumns="False" Width="100%">
                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Expense Claim No." Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:LinkButton ID="lblRefCode" runat="server" Text='<%#Bind("U_Z_DocRefNo") %>'>
                                                                                                    </asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Serial No.">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:LinkButton ID="lblCode" runat="server" OnClick="lbtnlblCode_Click" Text='<%#Bind("Code") %>'>
                                                                                                    </asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Code" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblEmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Name" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblEmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Submitted Date" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblAgenda" runat="server" Text='<%#Bind("U_Z_SubDt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Trip Type" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lbltriptype" runat="server" Text='<%#Bind("U_Z_TripType") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Travel Description" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblTraDesc" runat="server" Text='<%#Bind("U_Z_TraDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Transaction Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lbltransdt" runat="server" Text='<%#Bind("U_Z_Claimdt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Expense Type">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblCouName" runat="server" Text='<%#Bind("U_Z_ExpType") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Client" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblstdate" runat="server" Text='<%#Bind("U_Z_Client") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Project" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lbleddate" runat="server" Text='<%#Bind("U_Z_Project") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Transaction Currency">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblCouType" runat="server" Text='<%#Bind("U_Z_Currency") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <div align="right">
                                                                                                    <asp:Label ID="lblCur" runat="server" Text="Total :"></asp:Label>&nbsp;</div>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Transaction Amount">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lbltraamt" runat="server" Text='<%#Bind("U_Z_CurAmt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <div align="right">
                                                                                                    <asp:Label ID="lblCurTotal" runat="server"></asp:Label>&nbsp;</div>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Exchange Rate">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblExcdamt" runat="server" Text='<%#Bind("U_Z_ExcRate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Local Currency Amount">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblusdamt" runat="server" Text='<%#Bind("U_Z_UsdAmt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <div align="right">
                                                                                                    <asp:Label ID="lblLocCurTotal" runat="server"></asp:Label>&nbsp;</div>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="To be Reimbursed?">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblreimburse" runat="server" Text='<%#Bind("U_Z_Reimburse") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Reimbursement Amount">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblredim" runat="server" Text='<%#Bind("U_Z_ReimAmt") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Notes">
                                                                                            <ItemTemplate>
                                                                                                <div align="right">
                                                                                                    &nbsp;<asp:Label ID="lblnotes" runat="server" Text='<%#Bind("U_Z_Notes") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Attachments">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;
                                                                                                    <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("U_Z_Attachment") %>'
                                                                                                        OnClick="lnkDownload_Click" Text="Download" Width="100px"></asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:DropDownList ID="ddlAppStatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'>
                                                                                                        <asp:ListItem Value="P">Pending</asp:ListItem>
                                                                                                        <asp:ListItem Value="A">Approved</asp:ListItem>
                                                                                                        <asp:ListItem Value="R">Rejected</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Payroll Month">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:DropDownList ID="ddlpaymonth" runat="server" DataValueField='<%# Bind("U_Z_Month") %>'>
                                                                                                    </asp:DropDownList>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Payroll Year">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:DropDownList ID="ddlpayyear" runat="server" DataValueField='<%# Bind("U_Z_Year") %>'>
                                                                                                    </asp:DropDownList>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Remarks">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("U_Z_Remarks") %>'></asp:TextBox>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Required" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblAppreq" runat="server" Text='<%#Bind("U_Z_AppRequired") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblreqdt" runat="server" Text='<%#Bind("U_Z_AppReqDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Time" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblreqtime" runat="server" Text='<%#Bind("U_Z_ReqTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Current Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblcurapp" runat="server" Text='<%#Bind("U_Z_CurApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Next Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblnxtapp" runat="server" Text='<%#Bind("U_Z_NxtApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Posting Type">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblPostType" runat="server" Text='<%#Bind("U_Z_Posting") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" CssClass="GridBG" />
                                                                                    <FooterStyle Height="25px" BackColor="#CCCCCC" CssClass="GridBG" Font-Bold="true" />
                                                                                    <PagerStyle CssClass="pgr" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <div style="overflow-x: scroll; width: 700px;">
                                                                                <a class="txtbox" style="text-decoration: underline; font-weight: bold;">Approval History</a>
                                                                                <asp:GridView ID="grdApprovalHis" runat="server" CellPadding="4" AllowPaging="True"
                                                                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                    AutoGenerateColumns="False" Width="100%">
                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Request Code" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblHDocNo" runat="server" Text='<%#Bind("DocEntry") %>'>
                                                                                                    </asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Reference No" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblhrefno" runat="server" Text='<%#Bind("U_Z_DocEntry") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DocType" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblhdoctype" runat="server" Text='<%#Bind("U_Z_DocType") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Employee ID">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhempid" runat="server" Text='<%#Bind("U_Z_EmpId") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Employee Name">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhempname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approved By">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhAppby" runat="server" Text='<%#Bind("U_Z_ApproveBy") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approved Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhcrdate" runat="server" Text='<%#Bind("CreateDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approved Time">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhcrtime" runat="server" Text='<%#Bind("CreateTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Update Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhupdate" runat="server" Text='<%#Bind("UpdateDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Update Time">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhuptime" runat="server" Text='<%#Bind("UpdateTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approved Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhAppstatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Remarks">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhremarks" runat="server" Text='<%#Bind("U_Z_Remarks") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" CssClass="GridBG" />
                                                                                    <PagerStyle CssClass="pgr" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                                <tr>
                                                                                    <td>
                                                                                        Approval Status(All Transactions)
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlAppStatus" runat="server" CssClass="txtbox1" AutoPostBack="True">
                                                                                            <asp:ListItem Value="P">Pending</asp:ListItem>
                                                                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                                                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Document Status
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlfinaldoc" runat="server" CssClass="txtbox1">
                                                                                            <asp:ListItem Value="O">Open</asp:ListItem>
                                                                                            <asp:ListItem Value="C">Closed</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <br />
                                                                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn" OnClientClick="return Confirmation();"
                                                                                            Text="Save &amp; Submit" Width="125px" />
                                                                                        <asp:Button ID="btncancel" runat="server" CssClass="btn" Text="Cancel" Width="85px" />
                                                                                    </td>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </ajx:TabPanel>
                                                    </ajx:TabContainer>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PanelItemEntry" runat="server" BackColor="White" Style="display: none;
                            padding: 10px; width: 1000px;">
                            <div>
                                <span style="float: right;">
                                    <asp:Button ID="btnclstech2" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <br />
                            <asp:Panel ID="Panel3" runat="server" Height="400px">
                                <div style="overflow-y: scroll;">
                                    <asp:Panel ID="Panel1" runat="server" Width="100%">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                            <tr>
                                                <td>
                                                    <a class="txtbox" style="text-decoration: underline; font-weight: bold;">Approval History</a>
                                                    <br />
                                                    <asp:GridView ID="grdHistorySummary" runat="server" CellPadding="4" AllowPaging="True"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="mGrid"
                                                        HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                        AutoGenerateColumns="false" Width="100%" PageSize="10">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Request Code" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="center">
                                                                        <asp:Label ID="lblDocNo" runat="server" Text='<%#Bind("DocEntry") %>'>
                                                                        </asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reference No" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="center">
                                                                        <asp:Label ID="lblrefno" runat="server" Text='<%#Bind("U_Z_DocEntry") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DocType" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="center">
                                                                        <asp:Label ID="lbldoctype" runat="server" Text='<%#Bind("U_Z_DocType") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee ID">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblempid" runat="server" Text='<%#Bind("U_Z_EmpId") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee Name">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblempname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved By">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblAppby" runat="server" Text='<%#Bind("U_Z_ApproveBy") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved Date">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblcrdate" runat="server" Text='<%#Bind("CreateDate") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved Time">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblcrtime" runat="server" Text='<%#Bind("CreateTime") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Update Date">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblupdate" runat="server" Text='<%#Bind("UpdateDate") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Update Time">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lbluptime" runat="server" Text='<%#Bind("UpdateTime") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved Status">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblAppstatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblremarks" runat="server" Text='<%#Bind("U_Z_Remarks") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="TabContainer2$TabPanel3$btnAdd" />
            <asp:PostBackTrigger ControlID="TabContainer2$TabPanel3$btnCancel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
