<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DashBoardCentrum.aspx.cs" Inherits="DMS.Reports.DashBoardCentrum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <center>
        <asp:Label runat="server" ID="lblTitle" SkinID="Title" Text="Dashboard"></asp:Label>
    </center>
    <table width="931" cellpadding="1" cellspacing="0" border="0">
        <tr>
            <td align="right">
                   <asp:Label runat="server" ID="lblDepartment"  Text="Department :"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddldepartment" runat="server" OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RfvDepartment" runat="server" ControlToValidate="ddldepartment" ErrorMessage="*"></asp:RequiredFieldValidator></td><td></td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label runat="server" ID="lblBranch"  Text="Branch :"></asp:Label>
                 
            </td>
            <td> 
                <asp:DropDownList ID="ddlBranch" runat="server"  ></asp:DropDownList>
            </td>
            <td></td><td></td>
        </tr>

        <tr>
            <td  align="right">
                 <asp:Label runat="server" ID="lblAccOpDateFrom"  Text="Account Opening Date From :"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOpnFrom"></asp:TextBox>
                 <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtOpnFrom"
        PopupButtonID="txtOpnFrom" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>  
            <td>
                 <asp:Label runat="server" ID="lblAccOpDateTo"  Text=" To :"></asp:Label>
            </td>           
            <td>
                <asp:TextBox runat="server" ID="txtOpnTo"></asp:TextBox>
                 <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOpnTo"
        PopupButtonID="txtOpnTo" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>
        </tr>

         <tr>
            <td  align="right">
                 <asp:Label runat="server" ID="lblAccClDateFrom"  Text="Account Closing Date From :"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFromClose" ></asp:TextBox>
                <ajaxtoolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFromClose"
        PopupButtonID="txtFromClose" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>  
            <td>
                 <asp:Label runat="server" ID="lblAccClDateTo"  Text=" To :"></asp:Label>
            </td>           
            <td>
                <asp:TextBox runat="server" ID="txtToClose" ></asp:TextBox>
                <ajaxtoolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtToClose"
        PopupButtonID="txtToClose" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>
        </tr>

          <tr>
            <td  align="right">
                 <asp:Label runat="server" ID="lblUploadFrom"  Text="Date of Upload Document From :"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtUploadFrom" ></asp:TextBox>
                 <ajaxtoolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtUploadFrom"
        PopupButtonID="txtUploadFrom" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>  
            <td>
                 <asp:Label runat="server" ID="lblUploadTo"  Text=" To :"></asp:Label>
            </td>           
            <td>
                <asp:TextBox runat="server" ID="txtUploadTo" ></asp:TextBox>
                 <ajaxtoolkit:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtUploadTo"
        PopupButtonID="txtUploadTo" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>
        </tr>

         <tr>
            <td  align="right">
                 <asp:Label runat="server" ID="lblModifyFrom"  Text="Date of Modification Document From :"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtModifyFrom" ></asp:TextBox>
                 <ajaxtoolkit:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtModifyFrom"
        PopupButtonID="txtModifyFrom" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>  
            <td>
                 <asp:Label runat="server" ID="lblModifyTo"  Text=" To :"></asp:Label>
            </td>           
            <td>
                <asp:TextBox runat="server" ID="txtModifyTo"  ></asp:TextBox>
                 <ajaxtoolkit:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtModifyTo"
        PopupButtonID="txtModifyTo" Format="yyyy-MM-dd">
    </ajaxtoolkit:CalendarExtender>
            </td>
        </tr>
        <tr>
         <td align="right">
                   <asp:Label runat="server" ID="lblStatus"  Text="Status :" ></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" >
                <asp:ListItem Value="0">--Select--</asp:ListItem>
                  <asp:ListItem Value="1"> Approve </asp:ListItem>
                  <asp:ListItem Value="2"> Reject </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
            </tr>
        <tr>
            <td></td>
            <td>
                <asp:ImageButton ID="ibtnSubmit" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnSubmit_Click" />
                <asp:ImageButton ID="ibtnExport" runat="server" SkinID="ExportButton" OnClick="ibtnExport_Click"  />
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4" align="center">
              <asp:GridView ID="gvDashboard" runat="server" AutoGenerateColumns="false" 
                    ShowHeaderWhenEmpty="true" AllowPaging="true"
                    PageSize="15" Width="923px" OnPageIndexChanging="gvDashboard_PageIndexChanging">
                    <Columns>
                         <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>    
                        <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name " />                      
                        <asp:BoundField DataField="Pan_No" HeaderText="Pan No" />      
                        <asp:BoundField DataField="Branch" HeaderText="Branch" />                           
                        <asp:BoundField DataField="Department" HeaderText="Department" />  
                        <asp:BoundField DataField="Account_Opening_Date" HeaderText="Account Opening Date" />   
                        <asp:BoundField DataField="Account_Closing_Date" HeaderText="Account Closing Date" />   
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name " />                      
                        <asp:BoundField DataField="FolderName" HeaderText="Folder Name" />                       
                        
                    </Columns>
                    <EmptyDataTemplate>
                        No Such record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        </table>
</asp:Content>
