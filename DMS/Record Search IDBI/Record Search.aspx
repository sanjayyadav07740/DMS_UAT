<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Record Search.aspx.cs" Inherits="DMS.Record_Search_IDBI.Record_Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
<table>
<tr>
<td>
<uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="false" />
</td>
</tr>
            <tr>
                <td align="center">
                    <asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="false" onclick="ibtnShow_Click1" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="ibtnPolicyReport" runat="server" CausesValidation="false" 
                    Height="35px" ImageUrl="../Images/excel.jpg" 
                    ToolTip="Print" Visible="true" Width="25px" onclick="ibtnPolicyReport_Click" />
                </td>
            </tr>

<tr align="center">
              <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                        PageSize="5" AllowPaging="true" DataKeyNames="ID,MetaDataID,DocumentStatusID"
                        OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand"
                        OnRowDataBound="gvwDocument_RowDataBound" OnSorting="gvwDocument_Sorting" ShowFooter="true">
                        <Columns>
                             <asp:TemplateField>
                                <FooterTemplate>
                                    
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentName" HeaderText="DocumentName" SortExpression="DocumentName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A"
                                Visible="false" />
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                            <%--<asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />--%>                          
                        </Columns>
                    </asp:GridView>
</tr>
</table>
</asp:Content>
