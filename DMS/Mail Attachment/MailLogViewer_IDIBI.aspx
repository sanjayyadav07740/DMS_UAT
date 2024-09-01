<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="MailLogViewer_IDIBI.aspx.cs" Inherits="DMS.MailLogViewer_IDIBI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <table width="931px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblTo" runat="server" Text="To :" Width="87px" EnableTheming="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTo" runat="server" Width="500px" EnableTheming="false"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="RevTo" SetFocusOnError="true"
                    Text="Example: username@gmail.com" ControlToValidate="txtTo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                <asp:RequiredFieldValidator ID="rfvTo" runat="server" SetFocusOnError="true" ControlToValidate="txtto"
                    ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="style1">
            <td>
                <asp:Label ID="lblCC" runat="server" Text="Cc :" Width="87px"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCC" runat="server" Width="500px" EnableTheming="false"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="revCC" SetFocusOnError="true"
                    Text="Example: username@gmail.com" ControlToValidate="txtCC" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Display="Dynamic" ValidationGroup="save" ForeColor="Red" /><br />
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSubject" runat="server" Text="Subject :" Width="87px"></asp:Label>
            </td>
            <td align="left" class="style19">
                <asp:TextBox ID="txtsubject" runat="server" Width="500px" EnableTheming="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RqvSubject" runat="server" ControlToValidate="txtsubject"
                    Display="Static" ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" ValidationGroup="valGroupNewTax">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPan" runat="server" Text="MailBody :" Width="83px"></asp:Label>
            </td>
            <td align="left" class="style19">
                <%--<asp:TextBox ID="Txtmailbody" runat="server" Width="340px" TextMode="MultiLine"></asp:TextBox>--%>
                <%--<cc1:Editor ID="edtMailbody" runat="server" Height="250" Width="750"/>--%>
                <cc2:Editor ID="edtMailbody" runat="server" Height="250" Width="750" />
                <asp:RequiredFieldValidator ID="Rqvmailbody" runat="server" ControlToValidate="edtMailbody"
                    Display="Static" ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" ValidationGroup="valGroupNewTax">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblAttachment" runat="server" Text="Attachment :" Width="83px"></asp:Label>
            </td>
            <td align="left">
                <asp:FileUpload ID="filAttach" runat="server" Width="338px" />
                <%-- <asp:RequiredFieldValidator ID="rqvattachment" runat="server" 
                                        ControlToValidate="filAttach" Display="Static" ErrorMessage="*" 
                                        ForeColor="Red"  SetFocusOnError="true" 
                                        ValidationGroup="valGroupNewTax">
                            </asp:RequiredFieldValidator>
                --%>
                <%-- <td>
                            <asp:Button ID="btnUplaod" runat="server" Text="Upload" onclick="btnUplaod_Click" />
                            </td>--%>
            </td>
        </tr>
        <tr>
            <asp:Label ID="lbllabel" runat="server"></asp:Label>
        </tr>
        <tr>
            <td colspan="2" align="center">
               
                <asp:ImageButton ID="btnSubmit" runat="server" img src="../Images/DMSButton/submit.jpg" Style="text-align: right"
                    Width="102px" Height="30px" OnClick="btnSubmit_Click" />
                &nbsp;&nbsp;
                <asp:ImageButton ID="btnCancel" runat="server" img src="../Images/DMSButton/cancel.jpg"
                    Style="text-align: right" Width="102px" Height="30px" OnClick="btnCancel_Click"
                    CausesValidation="False" />
            </td>
        </tr>
        <asp:Label ID="lblerror" runat="server" Visible="false"></asp:Label>
    </table>
</asp:Content>
