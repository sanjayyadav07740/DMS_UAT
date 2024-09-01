<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Showdeatils.aspx.cs" Inherits="DMS.Dashoboard_ViewDocuments.Showdeatils"
    MasterPageFile="~/MainMasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cphDashBoardHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDashBoardMain" ContentPlaceHolderID="cphmain" runat="server">
    <table width="100%" border="0px" cellpadding="0px" cellspacing="0px">
        <tr>
            <td align="center">
                <table width="850px">
                    <tr>
                        <td colspan="2" align="center" style="padding-left: 20px;">
                            <table width="410" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="left" valign="bottom" class="cp-container-head">
                                        Total Count<br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top" class="cp-container-bg">
                                        <table width="380">
                                            <tr>
                                                <td>
                                                    From:
                                                    <asp:TextBox ID="txtFrom" runat="server" Width="120px" EnableTheming="false" />&nbsp;
                                                    <cc1:CalendarExtender ID="calExtFrom" runat="server" TargetControlID="txtFrom" PopupPosition="BottomRight"
                                                        PopupButtonID="txtFrom" Format="dd-MMM-yyyy" />
                                                    TO:
                                                    <asp:TextBox ID="txtTO" runat="server" Width="120px" EnableTheming="false" />
                                                    <cc1:CalendarExtender ID="CalextTO" runat="server" TargetControlID="txtTO" PopupPosition="BottomRight"
                                                        PopupButtonID="txtTO" Format="dd-MMM-yyyy" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/DMSButton/show.jpg"
                                                        Width="90px" Height="25px" OnClick="btnShow_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="GridCount" runat="server" GridLines="None" ShowHeader="false" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="GridCount_PageIndexChanging"
                                                        PageSize="5">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("StatusName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Count">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcount" runat="server" Text='<%#Eval("TotalCount")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px;">
                            <table>
                                <tr>
                                    <td align="right">
                                        <table align="left" width="410" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="bottom" class="cp-container-head">
                                                    Most View documents<br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="cp-container-bg">
                                                    <asp:GridView ID="GrvViewMostview" runat="server" GridLines="None" ShowHeader="false"
                                                        AutoGenerateColumns="False" DataKeyNames="ID,METADATAID,DOCUMENTSTATUSID" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GrvViewMostview_PageIndexChanging" OnRowCommand="GrvViewMostview_RowCommand1"
                                                        PageSize="5">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="DocumentID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldocId" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="METADATAID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmetadataId" runat="server" Text='<%# Eval("METADATAID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RepositoryName" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRepositoryName" runat="server" Text='<%# Eval("RepositoryName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DocumentName">
                                                                <ItemTemplate>
                                                                    <div id="divId" style="overflow: auto; width: 300px; height: 25px;">
                                                                        <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DOCUMENTNAME") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Count" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcount" runat="server" Text='<%#Eval("Count")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnShow" Text="showLog" CommandName="SHOWLOG" runat="server"
                                                                        ImageUrl="~/Images/DMSButton/view.jpg" Width="80px" Height="25px" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table align="left" width="410" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="bottom" class="cp-container-head">
                                                    Recently Added Documents<br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="cp-container-bg">
                                                    <asp:GridView ID="GridViewRecentAdd" runat="server" GridLines="None" ShowHeader="false"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GridViewRecentAdd_PageIndexChanging"
                                                        PageSize="5">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="DocumentName" ItemStyle-Width="250px">
                                                                <ItemTemplate>
                                                                    <div id="divId" style="overflow: auto; width: 300px; height: 25px;">
                                                                        <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DOCUMENTNAME") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
