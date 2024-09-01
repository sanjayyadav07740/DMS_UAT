<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mhada_Invoice.aspx.cs" Inherits="DMS.FAS.Mhada_Invoice" MasterPageFile="~/MainMasterPage.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblInvoice" runat="server" Text="MHADA Invoice" SkinID="Title"></asp:Label>
    </center>
    <center>
    <table width="931" border="0" cellpadding="0" cellspacing="0">
    <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" style="width:150px;">
                    <asp:Label ID="lblClient" runat="server" Text="Client Name:"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtClient" runat="server" Text="MHADA"></asp:TextBox>
                </td>
               
            </tr>
            <tr>
            <td align="left">
            <asp:Label ID="lblDept" runat="server" Text="Department"></asp:Label>
            </td>
            <td align="left">
                    <asp:DropDownList ID="ddlDept" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td align="left">
            <asp:Label ID="lblFromDate" runat="server" Text="From"></asp:Label>
            </td>
             <td align="left">
            <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
             <cc1:CalendarExtender ID="txt_From_Date_CalendarExtender" runat="server" TargetControlID="txtFromDate"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgCal" Enabled="True">
                                     </cc1:CalendarExtender>

                                     <asp:ImageButton ID="imgCal" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                     CausesValidation="false" />
            </td>
            </tr>
            <tr>
            <td align="left">
            <asp:Label ID="lblToDate" runat="server" Text="To"></asp:Label>
            </td>
             <td align="left">
            <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgCal" Enabled="True">
                                     </cc1:CalendarExtender>

                                     <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                     CausesValidation="false" />
            </td>
            </tr>
            <tr>
            <td colspan="2" align="center">
            <asp:ImageButton ID="btnGenerate" runat="server" Text="Generate Bill" Visible="false"
                        Height="30px" ImageUrl="~/Images/DMSButton/Generateinvoice.jpg" OnClick="btnGenerate_Click1"
                        ToolTip="Generate Invoice" />
                        <asp:ImageButton ID="btnCancel" runat="server" SkinID="CancelButton" ImageUrl="~/Images/DMSButton/cancel.jpg" />
            </td>
            </tr>
    </table>
    </center>
    </asp:Content>
