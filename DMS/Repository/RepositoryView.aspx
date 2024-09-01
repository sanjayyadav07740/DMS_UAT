<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="RepositoryView.aspx.cs" Inherits="DMS.Repository.RepositoryView" %>
<asp:Content ID="cphRepositoryHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphRepositoryMain" ContentPlaceHolderID="cphMain" runat="server">
<center><asp:Label ID="lblTitle" runat="server"  SkinID="Title"></asp:Label> </center>
    <center><asp:ImageButton ID="ibtnAddNew" runat="server" SkinID="AddNewButton"
        onclick="ibtnAddNew_Click" ToolTip='<%$ Resources:Resource,AddNew %>'/></center>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:GridView ID="gvwRepository" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" DataKeyNames="ID" OnPageIndexChanging="gvwRepository_PageIndexChanging"
                    OnRowCommand="gvwRepository_RowCommand" OnRowDataBound="gvwRepository_RowDataBound"
                    OnSorting="gvwRepository_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RepositoryName" HeaderText="RepositoryName" SortExpression="RepositoryName" NullDisplayText="N/A"/>
                        <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn"  NullDisplayText="N/A" 
                            DataFormatString="{0:f}" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"  NullDisplayText="N/A"/>
                        <asp:BoundField DataField="UpdatedOn" HeaderText="UpdatedOn" SortExpression="UpdatedOn" NullDisplayText="N/A"
                            DataFormatString="{0:f}" />
                        <asp:BoundField DataField="UpdatedBy" HeaderText="UpdatedBy" SortExpression="UpdatedBy" NullDisplayText="N/A" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"  NullDisplayText="N/A"/>
                        <asp:ButtonField ButtonType="Image" CommandName="EditRepository" HeaderText="Edit" ImageUrl="~/Images/DMSButton/edit.jpg" ControlStyle-Width="80px"/>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
