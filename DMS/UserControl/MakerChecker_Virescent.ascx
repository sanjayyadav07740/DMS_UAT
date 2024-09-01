<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MakerChecker_Virescent.ascx.cs"
    Inherits="DMS.UserControl.MakerChecker_Virescent" %>

<script type="text/javascript">
    function DisplayPdfInFullScreen() {
        var pdfViewer = document.getElementsByTagName('iframe');
        pdfViewer[1].src = pdfViewer[0].src;
    }

   

</script>

<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script>
    $(document).ready(function () {
        $("button").click(function () {
            $(".blueboldvardanabg").removeClass("blueboldvardanabg");
        });
    });
</script>--%>

<div>
    <asp:UpdatePanel ID="upanPanel" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" DataKeyNames="DocumentID"
                AllowPaging="true" PageSize="1" OnPageIndexChanging="gvwDocument_PageIndexChanging"
                OnRowDataBound="gvwDocument_RowDataBound" OnRowCommand="gvwDocument_RowCommand" >
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="941">
                                <tr>
                                    <td style="width: 125px;">
                                        <asp:Label ID="lblTotalRecordLabel" runat="server" Width="125px" Text="Current Entry:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalRecord" runat="server" Font-Size="Larger" Width="100px" ForeColor="Blue"></asp:Label>
                                    </td>
                                    <td style="width: 125px;">
                                        <asp:Label ID="lblCheckedRecordLabel" runat="server" Width="125px" Text="Verified Entry:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCheckedRecord" runat="server" Font-Size="Larger" Width="100px"
                                            ForeColor="Green"></asp:Label>
                                    </td>
                                    <td style="width: 125px;">
                                        <asp:Label ID="lblPendingRecordLabel" runat="server" Width="125px" Text="Pending Entry:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPendingRecord" runat="server" Font-Size="Larger" Width="100px"
                                            ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 125px;">
                                        <asp:Label ID="lblApprovedRecordLabel" runat="server" Width="125px" Text="Approved Entry:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblApprovedRecord" runat="server" Font-Size="Larger" Width="100px"
                                            ForeColor="Orange"></asp:Label>
                                    </td>
                                    <td style="width: 125px;">
                                        <asp:Label ID="lblRejectedRecordLabel" runat="server" Width="125px" Text="Rejected Entry:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRejectedRecord" runat="server" Font-Size="Larger" Width="100px"
                                            ForeColor="DarkMagenta"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Panel ID="pnlHolderMain" runat="server" DefaultButton="btnSave">
                                <table>
                                    <tr>
                                        <td style="z-index: -1;">
                                            <cc1:ShowPdf ID="pdfViewer" runat="server" Height="425" Width="460" Style="z-index: -1;" />
                                        </td>
                                        <td>
                                            <asp:Panel ID="pnlHolder" runat="server" Height="425" Direction="LeftToRight" Width="460"
                                                ScrollBars="Auto">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="btnApprove" runat="server" SkinID="ApproveButton" CommandName="Approve"
                                                CommandArgument='<%# Eval("DocumentID") %>' CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="btnReject" runat="server" SkinID="RejectButton" CommandName="Reject"
                                                CommandArgument='<%# Eval("DocumentID") %>' CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="btnViewFullSize" runat="server" CommandName="ViewFullSize" SkinID="FullScreenButton" CommandArgument='<%# Eval("DocumentID") %>'
                                                CausesValidation="false" OnClientClick="javascript:DisplayPdfInFullScreen();">
                                            </asp:ImageButton>
                                            <table id="tblViewFullSize" runat="server" style="display: none; height: 600px; width: 80%;">
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <input id="imgClose" runat="server" type="image" src="../Images/Close.png" title="Close" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: White;">
                                                        <iframe id="pdfViewFullSize" runat="server" height="600" width="100%" />
                                                        <iframe id="ifrmDownloader" style="display: none;" target="_blank"></iframe>
                                                    </td>
                                                    <td style="width: 15px;"></td>
                                                </tr>
                                            </table>
                                            <ajaxtoolkit:ModalPopupExtender ID="mpopViewFullSize" runat="server" TargetControlID="btnViewFullSize"
                                                PopupControlID="tblViewFullSize" BackgroundCssClass="modalpopextenderbackground" CancelControlID="imgClose">
                                            </ajaxtoolkit:ModalPopupExtender>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnFirst" runat="server" CommandName="First" CausesValidation="false"
                                                SkinID="FirstButton" AccessKey="F"></asp:ImageButton>
                                            <asp:ImageButton ID="btnPrevious" runat="server" CommandName="Previous" CausesValidation="false"
                                                SkinID="PreviousButton" AccessKey="P"></asp:ImageButton>
                                            <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# Eval("DocumentID") %>'
                                                SkinID="SaveButton" OnClick="btnSave_Click"></asp:ImageButton>
                                            <asp:ImageButton ID="btnNext" runat="server" CommandName="Next" CausesValidation="false"
                                                SkinID="NextButton" AccessKey="N"></asp:ImageButton>
                                            <asp:ImageButton ID="btnLast" runat="server" CommandName="Last" CausesValidation="false"
                                                SkinID="LastButton" AccessKey="L"></asp:ImageButton>
                                            <%-- <img src="../UserControl/Image/Save.png" style="width: 20px;" onclick="javascript:downloadFile();" class="toolBarIcon" title="Save File"/>--%>
<%--                                             <asp:ImageButton ID="btnDownload" runat="server" SkinID="SaveButton" CausesValidation="false" OnClick="btnDownload_Click" ></asp:ImageButton>--%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hdfField" runat="server" Value="1" />
            <%--<asp:TextBox ID="TextBox1" runat="server" Font-Names="Kruti Dev 010"></asp:TextBox>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

