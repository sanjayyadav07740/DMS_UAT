<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentVerificationCentrum.aspx.cs" Inherits="DMS.Shared.DocumentVerificationCentrum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center>       
     <table width="400px"  border="0" cellpadding="1" cellspacing="0">
         <tr>
             <td colspan="3"><asp:Label ID="lblTitle" runat="server" Text="Document Verification Centrum" SkinID="Title"></asp:Label></td>
         </tr>
            <tr>             
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblRepositoryName" runat="server" Text="Repository Name : "></asp:Label>
                </td>
                <td align="left" >
                    <asp:DropDownList ID="ddlRepositoryName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRepositoryName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td align="left" style="width: 30px;">
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
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblMetaTemplateName" runat="server" Text="Department Name : "></asp:Label>
                </td>
                <td align="left" >
                    <asp:DropDownList ID="ddlMetaTemplateName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMetaTemplateName_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
                <td align="left" style="width: 30px;" >
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
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblCategoryName" runat="server" Text="Branch Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlCategoryName" runat="server" ToolTip='<%$ Resources:Resource,CategoryName %>' >
                    </asp:DropDownList>
                </td>
                <td align="left" style="width: 30px;">
                    <asp:UpdateProgress ID="uprsProgressCategory" runat="server" DisplayAfter="10000" Visible="false">
                        <ProgressTemplate>
                            <img src="../Images/Loading.gif" height="25px" width="25px" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td></td>
             <td colspan="2" align="left">
                 <asp:ImageButton ID="ibtnSubmit" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnSubmit_Click" />
             </td>
         </tr>
         </table>
    </center>
    <center>
          <table width="931px" cellpadding="0" cellspacing="0" border="0">
         <tr>
                <td align="left" valign="top" colspan="3" >
                    <asp:GridView ID="gvwDocumentList" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    Visible="false" PageSize="8" AllowPaging="true" DataKeyNames="ID,DocumentName" ShowFooter="true"
                      RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  Width="931px" 
                        OnRowDataBound="gvwDocument_RowDataBound"
                        OnPageIndexChanging="gvwDocumentList_PageIndexChanging" OnRowCommand="gvwDocumentList_RowCommand" >
                    <Columns>
                         <asp:TemplateField>
                                <FooterTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFilterGrid" runat="server" Text="Filter By : "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFilterGrid" runat="server">
                                                    <asp:ListItem Text="Pan No" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Name" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="txtFilterGrid" runat="server"></asp:TextBox>
                                            </td>
                                           
                                            <td colspan="2" align="center">
                                                <asp:ImageButton ID="ibtnFilterGrid" runat="server" CausesValidation="false" OnClick="ibtnFilterGrid_Click" SkinID="ViewButton"></asp:ImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:BoundField DataField="ID" HeaderText="Id" SortExpression="id" Visible="false" NullDisplayText="N/A"/>
                         <asp:BoundField DataField="Pan_no" HeaderText="Pan NO" SortExpression="Pan_no"  NullDisplayText="N/A"/>
                        <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" SortExpression="Customer_Name" NullDisplayText="N/A"/>
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" NullDisplayText="N/A" />  
                        <asp:BoundField DataField="FolderName" HeaderText="Folder Name" SortExpression="FolderName" NullDisplayText="N/A" />                       
                        <asp:ButtonField ButtonType="Image" CommandName="DocumentView" HeaderText="View"
                            ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />
                        <asp:ButtonField ButtonType="Image" CommandName="DocApprove" HeaderText="Approve"
                            ImageUrl="~/Images/DMSButton/approve.jpg" ControlStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
                </td>

          
        </tr>
              <tr>
                  <td>&nbsp;</td>
              </tr>
              <tr>
                  <td align="center">
                      <asp:Label ID="lblHeadingAppRejGrid" runat="server" Text="" Visible="false"></asp:Label>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:GridView ID="gvwAppRejectDtls" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                          Visible="false" PageSize="10" AllowPaging="true" DataKeyNames="ID,DocumentName" ShowFooter="true"
                          RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="931px">
                          <Columns>
                              <asp:TemplateField HeaderText="Sr.">
                                  <ItemTemplate>
                                      <%# Container.DataItemIndex+1 %>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:BoundField DataField="ID" HeaderText="Id" SortExpression="id" Visible="false" NullDisplayText="N/A" />
                              <asp:BoundField DataField="Pan_no" HeaderText="Pan NO" SortExpression="Pan_no" NullDisplayText="N/A" />
                              <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" SortExpression="Customer_Name" NullDisplayText="N/A" />
                              <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" NullDisplayText="N/A" />
                              <asp:BoundField DataField="FolderName" HeaderText="Folder Name" SortExpression="FolderName" NullDisplayText="N/A" />
                              <asp:BoundField DataField="Status" HeaderText="Document Status" SortExpression="FolderName" NullDisplayText="N/A" />
                          </Columns>
                      </asp:GridView>
                  </td>
              </tr>
        </table>
        </center>
</asp:Content>
