<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="SystemTool.aspx.cs" Inherits="DMS.SystemTool" %>

<asp:Content ID="cphSystemToolHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphSystemToolmain" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <table>
            <tr>
                <td colspan="3" align="center">
                    <b>Password Encryption And Decryption</b>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPassword" runat="server" Text="Password : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPassword" runat="server" ToolTip='<%$ Resources:Resource,Password %>'
                        ValidationGroup="Password"> </asp:TextBox>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ToolTip='<%$ Resources:Resource,Password %>' ValidationGroup="Password"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt" ValidationGroup="Password"
                        OnClick="btnEncrypt_Click" />
                    <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" ValidationGroup="Password"
                        OnClick="btnDecrypt_Click" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblResult" runat="server" Text="Resulting Password : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtResult" runat="server"> </asp:TextBox>
                </td>
                <td align="left">
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gvwLogViewer" runat="server" AutoGenerateColumns="false" 
                        AllowPaging="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Data To Display."
                        GridLines="Both" DataKeyNames="FullName" 
                        onpageindexchanging="gvwLogViewer_PageIndexChanging" 
                        onrowcommand="gvwLogViewer_RowCommand" 
                        onrowdatabound="gvwLogViewer_RowDataBound" onsorting="gvwLogViewer_Sorting">
                        <Columns>
                         <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  DataField="Name" HeaderText="File Name"/>
                        <asp:BoundField  DataField="Length" HeaderText="Length"/>
                        <asp:BoundField  DataField="CreationTime" HeaderText="Creation Time" DataFormatString="{0:f}" />
                        <asp:BoundField  DataField="LastAccessTime" HeaderText="Last Access Time" DataFormatString="{0:f}" />
                        <asp:ButtonField ButtonType="Link" Text="Download" HeaderText="Download" CommandName="Download" />
                        <asp:ButtonField ButtonType="Link" Text="Delete" HeaderText="Delete" CommandName="DeleteFile" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        
    </center>
</asp:Content>

