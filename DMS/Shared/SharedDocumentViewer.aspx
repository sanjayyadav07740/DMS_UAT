<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SharedDocumentViewer.aspx.cs" Inherits="DMS.Shared.SharedDocumentViewer1" %>
<%@ Register Src="~/UserControl/SharedDocumentViewer.ascx" TagPrefix="uc" TagName="DocumentViewer"  %>
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
        function initRequest(sender, args) {
            var pop = $find("mpopProgessExtender");
            pop.show();
        }

        function endRequest(sender, args) {
            var pop = $find("mpopProgessExtender");
            pop.hide();
        }
    </script>
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
                              
                                        <td style="width: 1250px;">
                                            <table id="tblMainContent" runat="server" width="1250px" border="0" cellspacing="0"
                                                cellpadding="0">

                                                <tr>
                                                    <td style="padding-top: 10px;" height="0" valign="top">
                                                        <table width="1250px" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="padding-top: 15px; padding-bottom: 10px;" class="tabborder" align="left">
      <uc:DocumentViewer ID="dvDocumentViewer" runat="server" />
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
                </td>
            </tr>
        </table>
       

            <%--<uc:DocumentViewer ID="dvDocumentViewer" runat="server" />--%>
        
    </form>
</body>
</html>
