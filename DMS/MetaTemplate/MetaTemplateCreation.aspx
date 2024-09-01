<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="MetaTemplateCreation.aspx.cs" Inherits="DMS.MetaTemplate.MetaTemplateCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<center><asp:Label ID="lblTitle" runat="server"  SkinID="Title"></asp:Label> </center>
    <center> <table>

        <tr>
            <td align="center" colspan="3">
           <uc:EntityModule ID="emodModule" runat="server" DisplayMetaTemplate="false" DisplayCategory="false" DisplayFolder="false" DisplayMetaDataCode="false" />
            </td>
        </tr>
        <tr>
            
            <td align="left">
                <asp:Label ID="lblMetaTemplateName" runat="server" Text="MetaTemplate Name :"></asp:Label>
            </td>
            <td  align="left">
                <asp:TextBox ID="txtMetaTemplateName" runat="server" MaxLength="50"></asp:TextBox>
            </td >
            <td align="left">
                <asp:RequiredFieldValidator ID="rfvMetaTemplateName"  runat="server" ControlToValidate="txtMetaTemplateName" ToolTip='<%$ Resources:Resource,RepositoryName  %>'></asp:RequiredFieldValidator>
                <%--<ajaxtoolkit:FilteredTextBoxExtender ID="fteMetaTemplateName" runat="server" TargetControlID="txtRepositoryName" SkinID="String"></ajaxtoolkit:FilteredTextBoxExtender>--%>
            </td>
        </tr>
        <tr>
            
            <td align="left">
                <asp:Label ID="lblMetaTemplateDescription" runat="server" Text="MetaTemplate<br/> Description : "></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMetaTemplateDescription" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
            </td>
            <td>
                
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
               
            </td>
        </tr>
        <tr>
           
            <td colspan="3" align="center">
                <asp:ImageButton ID="ibtnSave" runat="server" OnClick="ibtnSave_Click" SkinID="SubmitButton"/>&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" onclick="ibtnCancel_Click" CausesValidation="false" SkinID="CancelButton"/>
            </td>
           
        </tr>
     
    </table>
    </center>
    <asp:HiddenField ID="hdfMetaTemplateName" runat="server" />
    <asp:HiddenField ID="hdfRepositoryID" runat="server" />
</asp:Content>
