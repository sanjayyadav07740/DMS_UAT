<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="UserViewer.aspx.cs" Inherits="DMS.Reports.UserViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
                <asp:Label ID="lblusername" runat="server" Text="UserName"></asp:Label>
                </td>
            <td>
                &nbsp;
                <asp:DropDownList ID="Ddluserviewerlist" runat="server">
                 
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
              <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
            </td>
            <td>
             
                &nbsp;
                
                <asp:TextBox ID="Txtfrom" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="imgCal" runat="server" ImageUrl="../Images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                                <cc1:CalendarExtender ID="CalenderDateOfLogin" runat="server" TargetControlID="Txtfrom"
                                Format="dd-MMM-yyyy" PopupButtonID="imgCal" Enabled="True">
                            </cc1:CalendarExtender>

                            <asp:RegularExpressionValidator ID="Revtxtfrom" runat="server" ControlToValidate="Txtfrom"
                                Text="Invalid Date(e.g 02-Feb-2011)" Display="Dynamic" ForeColor="Red" Font-Size="X-Small"
                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-(19|20)\d\d$"
                                SetFocusOnError="true" ErrorMessage="Invalid Date(e.g 02-Feb-2011)"></asp:RegularExpressionValidator>
                            
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            &nbsp;
                <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
            </td>
            <td>&nbsp;
                <asp:TextBox ID="Txtto" runat="server"></asp:TextBox>

                 <asp:ImageButton ID="imgcal2" runat="server" ImageUrl="../Images/Calendar_scheduleHS.png"
                                CausesValidation="False" />

                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="Txtto"
                                Format="dd-MMM-yyyy" PopupButtonID="imgcal2" Enabled="True">
                            </cc1:CalendarExtender>

                            <asp:RequiredFieldValidator ID="rfvtxtto" runat="server" ControlToValidate="Txtto"
                                ErrorMessage="*" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
               &nbsp;<asp:Button ID="btnsubmit" runat="server" Text="Submit" 
                    onclick="btnsubmit_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
                <asp:GridView ID="Grvuserviewerlist" runat="server" AutoGenerateColumns="False" 
                    style="margin-right: 0px" Width="259px" Height="106px" 
                    onpageindexchanging="Grvuserviewerlist_PageIndexChanging" >
                <Columns>
                <asp:TemplateField>
                <FooterTemplate>
                <table>
                <tr>
                <td>
                
                    <asp:DropDownList ID="Ddrlusername" runat="server">
                    </asp:DropDownList>

                    <asp:Button ID="btnFilter" runat="server" Text="Filter" />
                 </td>
                 </tr>
                 </table>
                </FooterTemplate>
                </asp:TemplateField>
                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                    <asp:BoundField DataField="Logintime" HeaderText="Login Time" NullDisplayText="N/A" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss tt}"/>
                    <asp:BoundField DataField="Logouttime" HeaderText="Logout Time" NullDisplayText="N/A" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss tt}"/>
                </Columns>
                </asp:GridView>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
