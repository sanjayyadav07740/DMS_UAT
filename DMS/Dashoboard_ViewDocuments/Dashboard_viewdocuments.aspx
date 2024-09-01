<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="Dashboard_viewdocuments.aspx.cs" Inherits="DMS.Dashoboard_ViewDocuments.Dashboard_viewdocuments" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
    Recently Viewed Documents
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <asp:GridView ID="GrvView" runat="server" AutoGenerateColumns="False" AllowPaging="True"
        DataKeyNames="ID,METADATAID,DOCUMENTSTATUSID" OnPageIndexChanging="GrvView_PageIndexChanging"
        OnRowCommand="GrvView_RowCommand" OnRowDataBound="GrvView_RowDataBound">
        <Columns>
         <asp:TemplateField HeaderText="DocumentID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lbldocId" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="METADATAID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblmetadataId" runat="server" Text='<%# Eval("METADATAID")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="RepositoryName">
                <ItemTemplate>
                    <asp:Label ID="lblRepositoryName" runat="server" Text='<%# Eval("RepositoryName")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DocumentName">
                <ItemTemplate>
                    <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DOCUMENTNAME") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DocumentPath">
                <ItemTemplate>
                    <asp:Label ID="lblDocumentpathh" runat="server" Text='<%#Eval("DOCUMENTPATH") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DocumentPath" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblDocumentStatusId" runat="server" Text='<%#Eval("DOCUMENTSTATUSID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Count">
                <ItemTemplate>
                     <asp:Label ID="lblcount" runat="server" Text='<%#Eval("Count")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="btnShow" Text="showLog" CommandName="SHOWLOG" runat="server"
                        SkinID="ViewButton" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
