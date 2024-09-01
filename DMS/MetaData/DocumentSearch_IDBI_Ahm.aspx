<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentSearch_IDBI_Ahm.aspx.cs" Inherits="DMS.Shared.DocumentSearch_IDBI_Ahm" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphSearchDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
  <script type="text/javascript">
      function Validate() {
          var isValid = false;
          var regex = /^[0-9-+()]*$/;
          isValid = regex.test(document.getElementById("txtCustId").value);
          
          return isValid;
      }
</script>
</asp:Content>
<asp:Content ID="cphSearchDocumentMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Search" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table width="931" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
<asp:Label ID="lblCustId" runat="server" Text="Customer Id"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCustId" runat="server" MaxLength="8" OnTextChanged="txtCustId_TextChanged" AutoPostBack="True"></asp:TextBox>
                </td>
            </tr>
           
            <tr>
                <td>
                    <asp:Label ID="lblAccNo" runat="server" Text="Account Number"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAccNo" runat="server" MaxLength="16" onclick="check_field('txtAccNo');" OnTextChanged="txtAccNo_TextChanged"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Label ID="lblBarCode" runat="server" Text="BarCode"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBarCode" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
           
             <tr>
                <td>
                    <asp:Label ID="lblBoxNo" runat="server" Text="Box Number"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBoxNo" runat="server" MaxLength="6" OnTextChanged="txtBoxNo_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:ImageButton ID="ibtnSearch" runat="server" SkinID="SubmitButton" OnClick="ibtnSearch_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
                    </td>
                </tr>
            <tr>
                <td colspan="2" align="center">
                     <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="931px"
                        PageSize="5" AllowPaging="True" DataKeyNames="ID,MetaDataID,DocumentStatusID,DocumentName" OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand">
                        <Columns>
                            <%--<asp:TemplateField>
                                <FooterTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFilterGrid" runat="server" Text="Filter By : "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFilterGrid" runat="server">
                                                    <asp:ListItem Text="DocumentName" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Tag" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFilterGrid" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtnFilterGrid" runat="server" CausesValidation="false" OnClick="ibtnFilterGrid_Click" SkinID="ViewButton"></asp:ImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                NullDisplayText="N/A" />    
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A"  readonly="true" />
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />
                           <%-- <asp:TemplateField HeaderText="DocumentName" SortExpression="DocumentName">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDocName" runat="server" Text='<%# Bind("DocumentName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDocName" runat="server" Text='<%# Bind("DocumentName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A"
                                Visible="false"  readonly="true" />
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A"  readonly="true" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A"   readonly="true"/>
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A"  readonly="true" />
                            <asp:BoundField DataField="SHCIL_Barcode_Date" HeaderText="SHCIL Barcode Date" SortExpression="SHCIL Barcode Date" NullDisplayText="N/A" ReadOnly="true" />
                            <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" >
                             

<ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />           
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </td>
            </tr>
            </table>
        </center>
    </asp:Content>
