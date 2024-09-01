<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="BulkUpload.aspx.cs" Inherits="DMS.Shared.BulkUpload" %>

<asp:Content ID="cphBulkUploadHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphBulkUploadMain" ContentPlaceHolderID="cphmain" runat="server">
 <script language="javascript" type="text/javascript">
     function checkall(objCheckBox) {
         var control = document.getElementsByTagName('input');
         var i = 0;
         for (i = 0; i < control.length; i++) {
             if (control[i].type == 'checkbox') {
                 if (objCheckBox.checked == true)
                     control[i].checked = true;
                 else
                     control[i].checked = false;
             }
         }
     }
    </script>
    <center><asp:Label ID="lblTitle" runat="server" Text="Bulk Upload"  SkinID="Title"></asp:Label> </center>
    <table>
        <tr>
            <td colspan="3">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
            </td>
            <td align="center" valign="top">
                <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    PageSize="5" AllowPaging="true" DataKeyNames="ID,DownloadPath" OnPageIndexChanging="gvwDocument_PageIndexChanging"
                    OnRowCommand="gvwDocument_RowCommand" OnRowDataBound="gvwDocument_RowDataBound"
                    OnSorting="gvwDocument_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BulkUploadCode" HeaderText="BulkUploadCode" SortExpression="BulkUploadCode"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="UploadStatus" HeaderText="UploadStatus" SortExpression="UploadStatus"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn"
                            NullDisplayText="N/A" DataFormatString="{0:f}" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"
                            NullDisplayText="N/A" />
                        <asp:ButtonField ButtonType="Image" CommandName="Download" HeaderText="Download"
                            ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />
                    </Columns>
                </asp:GridView>

                  <asp:GridView ID="gvwUploadDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" DataKeyNames="ID" OnPageIndexChanging="gvwUploadDocument_PageIndexChanging"
                    OnRowCommand="gvwUploadDocument_RowCommand" OnRowDataBound="gvwUploadDocument_RowDataBound"
                    OnSorting="gvwUploadDocument_Sorting" >
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:checkall(this);" />
                                <asp:ImageButton ID="ibtnDeleteChecked" runat="server" CausesValidation="false" OnClick="ibtnDeleteChecked_Click" ToolTip="Click To Delete All Checked Document."  ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="60px"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRow" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="Size" HeaderText="Document Size" SortExpression="Size" ReadOnly="true"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType" ReadOnly="true"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" 
                            NullDisplayText="N/A" />
                        <asp:ButtonField ButtonType="Image" CommandName="RemoveDocument"  ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 150px;">
                <asp:Label ID="lblSelectFile" runat="server" Text="Select File : "></asp:Label>
            </td>
            <td align="left">
                <asp:FileUpload ID="filUpload" text="Select File" runat="server" />
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="rfvBrowse" runat="server" ControlToValidate="filUpload"
                    Enabled="false" ToolTip='<%$ Resources:Resource,Browse %>'> </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:ImageButton ID="ibtnDescription" runat="server" ToolTip='<%$ Resources:Resource,Description %>'
                    OnClick="ibtnDescription_Click" />
                <asp:ImageButton ID="ibtnUploadPattern" runat="server" ToolTip='<%$ Resources:Resource,UploadPattern %>'
                    OnClick="ibtnUploadPattern_Click" />
                <asp:ImageButton ID="ibtnUploadExcel" runat="server" ToolTip='<%$ Resources:Resource,UploadPattern %>'
                    OnClick="ibtnUploadExcel_Click" />
                <asp:ImageButton ID="ibtnUploadDocument" runat="server" ToolTip='<%$ Resources:Resource,UploadPattern %>'
                    OnClick="ibtnUploadDocument_Click" />
                <asp:ImageButton ID="ibtnSubmit" runat="server" 
                    ToolTip='<%$ Resources:Resource,UploadPattern %>' onclick="ibtnSubmit_Click"
                     />
                <asp:ImageButton ID="ibtnShow" runat="server" OnClick="ibtnShow_Click" ToolTip='<%$ Resources:Resource,Show %>'
                    SkinID="SubmitButton" CausesValidation="true" />
            </td>
        </tr>
    </table>
</asp:Content>
