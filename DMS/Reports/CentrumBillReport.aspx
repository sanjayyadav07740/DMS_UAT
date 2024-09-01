<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="CentrumBillReport.aspx.cs" Inherits="DMS.Reports.CentrumBillReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <table  width="931" cellpadding="0" cellspacing="3" border="0" align="center">
        <tr>
            <td align="right">
                   <asp:Label runat="server" ID="lblDepartment"  Text="Metatemplate :"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddldepartment" runat="server"  AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RfvDepartment" runat="server" ControlToValidate="ddldepartment" ErrorMessage="*"></asp:RequiredFieldValidator></td><td></td>
        </tr>

       <%-- <tr>
            <td align="right">
                <asp:Label runat="server" ID="lblBranch"  Text="Branch :"></asp:Label>
                 
            </td>
            <td> 
                <asp:DropDownList ID="ddlBranch" runat="server"  ></asp:DropDownList>
            </td>
            <td></td><td></td>
        </tr>--%>
     <tr>
                    <td align="center">
                        <asp:Label ID="lblFrom" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtFrom" runat="server" ToolTip="Please Enter From Date."></asp:TextBox>&nbsp;&nbsp
    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFrom"
        PopupButtonID="txtFrom" Format="dd/MM/yyyy">
    </ajaxtoolkit:CalendarExtender>
                      
                    </td>
                   
                    <td align="center">
                        <asp:Label ID="lblTo" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTo" runat="server" ToolTip="Please Enter To Date."></asp:TextBox>&nbsp;&nbsp;
            <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTo"
                PopupButtonID="txtTo" Format="dd/MM/yyyy">
            </ajaxtoolkit:CalendarExtender>
                    </td>
            
                   
                </tr>

         <tr>
            <td></td>
            <td>
                <asp:ImageButton ID="ibtnSubmit" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnSubmit_Click" />
                <asp:ImageButton ID="ibtnExport" runat="server" SkinID="ExportButton" OnClick="ibtnExport_Click" />
            </td>
            <td></td>
            <td></td>
        </tr>
          <tr>
            <td colspan="4" align="center">
              <asp:GridView ID="gvCentrumBill" runat="server" AutoGenerateColumns="false" 
                    ShowHeaderWhenEmpty="true" AllowPaging="true"
                    PageSize="15" Width="923px" OnPageIndexChanging="gvCentrumBill_PageIndexChanging" >
                    <Columns>
                         <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>    
                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name " /> 
                        <asp:BoundField DataField="PageCount" HeaderText="Page Count" />      
                        <asp:BoundField DataField="CreatedOn" HeaderText="Upload Date" />
                        <asp:BoundField DataField="MetatemplateName" HeaderText="Department" />      
                         <asp:BoundField DataField="FolderName" HeaderText="Pan No" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Area" />                        
                        
                    </Columns>
                   
                </asp:GridView>
            </td>
        </tr>
        </table>
</asp:Content>
