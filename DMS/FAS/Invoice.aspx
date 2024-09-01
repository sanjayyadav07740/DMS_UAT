<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="Invoice.aspx.cs" Inherits="DMS.FAS.Invoice" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <%--<script language="javascript" type="text/javascript">
    function checkDate(sender, args) {
        //compareMonthYear(sender);
        var chkDate = new Date();
        if (sender._selectedDate.localeFormat('MM-dd-yyyy') > chkDate.localeFormat('MM-dd-yyyy')) {
            alert("You cannot select a day Greater than todays date!");
            sender._selectedDate = new Date();                  // set the date back to the current date 
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
 </script>--%>
    <link href="../Css/style.css" rel="stylesheet" type="text/css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblInvoice" runat="server" Text="Invoice" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table width="931" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" style="width:150px;">
                    <asp:Label ID="lblClient" runat="server" Text="Client Name:"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlClient" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlClient_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvClient" runat="server" ErrorMessage="Please Select Client"
                        ControlToValidate="ddlClient" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
           
            <td align="left">
            <asp:Label ID="lblDept" runat="server" Text="Department:"></asp:Label>
            </td>
            <td align="left">
            <asp:DropDownList ID="ddlDept" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlDept_SelectedIndexChanged" Visible="False">
                <asp:ListItem>--Select--</asp:ListItem>
                <asp:ListItem Value="20">MHADA MB</asp:ListItem>
                <asp:ListItem Value="22">MHADA KB</asp:ListItem>
                <asp:ListItem Value="21">MSIB</asp:ListItem>
                <asp:ListItem Value="24">RRB</asp:ListItem>
                <asp:ListItem Value="19">MHADA Authority</asp:ListItem>
                </asp:DropDownList>
            </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblLocation" runat="server" Text="Location :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                     <asp:ListItem Value="1">Mumbai</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ErrorMessage="Please Select Location"
                        ControlToValidate="ddlLocation" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblBranch" runat="server" Text="Branch :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                     <asp:ListItem Value="1">Mahape</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                        ErrorMessage="Please Select Branch" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblbillmonthfrom" runat="server" Text="Billing Date From :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="userfield_style"  Enabled="true"></asp:TextBox>
                    <cc1:CalendarExtender ID="txt_From_Date_CalendarExtender" runat="server" TargetControlID="txtFrom"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgCal" Enabled="True">
                                     </cc1:CalendarExtender>

                                     <asp:ImageButton ID="imgCal" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                     CausesValidation="false" />
                                     <td>
                                      <asp:RequiredFieldValidator ID="rfvtxt_From_Date" runat="server" ErrorMessage="Please Select From Date."
                                        Text="*" ControlToValidate="txtFrom" SetFocusOnError="true" Font-Bold="true"
                                        ForeColor="Red">
                                        </asp:RequiredFieldValidator>
                    </td>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblbilldateto" runat="server" Text="Billing Date To :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTo" runat="server" CssClass="userfield_style"  Enabled="true"></asp:TextBox>
                    <asp:ImageButton ID="imgCal0" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="false" />
                                    <cc1:CalendarExtender ID="txt_To_Date_calendarextender" runat="server" TargetControlID="txtTo"
                                     Format="dd-MMM-yyyy" PopupButtonID="imgCal0" Enabled="True">
                                    </cc1:CalendarExtender>
                                    <td>
                                    <asp:RequiredFieldValidator ID="rfvtxt_To_Date" runat="server" ErrorMessage="Please Select To Date."
                                    Text="*" ControlToValidate="txtTo" SetFocusOnError="true" Font-Bold="true"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ImageButton ID="btnShow" runat="server" Text="Show" Height="30px" ImageUrl="~/Images/DMSButton/showinvoice.jpg"
                        OnClick="btnShow_Click1" ToolTip="Show Invoice" />
                    <asp:ImageButton ID="btnGenerate" runat="server" Text="Generate Bill" Visible="false"
                        Height="30px" ImageUrl="~/Images/DMSButton/Generateinvoice.jpg" OnClick="btnGenerate_Click1"
                        ToolTip="Generate Invoice" />
                    <asp:ImageButton ID="btnCancel" runat="server" Text="Cancel" SkinID="CancelButton"
                        OnClick="btnCancel_Click1" ToolTip="Cancel Billing" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnPolicyReport" runat="server"  ImageUrl="../Images/excel.jpg"
                        ToolTip="Export To Excel" Visible="true"  OnClick="ibtnPolicyReport_Click" />
                </td>
                <td>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:GridView ID="grvShowbill" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                        OnRowDataBound="grvShowbill_RowDataBound" AllowPaging="True" OnPageIndexChanging="grvShowbill_PageIndexChanging"
                        OnSorting="grvShowbill_Sorting">
                        <Columns>
                            <asp:BoundField DataField="RepositoryName" HeaderText="ClientName"/>
                            <%--<asp:BoundField DataField="Sevice" HeaderText="Sevice" />--%>
                            <asp:BoundField DataField="CreatedOn" HeaderText="Date of Activity" DataFormatString="{0:dd-MMM-yyyy}"
                                SortExpression="CreatedOn" />
                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                           <%-- <asp:BoundField DataField="Location" HeaderText="Location" />
                            <asp:BoundField DataField="Branch" HeaderText="Branch"/>--%>
                            <asp:TemplateField HeaderText="Document Type">
                                <ItemTemplate>
                                    <span style="color: Black; font-size: small;">.pdf</span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <td>
                    </td>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:GridView ID="grvOpeningStock" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                         AllowPaging="True" 
                        >
                        <Columns>
                            <asp:BoundField DataField="RepositoryName" HeaderText="ClientName"/>
                            <%--<asp:BoundField DataField="Sevice" HeaderText="Sevice" />--%>
                            <%--<asp:BoundField DataField="CreatedOn" HeaderText="Date of Activity" DataFormatString="{0:dd-MMM-yyyy}"
                                SortExpression="CreatedOn" />--%>
                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                           <%-- <asp:BoundField DataField="Location" HeaderText="Location" />
                            <asp:BoundField DataField="Branch" HeaderText="Branch"/>--%>
                            <asp:TemplateField HeaderText="Document Type">
                                <ItemTemplate>
                                    <span style="color: Black; font-size: small;">.pdf</span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
