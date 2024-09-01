<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentSearch.aspx.cs" MasterPageFile="~/MainMasterPage.Master" Inherits="DMS.Reports.DocumentSearch" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="cphDocument">
<b>Document Search</b>

<asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtDocument" runat="server"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="rfvDocument" runat="server" ControlToValidate="txtDocument" ErrorMessage="*" ForeColor="Red" ToolTip="Data required" />--%>
            </td>
            <td>
                <asp:DropDownList ID="ddlSearchType" runat="server">
                <asp:ListItem Text="...Select..." Value="7"></asp:ListItem>
                     <asp:ListItem Text="All" Value="4"></asp:ListItem>
                    <asp:ListItem Text="SoleId/FileName" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="1"></asp:ListItem>
                    <asp:ListItem Text="UserName" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Barcode" Value="3"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvDocumentType" runat="server" ControlToValidate="ddlSearchType" InitialValue="7" ErrorMessage="*" ForeColor="Red" ToolTip="Data required" />
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" 
                    onclick="btnSearch_Click" />
            </td>
          </tr>
          <tr>
          <td></td>
          <td></td>
          <td align="right">
       

            <asp:ImageButton ID="ibtnPolicyReport" runat="server" CausesValidation="false" 
                    Height="35px" ImageUrl="../Images/excel.jpg" onclick="ibtnReport_Click" 
                    ToolTip="Print" Visible="true" Width="25px" />
          </td>
              <caption>
                  
              </caption>
          </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:GridView ID="gvDocSearch" runat="server" AutoGenerateColumns="false" 
                    ShowHeaderWhenEmpty="true" AllowPaging="true" 
                    onpageindexchanging="gvDocSearch_PageIndexChanging" PageSize="5" Width="800px">
                    <Columns>
                        <asp:BoundField DataField="Sole_ID" HeaderText="Sole Id/FileName" />
                        <asp:BoundField DataField="Description" HeaderText="Description " />
                        <asp:BoundField DataField="User_Name" HeaderText="User Name " />
                        <asp:BoundField DataField="Barcode" HeaderText="Barcode" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Such record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Panel>

</asp:Content>


