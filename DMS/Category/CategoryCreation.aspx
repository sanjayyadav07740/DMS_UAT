<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="CategoryCreation.aspx.cs" Inherits="DMS.Category.CategoryCreation" %>

<asp:Content ID="cphCategoryHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphCategoryMain" ContentPlaceHolderID="cphMain" runat="server">
<center><asp:Label ID="lblTitle" runat="server"  SkinID="Title"></asp:Label> </center>
    <center>
        <table width="931" cellpadding="0" cellspacing="3" border="0">
            <tr>
                <td colspan="3">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayCategory="false" DisplayFolder="false"/>
                </td>
            </tr>
            <tr>
                <td align="left" style="width:150px;">
                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCategoryName" runat="server" ToolTip='<%$ Resources:Resource,CategoryName %>' MaxLength="50"> </asp:TextBox>
                    <%--<ajaxtoolkit:FilteredTextBoxExtender ID="fteCategoryName" runat="server" TargetControlID="txtCategoryName"  SkinID="String"></ajaxtoolkit:FilteredTextBoxExtender>--%>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ControlToValidate="txtCategoryName"
                        ToolTip='<%$ Resources:Resource,CategoryName  %>'> </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblCategoryDescription" runat="server" Text="Category Description : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCategoryDescription" runat="server" TextMode="MultiLine" Rows="5"
                        ToolTip='<%$ Resources:Resource,CategoryDescription %>'> </asp:TextBox>
                       
                </td>
                <td>&nbsp;</td>
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
                <td align="center" colspan="3">
                    <asp:ImageButton ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" ToolTip='<%$ Resources:Resource,Submit %>' SkinID="SubmitButton" />
                    <asp:ImageButton ID="ibtnBack" runat="server" OnClick="ibtnBack_Click" CausesValidation="false" SkinID="CancelButton"
                        ToolTip='<%$ Resources:Resource,Back %>' />
                </td>
            </tr>
        </table>
    </center>
    <asp:HiddenField ID="hdfCategoryName" runat="server" />
    <asp:HiddenField ID="hdfMetaTemplateID" runat="server" />
</asp:Content>
