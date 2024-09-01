<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewSharedDocuments.aspx.cs" Inherits="DMS.Shared.ViewSharedDocuments" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Page-Enter" content="Alpha(opacity=100)" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Virescent</title>

  

    <link href="Css/style_new.css" rel="stylesheet" type="text/css" />
    <link href="Css/pro_dropdown_2_new.css" rel="stylesheet" type="text/css" />
    <link href="Css/main.css" rel="stylesheet" type="text/css" />
    <link href="Css/main1.css" rel="stylesheet" type="text/css" />
    <link href="Css/main2.css" rel="stylesheet" type="text/css" />

    <style type="text/css" media="screen">
        .PrintButton {
            display: block;
        }
    </style>

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

    <%--<script language="javascript" type="text/javascript">
        function initRequest(sender, args) {
            var pop = $find("mpopProgessExtender");
            pop.show();
        }

        function endRequest(sender, args) {
            var pop = $find("mpopProgessExtender");
            pop.hide();
        }
    </script>--%>

</head>
<body id="masterbody" runat="server">
    <form id="frmMainMaster" runat="server" enctype="multipart/form-data" method="post">
        
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="top">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="background-color: #ffffff; border-bottom: 1px solid #000000;">
                                    <tr>
                                        <td align="left" style="padding-left: 0px; padding-top: 10px; width: 450px;">
                                            <img src="../Images/logo.jpg" width="180" height="85" />
                                        </td>
                                        <%--<td align="left" style="font-size: 28px; font-weight: bold; font-family: korinna; color: #032d54;">Document Management Solution</td>--%>
                                        <td align="left" style="font-size: 28px; font-weight: bold; font-family: korinna; color: #364943">Document Management Solution</td>
                                        <td> </td>
                                        <td align="right" valign="top" padding-left: 0px; padding-top: 10px; width: 450px;>
                                            <div style="" ><img src="../Images/V1_Virescent_Backdrop_1800x1800_4-6-2021-01.jpg" width="280" height="85"  />
                                            </div>
                                 
                                            
                                        </td>
                         
                                  <td  align="right" valign="bottom">
                                              
                                        </td>
                              
                                    </tr>
                                   
                                  
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <%--<td style="background-color:#032d54;width:100%;padding:10px 0px;text-align:center;">--%>
                            <td style="background-color:#496150;width:100%;padding:0px 0px;text-align:center;">
                            <center><asp:Label ID="lblTitle" runat="server" Text="List Of Shared Documents "  style="margin-top:1%;color:white" SkinID="Title"></asp:Label> 
                                
                                <asp:ImageButton ID="ibtnLogout" runat="server" SkinID="logoutButton" OnClick="ibtnLogout_Click"
                                    ToolTip="Logout" CausesValidation="false" />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <table border="0" cellspacing="0" cellpadding="0" width="1250px">
                                    <tr>
                                        <td style="width: 1250px;">
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
                                    <%--<tr>
                                        <td align="left" valign="top" class="hor-dotted-divider">&nbsp;
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding:5px;"></td>
                        </tr>
                        <tr>
                            <td style="text-align: left; position: relative; bottom: 0;border-top:1px solid #000000;width:100%;background-color:#ffffff;opacity:0.8;padding:8px;">Copyright © 2018 StockHolding DMS Ltd. All rights reserved.
                            </td>
                        </tr>
                        
                    </table>
       

            <%--<uc:DocumentViewer ID="dvDocumentViewer" runat="server" />--%>
        
    </form>
</body>
</html>
