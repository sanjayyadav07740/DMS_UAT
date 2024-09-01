<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentDashboard_IDBI_Ahm.aspx.cs" Inherits="DMS.Reports.DocumentDashboard_IDBI_Ahm" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphDocumentDashBoardHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDocumentDashBoardMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Dashboard for IDBI" SkinID="Title"></asp:Label>
    </center>
    <table width="931" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate"
        PopupButtonID="txtFromDate" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>
            <td>
                <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
        PopupButtonID="txtToDate" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:ImageButton ID="ibtnShow" runat="server" SkinID="showbutton" OnClick="ibtnShow_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" OnClick="ibtnCancel_Click" />
            </td>
        </tr>
</table>
    </asp:Content>