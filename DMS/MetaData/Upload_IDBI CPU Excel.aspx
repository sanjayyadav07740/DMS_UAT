<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload_IDBI CPU Excel.aspx.cs" Inherits="DMS.Shared.Upload_IDBI_CPU_Excel" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphBulkUploadHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphBulkUploadMain" ContentPlaceHolderID="cphmain" runat="server">
     <center><asp:Label ID="lblTitle" runat="server" Text="Upload Merged Unmerged Details Excel"  SkinID="Title"></asp:Label> </center>
    <table align="center">
        <tr>
            <td>
                <asp:Label ID="lblUpload" runat="server" Text="Upload Excel"></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="flUpload" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
            </td>
        </tr>
        </table>
</asp:Content>
