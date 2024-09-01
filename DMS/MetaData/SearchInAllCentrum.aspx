<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="SearchInAllCentrum.aspx.cs" Inherits="DMS.Shared.SearchInAllCentrum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <div runat="server" >
       
        <%--<table runat="server" align="center" width ="60%" style="margin-top:5%; background-color:#B5C7DE" >
            <tr>
                
                <td colspan="3" align="center"><asp:Label ID="lblSearchInAll" runat="server" Text="Search In All"></asp:Label> </td>
                
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRepository" runat="server" Text="Repository : "></asp:Label> </td>
                <td>
                    <asp:DropDownList ID="ddlRepository" runat="server" Width="140px"></asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>

                <td align="right">
                    <asp:Label ID="lblSearch" runat="server" Text="Search : "></asp:Label></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchInAll" runat="server" Width="140px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSearch" runat="server" ErrorMessage="Please Enter Text To Search."
                    ControlToValidate="txtSearchInAll" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>

                </td>
                <td align="center">
                    <asp:ImageButton ID="btnSearchInAll" runat="server" ImageUrl="~/Images/search.jpg"  Width="85" OnClick="btnSearchInAll_Click"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>--%>
        <center>
        <%-- <table  cellpadding="0" cellspacing="0" border="0" >
            <tr>
                <td  colspan="3">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
                </td>        
               
                </tr>
        
            </table>--%>
            
                     <Table style="margin-top:1%; ">
                     <tr>
                 <td align="right">
                    <asp:Label ID="lblSearch" runat="server" Text="PAN No : "></asp:Label></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchInAllPan" runat="server" Width="140px"></asp:TextBox>
                    </td>
                 <td>
                   <%-- <asp:RequiredFieldValidator ID="rfvSearch" runat="server" ErrorMessage="Please Enter Text To Search."
                    ControlToValidate="txtSearchInAllPan" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                 </td>
                         </tr>
                         <tr>
                 <td align="right">
                    <asp:Label ID="lblCustname" runat="server" Text="Customer Name : "></asp:Label></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchInAllCust" runat="server" Width="140px"></asp:TextBox>
                    </td>
                 <td>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Text To Search."
                    ControlToValidate="txtSearchInAllCust" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                 </td>
                         </tr>
                        <tr>
                             <td align="center" colspan="3">
                                 <asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnShow_Click" />
                             </td>
                         </tr>
                </Table>
        </center>
        <table align="center"  style="margin-top:2%;">
            <tr>
               <td align="center">
                <asp:GridView ID="gvDocSearch" runat="server" AutoGenerateColumns="false" 
                    ShowHeaderWhenEmpty="true" AllowPaging="true" DataKeyNames="Pan_No,Branch,Department" 
                    PageSize="5" Width="800px" OnPageIndexChanging="gvDocSearch_PageIndexChanging" OnRowCommand="gvDocSearch_RowCommand" EmptyDataText="No Data Found">
                    <Columns>
                         <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>         
                        <asp:BoundField DataField="Pan_No" HeaderText="Pan No" />
                           <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name " />
                        <asp:BoundField DataField="Branch" HeaderText="Branch"  />                     
                        <asp:BoundField DataField="Department" HeaderText="Department" Visible="false" />
                       <asp:BoundField DataField="Type_Of_Document" HeaderText="Folder Name" Visible="false" />
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
            <tr><td></td></tr>
            <tr>
               <td align="center">
                <asp:GridView ID="gvDocument" runat="server" AutoGenerateColumns="false" 
                    ShowHeaderWhenEmpty="true" AllowPaging="true" DataKeyNames="ID,DocumentName"
                    PageSize="5" Width="800px" OnPageIndexChanging="gvDocument_PageIndexChanging" OnRowCommand="gvDocument_RowCommand" emptyDataText="No Data Found">
                    <Columns>
                         <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>         
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                           <asp:BoundField DataField="DocumentName" HeaderText="Document Name " />                      
                        <asp:BoundField DataField="PageCount" HeaderText="Page Count" />      
                        <asp:BoundField DataField="MetatemplateName" HeaderText="Department" />      
                         <asp:BoundField DataField="CategoryName" HeaderText="Branch" />          
                           <asp:ButtonField ButtonType="Image" CommandName="viewdetaildoc" HeaderText="View Details"
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

        </table>
    </div>
</asp:Content>
