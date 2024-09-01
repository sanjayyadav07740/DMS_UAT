<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="DMS.Shared.ChangePassword" %>
<asp:Content ID="cphChangePasswordHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphChangePasswordMain" ContentPlaceHolderID="cphmain" runat="server">
<center><asp:Label ID="lblTitle" runat="server" Text="Change Password" SkinID="Title"></asp:Label> </center>
<center>
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblOldPassword" runat="server" Text="Old Password : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtOldPassword" runat="server" ToolTip='<%$ Resources:Resource,Password %>' TextMode="Password"
                        MaxLength="20"> </asp:TextBox>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword"
                        ToolTip='<%$ Resources:Resource,Password %>'></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNewPassword" runat="server" Text="New Password : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtNewPassword" runat="server" ToolTip='<%$ Resources:Resource,Password %>' TextMode="Password"
                        MaxLength="20"> </asp:TextBox>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                        ToolTip='<%$ Resources:Resource,Password %>'></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revNewPassword" runat="server" ForeColor="Red"
                                    Display="None" ErrorMessage="Your password must be at least 8 characters long, contain at least one one lower case letter, one upper case letter, one digit and one special character, Valid special characters."
                                    ControlToValidate="txtNewPassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"></asp:RegularExpressionValidator>
                    <ajaxtoolkit:ValidatorCalloutExtender ID="vceNewPassword" runat="server" TargetControlID="revNewPassword"></ajaxtoolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtConfirmPassword" runat="server" ToolTip='<%$ Resources:Resource,Password %>' TextMode="Password"
                        MaxLength="20"> </asp:TextBox>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        ToolTip='<%$ Resources:Resource,Password %>'></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                 <asp:CompareValidator ID="cpvExistingPassword" runat="server" ControlToValidate="txtOldPassword" 
                Text="Invalid Existing Password<br/>" Operator="Equal" Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator ID="cpvOldNewPassword" runat="server" ControlToValidate="txtNewPassword" ControlToCompare="txtOldPassword"
                Text="Old Password And New Password Can Not Be Same<br/>" Operator="NotEqual" Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator ID="cpvComparePassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                Text="New Password And Confirm Password Does Not Match" Operator="Equal" Display="Dynamic"></asp:CompareValidator>
                <ajaxtoolkit:PasswordStrength ID="pwdsPasswordStrength" runat="server" DisplayPosition="RightSide" TargetControlID="txtNewPassword"
                MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1" MinimumUpperCaseCharacters="1"
                PreferredPasswordLength="8" StrengthIndicatorType="Text"   >
                </ajaxtoolkit:PasswordStrength>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3">
                    <asp:ImageButton ID="ibtnSubmit" runat="server" 
                        ToolTip='<%$ Resources:Resource,Submit %>' SkinID="SubmitButton" 
                        onclick="ibtnSubmit_Click"/>
                    <asp:ImageButton ID="ibtnBack" runat="server"  CausesValidation="false" SkinID="CancelButton" OnClick="ibtnBack_Click"
                        ToolTip='<%$ Resources:Resource,Back %>' />
                </td>
            </tr>
        </table>
</center>
</asp:Content>
