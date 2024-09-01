<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Aspire_BulkMerge.aspx.cs" Inherits="DMS.Shared.Aspire_BulkMerge" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphUploadDocumentsHead" ContentPlaceHolderID="cphHead" runat="server">
    </asp:Content>
<asp:Content ID="cphUploadDocumentsMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Aspire Bulk Merge" SkinID="Title"></asp:Label>
    </center>
    <table border="0" cellpadding="0" cellspacing="3" width="931">
         <tr>
            <td align="right">
                <asp:Label ID="lblUpload" runat="server" Text="Upload Zip File"></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="flUpload" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="UploadButton" OnClick="ibtnSubmit_Click"/>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
            </td>
        </tr>
        
        </table>
    </asp:Content>