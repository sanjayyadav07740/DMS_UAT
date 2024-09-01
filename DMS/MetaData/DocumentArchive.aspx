<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentArchive.aspx.cs" Inherits="DMS.MetaData.DocumentArchive" %>
<asp:Content ID="cphRepositoryHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphRepositoryMain" ContentPlaceHolderID="cphMain" runat="server">

<center><asp:Label ID="lblTitle" runat="server" Text="Document Archive" SkinID="Title"></asp:Label> </center>
    
    <table width="100%">
         <tr>
            <td align="center">
                
                <asp:ImageButton ID="ibtnExportData" runat="server" ToolTip='<%$ Resources:Resource,Export %>' SkinID="ExportButton"
                        CausesValidation="false" OnClick="ibtnExportData_Click" />

            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblNoofRecords" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="gvwDocumentArchive" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" PageSize="15" DataKeyNames="ID" OnPageIndexChanging="gvwDocumentArchive_PageIndexChanging"
                    OnRowCommand="gvwDocumentArchive_RowCommand">                    
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MetaDataCode" HeaderText="MetaData Code" />               
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                        <asp:BoundField DataField="RepositoryName" HeaderText="Repository Name" />
                        <asp:BoundField DataField="MetaTemplateName" HeaderText="MetaTemplate Name" />
                        <asp:BoundField DataField="FolderName" HeaderText="Folder Name" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="UpdatedBy" HeaderText="Deleted By" />
                        <asp:BoundField DataField="UpdatedOn" HeaderText="Deleted On" />

                        <asp:ButtonField ButtonType="Image" CommandName="restore" HeaderText="Restore" ImageUrl="~/Images/DMSButton/restore.png" ControlStyle-Width="80px"/>

                        <asp:ButtonField ButtonType="Image" CommandName="permanetdelete" HeaderText="Permanent Delete" ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>

                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
