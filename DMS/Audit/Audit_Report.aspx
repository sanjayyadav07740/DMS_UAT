<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Audit_Report.aspx.cs" Inherits="DMS.Audit.Audit_Report"  Culture="en-GB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style>
        .tablespace tr td{
            padding:5px 10px;
        }

        .gridrow td,th {
    border: 1px solid #000000;
    color: #313131;
    font-size: 12px;
    font-weight: normal;
    vertical-align: middle;
    text-align: center !important;
    margin: 0px;
    height: 22px;
    padding: 5px;
}

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">

     <div style="text-align:center;">
     <asp:Label ID="lblPending" runat="server"  EnableTheming="false" Font-Size="12" Font-Underline="true" Font-Bold="true">
        Audit Report </asp:Label>
   
    </div>
    <br />
    <center>
     <table width="1200px" style=" text-align: center; margin: 0 auto;">
        <tr>
            <td align="center">
                <table width="1150px" cellpadding="0" cellspacing="0" border="0">                    
                    <tr> 
                        <td align="center"> 
                            <table width="700px" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:RadioButton ID="rdobtnSingleUser" runat="server" Text="Single User" AutoPostBack="True" GroupName="usergroup" />                                  
                                        <asp:RadioButton ID="rdobtnMultipleUser" runat="server" Text="Multiple User" Checked="true" GroupName="usergroup" AutoPostBack="True" />
                                        <asp:RadioButton ID="rdobtnCountVisit" runat="server" Text="Visit Count"  GroupName="usergroup" AutoPostBack="True" />
                                    </td>
                                </tr>
                                <tr><td colspan="4">&nbsp;</td></tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblUserName" runat="server" Text="Select User"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlUserViewer" runat="server" OnSelectedIndexChanged="ddlUserViewer_SelectedIndexChanged">
                                            <asp:ListItem>--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr><td colspan="4">&nbsp;</td></tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblFrom" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFrom" runat="server" AutoComplete="off" ToolTip="Please Enter From Date." ReadOnly="false"></asp:TextBox>
                                        <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFrom"
                                            PopupButtonID="txtFrom" Format="dd/MM/yyyy">
                                        </ajaxtoolkit:CalendarExtender>                                        
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblTo" runat="server" Text="To Date"></asp:Label>
                                     </td>
                                    <td>
                                        <asp:TextBox ID="txtTo" runat="server" AutoComplete="off" ToolTip="Please Enter To Date." ReadOnly="false"></asp:TextBox>
                                        <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo"
                                            PopupButtonID="txtTo" Format="dd/MM/yyyy">
                                        </ajaxtoolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                   <td colspan="2" align="center">
                                       <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFrom" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                                            ErrorMessage="Invalid date format." ValidationGroup="Group1" Display="Dynamic" />
                                   </td>
                                    <td colspan="2" align="center">
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtTo" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                                            ErrorMessage="Invalid date format." ValidationGroup="Group1" Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:CompareValidator ID="cvDateValidate" ValidationGroup="Date" ForeColor="Red" runat="server" ControlToValidate="txtFrom"
                                             ControlToCompare="txtTo" Operator="LessThanEqual" Type="Date" ErrorMessage="To date must be Greater than From date."
                                             Display="Dynamic" ></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr><td colspan="4">&nbsp;</td></tr>
                                <tr>
                                    <td id="btnSub" runat="server" colspan="2" align="right">                                  
                                          <%--<div  style="text-align: center;" class="iconbutton turquoise">
                                                    <span>&#10003;</span>--%>
                                   <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" ValidationGroup="Date" Text="Submit" OnClick="btnSubmit_Click" CssClass="btnentire" />
                                              <%--</div>--%>
                                        </td>
                                        <td id="btnExportMultiple" runat="server"  colspan="2" align="left"> &nbsp
                                        <%--<div  style="text-align: center;" class="iconbutton turquoise">
                                                    <span>
                                                        <img src="../Images/Icon-cancel.png" /></span>--%>
                                        <asp:Button ID="btnExport" runat="server" Text="Export"  CssClass="btnentire" OnClick="btnExport_Click" />
                                           <%-- </div>--%>
                                         </td>
                                </tr>
                                <tr><td colspan="4">&nbsp;</td></tr>
                                <tr>

                                    <td id="btnSub1" runat="server" colspan="2" align="right">

<%--                                        <div style="text-align: center;" class="iconbutton turquoise">
                                            <span>&#10003;</span>--%>
                                            <asp:Button ID="btnSubmit1" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="Date" OnClick="btnSubmit1_Click" CssClass="btnentire" />
                                        <%--</div>--%>
                                    </td>
                                    <td id="btnExportsingle" runat="server" colspan="2" align="left">&nbsp
                                   <%--     <div style="text-align: center;" class="iconbutton turquoise">
                                            <span>
                                                <img src="../Images/Icon-cancel.png" /></span>--%>
                                            <asp:Button ID="btnExport1" runat="server" Text="Export" CssClass="btnentire" OnClick="btnExport1_Click" />
                                       <%-- </div>--%>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>                                
                                <tr>
                                    <td id="btnSub2" runat="server" colspan="2" align="right">
                                        <%--<div style="text-align: center;" class="iconbutton turquoise">
                                            <span>&#10003;</span>--%>
                                            <asp:Button runat="server" ID="btnCountHits" CssClass="btnentire" Text="Activity Count" OnClick="btnCountHits_Click" />

                                      <%--  </div>--%>
                                    </td>
                                    <td id="btnSub4" runat="server" colspan="2" align="left">&nbsp
                                        <%--<div style="text-align: center;" class="iconbutton turquoise">
                                            <span>
                                                <img src="../Images/Icon-cancel.png" /></span>--%>
                                            <asp:Button ID="btnHitCountExp" runat="server" Text="Export" CssClass="btnentire" OnClick="btnHitCountExp_Click" />
                                        <%--</div>--%>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr> 
                            </table>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td>
                            <asp:Panel ID="pagecountresult" runat="server">
                                <table width="700px" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center" style="width: 150px;">
                                            <asp:Label ID="lblUser" runat="server" Text="User Name :"></asp:Label></td>
                                        <td align="left" style="width: 170px;">
                                            <asp:Label ID="lblSelectedUser" runat="server" Text="" align="left"></asp:Label></td>
                                        <td align="center" style="width: 150px;">
                                            <asp:Label ID="lbltotaldoc" runat="server" Text="Total Document :"></asp:Label></td>
                                        <td align="center" style="width: 160px;">
                                            <asp:Label ID="lbltotaldocument" runat="server" Text=""></asp:Label></td>
                                        <td align="center" style="width: 150px;">
                                            <asp:Label ID="lblTotalPage" runat="server" Text="Total Pages :"></asp:Label></td>
                                        <td align="center" style="width: 150px;">
                                            <asp:Label ID="lblpagecount" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel> 
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="text-align:center;">
                            <asp:GridView ID="gvAuditReports" Class="tablespace" runat="server"  PageSize="20" AutoGenerateColumns="False" OnSorting="gvAuditReports_Sorting" Width="100%" DataKeyNames="ID,IPAddress,MacAddress,DateOfActivity,Activity,PageName,UserName,DocumentName"
                                OnPageIndexChanging="gvAuditReports_PageIndexChanging"  AllowPaging="true" AllowSorting="true"  ShowFooter="false" ShowHeaderWhenEmpty="false">
                                
                                <Columns>
                                    
                                    <asp:TemplateField HeaderText="Sr.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1  %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="IP Address" DataField="IPAddress" />
                                    <asp:BoundField HeaderText="MacAddress" DataField="MacAddress" visible="false"/>
                                    <asp:BoundField HeaderText="Date Of Activity" DataField="DateOfActivity" DataFormatString="{0:dd MMM yy}" />
                                    <asp:BoundField HeaderText="Activity" DataField="Activity" />
                                    <asp:BoundField HeaderText="PageName" DataField="PageName"  />
                                    <asp:BoundField HeaderText="UserName" DataField="UserName"  />
                                     <asp:BoundField HeaderText="DocumentName" DataField="DocumentName" NullDisplayText="N/A"  />
                                </Columns>
                            </asp:GridView>                           
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:center;">
                            <asp:GridView ID="gvPageCount" Class="tablespace" runat="server"  PageSize="20" AutoGenerateColumns="False" DataKeyNames="PageName,Counting" Width="100%"
                                AllowPaging="true" AllowSorting="true"  ShowFooter="false" ShowHeaderWhenEmpty="false">
                                <Columns>  
                                     <asp:TemplateField HeaderText="Sr.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1  %>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:BoundField HeaderText="Page Name" DataField="PageName" />
                                    <asp:BoundField HeaderText="Total" DataField="Counting" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </center>
</asp:Content>
