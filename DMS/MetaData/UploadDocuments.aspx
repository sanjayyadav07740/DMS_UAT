<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="UploadDocuments.aspx.cs" Inherits="DMS.Shared.UploadDocuments" %>

<asp:Content ID="cphUploadDocumentsHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 249px;
        }
    </style>
</asp:Content>
<asp:Content ID="cphUploadDocumentsMain" ContentPlaceHolderID="cphMain" runat="server">
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
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Upload Document" SkinID="Title"></asp:Label>
    </center>
    <table border="0" cellpadding="0" cellspacing="3" width="931">
        <tr>
            <td align="left" colspan="3">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
            </td>
        
            <td align="left" valign="top" colspan="3">
                <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" DataKeyNames="ID" OnPageIndexChanging="gvwDocument_PageIndexChanging"
                    OnRowCommand="gvwDocument_RowCommand" OnRowDataBound="gvwDocument_RowDataBound"
                    OnSorting="gvwDocument_Sorting" OnRowCancelingEdit="gvwDocument_RowCancelingEdit"
                    OnRowEditing="gvwDocument_RowEditing" OnRowUpdating="gvwDocument_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:checkall(this);" />
                                <asp:ImageButton ID="ibtnDeleteChecked" runat="server" CausesValidation="false" OnClick="ibtnDeleteChecked_Click"
                                    ToolTip="Click To Delete All Checked Document." ImageUrl="~/Images/DMSButton/delete.jpg"
                                    ControlStyle-Width="60px" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRow" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"
                            ReadOnly="true" NullDisplayText="N/A" />
                        <asp:BoundField DataField="Size" HeaderText="Document Size" SortExpression="Size"
                            ReadOnly="true" Visible="false" NullDisplayText="N/A" />
                        <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                            ReadOnly="true" Visible="false" NullDisplayText="N/A" />
                        <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                        <%--<asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" ReadOnly="true" Visible="false"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="NotificationBefore" HeaderText="Notification Before" ReadOnly="true"
                            Visible="false" NullDisplayText="N/A" />
                        <asp:BoundField DataField="NotificationInterval" HeaderText="Notification Interval"
                            ReadOnly="true" Visible="false" NullDisplayText="N/A" />--%>
                        <asp:CommandField ButtonType="Image" ShowEditButton="true" CausesValidation="false"
                            EditImageUrl="~/Images/DMSButton/edit.jpg" UpdateImageUrl="~/Images/DMSButton/update.jpg"
                            CancelImageUrl="~/Images/DMSButton/cancel.jpg" ControlStyle-Width="80px" />
                        <asp:ButtonField ButtonType="Image" CommandName="RemoveDocument" ImageUrl="~/Images/DMSButton/delete.jpg"
                            ControlStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 150px;">
                <asp:Label ID="lblSelectFile" runat="server" Text="Select File : "></asp:Label>
            </td>
            <td align="left">
                <%--<asp:FileUpload ID="filUpload" text="Select File" runat="server" />--%>
                <asp:FileUpload ID="filUpload" AllowMultiple="true" stext="Select File" runat="server" CssClass="form-control"  />
            </td>
            <td align="left" class="auto-style1">
                <asp:RequiredFieldValidator ID="rfvBrowseButten" runat="server" ControlToValidate="filUpload"
                    ToolTip='<%$ Resources:Resource,Browse %>'> </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 150px;">
                <asp:Label ID="lblTag" runat="server" Text="Tag Value : "></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTagValue" runat="server" MaxLength="100" ToolTip="Enter Tag Value For Document." />
            </td>
            <td align="left" class="auto-style1">&nbsp;
            </td>
        </tr>
        <%--<tr>
            <td align="left" style="width: 150px;">
                <asp:Label ID="lblExpiryDate" runat="server" Text="Expiry Date : "></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtExpiryDate" runat="server"></asp:TextBox>
                <ajaxtoolkit:CalendarExtender ID="ceExpiryDate" TargetControlID="txtExpiryDate" runat="server">
                </ajaxtoolkit:CalendarExtender>
            </td>
            <td>&nbsp;</td>
        </tr>--%>
        <%--<tr>
            <td align="left" style="width: 150px;">
                <asp:Label ID="lblNotification" runat="server" Text="Notification Before (in days) :"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNotification" runat="server"></asp:TextBox>
            </td>
            <td align="left">
                <asp:Label ID="lblNotificationError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>--%>
        <%--<tr>
            <td align="left" style="width: 150px;">
                <asp:Label ID="lblNotificationInterval" runat="server" Text="Notification Interval (in days) : "></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNotificationInterval" runat="server"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>--%>
        <tr>
            <td align="center" colspan="3">
                <asp:ImageButton ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" ToolTip='<%$ Resources:Resource,Submit %>'
                    SkinID="SubmitButton" EnableTheming="True" />
                <asp:ImageButton ID="ibtnBack" runat="server" OnClick="ibtnBack_Click" CausesValidation="false"
                    SkinID="CancelButton" ToolTip='<%$ Resources:Resource,Back %>' />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:ImageButton ID="ibtnUpload" runat="server" OnClick="ibtnUpload_Click" CausesValidation="false"
                    SkinID="UploadButton" ToolTip='<%$ Resources:Resource,Upload %>' />
            </td>
        </tr>
    </table>
</asp:Content>
