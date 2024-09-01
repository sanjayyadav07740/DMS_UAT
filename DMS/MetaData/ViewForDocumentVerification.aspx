<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="ViewForDocumentVerification.aspx.cs" Inherits="DMS.Shared.ViewForDocumentVerification" %>

<asp:Content ID="cphViewForDocumentVerificationHead" ContentPlaceHolderID="cphHead"
    runat="server">
</asp:Content>
<asp:Content ID="cphViewForDocumentVerificationMain" ContentPlaceHolderID="cphMain"
    runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Verification" SkinID="Title"></asp:Label>
    </center>
    <table width="931px" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="left" style="width:400px;">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
            </td>
            <td align="left" valign="top">
                <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    Width="531px" PageSize="5" AllowPaging="true" DataKeyNames="ID,DOCID" OnPageIndexChanging="gvwDocument_PageIndexChanging"
                    OnRowCommand="gvwDocument_RowCommand" OnRowDataBound="gvwDocument_RowDataBound"
                    OnSorting="gvwDocument_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MetaDataCode" HeaderText="MetaData Code" SortExpression="MetaDataCode"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="Entry Completed Count" HeaderText="Entry Completed Count"
                            ControlStyle-Font-Bold="true" ControlStyle-ForeColor="Red" SortExpression="Entry Completed Count"
                            NullDisplayText="N/A" />
                        <asp:ButtonField ButtonType="Image" CommandName="DocumentVerification" HeaderText="Document Verification"
                            ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvwDocumentList" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    Visible="false" PageSize="10" AllowPaging="true" DataKeyNames="ID,MetaDataID,DocumentStatusID"
                    Width="531px" OnPageIndexChanging="gvwDocumentList_PageIndexChanging" OnRowCommand="gvwDocumentList_RowCommand"
                    OnRowDataBound="gvwDocumentList_RowDataBound" OnSorting="gvwDocumentList_Sorting">
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
                        <asp:ButtonField ButtonType="Image" CommandName="DocumentView" HeaderText="View"
                            ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnShow" runat="server" OnClick="ibtnShow_Click" ToolTip='<%$ Resources:Resource,Show %>'
                    SkinID="SubmitButton" CausesValidation="true" />
            </td>
            <td>&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
