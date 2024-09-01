<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityModuleCentrum.ascx.cs" Inherits="DMS.UserControl.EntityModuleCentrum" %>
<asp:UpdatePanel ID="upanPanel" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblRepositoryName" runat="server" Text="Repository Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlRepositoryName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRepositoryName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvRepositoryName" runat="server" ControlToValidate="ddlRepositoryName"
                        InitialValue="-1" ToolTip='<%$ Resources:Resource,RepositoryName  %>'></asp:RequiredFieldValidator>
                    <asp:UpdateProgress ID="uprsProgessRepository" runat="server" DisplayAfter="10000" Visible="false">
                        <ProgressTemplate>
                            <img src="../Images/Loading.gif" height="25px" width="25px" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblMetaTemplateName" runat="server" Text="MetaTemplate  Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlMetaTemplateName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMetaTemplateName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvMetaTemplateName" runat="server" ControlToValidate="ddlMetaTemplateName"
                        InitialValue="-1" ToolTip='<%$ Resources:Resource,MetaTemplateName  %>'> </asp:RequiredFieldValidator>
                    <asp:UpdateProgress ID="uprsProgressMetaTemplateName" runat="server" DisplayAfter="10000" Visible="false">
                        <ProgressTemplate>
                            <img src="../Images/Loading.gif" height="25px" width="25px" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlCategoryName" runat="server" ToolTip='<%$ Resources:Resource,CategoryName %>' OnSelectedIndexChanged="ddlCategoryName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td align="left">
                    <asp:UpdateProgress ID="uprsProgressCategory" runat="server" DisplayAfter="10000" Visible="false">
                        <ProgressTemplate>
                            <img src="../Images/Loading.gif" height="25px" width="25px" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:Label ID="lblSelectFolderName" runat="server" Text="Folder  Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:Panel ID="panFolder" runat="server" Width="200px" Height="200px">
                        <asp:TreeView ID="tvwFolder" runat="server" OnSelectedNodeChanged="tvwFolder_SelectedNodeChanged">
                        <SelectedNodeStyle BackColor="Yellow" ForeColor="Green" />
                        </asp:TreeView>
                    </asp:Panel>
                </td>
                <td align="left">
                 <asp:UpdateProgress ID="uprsProgressFolder" runat="server" DisplayAfter="10000" Visible="false">
                        <ProgressTemplate>
                            <img src="../Images/Loading.gif" height="25px" width="25px" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:Label ID="lblMetaDataCode" runat="server" Text="MetaDataCode :"></asp:Label>
                </td>
                <td align="left">
                     <asp:DropDownList ID="ddlMetaDataCode" runat="server" AutoPostBack="false" ToolTip='<%$ Resources:Resource,MetaDataCode  %>'>
                      </asp:DropDownList>
                </td>
                <td align="left">
                 <asp:UpdateProgress ID="uprsProgressMetaDataCode" runat="server" DisplayAfter="10000" Visible="false">
                        <ProgressTemplate>
                            <img src="../Images/Loading.gif" height="25px" width="25px" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
    
    </ContentTemplate>
</asp:UpdatePanel>
