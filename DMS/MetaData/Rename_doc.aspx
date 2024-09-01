<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Rename_doc.aspx.cs" Inherits="DMS.Shared.Rename_doc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
        <center id=""><asp:Label ID="lblTitle" runat="server" Text="Document Index Upload" SkinID="Title"></asp:Label>
        <table width="700">
             <tr>
                <td colspan="3" align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <br /><br />
                            <asp:Label ID="Label1" runat="server" Text="Upload Excel Document"></asp:Label>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ibtnSubmit" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>

            <tr>

                <td align="center" colspan="3">
                    <br />
                    <br />
                    <asp:ImageButton ID="ibtnSubmit" runat="server"  ToolTip='<%$ Resources:Resource,Submit %>' SkinID="UploadButton" Width="16px" OnClick="ibtnSubmit_Click" />
                    <asp:ImageButton ID="ibtnBack" runat="server"  CausesValidation="false" SkinID="CancelButton"
                        ToolTip='<%$ Resources:Resource,Back %>' />
                </td>
            </tr>

        </table>
    </center>
        </table>
    </center>
</asp:Content>
