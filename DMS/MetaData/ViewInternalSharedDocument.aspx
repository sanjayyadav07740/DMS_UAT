<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ViewInternalSharedDocument.aspx.cs" Inherits="DMS.MetaData.ViewInternalSharedDocument" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
    <script language="javascript" type="text/javascript">
        function checkall(objCheckBox) {

            var control = document.getElementsByTagName('input');

            var i = 0;
            for (i = 0; i < control.length; i++) {
                if (control[i].type == 'checkbox') {
                    if (objCheckBox.checked == true)
                        control[i].checked = true;
                    else
                        control[i].checked = false;
                }
            }
        }


        function validateCheckBoxes() {
            var valid = false;
            debugger;
            var gv = document.getElementById('<%= this.gvwDocument.ClientID %>');
            for (var i = 0; i < gv.getElementsByTagName("input").length; i++) {
                var node = gv.getElementsByTagName("input")[i];
                if (node != null && node.type == "checkbox" && node.checked) {
                    valid = true;
                    break;
                }
            }
            if (!valid) {
                alert("Please Select a Checkbox to Download Documents.");
            }
            return valid;
        }


    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center><asp:Label ID="lblTitle" runat="server" Text="List Of Shared Documents "  SkinID="Title"></asp:Label> 
                                
                              
                                </center>
                            
        
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                  
                <td align="center" valign="top">
                                <table border="0" cellspacing="0" cellpadding="0" >
                                    <tr>
                                        <td >
                                            <table id="tblMainContent" runat="server" width="1250px" border="0" cellspacing="0"
                                                cellpadding="0">

                                                <tr>
                                                    <td style="padding-top: 10px;" height="0" valign="top">
                                                        <table width="1250px" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="padding-top: 15px; padding-bottom: 10px;" class="tabborder" align="left">
                                                                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="100%"
                                                                        DataKeyNames="Document_ID,MetaDataID,DocumentStatusID,DocumentName,DocumentPath"
                                                                        OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand"
                                                                        OnRowDataBound="gvwDocument_RowDataBound" OnSorting="gvwDocument_Sorting"
                                                                        ShowFooter="True">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:checkall(this);" />
                                                                                    <%--OnCheckedChanged="chkHeader_CheckedChanged"--%>
                                                                                    <%-- onclick = "checkAll(this);--%>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkRow" AutoPostBack="false" runat="server"  />
                                                                                    <%--onclick = "Check_Click(this)"--%>
                                                                                </ItemTemplate>
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
                                                                                                <asp:ImageButton ID="ibtnFilterGrid" runat="server" CausesValidation="false" OnClick="ibtnFilterGrid_Click" SkinID="ViewButton"></asp:ImageButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Sr." ControlStyle-Width="20px">
                                                                                <ItemTemplate>
                                                                                    <%# Container.DataItemIndex+1 %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Document_ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                                                                NullDisplayText="N/A" />

                                                                            <asp:BoundField DataField="Document_ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                                                                NullDisplayText="N/A" />
                                                                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                                                                NullDisplayText="N/A" ReadOnly="true" />
                                                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />

                                                                            <asp:BoundField DataField="DocumentPath" HeaderText="DocumentPath" SortExpression="DocumentPath" NullDisplayText="N/A" ReadOnly="true" />
                                                                            <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                                                                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                                                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px">


                                                                                <ControlStyle Width="80px"></ControlStyle>
                                                                            </asp:ButtonField>
                                                                        </Columns>

                                                                    </asp:GridView>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2">

                                                                    <asp:ImageButton ID="ibtn_download" runat="server" OnClientClick="javascript:return validateCheckBoxes();" CausesValidation="false"
                                                                        SkinID="Download" OnClick="ibtn_download_Click" Visible="false" />
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
                      
                    
            </table>

            <%--<uc:DocumentViewer ID="dvDocumentViewer" runat="server" />--%>
        
  
</asp:Content>
