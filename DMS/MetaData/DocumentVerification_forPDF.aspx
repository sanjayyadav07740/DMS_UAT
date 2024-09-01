<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentVerification_forPDF.aspx.cs" Inherits="DMS.Shared.DocumentVerification_forPDF" %>

<%@ Register Src="~/UserControl/DocumentViewer.ascx" TagPrefix="li" TagName="DocumentViewer" %>

<%--<asp:Content ID="cphDocumentVerificationHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>--%>
<asp:Content ID="cphDocumentVerificationMain" ContentPlaceHolderID="cphmain" runat="server">
    <br />
    <center><asp:Label ID="lblTitle" runat="server" Text="View Document Entry"  SkinID="Title"></asp:Label> </center>
    <center>

        <div style="width:100%;">
            <center>
                <table runat="server" visible="false"> 
                    <tr>
                        <td>
                            <asp:GridView ID="gvwDocumentView" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="500px" DataKeyNames="ID,MetaDataID,DocumentName" OnRowCommand="gvwDocumentView_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                NullDisplayText="N/A" />    
                            
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentView"  HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:ButtonField>

                        </Columns>
                    </asp:GridView>
                            <br /><br />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <cc1:ShowPdf ID="pdfViewer" runat="server" Height="500px" Width="600px" Style="z-index: -1;" />
                              <%--<li:DocumentViewer runat="server" ID="DocumentViewer" Height="425" Width="460" Style="z-index:-1" />--%>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:GridView ID="gdv_DocumentField" runat="server" AllowPaging="True" AutoGenerateColumns="false" EnableViewState="false" DataKeyNames="id,FieldName,FieldData" OnRowCancelingEdit="gdv_DocumentField_RowCancelingEdit"
                                OnRowEditing="gdv_DocumentField_RowEditing" OnRowUpdating="gdv_DocumentField_RowUpdating" Height="250px" Width="400px">
                                <Columns>
                                    <asp:BoundField HeaderText="ID" Visible="false" DataField="id" />
                                    <asp:TemplateField HeaderText="Field Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFieldName" runat="server" Text='<%# Eval("FieldName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Field Data">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEditFieldData" runat="server" Text='<%# Eval("FieldData") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFieldData" runat="server" Text='<%# Eval("FieldData") %>' Width="200px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Link" ShowEditButton="true" Visible="true" HeaderText="Edit" />
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                            <asp:ImageButton  ID="ibtnCancel" runat="server" CausesValidation="false" ToolTip='<%$ Resources:Resource,Back %>'  SkinID="BackButton" OnClick="ibtnCancel_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>

        <%--<div class="container-fluid">
            <div class="row">
                <div class="col-lg-8">
                   <cc1:ShowPdf ID="pdfViewer" runat="server" Height="550px" Width="800px" Style="z-index: -1;" />
                </div>
                <div class="col-lg-4" style="margin-top: 3%;">
                    <asp:GridView ID="gdv_DocumentField"  runat="server" AllowPaging="True" AutoGenerateColumns="false" EnableViewState="false" DataKeyNames="id,FieldName,FieldData" OnRowCancelingEdit="gdv_DocumentField_RowCancelingEdit" 
                        OnRowEditing="gdv_DocumentField_RowEditing" OnRowUpdating="gdv_DocumentField_RowUpdating" Width="800px">
                        <Columns>
                            <asp:BoundField HeaderText ="ID" Visible="false" DataField="id"/>
                            <asp:TemplateField HeaderText="Field Name" >
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldName" Style="font-family:'Kruti Dev 010'" runat="server" Text='<%# Eval("FieldName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Field Data" >
                                <ItemTemplate>
                                    <asp:Label ID="lblEditFieldData" Style="font-family:'Kruti Dev 010'" runat="server" Text='<%# Eval("FieldData") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditFieldData" Style="font-family:'Kruti Dev 010'" runat="server" Text='<%# Eval("FieldData") %>' Width="150px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" ShowEditButton="true" Visible="true" HeaderText="Edit" />
                        </Columns>
                    </asp:GridView>
              <br />
                    <br />
                    <br />
                    <div class="row">
                        <asp:ImageButton  ID="ibtnApprove" runat="server" SkinID="ApproveButton" ToolTip='<%$ Resources:Resource,Submit %>' CausesValidation="false" OnClick="ibtnApprove_Click" Visible="false"/>
                        <asp:ImageButton  ID="ibtnReject" runat="server" CausesValidation="false" ToolTip='<%$ Resources:Resource,Back %>'  SkinID="RejectButton" OnClick="ibtnReject_Click" Visible="false" />
                        
                    </div>
                </div>
                <div>
                    <asp:ImageButton  ID="ibtnCancel" runat="server" CausesValidation="false" ToolTip='<%$ Resources:Resource,Back %>'  SkinID="BackButton" OnClick="ibtnCancel_Click" Visible="true" />
                </div>
            </div>
        </div>--%>

    </center>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
</asp:Content>--%>
