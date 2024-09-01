<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginForm.aspx.cs" Inherits="DMS.Shared.LoginForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Page-Enter" content="Alpha(opacity=100)" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />

    <title>DMS-UAT</title>

    <%-- <script src="Scripts/jquery-1.8.2.js"></script>
    <%--<link href="Content/css/bootstrap.css" rel="stylesheet" />--%>
    <script src="../js/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script src="https://kit.fontawesome.com/a076d05399.js" crossorigin="anonymous"></script>
    <link href="../Css/newloginpage.css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
        function check() {
            debugger;
            var name = document.getElementById("txtusername").value;
            var pass = document.getElementById("txtpassword").value;

            if (name == "") {
                document.getElementById("lblError").innerText = "Please Enter the User Id";
                return false
            } else if (pass == "") {

                document.getElementById("lblError").innerText = "Please Enter the Password";
                return false
            } else {

                return true;
            }

        }
    </script>


    <style type="text/css">
        .login-page {
            background-image: url(../../../Images/DashboardV1/bg3.jpg);
            background-size: cover;
            height: 100vh;
            left: 0px !important;
        }

        .alert-danger {
            color: #31708f;
            background-color: #d9edf7;
            border-color: #bce8f1;
            position: absolute;
            width: 96%;
            z-index: 2;
            left: 2%;
            top: 1%;
            margin-left: -1%;
            font-size: 13px;
        }

        .alert-dismissable {
            padding-right: 35px;
        }

        .alert {
            padding: 15px;
            margin-bottom: 20px;
            border: 1px solid transparent;
            border-radius: 4px;
        }

        .alert-dismissable .close {
            position: relative;
            top: 0px;
            right: 0px;
            color: inherit;
            text-decoration: none;
        }

        .close {
            float: right;
            font-size: 21px;
            font-weight: 700;
            line-height: 1;
            color: #000;
            text-shadow: 0 1px 0 #fff;
            filter: alpha(opacity=20);
            opacity: .2;
        }

        .fadeIn {
            display: inline-block;
            margin: 0px auto;
            position: fixed;
            transition: all 0.5s ease-in-out;
            z-index: 1031;
            top: 20px;
            left: 0px;
            right: 0px;
        }

        .fadeOut {
            height: 1px;
            position: absolute;
            top: -100px;
            transition: all 0.5s ease-in-out;
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
<body>
    <form id="frmLogin" runat="server" defaultbutton="btnLogin">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <%--<div class="main-content login-page screen-3" style="background-image: url(../Images/DMSButton/VB.jpg);">--%>
            <div class="main-content login-page screen-3" style="background-image: url(../Images/BG-3.jpg);">
                
                <section>
            <div class="db-overlay" style="background-color: rgb(127, 140, 140);"></div>
            <div class="db-head login-head" style="background-color: rgb(255, 255, 255);">
                <div class="main-container ot-pd">
                    <div class="db-head-content">
                        <div class="left-block">
                            <div class="menu-tab">
                                <!--<div class="menu-icon icon icon-menu-group">
                                </div>-->
                                <div class="menu-main-content">

                                    <div class="close-btn icon icon-wrong"></div>
                                </div>
                            </div>
                            <div class="main-logo">
                                <a href="#">
                                    <img src="https://zinghrnewux.blob.core.windows.net/virescent/Image/Logo/23445?sv=2015-04-05&amp;sr=c&amp;sig=DE0Vm6dWZTYUEUinHWktKQPbfoAZeVRwn1Q%2BaMuijYc%3D&amp;se=2021-06-03T08%3A47%3A43Z&amp;sp=rwl" alt="">
                                    <span>zing</span>
                                </a>
                            </div>
                        </div>
                        <div class="right-block" style="display:none">
                            <div class="imp-link" id="HDQuery" style="display: none;">
                                <a href="https://support.zinghr.com/support/home" target="_blank" style="color: rgb(225, 223, 232);">
                                    <span class="icon icon-faq" style="color: rgb(225, 223, 232);"></span>
                                    <span>have a query</span>
                                </a>
                            </div>
                            <div class="imp-link" id="HDSupport">
                                <a href="https://support.zinghr.com/support/tickets/new" target="_blank" style="color: rgb(225, 223, 232);">
                                    <span class="icon icon-24hours" style="color: rgb(225, 223, 232);"></span>
                                    <span>need support?</span>
                                </a>
                            </div>
                            
                            <div class="lang-sel" style="display:none">
                                <div class="dropdown-block single-select" id="anni-select">
                                    <span class="selected-item" style="color: rgb(225, 223, 232);">language</span>
                                    <span class="sel-down-arrow" style="border-top-color: rgb(225, 223, 232);"></span>
                                    <span class="sel-down-arrow" style="border-top-color: rgb(225, 223, 232);"></span>
                                    <div class="dropdown-list">
                                        <div class="dropdown-item">english</div>
                                        <div class="dropdown-item">arabic</div>
                                        <!--<div class="dropdown-item">hindi</div>-->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </section>

                <section>            
            <div class="login-body main-container ot-pd">
                <div class="login-block">
                    <div class="login-title-block" style="padding-top: 35px;padding-right:17px">
                        <%--<span class="icon icon-lock" style="color: rgb(199, 193, 171);"></span>--%>
                        <%--<span class="fa fa-lock fa-2x" style="color: rgb(199, 193, 171);"></span>--%>
                        <span ><img src="../Images/padlock_closed_inv.png" / style="height:35px"></span>
                        <%--<img src="../Images/icons8-lock-150.png" style="height:49px;" />--%>
                       <span style="height:5px"></span>
                        <span class="login-txt" style="color: rgb(199, 193, 171);">employer login</span>
                    </div>
                    <div class="login-body-block"> 
                        <div>
                  <asp:Label ID="lblError" runat="server" Text="" EnableTheming="false" ForeColor="Red"
                                                Font-Bold="true"></asp:Label>
                        </div>
                        <div class="input-group com-code" style="display: none;">
                            <input type="text" id="txtCompanyCode" required="" style="border-color: rgb(199, 193, 171);">
                            <label for="txtCompanyCode" data-languagetext="companycode" style="color: rgb(199, 193, 171);">company code</label>
                        </div>

                        <div class="input-group emp-code">
                            <%--<input type="text" id="txtEmpCode" required="" style="border-color: rgb(199, 193, 171);">--%>
                            <asp:TextBox ID="txtusername" runat="server" autocomplete="off" style="width: 270px"  ></asp:TextBox>
                            <label for="txtEmpCode" style="top: -10px; color: rgb(199, 193, 171);">UserId</label>
                        </div>

                        <div class="input-group pas-code">
                            <%--<input type="password" id="txtPassword" maxlength="24" required="" style="border-color: rgb(199, 193, 171);">--%>
                            <asp:TextBox ID="txtpassword" runat="server" autocomplete="off" style="width: 270px" TextMode="Password"   ></asp:TextBox>
                            <label for="txtPassword" style="top: -10px; color: rgb(199, 193, 171);">Password</label>
                          <%--  <span class="pass-eye icon icon-visibility-no" style="color: rgb(199, 193, 171);"></span>--%>
                        </div>
                        <div class="input-group pas-code">
                            <div style="padding-left:150px" >    <%--<asp:Image ID="imgCaptcha" runat="server"></asp:Image>--%>
                                <p>
                                <asp:UpdatePanel ID="upanCaptchaImage" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Image ID="imgCaptcha" runat="server"></asp:Image>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btn" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upanCaptchaText" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" ToolTip='<%$ Resources:Resource, Captcha %>'
                                                                        EnableTheming="false" autocomplete="off" style="width:270px;margin-left:-150px" ></asp:TextBox>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btn" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                 
                                                            <asp:RequiredFieldValidator ID="rfvCaptcha" runat="server" ControlToValidate="txtCaptcha"
                                                                ToolTip='<%$ Resources:Resource, Captcha %>' ValidationGroup="Login"></asp:RequiredFieldValidator>
                                </p>

                                </div>
                            <div>
                                <asp:UpdatePanel ID="upanCaptchaValidator" runat="server">
                                                <ContentTemplate>
                                                    <asp:CompareValidator ID="cpvCaptchaCode" runat="server" ControlToValidate="txtCaptcha"
                                                        Text="Invalid Captcha Code<br/>" Operator="Equal" Display="Dynamic" ValidationGroup="Login"></asp:CompareValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btn" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                            </div>
                                                              
                        </div>
                         <div runat="server" style="width:100px;align-items:center">

                          <span><asp:Button ID="btn" runat="server" OnClick="btn_Click" UseSubmitBehavior="false" Text="Refresh Captcha." style="color:rgb(199, 193, 171);border:none"  ToolTip="Click Here To Refresh The Captcha." /></span> 
                              
                          </div>  
                        <div class="login-input-list">
                            <div class="login-input-item re-me" id="chkRemember" style="color: rgb(199, 193, 171);display:none">
                                <span class="icon icon-ok"></span>
                                <span>Remember me</span>
                            </div>
                            <div class="login-input-item for-pass" id="btnForgot" style="display:none">
                                <span><a href="" style="color: rgb(199, 193, 171);">Forgot password?</a></span>
                            </div>
                        </div>
                        <div class="db-btn login-btn sub-btn" style="background-color:rgb(199, 193, 171)" runat="server"    >
                            <asp:Button ID="btnlogin" OnClientClick="if(!check()) return false"  runat="server" Text="login" BorderStyle="None" OnClick="btnlogin_Click" ClientIDMode="Static"  CausesValidation="true" UseSubmitBehavior="False" ValidationGroup="Login"/>                            
                        </div>
                        <div class="btn-group punch" style="display:none">
                            <div class="db-btn PunchBtn PunchButton" data-status="PUNCHIN" style="color: rgb(199, 193, 171); border-color: rgb(199, 193, 171);">
                                <span>punch in</span>
                            </div>
                            <div class="db-btn PunchBtn PunchButton" data-status="PUNCHOUT" style="color: rgb(199, 193, 171); border-color: rgb(199, 193, 171);">
                                <span>punch out</span>
                            </div>
                        </div>
                        <div class="login-footer footer-3" style="background-color: rgb(199, 193, 171);display:none;">
                            <div class="login-content-block">
                                <span style="color: rgb(199, 193, 171);">download app</span>
                                <div class="down-links">
                                    <a href="https://itunes.apple.com/in/app/zinghr/id967984663?mt=8" target="_blank" style="color: rgb(199, 193, 171);"><span class="icon icon-apple"></span></a>
                                    <a href="https://play.google.com/store/apps/details?id=com.zinghr.app&amp;hl=en" target="_blank" style="color: rgb(199, 193, 171);"><span class="icon icon-google-play"></span></a>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="zingo-desc">
                    
                    <div class="zingo-content">
                        <span class="loginTitle" style="color: rgb(85, 85, 85);">Welcome to ZingHR</span>
                        <span class="loginSubtitle" style="color: rgb(85, 85, 85);">Change Enabling a more connected, truly digital workforce to "Enabling a more connected, truly digital workforce"</span>
                        <a href="https://" target="_blank" class="login-link login-link-url " style="color: rgb(85, 85, 85);">Discover More...</a>
                    </div>
                </div>
                <div class="login-footer" style="background-color: rgb(199, 193, 171);">
                    <div class="login-content-block">
                        <span style="color: rgb(199, 193, 171);">download app</span>
                        <div class="down-links">
                            <a href="https://itunes.apple.com/in/app/zinghr/id967984663?mt=8" target="_blank" style="color: rgb(199, 193, 171);"><span class="icon icon-apple"></span></a>
                            <a href="https://play.google.com/store/apps/details?id=com.zinghr.app&amp;hl=en" target="_blank" style="color: rgb(199, 193, 171);"><span class="icon icon-google-play"></span></a>
                        </div>
                    </div>
                </div>
            </div>
         
        </section>

                <section></section>
            </div>
        </div>
    </form>
</body>
</html>
