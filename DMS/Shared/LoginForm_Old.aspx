<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginForm_Old.aspx.cs" Inherits="DMS.Shared.LoginForm_Old" %>

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
        .PrintButton {
            display: block;
        }

        @import "compass/css3";

        * {
            box-sizing: border-box;
        }

        body {
            margin: 0px;
            width: 100%;
            background-image: url(../images/Login-bg.jpg);
            background-position: left top;
            background-repeat: no-repeat;
            position: fixed;
        }

        #login {
            /*font-family: "HelveticaNeue-Light","Helvetica Neue Light","Helvetica Neue",Helvetica,Arial,"Lucida Grande",sans-serif;*/
            font-family: "Book Antiqua" color:#ffffff;
            font-size: 12px;
            /*background:#333 url(/images/classy_fabric.png);*/
        }

            #login .form {
                /*background:#111;*/
                background: #023361;
                width: 300px;
                /*margin:10px auto;*/
                margin-left: 50px;
                border-radius: 0.4em;
                border: 1px solid #191919;
                overflow: hidden;
                position: relative;
                box-shadow: 0 5px 10px 5px rgba(0,0,0,0.2);
            }

                #login .form:after {
                    content: "";
                    display: block;
                    position: absolute;
                    height: 1px;
                    width: 100px;
                    left: 20%;
                    background: linear-gradient(left, #111, #444, #b6b6b8, #444, #111);
                    top: 0;
                }

                #login .form:before {
                    content: "";
                    display: block;
                    position: absolute;
                    width: 8px;
                    height: 5px;
                    border-radius: 50%;
                    left: 34%;
                    top: -7px;
                    box-shadow: 0 0 6px 4px #fff;
                }

        .inset {
            padding: 20px;
            border-top: 1px solid #19191a;
        }

        #login .form h1 {
            font-size: 18px;
            text-shadow: 0 1px 0 black;
            text-align: center;
            padding: 15px 0;
            border-bottom: 1px solid rgba(0,0,0,1);
            position: relative;
            color: #ffffff;
        }

            #login .form h1:after {
                content: "";
                display: block;
                width: 250px;
                height: 100px;
                position: absolute;
                top: 0;
                left: 50px;
                pointer-events: none;
                transform: rotate(70deg);
                background: linear-gradient(50deg, rgba(255,255,255,0.15), rgba(0,0,0,0));
            }

        #login .form label {
            /*color:#666;*/
            color: #fff;
            display: block;
            padding-bottom: 9px;
        }

        #login input[type=text],
        #login input[type=password] {
            width: 100%;
            padding: 8px 5px;
            /*background:linear-gradient(#1f2124, #27292c);*/
            background: linear-gradient(#ffffff, #edfefd);
            border: 1px solid #222;
            box-shadow: 0 1px 0 rgba(255,255,255,0.1);
            border-radius: 0.3em;
            margin-bottom: 20px;
        }

        #login label[for=remember] {
            color: white;
            display: inline-block;
            padding-bottom: 0;
            padding-top: 5px;
        }

        #login input[type=checkbox] {
            display: inline-block;
            vertical-align: top;
        }

        .p-container {
            padding: 0 20px 20px 20px;
        }

            .p-container:after {
                clear: both;
                display: table;
                content: "";
            }

            .p-container span {
                display: block;
                float: left;
                color: #0d93ff;
                padding-top: 8px;
            }

        #login input[type=submit] {
            padding: 5px 20px;
            border: 1px solid rgba(0,0,0,0.4);
            text-shadow: 0 -1px 0 rgba(0,0,0,0.4);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.3), inset 0 10px 10px rgba(255,255,255,0.1);
            border-radius: 0.3em;
            background: #0184ff;
            color: white;
            float: right;
            font-weight: bold;
            cursor: pointer;
            font-size: 13px;
        }

            #login input[type=submit]:hover {
                box-shadow: inset 0 1px 0 rgba(255,255,255,0.3), inset 0 -10px 10px rgba(255,255,255,0.1);
            }

        #login input[type=text]:hover,
        #login input[type=password]:hover,
        #login label:hover ~ input[type=text],
        #login label:hover ~ input[type=password] {
            /*background:#27292c;*/
            background: #b6b6b6;
        }
    </style>
    <script type="text/javascript">
        function func_click(charVal) {
            finalresult = document.getElementById("txtPassword").value;
            document.getElementById("txtPassword").value = finalresult + charVal;
        }

        var capslockflag = false;

        function func_capslocktoggle() {

            capslockflag = !capslockflag;
            var keyboard = document.getElementById("frmLogin");
            if (capslockflag) {

                for (var z = 0; z < keyboard.btnVirKey.length; z++) {
                    keyboard.btnVirKey[z].value = keyboard.btnVirKey[z].value.toUpperCase();
                    keyboard.btnVirKey[z].name = keyboard.btnVirKey[z].value;

                }
            }
            else {
                for (var z = 0; z < keyboard.btnVirKey.length; z++) {
                    keyboard.btnVirKey[z].value = keyboard.btnVirKey[z].value.toLowerCase();
                    keyboard.btnVirKey[z].name = keyboard.btnVirKey[z].value;

                }
            }
        }
        function func_backspace() {
            resultString = document.getElementById("txtPassword").value;
            var count = resultString.length - 1;
            var interString = resultString.substr(0, count);
            document.getElementById("txtPassword").value = interString;
        }

        function func_clear() {
            document.getElementById("txtPassword").value = '';
        }
    </script>
</head>
<body id="masterbody" runat="server" style="width: 100%;">
    <form id="frmLogin" runat="server" defaultbutton="ibtnLogin">
        <ajaxtoolkit:ToolkitScriptManager ID="tsmManager" runat="server">
        </ajaxtoolkit:ToolkitScriptManager>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="height: 85px; vertical-align: top; background-color: #ffffff; opacity: 0.8; border-bottom: 2px solid #000000;">
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 500px;">
                                <img src="../Images/logo.jpg" width="180" height="85" />
                            </td>
                            <td align="left" style="font-size: 28px; font-weight: bold; font-family: korinna; color: #032d54;">Document Management Solution</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top" style="padding-top: 20px">
                    <table width="1200px" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td valign="top">
                                <table id="tblMainContent" runat="server" style="margin: 0 auto; text-align: center; width: 900px;" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="center">
                                            <div id="login">
                                                <div class="form">
                                                    <h1>Employer Log in</h1>
                                                    <div class="inset">
                                                        <p>
                                                            <label for="email">USER ID</label>
                                                            <asp:TextBox ID="txtUserName" runat="server" ToolTip='<%$ Resources:Resource, UserName %>'
                                                                EnableTheming="false"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName"
                                                                ToolTip='<%$ Resources:Resource, UserName %>' ValidationGroup="Login"></asp:RequiredFieldValidator>
                                                        </p>
                                                        <p>
                                                            <label for="password">PASSWORD</label>
                                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" ToolTip='<%$ Resources:Resource, Password %>'
                                                                EnableTheming="false"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                                ToolTip='<%$ Resources:Resource, Password %>' ValidationGroup="Login"></asp:RequiredFieldValidator>
                                                        </p>
                                                        <p>
                                                            <asp:UpdatePanel ID="upanCaptchaImage" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Image ID="imgCaptcha" runat="server"></asp:Image>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ibtnRefresh" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <asp:UpdatePanel ID="upanCaptchaText" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" ToolTip='<%$ Resources:Resource, Captcha %>'
                                                                        EnableTheming="false" autocomplete="off"></asp:TextBox>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ibtnRefresh" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <asp:ImageButton ID="ibtnRefresh" runat="server" ImageUrl="~/Images/refresh-captcha.png"
                                                                OnClick="ibtnRefresh_Click" CausesValidation="false" ToolTip="Click Here To Refresh The Captcha." />
                                                            <asp:RequiredFieldValidator ID="rfvCaptcha" runat="server" ControlToValidate="txtCaptcha"
                                                                ToolTip='<%$ Resources:Resource, Captcha %>' ValidationGroup="Login"></asp:RequiredFieldValidator>
                                                        </p>
                                                    </div>
                                                    <p class="p-container">
                                                        <span>
                                                            <asp:HyperLink ID="hplForgotPassword" runat="server" Text="Forgot Password" Visible="true"></asp:HyperLink></span>
                                                        <ajaxtoolkit:ModalPopupExtender ID="mpopForgotPassword" runat="server" TargetControlID="hplForgotPassword"
                                                            PopupControlID="tblModal" BackgroundCssClass="modalpopextenderbackground" CancelControlID="ibtnFPCancel">
                                                        </ajaxtoolkit:ModalPopupExtender>
                                                        <asp:Button ID="ibtnLogin" runat="server" ToolTip="Click Here To Login." OnClick="ibtnLogin_Click" BackColor="white" ForeColor="#023361" Text="Login" CssClass="button" ValidationGroup="Login" />
                                                    </p>
                                                </div>
                                            </div>
                                        </td>
                                        <td align="center" valign="top" width="450px" height="240px">
                                            <div id="divVirtualKeyBoard" runat="server" visible="true" style="background-color: #0a3a66; padding: 15px; border-radius: 6px;">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:UpdatePanel ID="upanCaptchaValidator" runat="server">
                                                <ContentTemplate>
                                                    <asp:CompareValidator ID="cpvCaptchaCode" runat="server" ControlToValidate="txtCaptcha"
                                                        Text="Invalid Captcha Code<br/>" Operator="Equal" Display="Dynamic" ValidationGroup="Login"></asp:CompareValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ibtnRefresh" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:Label ID="lblError" runat="server" Text="" EnableTheming="false" ForeColor="Red"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <%--end new login form--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" style="text-align: left; position: fixed; bottom: 0; border-top: 1px solid #000000; width: 100%; background-color: #ffffff; opacity: 0.8; padding: 8px 0px;">Copyright &copy; 2018 StockHolding DMS Ltd. All rights reserved.
                </td>

            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblModal" runat="server" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="center" style="padding-top: 15px; padding-bottom: 10px;">
                                        <table width="341" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="center" valign="bottom" class="login-container-head">Forgot Password
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="login-container-bg">
                                                    <table width="321" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td colspan="3" align="left"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="center">Access Your User Name And Password
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="left"></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="100" height="40" style="padding-left: 15px;">
                                                                <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
                                                            </td>
                                                            <td height="40" class="style1">
                                                                <asp:TextBox ID="txtFPUserName" runat="server" ValidationGroup="" ToolTip='<%$ Resources:Resource, UserName %>'></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <%--<asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ToolTip='<%$ Resources:Resource, UserName %>'></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="center">OR
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="100" height="40" style="padding-left: 15px;">
                                                                <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
                                                            </td>
                                                            <td height="40" class="style1">
                                                                <asp:TextBox ID="txtEmail" runat="server" ValidationGroup="" ToolTip='<%$ Resources:Resource, Password %>'></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <%--<asp:RequiredFieldValidator ID="rfvemail" runat="server" ControlToValidate="txtPassword" ToolTip='<%$ Resources:Resource, Password %>'></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="40" align="center" colspan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:ImageButton ID="ibtnSubmit" runat="server" ToolTip="Click Here To Submit." SkinID="LoginButton"
                                                                    OnClick="ibtnSubmit_Click" ValidationGroup="" />
                                                                <asp:ImageButton ID="ibtnFPCancel" runat="server" ToolTip="Click Here To Cancel."
                                                                    SkinID="CancelButton" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Label ID="Label3" runat="server" Text="" Font-Size="Small" Width="180PX" EnableTheming="false"
                                                                    ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ibtnSubmit" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="visibility: hidden;">
                    <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click Here To Cancel." SkinID="CancelButton" CausesValidation="false" OnClick="ibtnCancel_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
