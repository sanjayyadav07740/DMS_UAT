<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgot Password.aspx.cs" Inherits="DMS.Shared.Forgot_Password" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Page-Enter" content="Alpha(opacity=100)" />
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title></title>
<link href="../Css/style_new.css" rel="stylesheet" type="text/css" />
<link href="../Css/pro_dropdown_2_new.css" rel="stylesheet" type="text/css" />
<link href="../css/main.css" rel="stylesheet" type="text/css" />
<link href="../css/main1.css" rel="stylesheet" type="text/css" />
<link href="../css/main2.css" rel="stylesheet" type="text/css" />
<style type="text/css" media="screen">
    .PrintButton{
display:block;
}
    .style1
    {
        width: 279px;
    }
   
    
</style> 
</head>
<body id="masterbody" runat="server" >
 <form id="frmLogin" runat="server" defaultbutton="ibtnSubmit">

<div>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="top">
                    <table width="1003" border="0" cellpadding="0" cellspacing="0" style="background-color:#ffffff;">
                        <tr>
                            <td align="left" valign="top" class="Header-bg">
                                <table width="1003" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="149" align="right" valign="top" class="logo">&nbsp;
                                       
                                        </td>
                                        <td align="right" valign="bottom" style="padding-right: 32px;">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right" valign="bottom" style="padding-right:10px;padding-bottom:25px;">
                                            <img src="../Images/shcilpng2.png" />
                                            </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table width="1003" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="right" width="20" valign="top" class="middle-content-leftSHadow">
                                            &nbsp;
                                        </td>
                                        <td width="941" align="center" valign="top" class="middle-content">
                                            <table width="941" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left" valign="top" class="menu-left-curve" >
                                                        &nbsp;
                                                    </td>
                                                    <td align="center" valign="middle" class="menu-bg" >
                                                        <table border="0" cellspacing="0" cellpadding="0" width="650">
                                                            <tr>
                                                                <td id="Td1" align="left" runat="server" class="menu-bg">
                                                                    <asp:Menu ID="mnuMain" runat="server" Orientation="Horizontal" Font-Names="Verdana"
                                                                     ForeColor="White" Font-Size="14px" StaticEnableDefaultPopOutImage="false">
                                                                    <StaticMenuItemStyle Height="25px" HorizontalPadding="23px" ForeColor="White" BorderColor="#7CCA27"
                                                                      BorderWidth="1px" />
                                                                    <StaticMenuStyle VerticalPadding="2px" ForeColor="White" />
                                                                    <DynamicMenuItemStyle BackColor="#2EA017" Height="25px" HorizontalPadding="25px"
                                                                      Width="200px" BorderColor="#7CCA27" BorderWidth="0px" VerticalPadding="2px" />
                                                                    <DynamicMenuStyle Height="25px" HorizontalPadding="0px" VerticalPadding="2px" BorderColor="#7CCA27"
                                                                      BorderWidth="0px" />
                                                                    </asp:Menu>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" valign="top" class="menu-right-curve">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" valign="top" class="middle-white-container">
                                                        <table border="0" cellspacing="0" cellpadding="0" width="931">
                                                         <tr>
                                                             <td align="left" valign="top" class="hor-dotted-divider">&nbsp;
                                                             </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            <table id="tblMainContent" runat="server" width="931" border="0" cellspacing="0"
                                                                                cellpadding="0">
                                                                                <tr>
                                                                                    <td align="left" valign="top">
                                                                                        <img src="../images/table2_top1.jpg" width="931" height="13">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width="931" height="0" valign="top" class="bigtabbg" style="padding-top: 10px;">
                                                                                        <table width="931" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td align="center" style="padding-top: 15px; padding-bottom: 10px;" class="tabborder">
                                                                                                    <table width="341" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td align="Center" valign="bottom" class="login-container-head">Forgot Password</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" valign="top" class="login-container-bg"><table width="321" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td colspan="3" align="left"></td>
								                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="3" align="center">Access Your User Name And Password</td>
								                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="3" align="left"></td>
								                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="100" height="40" style="padding-left:15px;">
                                                                                                                <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                                                                                                            </td>
                                                                                                            <td height="40" class="style1">
                                                                                                                <asp:TextBox ID="txtUserName" runat="server" ToolTip='<%$ Resources:Resource, UserName %>'></asp:TextBox>
                                                                                                               
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <%--<asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ToolTip='<%$ Resources:Resource, UserName %>'></asp:RequiredFieldValidator>--%>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="3" align="center"> OR
                                                                                                            </td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="100" height="40" style="padding-left:15px;">
                                                                                                                <asp:Label ID="lblPassword" runat="server" Text="Email"></asp:Label>
                                                                                                            </td>
                                                                                                            <td height="40" class="style1">
                                                                                                                <asp:TextBox ID="txtEmail" runat="server" TextMode="Password" 
                                                                                                                    ToolTip='<%$ Resources:Resource, Password %>'></asp:TextBox>
                                                                                                                
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <%--<asp:RequiredFieldValidator ID="rfvemail" runat="server" ControlToValidate="txtPassword" ToolTip='<%$ Resources:Resource, Password %>'></asp:RequiredFieldValidator>--%>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td height="40" align="center" colspan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                                <asp:ImageButton ID="ibtnSubmit" runat="server" ToolTip="Click Here To Submit." 
                                                                                                                    SkinID="LoginButton" onclick="ibtnSubmit_Click" />
                                                                                                                <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click Here To Cancel." 
                                                                                                                    SkinID="CancelButton" CausesValidation="false" 
                                                                                                                    onclick="ibtnCancel_Click" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center" colspan="3">
                                                                                                                <asp:Label ID="lblError" runat="server" Text="" Font-Size="Small" Width="180PX" EnableTheming="false" ForeColor="Red"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                             </table></td>
                                                                                            </tr>
                                                                                      </table>
                                                                                   </td>
                                                                               </tr>
                                                                          </table>
                                                                        </td>
                                                                      </tr>
                                                                      <tr>
                                                                          <td align="left" valign="top">
                                                                               <img src="../images/table2_top2.jpg" width="931" height="13">
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
                                        <td width="20" style="height:600px;" align="left" valign="top">
                                            <img src="../images/middleCont-right-GShadow_new1.jpg" width="20" height="478" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="middle" class="footer PrintButton">
                                Copyright &copy; 2011 SHCIL Projects Ltd. All rights reserved.
                        </tr>
                   </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
