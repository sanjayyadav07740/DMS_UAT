<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="ComDashBoard.aspx.cs" Inherits="DMS.Dashoboard_ViewDocuments.ComDashBoard" %>

<asp:Content ID="cphDashBoardHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDashBoardMain" ContentPlaceHolderID="cphmain" runat="server">
    <center><asp:Label ID="lblTitle" runat="server" Text="Recently Viewed Documents"  SkinID="Title"></asp:Label> </center>                         
    <center>
        <table>
            <tr>
                  <td colspan="2">
                    <br/>
                    </td>
                     <td align="center">
                    <asp:GridView ID="GrvView" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="ID,METADATAID,DOCUMENTSTATUSID" AllowPaging="True" 
                             onpageindexchanging="GrvView_PageIndexChanging" 
                             onrowcommand="GrvView_RowCommand1">
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
                            <asp:TemplateField HeaderText="RepositoryName">
                                <ItemTemplate>
                                    <asp:Label ID="lblRepositoryName" runat="server" Text='<%# Eval("RepositoryName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DocumentName">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DocumentPath">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocumentpathh" runat="server" Text='<%#Eval("DOCUMENTPATH") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DocumentPath" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocumentStatusId" runat="server" Text='<%#Eval("DOCUMENTSTATUSID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Count">
                                <ItemTemplate>
                                    <asp:Label ID="lblcount" runat="server" Text='<%#Eval("Count")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnShow" Text="showLog" CommandName="SHOWLOG" runat="server"
                                        SkinID="ViewButton" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
            <td colspan="3">
                <br/>
                </td>
            </tr>
        </table>
        <center><asp:Label ID="Label1" runat="server" Text="Status Of Documents"  SkinID="Title"></asp:Label> </center> 
        <asp:GridView ID="gvwDocumentRepositoryLevel" runat="server" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" PageSize="5" AllowPaging="true" OnPageIndexChanging="gvwDocumentRepositoryLevel_PageIndexChanging"
            DataKeyNames="RepositoryID,RepositoryName" OnRowCommand="gvwDocumentRepositoryLevel_RowCommand"
            OnRowDataBound="gvwDocumentRepositoryLevel_RowDataBound" Visible="true" OnSorting="gvwDocumentRepositoryLevel_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Sr.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Link" DataTextField="RepositoryName" HeaderText="Repository Name"
                    SortExpression="RepositoryName" CommandName="RepositoryLevel" />
                <asp:BoundField DataField="TotalDocument" HeaderText="Total Document" NullDisplayText="N/A"
                    SortExpression="TotalDocument" ControlStyle-ForeColor="Aqua" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblApproved" runat="server" Text="Approved Document" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkApproved" runat="server" Font-Bold="true" ForeColor="Orange"
                            Enabled="false" CommandName="Approved" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedDocument").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"ApprovedDocument").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblRejected" runat="server" Text="Rejected Document" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRejected" runat="server" Font-Bold="true" ForeColor="DarkMagenta"
                            Enabled="false" CommandName="Rejected" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Text='<%# DataBinder.Eval(Container.DataItem,"RejectedDocument").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"RejectedDocument").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblPending" runat="server" Text="Pending For Entry" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPending" runat="server" Font-Bold="true" ForeColor="Red" CommandName="Pending"
                            CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Enabled="false" Text='<%# DataBinder.Eval(Container.DataItem,"PendingForEntry").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"PendingForEntry").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblEntryCompleted" runat="server" Text="Entry Completed" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEntryCompleted" runat="server" Font-Bold="true" ForeColor="Green"
                            Enabled="false" CommandName="EntryCompleted" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Text='<%# DataBinder.Eval(Container.DataItem,"EntryCompleted").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"EntryCompleted").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:Label ID="lblRepositoryName" runat="server" Visible="false" EnableTheming="false"
            Font-Size="12" Font-Underline="true" Font-Bold="true"></asp:Label>
        <br />
        <asp:GridView ID="gvwDocumentMetaTemplateLevel" runat="server" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" PageSize="5" AllowPaging="true" OnPageIndexChanging="gvwDocumentMetaTemplateLevel_PageIndexChanging"
            DataKeyNames="ID,MetaDataCode" OnRowCommand="gvwDocumentMetaTemplateLevel_RowCommand"
            OnRowDataBound="gvwDocumentMetaTemplateLevel_RowDataBound" Visible="true" OnSorting="gvwDocumentMetaTemplateLevel_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Sr.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MetaTemplate" HeaderText="MetaTemplate" SortExpression="MetaTemplate"
                    NullDisplayText="N/A" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category"
                    NullDisplayText="N/A" />
                <asp:BoundField DataField="Folder" HeaderText="Folder" SortExpression="Folder" NullDisplayText="N/A" />
                <asp:ButtonField DataTextField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                    CommandName="MetaTemplateLevel" />
                <asp:BoundField DataField="TotalDocument" HeaderText="Total Document" NullDisplayText="N/A"
                    SortExpression="TotalDocument" ControlStyle-ForeColor="Aqua" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblApproved" runat="server" Text="Approved Document" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkApproved" runat="server" Font-Bold="true" ForeColor="Orange"
                            Enabled="false" CommandName="Approved" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Text='<%# DataBinder.Eval(Container.DataItem,"ApprovedDocument").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"ApprovedDocument").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblRejected" runat="server" Text="Rejected Document" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRejected" runat="server" Font-Bold="true" ForeColor="DarkMagenta"
                            Enabled="false" CommandName="Rejected" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Text='<%# DataBinder.Eval(Container.DataItem,"RejectedDocument").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"RejectedDocument").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblPending" runat="server" Text="Pending For Entry" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPending" runat="server" Font-Bold="true" ForeColor="Red" CommandName="Pending"
                            CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Enabled="false" Text='<%# DataBinder.Eval(Container.DataItem,"PendingForEntry").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"PendingForEntry").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblEntryCompleted" runat="server" Text="Entry Completed" ForeColor="Black"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEntryCompleted" runat="server" Font-Bold="true" ForeColor="Green"
                            Enabled="false" CommandName="EntryCompleted" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Text='<%# DataBinder.Eval(Container.DataItem,"EntryCompleted").ToString().Trim()==""?"0":DataBinder.Eval(Container.DataItem,"EntryCompleted").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:Label ID="lblMetaDataCode" runat="server" Visible="false" EnableTheming="false"
            Font-Size="12" Font-Underline="true" Font-Bold="true"></asp:Label>
        <br />
        <asp:GridView ID="gvwDocumentMetaDataLevel" runat="server" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" PageSize="5" AllowPaging="true" OnPageIndexChanging="gvwDocumentMetaDataLevel_PageIndexChanging"
            OnRowCommand="gvwDocumentMetaDataLevel_RowCommand" OnRowDataBound="gvwDocumentMetaDataLevel_RowDataBound"
            Visible="true" OnSorting="gvwDocumentMetaDataLevel_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Sr.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DocumentName" HeaderText="DocumentName" SortExpression="DocumentName"
                    NullDisplayText="N/A" />
                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A" />
                <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                    NullDisplayText="N/A" />
                <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                    NullDisplayText="N/A" />
            </Columns>
        </asp:GridView>
        <table>
            <tr>
                <td align="center">
                    <asp:Chart ID="chtRepositoryLevel" runat="server">
                        <Series>
                            <asp:Series Name="seriesTotalDocument" XValueMember="RepositoryName" YValueMembers="TotalDocument"
                                IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="chartArea" Area3DStyle-Enable3D="true">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
                <td align="center">
                    <asp:Chart ID="chtMetaTemplateLevel" runat="server">
                        <Series>
                            <asp:Series Name="series" XValueMember="MetaDataCode" YValueMembers="TotalDocument"
                                IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="chartArea" Area3DStyle-Enable3D="true">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
                <td align="center">
                    <asp:Chart ID="chtMetaDataLevel" runat="server">
                        <Series>
                            <asp:Series Name="series" XValueMember="DocumentStatus" YValueMembers="TotalDocument"
                                IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="chartArea" Area3DStyle-Enable3D="true">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
