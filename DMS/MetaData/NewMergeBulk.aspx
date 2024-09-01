<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="NewMergeBulk.aspx.cs" Inherits="DMS.Shared.NewMergeBulk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
       <center><asp:Label ID="lblTitle" runat="server" Text="BulkMerging"  SkinID="Title"></asp:Label> </center>
    <table align="center">
        <tr>
            <td>
                <asp:Label ID="lblUpload" runat="server" Text="Upload Zip File"></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="flUpload" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="UploadButton" OnClick="ibtnSubmit_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
            </td>
        </tr>
        
        </table>
</asp:Content>
