<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="DMS.Shared.SendMail" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <b>Send Mail</b>
<table style="width: 100%;" align="center">
<%--<tr align="center">
<td style="width: 40%" align="right">
<asp:Label ID="lblFrom" Text="From" runat="server"></asp:Label>
</td>
<td align="left">
<asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
</td>
</tr>--%>
<tr align="center">
<td style="width: 40%" align="right">
<asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
</td>
<td align="left">
<asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
</td>
</tr>
<tr align="center">
<td style="width: 40%" align="right">
<asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
</td>
<td align="left">
<asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
</td>
</tr>
<tr align="center">
<td style="width: 40%" align="right">
<asp:Label ID="lblBody" runat="server" Text="Message"></asp:Label>
</td>
<td align="left">
<asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Height="157px" 
        Width="367px"></asp:TextBox>
</td>
</tr>
<tr align="center">
<td style="width: 40%" align="right">
<asp:Label ID="lblAttachment" runat="server" Text="Attachment"></asp:Label>
</td>
<td align="left">
<asp:TextBox ID="txtAttachment" runat="server" ></asp:TextBox>
</td>
</tr>
<tr>
<td colspan="2" align="center">
 <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" 
        CausesValidation="true" onclick="ibtnSubmit_Click"/>
 &nbsp;
 <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" 
        CausesValidation="false" onclick="ibtnCancel_Click" />
</td>
</tr>
</table>
</asp:Content>