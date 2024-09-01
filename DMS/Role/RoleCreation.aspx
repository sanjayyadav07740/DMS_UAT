<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="RoleCreation.aspx.cs" Inherits="DMS.Role.RoleCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table border="0" cellpadding="0" cellspacing="0" width="931">
            <tr>
                <td align="left" style="width:150px;">
                    <asp:Label ID="lblrepository" runat="server" Text="Repository :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlrepository" runat="server" TabIndex="16" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td style="width: 26px" align="left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRoleName"
                        ToolTip='<%$ Resources:Resource,RoleName %>'></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left" style="width:150px;">
                    <asp:Label ID="lblRoleName" runat="server" Text="RoleName :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtRoleName" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td style="width: 26px" align="left">
                    <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ControlToValidate="txtRoleName"
                        ToolTip='<%$ Resources:Resource,RoleName %>'></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblDisplayName" runat="server" Text="DisplayName :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td style="width: 26px" align="left">
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblRoleType" runat="server" Text="Role Type :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlRoleType" runat="server" TabIndex="16" 
                        onselectedindexchanged="ddlRoleType_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="System Admin" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Repository Admin" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Repository User" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 26px" align="left">
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr id="trRoleRights" runat="server" visible="false">
                <td align="left">
                    <asp:Label ID="lblRoleRights" runat="server" Text="Role Rights :" ></asp:Label>
                </td>
                <td align="left">
                <asp:Panel ID="panRoleRights" runat="server" ScrollBars="Auto" Height="75" >
                    <asp:CheckBoxList ID="cblRoleRights" runat="server">
                    </asp:CheckBoxList>
                    </asp:Panel>
                </td>
                <td style="width: 26px" align="left">
                </td>
            </tr>
            <tr style="height: 8px">
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblStatus" runat="server" Text="Status :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="16">
                    </asp:DropDownList>
                </td>
                <td style="width: 26px" align="left">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <hr />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:ImageButton ID="ibtnSubmit" runat="server" Text="Submit" OnClick="ibtnSubmit_Click"
                        SkinID="SubmitButton" />
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnBack" runat="server" Text="Submit" OnClick="ibtnBack_Click"
                        SkinID="CancelButton" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </center>
    <asp:HiddenField ID="hdfRoleName" runat="server" />
</asp:Content>
