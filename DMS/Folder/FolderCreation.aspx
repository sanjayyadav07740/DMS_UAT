<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="FolderCreation.aspx.cs" Inherits="DMS.Folder.FolderCreation" %>

<asp:Content ID="cphFolderHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphFolderMain" ContentPlaceHolderID="cphMain" runat="server">
    <center><asp:Label ID="lblTitle" runat="server"  SkinID="Title"></asp:Label> </center>
    <center>
        <table width="931" cellpadding="0" cellspacing="3" border="0">
            <tr>
                <td colspan="3">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayCategory="true" />
                </td>
            </tr>
            <tr>
                <td align="left" style="width:150px;">
                    <asp:Label ID="lblFolderName" runat="server" Text="Folder  Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFolderName" runat="server" ToolTip='<%$ Resources:Resource,FolderName %>' MaxLength="50"> </asp:TextBox>
                   <%-- <ajaxtoolkit:FilteredTextBoxExtender ID="fteFolderName" runat="server" TargetControlID="txtFolderName"  SkinID="String"></ajaxtoolkit:FilteredTextBoxExtender>--%>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvFolderName" runat="server" ControlToValidate="txtFolderName"
                        ToolTip='<%$ Resources:Resource,FolderName %>'></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblFolderDescription" runat="server" Text="Folder  Description : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFolderDescription" runat="server" ToolTip='<%$ Resources:Resource,FolderDescription %>'
                        TextMode="MultiLine" Rows="5"> </asp:TextBox>
                    
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblStatus" runat="server" Text="Status : "></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlStatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td >
                    <asp:ImageButton ID="ibtnSubmit" runat="server" 
                        ToolTip='<%$ Resources:Resource,Submit %>' onclick="ibtnSubmit_Click" SkinID="SubmitButton"/>
                    <asp:ImageButton ID="ibtnBack" runat="server" CausesValidation="false" 
                        ToolTip='<%$ Resources:Resource,Back %>' onclick="ibtnBack_Click" SkinID="CancelButton"/>
                </td>
            </tr>
        </table>
    </center>
    <asp:HiddenField ID="hdfFolderName" runat="server" />
    <asp:HiddenField ID="hdfMetaTemplateID" runat="server" />
    <asp:HiddenField ID="hdfParentFolderID" runat="server" />
    <asp:HiddenField ID="hdfCategoryID" runat="server" />
</asp:Content>
