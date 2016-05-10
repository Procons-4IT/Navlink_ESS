﻿<%@ Page Title="Claim Request" Language="vb" AutoEventWireup="false" MasterPageFile="~/HRMS.Master" CodeBehind="ClaimRequest.aspx.vb" Inherits="HRMS.ClaimRequest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Confirmation() {
            if (confirm("Do you want to confirm?") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function Confirmation1() {
            if (confirm("Sure Want to withdraw the request?") == true) {
                return true;
            }
            else {
                return false;
            }
        }


        function AllowNumbers(el) {
            var ex = /^[0-9.]+$/;
            if (ex.test(el.value) == false) {
                el.value = el.value.substring(0, el.value.length - 1);
            }
        }
        function alphanumerichypen(el) {
            var ex = /^[A-Za-z0-9_-]+$/;
            if (ex.test(el.value) == false) {
                el.value = el.value.substring(0, el.value.length - 1);
            }
        }

        function checkDec(el) {
            el.value = el.value.replace(/^[ 0]+/, '');
            var ex = /^\d*\.?\d{0,2}$/;
            if (ex.test(el.value) == false) {
                el.value = el.value.substring(0, el.value.length - 1);
            }
        }

        function RemoveZero(el) {
            el.value = el.value.replace(/^[ 0]+/, '');
        }

        function popupdisplay(option, uniqueid, DocNo) {
            if (uniqueid.length > 0) {
                var uniid = document.getElementById("<%=txtpopunique.ClientID%>").value;
                var tno = document.getElementById("<%=txtpoptno.ClientID%>").value;
                var opt = document.getElementById("<%=txthidoption.ClientID%>").value;
                uniid = ""; tno = ""; opt = "";
                if (uniid != uniqueid && tno != DocNo && opt != option) {
                    document.getElementById("<%=txtpopunique.ClientID%>").value = uniqueid;
                    document.getElementById("<%=txtpoptno.ClientID%>").value = DocNo;
                    document.getElementById("<%=txthidoption.ClientID%>").value = option;
                    document.getElementById("<%=Btncallpop.ClientID%>").onclick();
                }
            }
        }
   </script>

    <style type="text/css" >
.modalPopup
{
background-color: #696969;
filter: alpha(opacity=40);
opacity: 0.7;
xindex:-1;
}
</style>

<style type="text/css">
/*Calendar Control CSS*/
.cal_Theme1 .ajax__calendar_container   {
background-color: #DEF1F4;
border:solid 1px #77D5F7;
}

.cal_Theme1 .ajax__calendar_header  {
background-color: #ffffff;
margin-bottom: 4px;
}

.cal_Theme1 .ajax__calendar_title,
.cal_Theme1 .ajax__calendar_next,
.cal_Theme1 .ajax__calendar_prev    {
color: #004080;
padding-top: 3px;
}

.cal_Theme1 .ajax__calendar_body    {
background-color: #ffffff;
border: solid 1px #77D5F7;
}

.cal_Theme1 .ajax__calendar_dayname {
text-align:center;
font-weight:bold;
margin-bottom: 4px;
margin-top: 2px;
color: #004080;
}

.cal_Theme1 .ajax__calendar_day {
color: #004080;
text-align:center;
}

.cal_Theme1 .ajax__calendar_hover .ajax__calendar_day,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_month,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_year,
.cal_Theme1 .ajax__calendar_active  {
color: #004080;
font-weight: bold;
background-color: #DEF1F4;
}

.cal_Theme1 .ajax__calendar_today   {
font-weight:bold;
}

.cal_Theme1 .ajax__calendar_other,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_today,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
color: #bbbbbb;
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
<ProgressTemplate>
<asp:Image ID="Image1" ImageUrl="../Images/waiting.gif" AlternateText="Processing" runat="server" />
</ProgressTemplate>
</asp:UpdateProgress>
<ajx:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress" PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
<%--<asp:UpdatePanel ID="Update" runat="server">
<ContentTemplate>--%>

<table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
  <tr>
 <td>
     <asp:TextBox ID="txtHEmpID" runat="server" Width="93px"  Style="display: none"></asp:TextBox>
   <asp:TextBox ID="txtpopunique" runat="server" Style="display: none"></asp:TextBox>
    <asp:TextBox ID="txtpoptno" runat="server" Style="display: none"></asp:TextBox>
    <asp:TextBox ID="txthidoption" runat="server" Style="display: none"></asp:TextBox>
     <asp:TextBox ID="txttname" runat="server" Style="display: none"></asp:TextBox>
    <input id="Btncallpop" runat="server" onserverclick="Btncallpop_ServerClick" style="display: none" type="button" value="button" />

 </td>
 </tr>

 <tr>

    <td height="30" align="left" colspan="2" valign="bottom" background="images/h_bg.png"; style="border-bottom:1px dotted; border-color: #f45501; background-repeat:repeat-x">     
      <div >&nbsp; <asp:Label ID="Label3" runat="server" Text="Expenses Claim Request"  CssClass="subheader" style="float:left;" ></asp:Label>  <span ><asp:Label ID="lblNewTrip" runat="server" Text="" Visible="false"></asp:Label></span> </div>
      </td>    
 </tr>

 <tr>      
      <td align="center">
        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
        <tr>
        <td>
          <asp:Panel ID="panelhome" runat="server" Width="100%"> 
            <asp:ImageButton ID="btnhome" runat="server" ImageUrl="~/images/Homeicon.jpg"  PostBackUrl="~/Home.aspx"
                    ToolTip="Home" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="btnnew" runat="server" ImageUrl="~/images/Add.jpg" ToolTip="Add new record" />
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        
            </asp:Panel> 
            <asp:Label ID="Label2" runat="server" Text="" style="color:Red;"></asp:Label>
              <asp:Panel ID="panelview" runat="server" Width="100%" BorderColor="LightSteelBlue"  BorderWidth="2">
            <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
              <tr>
              <td valign="top">  
                <ajx:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0"  CssClass="ajax__tab_yuitabview-theme" Width="100%" >
                  <ajx:TabPanel  ID="TabPanel3" runat="server" HeaderText="Expenses Claim Request" ><ContentTemplate>
                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                              <tr>
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <td>
                                      <asp:GridView ID="grdExpClaimRequest" runat="server" CellPadding="4" AllowPaging="True" ShowHeaderWhenEmpty="true"  CssClass="mGrid" HeaderStyle-Cssclass="GridBG" PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false" Width="100%" PageSize="15" >
                                          <Columns>
                                              <asp:TemplateField HeaderText="Request Code">
                                                  <ItemTemplate>
                                                      <div align="center">
                                                          <asp:LinkButton ID="lbtndocnum" runat="server" Text='<%#Bind("Code") %>' onclick="lbtndocnum_Click">
                                                  </asp:LinkButton>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Trip Type">
                                                  <ItemTemplate>
                                                      <div align="center">
                                                          <asp:label ID="lblTripType" runat="server" Text='<%#Bind("U_Z_TripType") %>'  ></asp:label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Claim Date">
                                                  <ItemTemplate>
                                                      <div align="center">
                                                          <asp:label ID="lblcldate" runat="server" Text='<%#Bind("U_Z_Claimdt") %>'  ></asp:label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Travel Code">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblTraCode" runat="server" Text='<%#Bind("U_Z_TraCode") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Travel Description">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblTraName" runat="server" Text='<%#Bind("U_Z_TraDesc") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expense Type">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblexptype" runat="server" Text='<%#Bind("U_Z_ExpType") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Transaction Currency">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblTraCur" runat="server" Text='<%#Bind("U_Z_Currency") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Transaction Amount">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lbltramt" runat="server" Text='<%#Bind("U_Z_CurAmt") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Exchange Rate">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblexrate" runat="server" Text='<%#Bind("U_Z_ExcRate") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Local Currency Amount">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblLocCur" runat="server" Text='<%#Bind("U_Z_UsdAmt") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="To be Reimbursed?">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblreimb" runat="server" Text='<%#Bind("U_Z_Reimburse") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reimbursement Amount">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblreimbamt" runat="server" Text='<%#Bind("U_Z_ReimAmt") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Comments">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblreason" runat="server" Text='<%#Bind("U_Z_Notes") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Attachment">
                                                    <ItemTemplate>
                                                     <asp:LinkButton ID="lnkDownload" Text = "Download" CommandArgument = '<%# Eval("U_Z_Attachment") %>' runat="server" OnClick="lnkDownload_Click" Width="100px"></asp:LinkButton>
                                                   </ItemTemplate>                                    
                                                </asp:TemplateField>  
                                              <asp:TemplateField HeaderText="Approval Status">
                                                  <ItemTemplate>
                                                      <div align="left">
                                                          &nbsp;<asp:Label ID="lblstatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>' ></asp:Label>
                                                      </div>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Approval History">
                                                    <ItemTemplate>
                                                        <div align="left">&nbsp;<asp:LinkButton ID="lbtAppHistory" runat="server" Text="View" onclick="lbtAppHistory_Click" ></asp:LinkButton></div>
                                                    </ItemTemplate>                                    
                                                </asp:TemplateField>
                                          </Columns>
                                          <HeaderStyle HorizontalAlign="Center" height="25px" BackColor="#CCCCCC"/>
                                      </asp:GridView>
                                  </td>
                              </tr>
                          </table>
                      </ContentTemplate></ajx:TabPanel>                
                  
                    
                 </ajx:TabContainer>   
              
              </td> 
              </tr> 
              </table> 
          </asp:Panel> 
      
            <asp:Panel ID="PanelNewRequest"  runat="server" Width="100%" BorderColor="LightSteelBlue"  BorderWidth="2">
                           <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content"> 
                                <tr>
                                 <td >Request Code</td>
                                     <td>                                     
                                       <asp:TextBox ID="txtreqcode" CssClass="txtbox" runat="server" Enabled="False" ></asp:TextBox>    
                                                                                    
                                     </td>  
                                       <td>Request Date</td>
                                     <td>
                                     <asp:TextBox ID="txtreqdate" CssClass="txtbox"  runat="server"  Enabled="False" ></asp:TextBox>                                           
                                     </td>
                                     </tr>

                                      <tr>
                                    <td >Employee Code</td>
                                     <td>                                     
                                       <asp:TextBox ID="txtempid" CssClass="txtbox" runat="server" Enabled="False" ></asp:TextBox>                                                
                                     </td>  
                                       <td>Employee Name</td>
                                     <td>
                                     <asp:TextBox ID="txtempname" CssClass="txtbox"  runat="server"  Enabled="False" ></asp:TextBox>                                           
                                     </td>
                                     </tr>

                                      <tr>
                                    <td >Trip Type</td>
                                     <td>                                     
                                         <asp:DropDownList ID="ddltriptype" runat="server" CssClass="txtbox1" AutoPostBack="true">
                                           <asp:ListItem Value="-">---Select---</asp:ListItem>
                                         <asp:ListItem Value="N">New</asp:ListItem>
                                          <asp:ListItem Value="E">Existing</asp:ListItem>
                                         </asp:DropDownList>                                              
                                     </td>  
                                       <td>Travel Description (*)</td>
                                     <td>
                                     <asp:TextBox ID="txttravelCode" CssClass="txtbox"  runat="server"  Visible="False" ></asp:TextBox> 
                                      <asp:TextBox ID="txttraveldesc" CssClass="txtbox"  runat="server"  Enabled="False" ></asp:TextBox>   
                                       <asp:ImageButton ID="btnfindtrcode" runat="server" Text="Find" ImageUrl="~/images/search.jpg" />
                  <ajx:ModalPopupExtender ID="ModalPopupExtender7" runat="server" DropShadow="True" PopupControlID="Panelpoptechnician" TargetControlID="btnfindtrcode" CancelControlID="btnclstech" BackgroundCssClass="modalBackground">
                    </ajx:ModalPopupExtender>                                           
                                     </td>
                                     </tr>

                                      <tr>
                                    <td >Expenses Type (*)</td>
                                     <td>                                     
                                       <asp:TextBox ID="txtexpcode" CssClass="txtbox" runat="server" Visible="false" ></asp:TextBox>  
                                       <asp:TextBox ID="txtexptype" CssClass="txtbox" runat="server" Enabled="false" ></asp:TextBox> 
                                        <asp:ImageButton ID="btnfindexp" runat="server" Text="Find" ImageUrl="~/images/search.jpg" />
                  <ajx:ModalPopupExtender ID="ModalPopupExtender1" runat="server" DropShadow="True" PopupControlID="panemlpopexp" TargetControlID="btnfindexp" CancelControlID="btnclstech1" BackgroundCssClass="modalBackground">
                    </ajx:ModalPopupExtender>                                               
                                     </td>  
                                       <td>Allowance Code</td>
                                     <td>
                                     <asp:TextBox ID="txtAllowance" CssClass="txtbox"  runat="server" Enabled="false" ></asp:TextBox>                                           
                                     </td>
                                     </tr>
                                      <tr>
                                    <td >Client</td>
                                     <td>                                     
                                       <asp:TextBox ID="txtClient" CssClass="txtbox" runat="server" ></asp:TextBox>  
                                                                                   
                                     </td>  
                                       <td>Project</td>
                                     <td>
                                     <asp:TextBox ID="txtproject" CssClass="txtbox"  runat="server"></asp:TextBox>                                           
                                     </td>
                                     </tr>

                                      <tr>
                                      <td >Transaction Date (*)</td>
                                     <td>
                                      <asp:TextBox ID="txttrasdt" CssClass="txtbox" runat="server"></asp:TextBox> 
                                       <ajx:CalendarExtender ID="CalendarExtender2" Animated="true" Format="dd/MM/yyyy" runat="server" TargetControlID="txttrasdt" CssClass= "cal_Theme1"></ajx:CalendarExtender>                                             
                                     </td>
                                      <td >City</td>
                                     <td>
                                      <asp:TextBox ID="txtcity" CssClass="txtbox"  runat="server" ></asp:TextBox>  
                                     </td>
                                     </tr>


                                      <tr>
                                    <td >Transaction Currency (*)</td>
                                     <td>                                     
                                         <asp:DropDownList ID="ddltranscur" runat="server" CssClass="txtbox1">
                                         </asp:DropDownList>
                                     </td>  
                                       <td>Transaction Amount (*)</td>
                                     <td>
                                     <asp:TextBox ID="txttrasamt" CssClass="txtbox"  runat="server" AutoPostBack="true" onkeypress="AllowNumbers(this);checkDec(this);RemoveZero(this);" onkeyup="AllowNumbers(this);checkDec(this);RemoveZero(this);"></asp:TextBox>                                           
                                     </td>
                                     </tr>      
                                     
                                       <tr>
                                    <td >Exchange Rate</td>
                                     <td>                                     
                                         <asp:TextBox ID="txtexrate" CssClass="txtbox"  runat="server" AutoPostBack="true" onkeypress="AllowNumbers(this);checkDec(this);RemoveZero(this);" onkeyup="AllowNumbers(this);checkDec(this);RemoveZero(this);"></asp:TextBox> 
                                     </td>  
                                       <td>Local Currency Amount</td>
                                     <td>
                                     <asp:TextBox ID="txtlocamt" CssClass="txtbox"  runat="server" Enabled="false"></asp:TextBox>                                           
                                     </td>
                                     </tr>                                    
                                     
                                    
                                        <tr>
                                       <td >To be Reimbursed?</td>
                                     <td>
                                    <asp:DropDownList ID="ddlreimbused" runat="server" CssClass="txtbox1" AutoPostBack="true">
                                          <asp:ListItem Value="N">No</asp:ListItem>
                                          <asp:ListItem Value="Y">Yes</asp:ListItem>
                                         </asp:DropDownList>                                              
                                     </td>
                                     <td>Reimbursement Amount</td>
                                     <td>
                                     <asp:TextBox ID="txtreimbuse" CssClass="txtbox"  runat="server" Enabled="false"></asp:TextBox>                                           
                                     </td>
                                      </tr>
                                         <tr>
                                       <td >Payment Method</td>
                                     <td>
                                    <asp:DropDownList ID="ddlpaymethod" runat="server" CssClass="txtbox1">
                                         
                                         </asp:DropDownList>                                              
                                     </td>
                                     <td>Notes</td>
                                     <td>
                                     <asp:TextBox ID="txtnotes" CssClass="txtbox"  runat="server"></asp:TextBox>                                           
                                     </td>
                                      </tr>
                                      <tr>
                                      <td >Attachment</td>
                                     <td>
                                         <asp:FileUpload ID="fileattach" runat="server" />
                                     </td>
                                      <td >Status</td>
                                      <td> 
                                       <asp:DropDownList ID="ddlStatus" runat="server" CssClass="txtbox1" Enabled="false">
                                          <asp:ListItem Value="P">Pending</asp:ListItem>
                                          <asp:ListItem Value="A">Approved</asp:ListItem>
                                          <asp:ListItem Value="R">Rejected</asp:ListItem>
                                         </asp:DropDownList>                                           
                                      </td>
                                     </tr>
                                                                     
                                    <tr>
                <td colspan="2" align="center">
                <br />
                    <asp:Button ID="btnAdd"  CssClass="btn" Width="85px" runat="server" Text="Save"  OnClientClick="return Confirmation();" /> 
                   <asp:Button ID="btncancel"  CssClass="btn" Width="85px" runat="server" Text="Cancel" />
                    <asp:Button ID="btnWithdraw"  CssClass="btn"  runat="server" Text="WithDraw Request" Visible="false" OnClientClick="return Confirmation1();" /> 
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
                            <asp:Panel ID="Panelpoptechnician" runat="server" BackColor="White" style=" display:none; padding:10px; width:500px; ">
                                <div><span class="sideheading" style="color:Green;">Travel Details</span> <span style="float:right;"> 
                                <asp:Button ID="btnclstech" runat="server"  CssClass="btn" Width="30px" Text="X" /></span></div>
                                   <br />
                          <%--  <div><span><asp:Label ID="lblname1" runat="server" Text="Travel Code"></asp:Label></span>
                              <asp:TextBox ID="txtpoptraincode" runat="server"></asp:TextBox><br />
                              <span><asp:Label ID="lblname2" runat="server" Text="Travel Description"></asp:Label></span>
                              <asp:TextBox ID="txtpopcouname" runat="server"></asp:TextBox>
                              <asp:Button ID="btnpopemp1" runat="server" Text="Go"  CssClass="btn" Width="65px" /> 
                            <asp:LinkButton runat="server" id="LnkViewall">View All</asp:LinkButton>
                               <br />
                              </div>--%>
                              <br /><asp:Panel ID="Panel4" runat="server" Height="200px" ScrollBars="Vertical">
                                  <asp:GridView ID="grdtravel" runat="server" CellPadding="4" RowStyle-CssClass="mousecursor" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" CssClass="mGrid" HeaderStyle-Cssclass="GridBG"  
                                    PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt" 
                                        AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Travel Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblltracode" runat="server" Text='<%#Bind("U_Z_TraCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Travel Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblltraname" runat="server" Text='<%#Bind("U_Z_TraName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                                                          
                                    </Columns>
                                    <HeaderStyle BackColor="Gray" HorizontalAlign="Center" ForeColor="White" Height="25px" />
                                      <RowStyle HorizontalAlign="Center" />
                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                  </asp:GridView></asp:Panel>
                            </asp:Panel>
            
                          </td>
                          </tr>    

                            <tr>
                          <td>
                            <asp:Panel ID="panemlpopexp" runat="server" BackColor="White" style=" display:none; padding:10px; width:500px; ">
                                <div><span class="sideheading" style="color:Green;">Expenses Details</span> <span style="float:right;"> 
                                <asp:Button ID="btnclstech1" runat="server"  CssClass="btn" Width="30px" Text="X" /></span></div>
                                   <br />
                           <%-- <div><span><asp:Label ID="Label1" runat="server" Text="Expenses Code"></asp:Label></span>
                              <asp:TextBox ID="txtpopexpcode" runat="server"></asp:TextBox><br />
                              <span><asp:Label ID="Label4" runat="server" Text="Expenses Name"></asp:Label></span>
                              <asp:TextBox ID="txtpopexpname" runat="server"></asp:TextBox>
                              <asp:Button ID="btngoexp" runat="server" Text="Go"  CssClass="btn" Width="65px" /> 
                            <asp:LinkButton runat="server" id="lntexpenses">View All</asp:LinkButton>
                               <br />
                              </div>--%>
                              <br /><asp:Panel ID="Panel2" runat="server" Height="200px" ScrollBars="Vertical">
                                  <asp:GridView ID="grdexpenses" runat="server" CellPadding="4" RowStyle-CssClass="mousecursor" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" CssClass="mGrid" HeaderStyle-Cssclass="GridBG"  
                                    PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt" 
                                        AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Expenses Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllexpcode" runat="server" Text='<%#Bind("Code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expenses Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllexpname" runat="server" Text='<%#Bind("U_Z_ExpName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                          <asp:TemplateField HeaderText="Allowance Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllAllCode" runat="server" Text='<%#Bind("U_Z_AlloCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                                                          
                                    </Columns>
                                    <HeaderStyle BackColor="Gray" HorizontalAlign="Center" ForeColor="White" Height="25px" />
                                      <RowStyle HorizontalAlign="Center" />
                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                  </asp:GridView></asp:Panel>
                            </asp:Panel>
            
                          </td>
                          </tr>    

                          <tr>
        <td>
          <div style="visibility:hidden">
<asp:Button id="btnSample" runat="server"/>
</div>
<ajx:ModalPopupExtender ID="ModalPopupExtender2" runat="server" DropShadow="True" PopupControlID="Panelpoptechnician1" TargetControlID="btnSample" CancelControlID="btnclstech2" BackgroundCssClass="modalBackground">
                    </ajx:ModalPopupExtender>  
        </td>
        </tr>
        <tr>
 <td>
  <asp:Panel ID="Panelpoptechnician1" runat="server" BackColor="White" style=" display:none; padding:10px; width:900px; ">
                                <div><span class="sideheading" style="color:Green;">Approval History Details</span> <span style="float:right;"> 
                                <asp:Button ID="btnclstech2" runat="server"  CssClass="btn" Width="30px" Text="X" /></span></div>
                                   <br />
                           
                              <br /><asp:Panel ID="Panel3" runat="server" Height="400px" ScrollBars="Vertical">
                                  <asp:Label ID="Label1" runat="server" Text="" CssClass="txtbox" ForeColor="Red"></asp:Label>
                                 <asp:GridView ID="grdRequesttohr" runat="server" CellPadding="4" ShowHeaderWhenEmpty="true"  CssClass="mGrid" HeaderStyle-Cssclass="GridBG"   AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false" Width="100%" ><Columns>
                                          <asp:TemplateField HeaderText="Employee ID">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lbtndocnum" runat="server" Text='<%#Bind("U_Z_EmpId") %>'  ></asp:label></div></ItemTemplate></asp:TemplateField>
                                          <asp:TemplateField HeaderText="Employee Name">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblactivity" runat="server" Text='<%#Bind("U_Z_EmpName") %>'  ></asp:label></div></ItemTemplate></asp:TemplateField>
                                          <asp:TemplateField HeaderText="Approved By">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lbltype" runat="server" Text='<%#Bind("U_Z_ApproveBy") %>'  ></asp:label></div>
                                                  </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="CreateDate">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblcrDate" runat="server" Text='<%#Bind("CreateDate") %>'  ></asp:label></div></ItemTemplate></asp:TemplateField>
                                          <asp:TemplateField HeaderText="Create Time">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblcrtime" runat="server" Text='<%#Bind("CreateTime") %>'  ></asp:label></div></ItemTemplate></asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Update Date">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblupdate" runat="server" Text='<%#Bind("UpdateDate") %>'  ></asp:label></div></ItemTemplate></asp:TemplateField>
                                          <asp:TemplateField HeaderText="Update Time">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblupdatime" runat="server" Text='<%#Bind("UpdateTime") %>'  ></asp:label></div></ItemTemplate></asp:TemplateField>

                                              <asp:TemplateField HeaderText="Approved Status">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblactsubject" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'  ></asp:label></div>
                                                  </ItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Remarks">
                                              <ItemTemplate><div align="center">
                                                  <asp:label ID="lblAssigned" runat="server" Text='<%#Bind("U_Z_Remarks") %>'  ></asp:label></div>
                                                  </ItemTemplate>
                                            </asp:TemplateField>                                             
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" height="25px" BackColor="#CCCCCC"/>
                                    </asp:GridView>
                                    </asp:Panel>
                            </asp:Panel>
 </td>
 </tr>
</table> 

<%--</ContentTemplate> 
</asp:UpdatePanel> --%>
</asp:Content>
