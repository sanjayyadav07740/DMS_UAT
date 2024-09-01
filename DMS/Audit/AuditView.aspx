<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="AuditView.aspx.cs" Inherits="DMS.Audit.AuditView" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cphAuditViewHead" ContentPlaceHolderID="cphHead" runat="server">
    <script language='javascript' type="text/javascript">
        function expandcollapse(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = "minus.gif";
                }
                else {
                    img.src = "minus.gif";
                }
                img.alt = "Close to view other Customers";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "plus.gif";
                }
                else {
                    img.src = "plus.gif";
                }
                img.alt = "Expand to show Orders";
            }
        } 
    </script>
    <%--<table>
    <tr>
        <td align="center">
            <uc:EntityModule ID="emodModule" runat="server" DisplayMetaTemplate="false" DisplayCategory="false"
                DisplayFolder="false" DisplayMetaDataCode="false" />
        </td>
    </tr>
    </table>--%>
</asp:Content>
 
   
<asp:Content ID="cphAuditViewMain" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%">
        <tr>
            <td>
                <uc:EntityModule ID="EntityModule1" runat="server" DisplayMetaTemplate="false" DisplayCategory="false"
                    DisplayFolder="false" DisplayMetaDataCode="false" />
                <asp:UpdatePanel ID="upnAudit" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                <asp:GridView ID="grvaudit" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                    OnRowCommand="grvaudit_rowcommand" OnRowDataBound="grvaudit_onrowdatabound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="javascript:expandcollapse('div<%# Eval("RepositoryId") %>', 'one');">
                                    <img id="imgdiv<%# Eval("RepositoryId") %>" alt="Click to show/hide Orders for Customer <%# Eval("RepositoryId") %>"
                                        width="9px" src="../Images/DMSButton/plus.gif" />
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RepositoryName" SortExpression="RepositoryName">
                            <ItemTemplate>
                                <asp:Label ID="lblRepositoryName" Text='<%# Eval("RepositoryId") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRepositoryName" Text='<%# Eval("RepositoryName") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblRepositoryName" Text='<%# Eval("RepositoryName") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtRepositoryName" Text='' runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CreatedOn" SortExpression="CreatedOn">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedOn" Text='<%# Eval("CreatedOn") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblCreatedOn" Text='<%# Eval("CreatedOn") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCreatedOn" Text='' runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UpdatedOn" SortExpression="UpdatedOn">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedOn" Text='<%# Eval("UpdatedOn") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblUpdatedOn" Text='<%# Eval("UpdatedOn") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtUpdatedOn" Text='' runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtStatus" Text='' runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action Performed" SortExpression="Action Performed">
                            <ItemTemplate>
                                <asp:Label ID="lblActionPerformed" Text='<%# Eval("Action Performed") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblActionPerformed" Text='<%# Eval("Action Performed") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtActionPerformed" Text='' runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%-- <asp:CommandField HeaderText="Edit" ShowEditButton="True" />--%>
                        <%-- <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="linkDelete" CommandName="Delete" runat="server">Delete</asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="linkAdd" CommandName="Add" runat="server">Add</asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:GridView ID="grvauditreport" runat="server" AllowPaging="True" AllowSorting="True">
                               
                                </asp:GridView>
                                <%--  </div>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </ContentTemplate>   
                </asp:UpdatePanel> 
            </td>
        </tr>
    </table>
</asp:Content>
