<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload_CentrumMaster.aspx.cs" Inherits="DMS.Shared.Upload_CentrumMaster" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphUploadDocumentsHead" ContentPlaceHolderID="cphHead" runat="server">
    </asp:Content>
<asp:Content ID="cphUploadDocumentsMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Upload Centrum Master" SkinID="Title"></asp:Label>
    </center>
    <table border="0" cellpadding="0" cellspacing="3" width="931">
        <tr>
            <td>
                <asp:Label ID="lblMetatemplateName" runat="server" Text="Metatemplate Name"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMetatemplateName" runat="server" Width="160px"></asp:DropDownList>
            </td>
            </tr>
        <tr>
            <td>
            <asp:Label ID="lblBrowse" runat="server" Text="Select Master Excel"></asp:Label>
                </td>
            <td>
                <asp:FileUpload ID="dlUpload" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
            </td>
        </tr>
        
        </table>
</asp:Content>
