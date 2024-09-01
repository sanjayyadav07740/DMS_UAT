<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListItems.ascx.cs" Inherits="DMS.UserControl.ListItems" %>
<asp:UpdatePanel ID="upanPanel" runat="server">
    <ContentTemplate>
        <table style="width: 100%;">
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:GridView ID="gdvListItem" runat="server" DataKeyNames="ID" AllowPaging="true"
                        CellPadding="2" PageSize="10" AutoGenerateColumns="false" OnRowDataBound="gdvListItem_RowDataBound"
                        OnPageIndexChanging="gdvListItem_PageIndexChanging" OnRowCommand="gdvListItem_RowCommand">
                        <Columns>
                            <%-- <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field Name">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFieldName" Enabled="false" runat="server" Text='<%#Bind("FieldName") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="List Items">
                            <ItemTemplate>
                                <asp:Label ID="lblListItem" runat="server" Text='<%#Eval("ListItemText")%>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtListItem" runat="server" Text='<%#Bind("ListItemText") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="List Items">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlListItem" runat="server" Enabled="false">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlListItem" runat="server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Sno" >
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %></ItemTemplate>
                        </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Field Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFieldName" Enabled="false" runat="server" Text='<%#Bind("FieldName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlListItem" runat="server">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="List Items">
                                <ItemTemplate>
                                    <asp:Label ID="lblListItem" runat="server" Text='<%#Eval("ListItemText")%>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlListItem" runat="server" Text='<%#Bind("ListItemText") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFieldName" runat="Server"></asp:TextBox>&nbsp;
                                    <ajaxtoolkit:FilteredTextBoxExtender ID="fteFieldName" runat="server" TargetControlID="txtFieldName"
                                        SkinID="String">
                                    </ajaxtoolkit:FilteredTextBoxExtender>
                                    &nbsp;
                                    <asp:ImageButton SkinID="AddButton"  ID="btnInsert" runat="Server" Text="Insert" CommandName="Insert" UseSubmitBehavior="False" />
                                    <asp:ImageButton SkinID="CancelButton" ID="btnCancel" runat="Server" Text="Cancel" CommandName="CancelInsert" UseSubmitBehavior="False" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="Edit" ShowEditButton="true" ButtonType="Image" EditImageUrl="~/Images/DMSButton/edit.jpg"
                                UpdateImageUrl="~/Images/DMSButton/update.jpg" CancelImageUrl="~/Images/DMSButton/cancel.jpg"
                                ControlStyle-Width="80px" Visible="false" />
                        </Columns>
                        <%--<FooterStyle BackColor="#CCCC99" />
                        <RowStyle BackColor="#F7F7DE" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />--%>
                        <EmptyDataTemplate>
                            Select Field :
                            <asp:DropDownList ID="ddlListItem" runat="server">
                            </asp:DropDownList>
                            List Item :
                            <asp:TextBox ID="txtFieldName" runat="Server"></asp:TextBox>
                            <ajaxtoolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                TargetControlID="txtFieldName" SkinID="String">
                            </ajaxtoolkit:FilteredTextBoxExtender>
                            <asp:ImageButton SkinID="AddButton" ID="btnInsert" runat="Server" Text="Insert" CommandName="EmptyInsert"
                                UseSubmitBehavior="False" /></EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <%--<asp:HiddenField ID="hdfListItem" runat="server" />--%>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Label ID="lblSelectFiels" runat="server" Text="Select Field :" Visible="False"></asp:Label>&nbsp;
                    <asp:DropDownList ID="ddlSelectField" runat="server" Visible="False">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Label ID="lblEnterListItem" runat="server" Text="List Item :" Visible="False"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtListItem" runat="server" Visible="False"></asp:TextBox>&nbsp;
                    <asp:ImageButton ID="imgbtnAddItem" runat="server" OnClick="imgbtnAddItem_Click" SkinID="AddButton"/>&nbsp;
                    <asp:ImageButton ID="imgbtnCancel" runat="server" OnClick="imgbtnCancel_Click" SkinID="CancelButton"/>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
