<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentRename.aspx.cs" Inherits="DMS.Shared.DocumentRename" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphSearchDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
    <script
  src="https://code.jquery.com/jquery-1.12.4.js"
  integrity="sha256-Qw82+bXyGq6MydymqBxNPYTaUXXq7c8v3CwiYwLLNXU="
  crossorigin="anonymous"></script>
    <style>
        tr.gridheader th,tr.gridrow td {
            padding: 5px 10px;
        }
        select{
            min-height:23px;
        }
        .centerbtn {
            margin: 0 auto;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="cphSearchDocumentMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Rename" SkinID="Title"></asp:Label>
    </center>
    <br />
    <center>
        <table>
            <tr>
                <td>
                    <asp:Label ID="DocName" Text="Document Name" runat="server"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:DropDownList ID="ddlCriteria" runat="server">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem>Equal</asp:ListItem>
                        <asp:ListItem>Like</asp:ListItem>
                        <asp:ListItem>Not</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;&nbsp;<asp:TextBox ID="txtDocName" Width="400px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center"><br />
                    <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" OnClick="ibtnSubmit_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" Visible="false" />
                </td>
            </tr>
            
            </table>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="grvDocumentDetails" runat="server" AutoGenerateColumns="false" DataKeyNames="ID,MetaDataID,MetatemplateId,DocumentName,DocumentType,PageCount" OnRowCommand="grvDocumentDetails_RowCommand" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr.No">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Metadata Code" DataField="MetaDataCode" ReadOnly="true" />
                            <asp:BoundField HeaderText="Repository Name" DataField="RepositoryName" ReadOnly="true" />
                            <asp:BoundField HeaderText="MetaTemplate Name" DataField="MetaTemplateName" ReadOnly="true" />
                            <%-- <asp:BoundField HeaderText="Document Name" DataField="documentname" />--%>
                            <asp:TemplateField HeaderText="Document Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DocumentName" runat="server" Text='<%# Eval("documentname") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_DocumentName" runat="server" Text='<%# System.IO.Path.GetFileNameWithoutExtension(Eval("documentname").ToString())  %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" ControlStyle-CssClass="centerbtn" />
                            <asp:TemplateField HeaderText="Document Rename">
                                <ItemTemplate>
                                    <%--<asp:LinkButton Text="Edit" runat="server" CommandName="DocumentRename" />--%>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ControlStyle-Width="80px" CommandArgument="<%# Container.DataItemIndex %>" CommandName="DocumentRename" ImageUrl="~/Images/DMSButton/edit.jpg" CssClass="centerbtn" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--<asp:LinkButton Text="Update" runat="server" OnClick="OnUpdate" />--%>
                                    <asp:ImageButton ID="ibtnUpdate" runat="server" ControlStyle-Width="80px" OnClick="OnUpdate" ImageUrl="~/Images/DMSButton/update.jpg" />
                                    <%--<asp:LinkButton Text="Cancel" runat="server" OnClick="OnCancel" />--%>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ControlStyle-Width="80px" OnClick="OnCancel" ImageUrl="~/Images/DMSButton/cancel.jpg" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>    
                </td>
            </tr>
        </table>
         </center>

      <asp:Panel ID="pnlpopup" runat="server"  BackColor="White" Height="175px"
            Width="300px" Style="z-index:11111;background-color: White; position: absolute; left: 35%; top: 12%;  
border: outset 2px gray;padding:5px;display:none;">
            <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
                <tr style="background-color: #0924BC">
                    <td colspan="2" style="color:White; font-weight: bold; font-size: 1.2em; padding:3px"
                        align="center">
                        Document Rename <a id="closebtn" style="color: white; float: right;text-decoration:none" class="btnClose"  href="#">X</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 45%; text-align: center;">
                        <asp:Label ID="LabelValidate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Document Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDocumentName" runat="server" />
                    </td>
                </tr>                
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" />
                        <asp:Button ID="btnClose" runat="server" Text="Close"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </asp:Content>
