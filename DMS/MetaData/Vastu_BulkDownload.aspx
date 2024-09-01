<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Vastu_BulkDownload.aspx.cs" Inherits="DMS.Shared.Vastu_BulkDownload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script>
        /*Select All CheckBoxes*/
        function SelectAllCheckboxes(spanChk, grName) {
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];

            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" && elm[i].id != theBox.id && elm[i].id.indexOf(grName) > -1) {
                    if (elm[i].checked != xState)
                        elm[i].click();
                }
        }


        function validateCheckBoxes() {
            var valid = false;
            var gv = document.getElementById('<%= this.GridView1.ClientID %>');
             for (var i = 0; i < gv.getElementsByTagName("input").length; i++) {
                 var node = gv.getElementsByTagName("input")[i];
                 if (node != null && node.type == "checkbox" && node.checked) {
                     valid = true;
                     break;
                 }
             }
             if (!valid) {
                 alert("Invalid. Please select a checkbox to continue.");
             }
             return valid;
         }


    </script>
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
                <asp:Label runat="server" ID="lblfoldername" Text="Select Location : "></asp:Label>
            </td>

            <td>
                <asp:DropDownList runat="server" ID="drpfolder" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpfolder_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="drpfolder" InitialValue="-1" runat="server"></asp:RequiredFieldValidator>
            </td>

        </tr>

        <tr>
            <td>
                <asp:Label runat="server" ID="lblSubfolder" Text="Select Folder : "></asp:Label>
            </td>

            <td>
                <asp:DropDownList runat="server" ID="ddlSubFolder" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSubFolder_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddlSubFolder" InitialValue="-1" runat="server"></asp:RequiredFieldValidator>
            </td>

        </tr>

        <tr>
            <td colspan="3" align="center">
                <%--<asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>' SkinID="SubmitButton" CausesValidation="true" Visible="false" Width="16px" OnClick="ibtnShow_Click" />--%>
                
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
                    SkinID="SubmitButton" ToolTip='<%$ Resources:Resource,Upload %>' OnClick="ibtn_download_Click" Visible="false" />
                <asp:ImageButton ID="ibtn_delete" runat="server"  OnClientClick="javascript:return CheckBoxValidation();" CausesValidation="false"
                    SkinID="DeleteButton" ToolTip='<%$ Resources:Resource,Upload %>' OnClick="ibtn_delete_Click" Visible="false"/>
            </td>
        </tr>

        <tr align="center">
            <td colspan="2">
                <asp:GridView ID="GridView1" runat="server" Width="100%" DataKeyNames="DocumentName" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="50" AutoGenerateColumns="false">
                    <Columns>
                           <asp:TemplateField HeaderStyle-CssClass="gridheaderleft" ItemStyle-CssClass="boundfield" HeaderStyle-Width="20px">
                                            <HeaderTemplate>
                                                 <asp:CheckBox ID="ChkAll" runat="server" onclick="SelectAllCheckboxes(this,'GridView1');" />
                                            </HeaderTemplate>
                                            <ItemTemplate> 
                                                <asp:CheckBox ID="ChkAction" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1  %>
                            </ItemTemplate>
                        </asp:TemplateField>

                      


                        <asp:TemplateField HeaderText="Document Name">
                            <ItemTemplate>
                              <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("DocumentName") %>'></asp:Label>
                                 <asp:Label ID="lblFilePathAll" Text='<%#Eval("DocumentPath") %>' runat="server" Height="18px" Width="396px" Visible="false"></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>

                          <asp:TemplateField HeaderText="Size (MB)">
                            <ItemTemplate>
                               <%#Eval("Size") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </td>
        </tr>

        <tr align="center">
            <td colspan="2">
                <br />
                <asp:Button ID="cmdDownload" runat="server" Text="Download" OnClick="cmdDownload_Click" Visible="false" OnClientClick="javascript:return validateCheckBoxes();" />

                            <asp:Button ID="cmdCancel" runat="server" Text="Cancel" OnClick="cmdCancel_Click" Visible="false" />
            </td>
        </tr>
        </table>
</asp:Content>
