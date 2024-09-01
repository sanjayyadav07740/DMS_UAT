<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentEntry_Virescent.aspx.cs" Inherits="DMS.Shared.DocumentEntry_Virescent" %>

<%@ Register Src="~/UserControl/MakerChecker_Virescent.ascx" TagPrefix="li" TagName="MakerChecker_Virescent" %>

<asp:Content ID="cphDocumentEntryHead" ContentPlaceHolderID="cphHead" runat="server">

</asp:Content>
<asp:Content ID="cphDocumentEntryMain" ContentPlaceHolderID="cphDocument" runat="server">
    
    <center><asp:Label ID="lblTitle" runat="server" Text="Document Entry"  SkinID="Title"></asp:Label> </center>
    <div style="width:100%;">

       <center>
      <%--<uc:MakerChecker ID="mkcChecker_UP" runat="server" ViewType="DocumentEntry" />--%>
          <%-- <li:MakerChecker_UP runat="server" ID="MakerChecker_UP" ViewType="DocumentEntry" />--%>
           <li:MakerChecker_Virescent  runat="server" ID="MakerChecker_Virescent"  ViewType="DocumentEntry" />
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
