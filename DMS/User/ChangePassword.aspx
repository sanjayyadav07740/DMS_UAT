<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="DMS.User.ChangePassword" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphUploadDocumentsHead" ContentPlaceHolderID="cphHead" runat="server">
    </asp:Content>
<asp:Content ID="cphUploadDocumentsMain" ContentPlaceHolderID="cphMain" runat="server">
     <center>
        <asp:Label ID="lblTitle" runat="server" Text="Change Password" SkinID="Title"></asp:Label>
    </center>
    <table border="0" cellpadding="0" cellspacing="3" width="931">
        <tr>
            <td>
                <asp:Label ID="lblUserName" runat="server" Text="UserName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOldPwd" runat="server" Text="Old Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNewPwd" runat="server" Text="New Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" OnClick="ibtnSubmit_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
            </td>
        </tr>
        </table>
    </asp:Content>