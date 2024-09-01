<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentVerification.aspx.cs" Inherits="DMS.Shared.DocumentVerification" %>
<asp:Content ID="cphDocumentVerificationHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDocumentVerificationMain" ContentPlaceHolderID="cphDocument" runat="server">
    <center><asp:Label ID="lblTitle" runat="server" Text="Document Verification"  SkinID="Title"></asp:Label> </center>
    <div style="width:100%;">
    <center>
      <uc:MakerChecker ID="mkcChecker" runat="server" ViewType="DocumentVerification" />
      </center>
    </div>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" SkinID="SubmitButton" ToolTip='<%$ Resources:Resource,Submit %>' CausesValidation="false" />
                <asp:ImageButton ID="ibtnBack" runat="server"  CausesValidation="false"
                    ToolTip='<%$ Resources:Resource,Back %>' onclick="ibtnBack_Click" SkinID="CancelButton"/>
            </td>
        </tr>
    </table>
</asp:Content>
