<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="CategoryView.aspx.cs" Inherits="DMS.Category.CategoryView" %>


<asp:Content ID="cphCategoryHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphCategoryMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <center>
        <asp:ImageButton ID="ibtnAddNew" runat="server" CausesValidation="false" SkinID="AddNewButton"
            OnClick="ibtnAddNew_Click" ToolTip='<%$ Resources:Resource,AddNew %>' /></center>
    <center>
        <table width="931" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayCategory="false" DisplayFolder="false" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:UpdatePanel ID="upanPanel" runat="server">
            <ContentTemplate>
                <table width="931" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left">
                            <asp:GridView ID="gvwCategory" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="931"
                                AllowPaging="true" DataKeyNames="ID" OnPageIndexChanging="gvwCategory_PageIndexChanging"
                                OnRowCommand="gvwCategory_RowCommand" OnRowDataBound="gvwCategory_RowDataBound"
                                OnSorting="gvwCategory_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CategoryName" HeaderText="CategoryName" SortExpression="CategoryName"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn"
                                        NullDisplayText="N/A" DataFormatString="{0:f}" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="UpdatedOn" HeaderText="UpdatedOn" SortExpression="UpdatedOn"
                                        NullDisplayText="N/A" DataFormatString="{0:f}" />
                                    <asp:BoundField DataField="UpdatedBy" HeaderText="UpdatedBy" SortExpression="UpdatedBy"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText="N/A" />
                                    <asp:ButtonField ButtonType="Image" CommandName="EditCategory" HeaderText="Edit"
                                        ImageUrl="~/Images/DMSButton/edit.jpg" ControlStyle-Width="80px" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>
