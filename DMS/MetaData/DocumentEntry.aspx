<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentEntry.aspx.cs" Inherits="DMS.Shared.DocumentEntry" %>
<asp:Content ID="cphDocumentEntryHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDocumentEntryMain" ContentPlaceHolderID="cphDocument" runat="server">
    <center><asp:Label ID="lblTitle" runat="server" Text="Document Entry"  SkinID="Title"></asp:Label> </center>
    <div style="width:100%;">
       <center>
      <uc:MakerChecker ID="mkcChecker" runat="server" ViewType="DocumentEntry" />
      </center>
    </div>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" 
                    ToolTip='<%$ Resources:Resource,Submit %>' SkinID="SubmitButton" 
                    CausesValidation="false" />
                <asp:ImageButton ID="ibtnBack" runat="server"  CausesValidation="false"  SkinID="CancelButton"
                    ToolTip='<%$ Resources:Resource,Back %>' onclick="ibtnBack_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
