<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentViewer.aspx.cs" Inherits="DMS.Shared.SharedDocumentViewer" %>

<%@ Register Src="~/UserControl/DocumentViewer.ascx" TagPrefix="uc" TagName="DocumentViewer"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $(document).on({
                mouseover: function () {
                    $("#divPrintScr").hide();
                },

                mouseout: function () {
                    $("#divPrintScr").show();
                }

            });

            $(window).keyup(function (e) {
                if (e.keyCode == 91) {     // For Left Window key
                    $("body").hide();
                    window.location.href = "SearchDocument.aspx";
                }
            });

            $(window).keyup(function (e) {
                if (e.keyCode == 92) {  // For Right Window key
                    $("body").hide();
                    window.location.href = "SearchDocument.aspx";
                }
            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server" Visible="false">
    <uc:DocumentViewer ID="dvDocumentViewer" runat="server" />
</asp:Content>
