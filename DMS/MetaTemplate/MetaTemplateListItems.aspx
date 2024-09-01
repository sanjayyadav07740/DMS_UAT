<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMasterPage.Master"
    CodeBehind="MetaTemplateListItems.aspx.cs" Inherits="DMS.MetaTemplate.MetaTemplateListItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title" Text="MetaTemplate List Items"></asp:Label><br />
        <br />
    </center>
    <table style="width: 100%;">
        <tr>
            <td colspan="3">
                <li:ListItem ID="liListItem" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
