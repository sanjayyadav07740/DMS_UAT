<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="DocumentVersioning.aspx.cs" Inherits="DMS.Versioning.DocumentVersioning" %>

<asp:Content ID="cphDocumentVersioningHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDocumentVersioningMain" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Versioning" SkinID="Title"></asp:Label>
    </center>
    <table>
        <tr>
            <td colspan="3">                
                <uc:EntityModule ID="enuModule" runat="server" DisplayMetaDataCode="true" />
            </td>
            <td>
            <asp:GridView ID="gvwDocumentList" runat="server" AutoGenerateColumns="false" 
                    AllowSorting="true" Visible="false"
                    PageSize="10" AllowPaging="true" DataKeyNames="ID" 
                    onpageindexchanging="gvwDocumentList_PageIndexChanging" 
                    onrowcommand="gvwDocumentList_RowCommand" 
                    onrowdatabound="gvwDocumentList_RowDataBound" 
                    onselectedindexchanged="gvwDocumentList_SelectedIndexChanged" 
                    onsorting="gvwDocumentList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" />
                        <asp:BoundField DataField="Size" HeaderText="Document Size" SortExpression="Size" />
                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" SortExpression="DocumentType" />
                        <asp:BoundField DataField="DocumentStatus" HeaderText="Document Status" SortExpression="DocumentStatus" />
                        <asp:ButtonField ButtonType="Image" CommandName="DocumentView" HeaderText="View"  ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px"/>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:ImageButton ID="ibtnShow" runat="server" 
                    ToolTip='<%$ Resources:Resource,Show %>' SkinID="SubmitButton" 
                    CausesValidation="true" onclick="ibtnShow_Click" />
            </td>
            
        </tr>
        
        
    </table>
</asp:Content>
