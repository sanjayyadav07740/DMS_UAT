<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="FolderView.aspx.cs" Inherits="DMS.Folder.FolderView" %>

<asp:Content ID="cphFolderViewHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphFolderViewMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <center>
        <asp:ImageButton ID="ibtnAddNew" runat="server" OnClick="ibtnAddNew_Click" SkinID="AddNewButton"
            ToolTip='<%$ Resources:Resource,AddNew %>' CausesValidation="false" />
    </center>
    <table width="931" cellpadding="0" cellspacing="3" border="0">
        <tr>
            <td align="left">
                <uc:EntityModule ID="emodModule" runat="server" DisplayCategory="false" />
            </td>
            <td align="left" valign="top">
                <asp:UpdatePanel ID="upanPanel" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvwFolder" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="931"
                            AllowPaging="true" DataKeyNames="ID" OnPageIndexChanging="gvwFolder_PageIndexChanging"
                            OnRowCommand="gvwFolder_RowCommand" OnRowDataBound="gvwFolder_RowDataBound" OnSorting="gvwFolder_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FolderName" HeaderText="FolderName" SortExpression="FolderName"
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
                                <asp:ButtonField ButtonType="Image" CommandName="EditFolder" HeaderText="Edit" ImageUrl="~/Images/DMSButton/edit.jpg"
                                    ControlStyle-Width="80px" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
