<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="DMS.Reports.AuditLog" MasterPageFile="~/MainMasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
  <center>
        <asp:Label ID="lblTitle" runat="server" Text="Audit Log" SkinID="Title"></asp:Label>
    </center>
<table style="width: 100%;" align="center">
    <tr>
        <td align="right">
        <asp:Label ID="lblUserName" runat="server" Text="User Name" Width="137px"></asp:Label>
          
        </td>
        <td>
            <asp:DropDownList ID="ddlUserName" runat="server" Width="216px" AutoPostBack="True" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
    
<tr>
<td align="center" colspan="2">
<asp:GridView ID="grvAuditReport" AutoGenerateColumns="false" runat="server"  
        ShowHeaderWhenEmpty="true" AllowPaging="true" 
                     PageSize="10" Width="800px" 
        onpageindexchanging="grvAuditReport_PageIndexChanging">
<Columns>
<asp:TemplateField HeaderText="Serial No">
<ItemTemplate>
<asp:Label ID="lblSRNO" runat="server" 
                Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="IP Address" DataField="IPAddress" />
<asp:BoundField HeaderText="Date Of Activity" DataField="DateOfActivity"/>
<asp:BoundField HeaderText="Activity" DataField="Activity"/>
<asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
<asp:BoundField HeaderText="User Name" DataField="UserName" />
</Columns>
</asp:GridView>
</td>
</tr>
    <tr align="center" >
        <td colspan="2"><asp:ImageButton ID="ibtnExport" runat="server" SkinID="ExportButton" OnClick="ibtnExport_Click" /></td>
    </tr>
</table>
</asp:Content>
