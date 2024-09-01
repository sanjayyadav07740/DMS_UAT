<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="SBI Mutual Fund Trustee Company Private limited_Download.aspx.cs" Inherits="DMS.Shared.SBI_Mutual_Fund_Trustee_Company_Private_limited_Download" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Download Document" SkinID="Title"></asp:Label>
    </center>
    <table width="900px">
          <tr>
               <td>
                <asp:Label runat="server" ID="lblfoldername" Text="Select Folder : "></asp:Label>
            </td>

            <td>
                <asp:DropDownList runat="server" ID="drpfolder" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpfolder_SelectedIndexChanged" ></asp:DropDownList>
            </td>
          <td>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="drpfolder" InitialValue="-1" runat="server"></asp:RequiredFieldValidator>
          </td>
           
           </tr>

        <tr>
            <td colspan="3" align="center">
                <asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                    SkinID="SubmitButton" CausesValidation="true" Width="16px" OnClick="ibtnShow_Click" />
                
            </td>
                   </tr>

         <tr>
               <td>
                <asp:Label runat="server" ID="lbldate" Text="Select Date : " Visible="false"></asp:Label>
            </td>

            <td>
                <asp:DropDownList runat="server" ID="drpdate" EnableViewState="true" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="drpdate_SelectedIndexChanged"></asp:DropDownList>
            </td>
          <td>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="drpdate" InitialValue="-1" runat="server"></asp:RequiredFieldValidator>
          </td>
           
           </tr>

  <%--     <tr>
            <td colspan="3" align="center">
                <asp:GridView runat="server" ID="grvsearch" AutoGenerateColumns="false" PageSize="10" DataKeyNames="ID,DocumentPath,DocumentName" AllowPaging="true">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="DocID" Visible="false" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:checkall(this);" />
                                <asp:ImageButton ID="ibtnDeleteChecked" runat="server" CausesValidation="false" OnClick="ibtnDeleteChecked_Click" ToolTip="Click To Delete All Checked Document."  ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="60px"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRow" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />   
                          <asp:BoundField DataField="DocumentPath" HeaderText="Path" Visible="false" />    
                    </Columns>
                </asp:GridView>
                </td>
        </tr>--%>
         <tr>
            <td align="center" colspan="2">
                <asp:ImageButton ID="ibtn_download" runat="server"  OnClientClick="javascript:return CheckBoxValidation();" CausesValidation="false"
                    SkinID="DownloadButton" ToolTip='<%$ Resources:Resource,Upload %>' OnClick="ibtn_download_Click" Visible="false"/>
                <asp:ImageButton ID="ibtn_delete" runat="server"  OnClientClick="javascript:return CheckBoxValidation();" CausesValidation="false"
                    SkinID="DeleteButton" ToolTip='<%$ Resources:Resource,Upload %>' OnClick="ibtn_delete_Click" Visible="false"/>
            </td>
        </tr>
        </table>
</asp:Content>
