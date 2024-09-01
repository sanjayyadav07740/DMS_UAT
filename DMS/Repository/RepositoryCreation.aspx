<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="RepositoryCreation.aspx.cs" Inherits="DMS.Repository.RepositoryCreation" %>

<asp:Content ID="cphRepositoryCreationHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphRepositoryCreationMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table border="0" cellpadding="0" cellspacing="3" width="931" >
            <tr>
                <td align="left" style="width: 250px;">
                    <asp:Label ID="lblRepositoryName" runat="server" Text="Repository Name : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtRepositoryName" runat="server" ToolTip='<%$ Resources:Resource,RepositoryName %>'
                        MaxLength="50"> </asp:TextBox>
                    <ajaxtoolkit:FilteredTextBoxExtender ID="fteRepositoryName" runat="server" TargetControlID="txtRepositoryName"
                        SkinID="String">
                    </ajaxtoolkit:FilteredTextBoxExtender>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rfvRepositoryName" runat="server" ControlToValidate="txtRepositoryName"
                        ToolTip='<%$ Resources:Resource,RepositoryName %>'></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblRepositoryDescription" runat="server" Text="Repository Description : "></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtRepositoryDescription" runat="server" TextMode="MultiLine" Rows="5"
                        ToolTip='<%$ Resources:Resource,RepositoryDescription %>' MaxLength="500"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
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
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td >
                    <asp:ImageButton ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" ToolTip='<%$ Resources:Resource,Submit %>'
                        SkinID="SubmitButton" />
                    <asp:ImageButton ID="ibtnBack" runat="server" OnClick="ibtnBack_Click" CausesValidation="false"
                        SkinID="CancelButton" ToolTip='<%$ Resources:Resource,Back %>' />
                </td>
            </tr>
        </table>
    </center>
    <asp:HiddenField ID="hdfRepositoryName" runat="server" />
</asp:Content>
