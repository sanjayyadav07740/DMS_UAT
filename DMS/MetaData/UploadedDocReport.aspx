<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="UploadedDocReport.aspx.cs" Inherits="DMS.Shared.UploadedDocReport" %>

<asp:Content ID="cphDocumentDashBoardHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphDocumentDashBoardMain" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Upload Report" SkinID="Title"></asp:Label>
    </center>
    <br />
    <div align="center">
        <table>
            <tr>
                <td colspan="3">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
                </td>

                <td>                                      
                </td>
            </tr>             
              <tr>
                <td colspan="3">
                    <table>
                        <tr>
                            <td align="left" style="width:200px;">
                                <asp:Label ID="lblFrom" runat="server" Text="From Date:"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFrom" runat="server" Width="400px" AutoComplete="off"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtFrom" 
                                    PopupButtonID="txtFrom" Format="yyyy/MM/dd">
                                </ajaxtoolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width:200px;">
                                <asp:Label ID="lblTo" runat="server" Text="To Date:"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtTo" runat="server" Width="400px" AutoComplete="off"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender ID="caltodate" runat="server" TargetControlID="txtTo"
                                    PopupButtonID="txtTo" Format="yyyy/MM/dd">
                                </ajaxtoolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:ImageButton ID="ibtnShow" runat="server" OnClick="ibtnShow_Click" ToolTip='<%$ Resources:Resource,Show %>' SkinID="SubmitButton" CausesValidation="false" />
                    <asp:ImageButton ID="ibtnExportData" runat="server" ToolTip='<%$ Resources:Resource,Export %>' SkinID="ExportButton"
                        CausesValidation="false" OnClick="ibtnExportData_Click" />
                            </td>
                        </tr>
                    </table>
                  
                    
                </td>
                <td></td>
            </tr>   
        </table>      
    </div>

    <div align="left">        
       Total files : <asp:Label runat="server" ID="lbldtcount"></asp:Label>
    </div>

    <div align="center">
        <asp:GridView ID="gvwDocumentList" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="1100px"
            PageSize="10" AllowPaging="true" DataKeyNames="ID,MetaDataCode,DocumentName,CreatedOn"
            OnPageIndexChanging="gvwDocumentList_PageIndexChanging" OnSorting="gvwDocumentList_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Sr.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MetaDataCode" HeaderText="MetaData Code" />               
                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                <asp:BoundField DataField="RepositoryName" HeaderText="Repository Name" />
                <asp:BoundField DataField="MetaTemplateName" HeaderText="MetaTemplate Name" />
                <asp:BoundField DataField="FolderPath" HeaderText="Folder Path" />
                <%--<asp:BoundField DataField="CategoryName" HeaderText="Category Name" />--%>
                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" SortExpression="CreatedOn" />

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
