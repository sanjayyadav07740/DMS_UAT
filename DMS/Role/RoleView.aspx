<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="RoleView.aspx.cs" Inherits="DMS.Role.RoleView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <table width="100%">
        <tr>
            <td align="center" colspan="2">
                <asp:ImageButton ID="ibtnCreateRole" runat="server" OnClick="ibtnCreateRole_Click" SkinID="AddNewButton" />
            </td>
        </tr>
        <tr>

            <td align="right" class="auto-style1">
                <div id="divlblSelRepository" visible="false">
                    <asp:Label ID="lblSelRepository" runat="server" Text="Select Repository :" Width="170px" Font-Size="Medium" Visible="false"></asp:Label>

                </div>
            </td>
            <td align="left" class="auto-style1">
                <div id="divDDLSelRepository" visible="false">
                    <asp:DropDownList ID="DDLSelRepository" runat="server" Height="25px" Width="250px" Visible="false" OnSelectedIndexChanged="DDLSelRepository_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:Label ID="lblErrorMsg" runat="server" Font-Size="Small" ForeColor="Red" Height="20px" Text="* Please select repository" Visible="False" Width="198px"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" class="auto-style1">
                <asp:Label ID="lblNoofRecords" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gvwRoles" runat="server" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="30" AllowSorting="true"
                    DataKeyNames="ID,RoleName"
                    OnPageIndexChanging="gvwRoles_PageIndexChanging"
                    OnRowCommand="gvwRoles_RowCommand"
                    OnRowDataBound="gvwRoles_RowDataBound" ShowFooter="true" OnSorting="gvwRoles_Sorting">

                    <Columns>
                        <asp:TemplateField>
                            <FooterTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFilterGrid" runat="server" Text="Filter By Role : "></asp:Label>
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
                        <asp:BoundField HeaderText="Role Name" DataField="RoleName" NullDisplayText="N/A" SortExpression="RoleName" />
                        <asp:BoundField HeaderText="Display Name" DataField="DisplayName" NullDisplayText="N/A" SortExpression="DisplayName" />
                        <asp:BoundField HeaderText="Role Type" DataField="RoleType" NullDisplayText="N/A" SortExpression="RoleType" />
                        <asp:BoundField HeaderText="CreatedOn" DataField="CreatedOn" NullDisplayText="N/A" DataFormatString="{0:f}" SortExpression="CreatedOn" />
                        <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" NullDisplayText="N/A" SortExpression="CreatedBy" />
                        <asp:BoundField HeaderText="UpdatedOn" DataField="UpdatedOn" NullDisplayText="N/A" DataFormatString="{0:f}" SortExpression="UpdatedOn" />
                        <asp:BoundField HeaderText="UpdatedBy" DataField="UpdatedBy" NullDisplayText="N/A" SortExpression="UpdatedBy" />
                        <asp:BoundField HeaderText="Status" DataField="Status" NullDisplayText="N/A" SortExpression="Status" />
                        <asp:ButtonField ButtonType="Image" HeaderText="Edit" Text="Edit" CommandName="EditRole" ImageUrl="~/Images/DMSButton/edit.jpg" ControlStyle-Width="80px" />
                        <asp:ButtonField ButtonType="Image" HeaderText="Permission" Text="Permission" CommandName="EditPermission" ImageUrl="~/Images/DMSButton/permission.jpg" ControlStyle-Width="80px" />
                        <asp:ButtonField ButtonType="Image" HeaderText="AccessRights" Text="AccessRights" CommandName="EditAccessRights" ImageUrl="~/Images/DMSButton/access.jpg" ControlStyle-Width="80px" />
                    </Columns>
                    <FooterStyle HorizontalAlign="Left" />


                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
