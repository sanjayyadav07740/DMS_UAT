<%@ Page Title="Document Search" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="SearchDocument2.aspx.cs" Inherits="DMS.Shared.SearchDocument2" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="cphSearchDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphSearchDocumentMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Advanced Search" SkinID="Title"></asp:Label>
    </center>
    <center>
        <asp:UpdatePanel ID="upanPanel" runat="server">
            <ContentTemplate>
                <table width="1190" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <table width="590" cellpadding="7" cellspacing="1" border="1">
                                <tr>
                                    <td align="left" valign="top" colspan="2" class="gridheader" style="font-weight: bold">Collection Search Options
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" align="left">
                                        <asp:Label ID="lbl1" runat="server" Text="Repository Name :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRepository" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRepository_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvRepositoryName" runat="server" ControlToValidate="ddlRepository"
                                            InitialValue="-1" ToolTip='<%$ Resources:Resource,RepositoryName  %>'></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="MetaTemplate Name :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMetatemplate" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMetatemplate_SelectedIndexChanged"></asp:DropDownList>
                                        <%-- <asp:RequiredFieldValidator ID="rfvMetaTemplateName" runat="server" ControlToValidate="ddlMetatemplate"
                                            InitialValue="-1" ToolTip='<%$ Resources:Resource,MetaTemplateName  %>'> </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Category Name :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Folder :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TreeView ID="tvwFolder" runat="server" ImageSet="Simple" OnSelectedNodeChanged="tvwFolder_SelectedNodeChanged">
                                            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                NodeSpacing="0px" VerticalPadding="5px"></NodeStyle>
                                            <ParentNodeStyle Font-Bold="False" />
                                            <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="5px"
                                                VerticalPadding="5px" />
                                        </asp:TreeView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Folder Option :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkRecurse" runat="server" Text="Recursive SubFolders" Checked="true"></asp:CheckBox>
                                        <asp:Label ID="Label5" runat="server" Text="To Selct Recursive SubFolder, it will search all nested folders inside the selected folder(s)."
                                            CssClass="blueboldvardana"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" valign="top">
                            <table width="550" cellpadding="5" cellspacing="1" border="1">
                                <tr>
                                    <td align="left" valign="top" colspan="2" class="gridheader" style="font-weight: bold">Document Search Options
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="width: 200px;">
                                        <asp:Label ID="lblSearchBy" runat="server" Text="Search By :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdblSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdblSearchBy_SelectedIndexChanged">
                                            <asp:ListItem Text="All Folders" Value="4" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="MetaTemplate Field" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Document Name / Tag" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Document Content" Value="3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="trField" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblField" runat="server" Text="MetaTemplate Field : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlField" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvField" runat="server" ControlToValidate="ddlField"
                                            Enabled="false" InitialValue="-1" ToolTip='<%$ Resources:Resource,Field  %>'></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trCriteria" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblCriteria" runat="server" Text="Search Criteria : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlCriteria" runat="server">
                                            <asp:ListItem Text="Contains" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Begins with" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Ends with" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Exact word" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Any of these words" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trSearchText" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblTextToSeach" runat="server" Text="Text To Search : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTextToSeach" runat="server" TextMode="MultiLine" Rows="3" Height="62px" Width="237px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trFromDate" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblFromDate" runat="server" Text="Uploaded From Date : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                        <ajaxtoolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                                            Mask="99/99/9999" MaskType="Date">
                                        </ajaxtoolkit:MaskedEditExtender>
                                        <ajaxtoolkit:MaskedEditValidator ID="mevFromDate" runat="server" ControlExtender="meeFromDate"
                                            ControlToValidate="txtFromDate" InvalidValueMessage="Invalid Date (mm/dd/yyyy)" CssClass="redboldverdana10"></ajaxtoolkit:MaskedEditValidator>
                                        <%-- <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                            Text="*"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr id="trToDate" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblToDate" runat="server" Text="Uploaded To Date : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                        <ajaxtoolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                                            Mask="99/99/9999" MaskType="Date">
                                        </ajaxtoolkit:MaskedEditExtender>
                                        <ajaxtoolkit:MaskedEditValidator ID="mevToDate" runat="server" ControlExtender="meeToDate"
                                            ControlToValidate="txtToDate" InvalidValueMessage="Invalid Date (mm/dd/yyyy)" CssClass="redboldverdana10"></ajaxtoolkit:MaskedEditValidator>
                                        <%-- <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                            Text="*"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" style="height: 15px; padding: 5px 5px 5px 5px;">
                            <asp:Button ID="ibtnShow" runat="server" CausesValidation="true" OnClick="ibtnShow_Click" Text="Search.."
                                BackColor="#023361" CssClass="button" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="height: 15px; padding: 5px 5px 5px 5px;">
                            <asp:Label ID="LblCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <table width="100%" border="0" style="background-color: white;">
                                <tr>
                                    <td width="900px">
                                        <%-- <asp:DataList ID="dlDocument" runat="server" RepeatDirection="Horizontal" DataKeyField="ID"
                                            EnableViewState="True" RepeatColumns="3" RepeatLayout="Table" Width="900px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="5" width="300px" border="1">
                                                    <tr runat="server" id="tr_category">
                                                        <td align="center" valign="middle" style="background-color: lightgray; width: 80px;">
                                                            <asp:Image ID="imgDocType" runat="server" ImageUrl='<%# Eval("DocumentTypeURL") %>' Width="80px" Height="80px" />
                                                        </td>
                                                        <td style="background-color: ghostwhite; vertical-align: top;">
                                                            <asp:Label ID="lblHeader" runat="server" Font-Bold="true" Text='<%# Eval("DocumentName") %>' Font-Size="X-Small"></asp:Label>
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("FolderName") %>' Font-Size="X-Small"></asp:Label>
                                                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("CreatedOn") %>' Font-Size="X-Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <div style="margin: 5px; align-content:center;">
                                            <asp:Repeater ID="rptPager" runat="server">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                        CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:Label ID="lblPager" runat="server"></asp:Label>
                                        </div>
                                        <br />--%>
                                        <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowPaging="true" Width="900px"
                                            DataKeyNames="ID,MetaDataID,DocumentName" Style="margin-top: 0px" Visible="false" PageSize="100"
                                            OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:BoundField DataField="FolderName" HeaderText="Folder" NullDisplayText="N/A" ReadOnly="true" />--%>
                                                <asp:TemplateField HeaderText="Folder Path / Document Name" ItemStyle-Width="500">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblDocName" runat="server" Text='<%# Eval("DocumentName") %>' ReadOnly="true" SkinID="NormalText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="width: 50px">
                                                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("FolderName") == "" ? "" : "Path :" %>' ReadOnly="true"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblFolderName" runat="server" Text='<%# Eval("FolderName") %>' ReadOnly="true" SkinID="NormalText"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="width: 50px">
                                                                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("Tag") == "" ? "" : "Tag :" %>' ReadOnly="true"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("Tag") %>' ReadOnly="true" SkinID="NormalText"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                        <%-- <asp:Label ID="lblTag" runat="server" Text=<%# Eval("Tag") %> ReadOnly="true"></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc. Type" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgDocType" runat="server" ImageUrl='<%# Eval("DocumentTypeURL") %>' Width="30px" Height="30px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Size" HeaderText="Size" NullDisplayText="N/A" ReadOnly="true" />
                                                <%--<asp:BoundField DataField="DocumentType" HeaderText="DocumentType" NullDisplayText="N/A" ReadOnly="true" />--%>
                                                <asp:ButtonField ButtonType="Button" CommandName="DocumentSearch" HeaderText="View Document" Text="View" ControlStyle-CssClass="button" ControlStyle-BackColor="#023361">
                                                    <ControlStyle BackColor="#023361" CssClass="button" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td width="300px">
                                    </td>
                                </tr>
                            </table>

                            <br />
                            <asp:Button ID="ibtnDownloadFiles" runat="server" CausesValidation="false" Text="Download All" CssClass="button"
                                BackColor="#023361" Visible="false" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>
