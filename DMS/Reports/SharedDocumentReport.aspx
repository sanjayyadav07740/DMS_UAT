<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="SharedDocumentReport.aspx.cs" Inherits="DMS.Reports.SharedDocumentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Shared Document Report" SkinID="Title"></asp:Label>
    </center>
    <div align="center">
        <table>
            <tr>
                <td align="left" style="width: 200px;">
                    <asp:Label ID="lblselect" runat="server" Text="Select User Type :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        <asp:ListItem Value="--Select--">-- Select --</asp:ListItem>
                        <asp:ListItem Value="InternalUser">Internal User</asp:ListItem>
                        <asp:ListItem Value="ExternalUser">External User</asp:ListItem>
                    </asp:DropDownList>

                    
                              
                </td>
            </tr>

        </table>
    </div>

</asp:Content>
