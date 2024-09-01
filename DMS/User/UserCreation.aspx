<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="UserCreation.aspx.cs" Inherits="DMS.User.UserCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .style3
        {
            height: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<center><asp:Label ID="lblTitle" runat="server"  SkinID="Title"></asp:Label> </center>
    <center>
        <table width="931" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <asp:Label ID="lblUserName" runat="server" Text="Login ID :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" TabIndex="1"></asp:TextBox>
                    
               
                </td>
                <td style="width:26px" align="left">
                    <asp:RequiredFieldValidator ID="rfvRUserName" runat="server" ControlToValidate="txtUserName"
                        ToolTip='<%$ Resources:Resource,LoginID %>'></asp:RequiredFieldValidator>
                </td>
                <td align="left">
                    <asp:Label ID="lblAddress" runat="server" Text="Address :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" TabIndex="9"></asp:TextBox>
                </td>
            </tr>
            <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblPassword" runat="server" Text="Password :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password" TabIndex="2"></asp:TextBox>
                </td>
               <td style="width:26px" align="left">
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ToolTip='<%$ Resources:Resource,Password %>'></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ForeColor="Red"
                                    Display="None" ErrorMessage="Your password must be at least 8 characters long, contain at least one one lower case letter, one upper case letter, one digit and one special character, Valid special characters."
                                    ControlToValidate="txtPassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"></asp:RegularExpressionValidator>
                    <ajaxtoolkit:ValidatorCalloutExtender ID="vcePassword" runat="server" TargetControlID="revPassword"></ajaxtoolkit:ValidatorCalloutExtender>
                </td>
                <td align="left">
                    <asp:Label ID="lblCity" runat="server" Text="City :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" TabIndex="10"></asp:TextBox>
                      <ajaxtoolkit:FilteredTextBoxExtender ID="fteCity" runat="server" TargetControlID="txtCity" SkinID="OnlyString"></ajaxtoolkit:FilteredTextBoxExtender>
                </td>
            </tr>
             <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUserType" runat="server" Text="User Type :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUserType" runat="server" TabIndex="3">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="lblCountry" runat="server" Text="Country :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                        TabIndex="11">
                    </asp:DropDownList>
                </td>
            </tr>
             <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblRole" runat="server" Text="Role :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server" TabIndex="4">
                    </asp:DropDownList>
                </td>
               <td style="width:26px" align="left">
                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole" InitialValue="-1"
                        ToolTip='<%$ Resources:Resource,Role %>'></asp:RequiredFieldValidator>
                </td>
                <td align="left">
                    <asp:Label ID="lblState" runat="server" Text="State :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" TabIndex="12">
                    </asp:DropDownList>
                </td>
            </tr>
             <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUserIS" runat="server" Text="UserIS :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUserIS" runat="server" TabIndex="5" OnSelectedIndexChanged="ddlUserIS_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="lblPinCode" runat="server" Text="PinCode :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPinCode" runat="server" MaxLength="10" TabIndex="13"></asp:TextBox>
                   <ajaxtoolkit:FilteredTextBoxExtender ID="ftePinCode" runat="server" TargetControlID="txtPinCode" SkinID="OnlyNumber"></ajaxtoolkit:FilteredTextBoxExtender>
                </td>
            </tr>
             <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblFirstName" runat="server" Text="FirstName :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" TabIndex="6"></asp:TextBox>
                     <ajaxtoolkit:FilteredTextBoxExtender ID="fteFirstName" runat="server" TargetControlID="txtFirstName" SkinID="OnlyString"></ajaxtoolkit:FilteredTextBoxExtender>
                </td>
                <td style="width:26px" align="left">
                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                        ToolTip='<%$ Resources:Resource,FirstName %>'></asp:RequiredFieldValidator>
                </td>
                <td align="left">
                    <asp:Label ID="lblMobileNo" runat="server" Text="Mobile No :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="15" TabIndex="14"></asp:TextBox>
 <ajaxtoolkit:FilteredTextBoxExtender ID="fteMobileNo" runat="server" TargetControlID="txtMobileNo" SkinID="OnlyNumber"></ajaxtoolkit:FilteredTextBoxExtender>
                </td>
            </tr>
             <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblMiddleName" runat="server" Text="MiddleName :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="50" TabIndex="7"></asp:TextBox>
                     <ajaxtoolkit:FilteredTextBoxExtender ID="fteMiddleName" runat="server" TargetControlID="txtMiddleName" SkinID="OnlyString"></ajaxtoolkit:FilteredTextBoxExtender>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="lblEmailID" runat="server" Text="Email ID :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmailID" runat="server" MaxLength="50" TabIndex="15"></asp:TextBox>
                </td>
            </tr>
             <tr style="height:8px"><td colspan="5"></td></tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblLastName" runat="server" Text="LastName :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" TabIndex="8"></asp:TextBox>
                    <ajaxtoolkit:FilteredTextBoxExtender ID="fteLastName" runat="server" TargetControlID="txtLastName" SkinID="OnlyString"></ajaxtoolkit:FilteredTextBoxExtender>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="lblStatus" runat="server" Text="Status :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="16">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <br />
                    <br />
                    <hr />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:ImageButton ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" SkinID="SubmitButton"
                        TabIndex="17" />
                </td>
                <td colspan="2" align="left">
                    <asp:ImageButton ID="ibtnBack" runat="server" OnClick="ibtnBack_Click" SkinID="CancelButton" CausesValidation="false"
                        TabIndex="18" Visible="true" />
                </td>
            </tr>
        </table>
    </center>
    <asp:HiddenField ID="hdfUserName" runat="server" />
</asp:Content>
