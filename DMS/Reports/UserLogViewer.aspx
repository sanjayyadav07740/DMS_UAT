<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="UserLogViewer.aspx.cs" Inherits="DMS.Reports.UserLogViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <table width="931" cellpadding="0" cellspacing="3" border="0">
        <tr>
            <td style="width:150" align="left">
                <asp:Label ID="lblRoleName" runat="server" Text="Role Name (Optional) :"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlRoleName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRoleName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width:150" align="left">
                <asp:Label ID="lblUserName" runat="server" Text="User Name (Optional) :"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlUserViewer" runat="server">
                    <asp:ListItem>--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblFromDate" runat="server" Text="From :"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ibtnFromDate" runat="server" SkinID="CalenderButton" CausesValidation="false" />
                <ajaxtoolkit:CalendarExtender ID="cleFromDate" runat="server" TargetControlID="txtFromDate"
                    Format="dd-MMM-yyyy" PopupButtonID="ibtnFromDate">
                </ajaxtoolkit:CalendarExtender>
                <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate"
                    Text="Invalid Date(e.g 02-Feb-2011)" Display="Dynamic" ForeColor="Red" Font-Size="X-Small"
                    ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-(19|20)\d\d$"
                    SetFocusOnError="true" ErrorMessage="Invalid Date(e.g 02-Feb-2011)"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblToDate" runat="server" Text="To :"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ibtnToDate" runat="server" SkinID="CalenderButton" CausesValidation="false" />
                <ajaxtoolkit:CalendarExtender ID="cleToDate" runat="server" TargetControlID="txtToDate"
                    Format="dd-MMM-yyyy" PopupButtonID="ibtnToDate">
                </ajaxtoolkit:CalendarExtender>
                <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate"
                    Text="Invalid Date(e.g 02-Feb-2011)" Display="Dynamic" ForeColor="Red" Font-Size="X-Small"
                    ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-(19|20)\d\d$"
                    SetFocusOnError="true" ErrorMessage="Invalid Date(e.g 02-Feb-2011)"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" CausesValidation="true"
                    OnClick="ibtnsubmit_Click" />
                <asp:ImageButton ID="ibtnExport" runat="server" SkinID="ExportButton" CausesValidation="false"
                    OnClick="ibtnExport_Click" />
            </td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="gvwUserLogViewer" runat="server" AutoGenerateColumns="False" ShowFooter="true" Width="931"
                    Height="106px" OnPageIndexChanging="gvwUserLogViewer_PageIndexChanging" AllowPaging="True"
                    OnSelectedIndexChanged="gvwUserLogViewer_SelectedIndexChanged">
                    <Columns>
                        <%-- <asp:TemplateField>
                            <FooterTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text="UserName"></asp:Label>
                                <asp:DropDownList ID="ddlUserName" runat="server">
                                </asp:DropDownList>
                                <asp:Button ID="btnFilter" runat="server" Text="Filter" CausesValidation="false"
                                    OnClick="btnFilter_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                        <asp:BoundField DataField="LoginTime" HeaderText="Login Time" NullDisplayText="N/A"
                            DataFormatString="{0:f}" />
                        <asp:BoundField DataField="LogoutTime" HeaderText="Logout Time" NullDisplayText="N/A"
                            DataFormatString="{0:f}" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
