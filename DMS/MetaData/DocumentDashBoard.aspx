<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="DocumentDashBoard.aspx.cs" Inherits="DMS.Shared.DocumentDashBoard" %>

<asp:Content ID="cphDocumentDashBoardHead" ContentPlaceHolderID="cphHead" runat="server">
    </asp:Content>
<asp:Content ID="cphDocumentDashBoardMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Dashboard" SkinID="Title"></asp:Label>
    </center>
    <table width="931" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="left" style="width:400;">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />

                <table>
                    <tr>
                        <td align="left" >
                            <asp:Label ID="lblFrom" runat="server" Text="From Date :"></asp:Label>
                        &nbsp;</td>
                        <td align="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtFrom" runat="server" Width="200px" AutoComplete="off"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtFrom"
                                PopupButtonID="txtFrom" Format="yyyy/MM/dd">
                            </ajaxtoolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblTo" runat="server" Text="To Date :"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtTo" runat="server" Width="200px" AutoComplete="off"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender ID="caltodate" runat="server" TargetControlID="txtTo"
                                PopupButtonID="txtTo" Format="yyyy/MM/dd">
                            </ajaxtoolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">&nbsp;
                        </td>
                    </tr>
                </table>

            </td>
            <td align="left" valign="top">
                <div runat="server" id="totalfilediv" visible="false">
                    Total files :
                    <asp:Label runat="server" ID="lbldtcount"></asp:Label>
                </div>

                <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="531"
                    PageSize="10" AllowPaging="true" DataKeyNames="ID,DOCID" OnPageIndexChanging="gvwDocument_PageIndexChanging"
                    OnRowCommand="gvwDocument_RowCommand" OnRowDataBound="gvwDocument_RowDataBound"
                    Visible="false" OnSorting="gvwDocument_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MetaDataCode" HeaderText="MetaData Code" SortExpression="MetaDataCode"
                            NullDisplayText="N/A" />
                        <asp:BoundField DataField="Total Document" HeaderText="Total Document" NullDisplayText="N/A"
                            SortExpression="Total Document" ControlStyle-ForeColor="Aqua" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblApproved" runat="server" Text="Approved Document" ForeColor="Black"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkApproved" runat="server" Font-Bold="true" ForeColor="Orange"
                                    CommandName="Approved" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Text='<%# DataBinder.Eval(Container.DataItem,"Approved Document").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"Approved Document").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblRejected" runat="server" Text="Rejected Document" ForeColor="Black"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRejected" runat="server" Font-Bold="true" ForeColor="DarkMagenta"
                                    CommandName="Rejected" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Text='<%# DataBinder.Eval(Container.DataItem,"Rejected Document").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"Rejected Document").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblPending" runat="server" Text="Pending For Entry" ForeColor="Black"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPending" runat="server" Font-Bold="true" ForeColor="Red" CommandName="Pending"
                                    CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Text='<%# DataBinder.Eval(Container.DataItem,"Pending For Entry").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"Pending For Entry").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblEntryCompleted" runat="server" Text="Entry Completed" ForeColor="Black"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEntryCompleted" runat="server" Font-Bold="true" ForeColor="Green"
                                    CommandName="EntryCompleted" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Text='<%# DataBinder.Eval(Container.DataItem,"Entry Completed").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"Entry Completed").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gvwDocumentList" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    Visible="false" PageSize="10" AllowPaging="true" DataKeyNames="ID,MetaDataID,DocumentStatusID"
                    OnPageIndexChanging="gvwDocumentList_PageIndexChanging" OnRowCommand="gvwDocumentList_RowCommand"
                    OnRowDataBound="gvwDocumentList_RowDataBound" OnSorting="gvwDocumentList_Sorting" Width="750px">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="MetaDataID" HeaderText="MetaDataID" Visible="false" />
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" />
                        <asp:BoundField DataField="PageCount" HeaderText="PageCount" />
                        <asp:BoundField DataField="Size" HeaderText="Document Size" SortExpression="Size" />
                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" SortExpression="DocumentType" Visible="false"/>
                        <asp:BoundField DataField="DocumentStatusID" HeaderText="Document Status" SortExpression="DocumentStatus" Visible="false"/>
                        <asp:ButtonField ButtonType="Image" CommandName="DocumentView" HeaderText="View"
                            ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" Visible="false"/>
                    </Columns>
                </asp:GridView>

            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnShow" runat="server" OnClick="ibtnShow_Click" ToolTip='<%$ Resources:Resource,Show %>'
                    SkinID="SubmitButton" CausesValidation="false" />
            </td>
            <td align="center">
                Type:
                <asp:DropDownList ID="ddlExport" runat="server">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem Selected="True">Excel</asp:ListItem>
                </asp:DropDownList>
                <asp:ImageButton ID="ibtnExportData" runat="server" ToolTip='<%$ Resources:Resource,Export %>'
                    SkinID="ExportButton" CausesValidation="false" OnClick="ibtnExportDataNew_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
