<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditViews.aspx.cs" Inherits="DMS.Audit.AuditViews"
    MasterPageFile="~/MainMasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="cphSearchDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
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
</asp:Content>
<asp:Content ID="cphSearchDocumentMain" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%">
        <tr>
            <td colspan="4" align="center">
                <table>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblType" Text="AuditView" runat="server"></asp:Label>
                    </td>
                    <td align="center" colspan="2">
                        <%-- <uc:EntityModule ID="emodModule" runat="server" DisplayMetaTemplate="false" DisplayCategory="false"
                    DisplayFolder="false" DisplayMetaDataCode="false" />--%>
                        <asp:DropDownList ID="ddlTypeOfdetails" runat="server">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem Value="1">Repository</asp:ListItem>
                            <asp:ListItem Value="2">Metatemplate</asp:ListItem>
                            <asp:ListItem Value="3">Category</asp:ListItem>
                            <asp:ListItem Value="4">Folder</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ImageButton ID="imgbtn" runat="server" OnClick="imgbtn_Click" SkinID="ViewButton" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grvaudit" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="80%"
                    OnRowCommand="grvaudit_RowCommand" OnRowDataBound="grvaudit_RowDataBound" DataKeyNames="MainID"
                    OnPageIndexChanging="grvaudit_PageIndexChanging" PageSize="5">
                    <Columns>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <a href="javascript:expandcollapse('div<%# Eval("MainID") %>', 'one');">
                                    <img id="imgdiv<%# Eval("MainID") %>" alt="Click to show/hide Orders for Customer <%# Eval("MainID") %>"
                                        width="9px" src="../Images/DMSButton/plus.gif" />
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="MainID" SortExpression="RepositoryId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMainID" Text='<%# Eval("MainID") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblRepositoryName" Text='<%# Eval("Name") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CreatedOn" SortExpression="CreatedOn">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedOn" Text='<%# Eval("CreatedOn") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UpdatedOn" SortExpression="UpdatedOn">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedOn" Text='<%# Eval("UpdatedOn") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnShow" Text="showLog" CommandName="SHOWLOG" runat="server" SkinID="ViewButton" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
         </table>
         <table width="100%">
          <asp:Button ID="btnModelPopup" runat="server" Visible="false" />
    <asp:ModalPopupExtender ID="Mpextender" runat="server" X="380" Y="200" TargetControlID="btnModelPopup"
        DropShadow="true" PopupControlID="PnlPopUp" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
        <tr>
            <td align="center" colspan="2">
                <asp:Panel ID="PnlPopUp" runat="server" BackColor="WhiteSmoke" BorderColor="#FF3300" Width="600px" Visible="false"
                    Height="250px">
                    <asp:UpdatePanel ID="upnl" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="center">
                                        <br>
                                    </br>
                                        <asp:Label ID="lblNameText" runat="server" Text="View Log" Font-Bold="True"></asp:Label>
                                        <br>
                                        </br>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="grvauditReportLog" runat="server" AutoGenerateColumns="true" Width="90%"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <br>
                                    </br>

                                        <asp:ImageButton ID="btnCancel" runat="server" Text="Cancel" SkinID="CancelButton" OnClick="btnCancel_Click1" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        </table>
   
   
</asp:Content>
