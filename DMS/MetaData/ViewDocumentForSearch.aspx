<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" EnableViewState="true" AutoEventWireup="true" CodeBehind="ViewDocumentForSearch.aspx.cs" Inherits="DMS.Shared.ViewDocumentForSearch"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <%-- <script type="text/javascript">
         function Confirm() {
             var confirm_value = document.createElement("INPUT");
             confirm_value.type = "hidden";
             confirm_value.name = "confirm_value";
             if (confirm("Do you want to Process?")) {
                 confirm_value.value = "Yes";
             } else {
                 confirm_value.value = "No";
             }
             document.forms[0].appendChild(confirm_value);
         }
    </script>--%>
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title" Text="View Document"></asp:Label>

        <div id="divSave" runat="server" visible="true">
             <asp:ImageButton ID="ibtnDownload" runat="server" ImageUrl="~/UserControl/Image/download-btn.png" OnClick="ibtnDownload_Click" style="width: 100px;"/>
          
            <br />
        </div>

        <table width="931px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" style="z-index: -1;">
                    <cc1:ShowPdf ID="pdfViewer" runat="server" Height="550px" Width="931px"
                        Style="z-index: -1;" />
                </td>
            </tr>
</table>
        <table width="931px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>&nbsp</td>
            </tr>
            
            <tr>
                <td>&nbsp</td>
            </tr>

            <tr>
                <td align="center">
                    <asp:ImageButton ID="ibtnApprove" runat="server" CausesValidation="false" SkinID="ApproveButton"
                        ToolTip='Click Here To Approve This Page.' Visible="false" OnClick="ibtnApprove_Click" />
                    &nbsp;
                 <asp:ImageButton ID="ibtnReject" runat="server" CausesValidation="false" SkinID="RejectButton"
                     ToolTip='Click Here To Reject This Page.' Visible="false" OnClick="ibtnReject_Click" />
                    &nbsp;
                     <asp:ImageButton ID="ibtnBack" runat="server" SkinID="BackButton" OnClick="ibtnBack_Click" />
                </td>
            </tr>

        </table>
    </center>
</asp:Content>
