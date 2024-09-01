<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="SearchDocumentCentrum.aspx.cs" Inherits="DMS.Shared.SearchDocumentCentrum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
     <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Search" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table width="931" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left">
                    <uc:EntityModuleCentrum ID="emodModuleCentrum" runat="server" DisplayMetaDataCode="true" />
                </td>
                <td align="left" valign="top">
                    <table>
                        <tr>
                             <td align="left" valign="top">
                                        <asp:Label ID="lblSearchBy" runat="server" Text="Search By :"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:RadioButtonList ID="rdblSearchBy" runat="server" TextAlign="Right" AutoPostBack="true" OnSelectedIndexChanged ="rdblSearchBy_SelectedIndexChanged" >
                                            <asp:ListItem Text="Pan Number" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Customer Name" Value="2"></asp:ListItem>
                                            <%--<asp:ListItem Text="Document Content" Value="3"></asp:ListItem>--%>
                                             <asp:ListItem Text="File Name" Value="4"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                        </tr>
                         <tr>
                                    <td align="left">
                                        <asp:Label ID="lblTextToSeach" runat="server" Text="Text To Search : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTextToSeach" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                    </table>
                </td>
                </tr>
            <tr>
               <td align="center" colspan="3">
                                 <asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnShow_Click" style="height: 29px" />
                </td>
            </tr>
            </table>

        <table width="100%" style="margin-top:1%">
            <tr>
                <td>
                    <asp:GridView ID="gvDocSearch" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    ShowHeaderWhenEmpty="true" AllowPaging="true" DataKeyNames="Pan_No,Customer_Name,Branch,Department"
                    PageSize="5" Width="931" OnPageIndexChanging="gvDocSearch_PageIndexChanging" OnRowCommand="gvDocSearch_RowCommand" >
                    <Columns>
                         <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>         
                        <asp:BoundField DataField="Pan_No" HeaderText="Pan No" />
                           <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" SortExpression="Customer_Name"/>
                        <asp:BoundField DataField="Branch" HeaderText="Metatemplate Name" SortExpression="Branch"/>                     
                        <asp:BoundField DataField="Department" HeaderText="Category Name" Visible="false" SortExpression="Department" />
                       <asp:BoundField DataField="Type_Of_Document" HeaderText="Folder Name" Visible="false" />
                        <asp:BoundField DataField="Account_Opening_Date" HeaderText="Account Opening Date" SortExpression="Account_Opening_Date" NullDisplayText="N/A" />
                            <asp:BoundField DataField="Account_Closing_Date" HeaderText="Account Closing Date" SortExpression="Account_Closing_Date" NullDisplayText="N/A" />
                           <asp:ButtonField ButtonType="Image" CommandName="viewdetail" HeaderText="View Details"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" > 
<ControlStyle Width="80px"></ControlStyle>
                             </asp:ButtonField>                    
                        
                    </Columns>
                    <EmptyDataTemplate>
                        No Such record Found
                    </EmptyDataTemplate>
                </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true" width="931"
                        PageSize="5" AllowPaging="true" DataKeyNames="ID,DocumentName" ShowFooter="true" OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand">
                        <Columns>
                           <%--  <asp:TemplateField>
                                <FooterTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFilterGrid" runat="server" Text="Filter By : "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFilterGrid" runat="server">
                                                    <asp:ListItem Text="DocumentName" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Tag" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFilterGrid" runat="server"></asp:TextBox>
                                            </td>
                                             <td>
                                                <asp:ImageButton ID="ibtnFilterGrid" runat="server" CausesValidation="false" SkinID="ViewButton"></asp:ImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                           <%-- <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" />--%>
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"
                                NullDisplayText="N/A" />
                           <asp:BoundField DataField="ID" HeaderText="Doc ID" SortExpression="DocID" NullDisplayText="N/A"
                                Visible="false" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="DocumentType"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="CREATEDON" HeaderText="Upload Date" SortExpression="DocumentStatus"
                                NullDisplayText="N/A" />
                             
                          <%--  <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />--%>
                          <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />                          
                        </Columns>
                         <EmptyDataTemplate>
                        No Such record Found
                    </EmptyDataTemplate>
                    </asp:GridView>

                   <%--this grid is for content search--%>
                    <asp:GridView ID="gvwContentSearch" runat="server" AutoGenerateColumns="false" AllowSorting="true" width="931"
                        PageSize="5" ShowHeaderWhenEmpty="True"  AllowPaging="true" DataKeyNames="ID,DocumentName" ShowFooter="true" OnPageIndexChanging="gvwContentSearch_PageIndexChanging" OnRowCommand="gvwContentSearch_RowCommand">
                        <Columns>
                          
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                           <%-- <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" />--%>
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"
                                NullDisplayText="N/A" />
                           <asp:BoundField DataField="ID" HeaderText="Doc ID" SortExpression="ID" NullDisplayText="N/A"
                                Visible="false" />
                            <asp:BoundField DataField="Pan_no" HeaderText="Pan No" SortExpression="Pan_no"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" SortExpression="Customer_Name"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="MetaTemplateName" HeaderText="Branch" SortExpression="MetaTemplateName" NullDisplayText="N/A" />                            
                            <asp:BoundField DataField="Account_Opening_Date" HeaderText="Account Opening Date" SortExpression="Account_Opening_Date" NullDisplayText="N/A" />
                            <asp:BoundField DataField="Account_Closing_Date" HeaderText="Account Closing Date" SortExpression="Account_Closing_Date" NullDisplayText="N/A" />
                            <asp:BoundField DataField="CreatedOn" HeaderText="Upload Date" SortExpression="CreatedOn" NullDisplayText="N/A" />
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />                          
                        </Columns>
                         <EmptyDataTemplate>
                        No Such record Found
                    </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
