<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ApprovedDocument.aspx.cs" Inherits="DMS.Shared.ApprovedDocument" %>
<asp:Content ID="cphApprovedDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphApprovedDocumentMain" ContentPlaceHolderID="cphDocument" runat="server">
<center><asp:Label ID="lblTitle" runat="server" Text="Approved Document"  SkinID="Title"></asp:Label> </center>
    <div style="width:100%;">
    <center>
      <uc:MakerChecker ID="mkcChecker" runat="server" ViewType="ApprovedDocument" />
      </center>
    </div>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnBack" runat="server"  CausesValidation="false" SkinID="BackButton"
                    ToolTip='<%$ Resources:Resource,Back %>' onclick="ibtnBack_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
