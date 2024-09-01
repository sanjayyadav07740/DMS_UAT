<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMasterPage.Master"
    CodeBehind="MetaTemplateFieldCreation.aspx.cs" Inherits="DMS.MetaTemplate.MetaTemplateFieldCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title" Text="MetaTemplate Field Creation"></asp:Label>
    </center>
    <table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right" style="width:45%;">
                &nbsp;
            </td>
            <td align="left">
                &nbsp;
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                &nbsp;
            </td>
            <td align="left">
                &nbsp;
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblRepository" runat="server" Text="Repository Name :" 
                    Visible="False"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlRepository" runat="server" AutoPostBack="True" 
                    OnSelectedIndexChanged="ddlRepository_SelectedIndexChanged" Visible="False">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvRepository" runat="server" ControlToValidate="ddlRepository"
                    InitialValue="-1" ToolTip='<%$ Resources:Resource,RepositoryName  %>' 
                    Visible="False"></asp:RequiredFieldValidator>
            </td>
            <td align="left">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblMetaTemplateName" runat="server" Text="MetaTemplate Name :" 
                    Visible="False"></asp:Label>
            </td>
            <td align="left">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlMetaTemplate" runat="server" AutoPostBack="True" 
                            OnSelectedIndexChanged="ddlMetaTemplate_SelectedIndexChanged" Visible="False">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMetaTemplateName" InitialValue="-1" runat="server"
                            ControlToValidate="ddlMetaTemplate" 
                            ToolTip='<%$ Resources:Resource,RepositoryName  %>' Visible="False"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRepository" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td align="left">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblFieldName" runat="server" Text="Field Name :"></asp:Label>
            </td>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFieldName" runat="server" AutoPostBack="false"></asp:TextBox>
                        <ajaxtoolkit:FilteredTextBoxExtender ID="fteFieldName" runat="server" SkinID="String" TargetControlID="txtFieldName"></ajaxtoolkit:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvFieldName" runat="server" ControlToValidate="txtFieldName"
                            ToolTip='<%$ Resources:Resource,RepositoryName  %>' Enabled="False"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblFieldDataType" runat="server" Text="Field DataType :"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlFieldDataType" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvFieldDataType" InitialValue="-1" runat="server"
                    ControlToValidate="ddlFieldDataType" ToolTip='<%$ Resources:Resource,RepositoryName  %>'
                    Enabled="False"></asp:RequiredFieldValidator>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblFieldType" runat="server" Text="Field Type :"></asp:Label>
            </td>
            <td align="left">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlFieldType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFieldType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblFieldLength" runat="server" Text="Field Length :"></asp:Label>
            </td>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFieldLength" runat="server" AutoPostBack="false"></asp:TextBox>
                        <ajaxtoolkit:FilteredTextBoxExtender ID="fteFieldLength" runat="server" SkinID="OnlyNumber" TargetControlID="txtFieldLength"></ajaxtoolkit:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvFieldLength" runat="server" ControlToValidate="txtFieldLength"
                            ToolTip='<%$ Resources:Resource,RepositoryName  %>' Enabled="False"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblIsPrimary" runat="server" Text="Is Primary Key :"></asp:Label>
            </td>
            </td>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkIsPrimary" runat="server" AutoPostBack="True" OnCheckedChanged="chkIsPrimary_CheckedChanged">
                        </asp:CheckBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblStatus" runat="server" Text="Status :"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlStatus" runat="server">
                </asp:DropDownList>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnAdd" runat="server" OnClick="ibtnAdd_Click" SkinID="AddButton"/>&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" OnClick="ibtnCancel_Click" SkinID="CancelButton" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2" align="center">
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gdvMetaTemplateFields" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                            AllowPaging="true" PageSize="5" OnRowCommand="gdvMetaTemplateFields_RowCommand"
                            OnPageIndexChanging="gdvMetaTemplateFields_PageIndexChanging" OnRowCancelingEdit="gdvMetaTemplateFields_RowCancelingEdit"
                            OnRowDeleting="gdvMetaTemplateFields_RowDeleting" OnRowEditing="gdvMetaTemplateFields_RowEditing"
                            OnRowUpdating="gdvMetaTemplateFields_RowUpdating" 
                            onrowdatabound="gdvMetaTemplateFields_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %></ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("ID")%>'></asp:Label>   
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Field Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFieldName" runat="server" Text='<%#Bind("FieldName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data Type">
                                    <ItemTemplate>
                                    <asp:DropDownList ID="ddlFieldDataType" runat="server" Enabled="false">
                                        </asp:DropDownList>
                                        <%--<asp:Label ID="lblFieldDataType" runat="server" Text='<%#Eval("FieldDataTypeID") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlFieldDataType" runat="server">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtPhone" runat="server" Text='<%#Bind("FieldDataTypeID") %>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Field Type">
                                    <ItemTemplate>
                                    <asp:DropDownList ID="ddlFieldType" runat="server" Enabled="false">
                                        </asp:DropDownList>
                                        <%--<asp:Label ID="lblFieldType" runat="server" Text='<%#Eval("FieldTypeID") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlFieldType" runat="server">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtEmail" runat="server" Text='<%#Bind("FieldTypeID") %>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Length">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLength" runat="server" Text='<%#Eval("FieldLength") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFieldLength" runat="server" Text='<%#Bind("FieldLength") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Primary Key">
                                    <ItemTemplate>
                                    <asp:CheckBox ID="chkIsPrimary" runat="server" Enabled="false"> </asp:CheckBox>
                                       <%-- <asp:Label ID="lblIsPrimary" runat="server" Text='<%#Eval("IsPrimary") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:CheckBox ID="chkIsPrimary" runat="server"> </asp:CheckBox>
                                        <%--<asp:TextBox ID="txtHobbies" runat="server" Text='<%#Bind("IsPrimary") %>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Repository" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRepository" runat="server" Text='<%#Eval("RepositoryID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlRepository" runat="server">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtHobbies" runat="server" Text='<%#Bind("RepositoryID") %>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MetaTemplate" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMetaTemplate" runat="server" Text='<%#Eval("MetaTemplateID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlMetaTemplate" runat="server">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtHobbies" runat="server" Text='<%#Bind("MetaTemplateID") %>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" Visible="true">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>--%>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtHobbies" runat="server" Text='<%#Bind("Status") %>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Edit" ShowEditButton="true" ButtonType="Image" EditImageUrl="~/Images/DMSButton/edit.jpg" UpdateImageUrl="~/Images/DMSButton/update.jpg" CancelImageUrl="~/Images/DMSButton/cancel.jpg" ControlStyle-Width="80px"/>
                                <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ButtonType="Image"  DeleteImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="80px"/>
                                <%--<asp:BoundField DataField="FieldName" HeaderText="Field Name" />
                <asp:BoundField DataField="FieldDataTypeID" HeaderText="Data Type" />
                <asp:BoundField DataField="FieldTypeID" HeaderText="Allow Null" />
                <asp:BoundField DataField="FieldLength" HeaderText="Length" />
                <asp:BoundField DataField="IsPrimary" HeaderText="Is Primary Key" />
                <asp:BoundField DataField="RepositoryID" HeaderText="Repository" />
                <asp:BoundField DataField="MetaTemplateID" HeaderText="MetaTemplate" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:ButtonField ButtonType="Image" CommandName="Edit" HeaderText="Edit"/>     
                <asp:ButtonField ButtonType="Image" CommandName="Delete" HeaderText="Delete"/>                        --%>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRepository" />
                        <asp:AsyncPostBackTrigger ControlID="ddlMetaTemplate" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2" align="center">
                <asp:ImageButton ID="ibtnSave" runat="server" OnClick="ibtnSave_Click" 
                    SkinID="SubmitButton" Visible="False"/>&nbsp;
                <asp:ImageButton ID="ibtnCancelAll" runat="server" 
                    OnClick="ibtnCancelAll_Click" SkinID="CancelButton" Visible="False"/>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
