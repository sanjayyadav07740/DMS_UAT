<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="MetaTemplateView.aspx.cs" Inherits="DMS.MetaTemplate.MetaTemplateView" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <center>
        <asp:ImageButton ID="ibtnMetaTemplateCreate" runat="server" OnClick="ibtnMetaTemplateCreate_Click"
            CausesValidation="false" SkinID="AddNewButton" />
    </center>
    <center>
        <table cellpadding="0" cellspacing="0" border="0" width="931">
            <tr>
                <td align="left">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayMetaTemplate="false" DisplayCategory="false"
                        DisplayFolder="false" DisplayMetaDataCode="false" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <table width="931" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="center">
                    <asp:GridView ID="gvwMetaTemplate" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="931"
                        AllowPaging="true" PageSize="10" DataKeyNames="ID" OnPageIndexChanging="gvwMetaTemplate_PageIndexChanging"
                        OnRowCommand="gvwMetaTemplate_RowCommand" OnSorting="gvwMetaTemplate_Sorting"
                        OnRowDataBound="gvwMetaTemplate_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="MetaTemplate ID" Visible="false" />
                            <asp:BoundField DataField="MetaTemplateName" HeaderText="MetaTemplate Name" SortExpression="MetaTemplateName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="CreatedOn" HeaderText="Created On" SortExpression="CreatedOn"
                                DataFormatString="{0:f}" NullDisplayText="N/A" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="UpdatedOn" HeaderText="Updated On" SortExpression="UpdatedOn"
                                DataFormatString="{0:f}" NullDisplayText="N/A" />
                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText="N/A" />
                            <asp:ButtonField ButtonType="Image" CommandName="EditMetaTemplate" HeaderText="Edit"
                                ImageUrl="~/Images/DMSButton/edit.jpg" ControlStyle-Width="80px" />
                            <asp:ButtonField ButtonType="Image" CommandName="EditMetaTemplateFields" HeaderText="Field"
                                ImageUrl="~/Images/DMSButton/field.jpg" ControlStyle-Width="80px" />
                            <asp:ButtonField ButtonType="Image" CommandName="EditListItemFields" HeaderText="List Item"
                                ImageUrl="~/Images/DMSButton/list.jpg" ControlStyle-Width="80px" Visible="false"/>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
