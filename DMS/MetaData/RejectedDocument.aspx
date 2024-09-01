<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="RejectedDocument.aspx.cs" Inherits="DMS.Shared.RejectedDocument" %>
<asp:Content ID="cphRejectedDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphRejectedDocumentMain" ContentPlaceHolderID="cphDocument" runat="server">
<center><asp:Label ID="lblTitle" runat="server" Text="Rejected Document"  SkinID="Title"></asp:Label> </center>
    <div style="width:100%;">
    <center>
      <uc:MakerChecker ID="mkcChecker" runat="server" ViewType="RejectedDocument" />
            <br />
      </center>
    </div>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnBack" runat="server"  CausesValidation="false"
                    ToolTip='<%$ Resources:Resource,Back %>' onclick="ibtnBack_Click" SkinID="BackButton"/>
            </td>
        </tr>
    </table>
</asp:Content>
