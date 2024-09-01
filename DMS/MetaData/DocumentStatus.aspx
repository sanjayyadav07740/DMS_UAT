<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentStatus.aspx.cs" Inherits="DMS.Shared.DocumentStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Status" SkinID="Title"></asp:Label>
    </center>
    <table width="931" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="left">
                <asp:GridView ID="GridView1" runat="server" Align='center' AutoGenerateColumns="False"
                    Width="931" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing"
                    OnRowUpdating="GridView1_RowUpdating" OnPageIndexChanging="GridView1_PageIndexChanging"
                    OnRowCommand="GridView1_RowCommand" AllowPaging="True" Height="400px" PageSize="10">
                    <Columns>
                        <%--<asp:TemplateField HeaderText="ID">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ID") %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                        <asp:BoundField ReadOnly="true" DataField="ID" HeaderText="ID" />
                        <asp:BoundField ReadOnly="true" DataField="DocumentName" HeaderText="DocumentName" />
                        <%--<asp:TemplateField HeaderText="DocumentName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDocumentName" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("DocumentName") %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Status">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatus" runat="server">
                                    <asp:ListItem Text="Active" Value="1" />
                                    <asp:ListItem Text="InActive" Value="0" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField HeaderText="Update" ShowEditButton="true" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

