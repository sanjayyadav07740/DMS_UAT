<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="UserView.aspx.cs" Inherits="DMS.User.UserView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnCreateUser" runat="server" SkinID="AddNewButton"
                    OnClick="ibtnCreateUser_Click" />

                <asp:ImageButton ID="ibtnExportData" runat="server" ToolTip='<%$ Resources:Resource,Export %>' SkinID="ExportButton"
                        CausesValidation="false" OnClick="ibtnExportData_Click" />

            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblNoofRecords" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="gvwUsers" runat="server" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="15"
                    DataKeyNames="ID,RoleID,UserName,Download,IsViewed,IsEdit,IsMerge,IsSplit,IsDelete"
                    OnRowCommand="gvwUsers_RowCommand"
                    OnPageIndexChanging="gvwUsers_PageIndexChanging"
                    OnRowDataBound="gvwUsers_RowDataBound"
                    AllowSorting="true" OnSorting="gvwUsers_Sorting" ShowFooter="true" Width="1200px">

                    <Columns>

                        <asp:TemplateField Visible="true">
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFilterGrid" runat="server" Text="Filter By Login Name : "></asp:Label>
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtFilterGrid" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnFilterGrid" runat="server" CausesValidation="false" OnClick="ibtnFilterGrid_Click" SkinID="ViewButton"></asp:ImageButton>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFilterErrorMsg" runat="server" Font-Size="Small" ForeColor="Red" Height="20px" Text="No Data Found" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1  %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Login Name" DataField="UserName" NullDisplayText="N/A" SortExpression="UserName" />
                        <asp:BoundField HeaderText="Full Name" DataField="FullName" NullDisplayText="N/A" SortExpression="FullName" Visible="true"/>
                        <asp:BoundField HeaderText="Role Name" DataField="RoleName" NullDisplayText="N/A" SortExpression="RoleName" Visible="true"/>
                        <asp:BoundField HeaderText="Mobile No" DataField="MobileNo" NullDisplayText="N/A" Visible="true"/>
                        <asp:BoundField HeaderText="Address" DataField="Address" NullDisplayText="N/A" Visible="false" />
                        <asp:BoundField HeaderText="City" DataField="City" NullDisplayText="N/A" Visible="false"/>
                        <asp:BoundField HeaderText="state" DataField="StateName" NullDisplayText="N/A" Visible="false" />
                        <asp:BoundField HeaderText="Country" DataField="CountryName" NullDisplayText="N/A" Visible="false" />

                        <asp:BoundField HeaderText="CreatedOn" DataField="CreatedOn" NullDisplayText="N/A" DataFormatString="{0:f}" SortExpression="CreatedOn" />
                        <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" NullDisplayText="N/A" SortExpression="CreatedBy" />
                        <asp:BoundField HeaderText="UpdatedOn" DataField="UpdatedOn" NullDisplayText="N/A" DataFormatString="{0:f}" SortExpression="UpdatedOn" Visible="false" />
                        <asp:BoundField HeaderText="UpdatedBy" DataField="UpdatedBy" NullDisplayText="N/A" SortExpression="UpdatedBy" Visible="false"/>
                        <asp:BoundField HeaderText="Status" DataField="Status" NullDisplayText="N/A" SortExpression="Status" />
                        
                        <asp:ButtonField ButtonType="Image" HeaderText="Edit" Text="Edit" CommandName="EditUser" ImageUrl="~/Images/DMSButton/edit.jpg" ControlStyle-Width="80px" />
                        <asp:ButtonField ButtonType="Image" HeaderText="Permission" Text="Permission" CommandName="EditPermission" ImageUrl="~/Images/DMSButton/permission.jpg" ControlStyle-Width="80px" />
                        <asp:ButtonField ButtonType="Image" HeaderText="AccessRights" Text="AccessRights" CommandName="EditAccessRights" ImageUrl="~/Images/DMSButton/access.jpg" ControlStyle-Width="80px" />

                        <asp:TemplateField HeaderText="Download">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbDownload" runat="server" AutoPostBack="true" OnCheckedChanged="cbDownload_CheckedChanged" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbView" runat="server" AutoPostBack="true" OnCheckedChanged="cbView_CheckedChanged" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" OnCheckedChanged="cbEdit_CheckedChanged" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Merge">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbMerge" runat="server" AutoPostBack="true" OnCheckedChanged="cbMerge_CheckedChanged" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Split">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbSplit" runat="server" AutoPostBack="true" OnCheckedChanged="cbSplit_CheckedChanged" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbDelete" runat="server" AutoPostBack="true" OnCheckedChanged="cbDelete_CheckedChanged" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

</asp:Content>
