<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="DocumentStatus.aspx.cs" Inherits="DMS.Reports.DocumentStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">
    <table>
        <tr>
            <td align="center">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="false" />
            </td>
        </tr>
        <tr><td align="center"> 
            <asp:Button ID="btnDocsStatus" runat="server" Text="View Document Status" 
                onclick="btnDocsStatus_Click" /></td></tr>
        <tr>
            <td>
             <asp:GridView ID="gvwDocsStatus" runat="server"  Width="100%" 
              AutoGenerateColumns="false" 
              AllowPaging="True" PageSize="10" onpageindexchanging="gvwDocsStatus_PageIndexChanging" 
              onrowdatabound="gvwDocsStatus_RowDataBound"
              AllowSorting="true" onsorting="gvwDocsStatus_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Sr.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DocumentName" HeaderText="DocumentName" SortExpression="DocumentName"
                    NullDisplayText="N/A" />
                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A" Visible="false" />
                <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"  Visible="false"
                    NullDisplayText="N/A" />
                <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                    NullDisplayText="N/A" />



                     <asp:BoundField DataField="RepositoryName" HeaderText="RepositoryName" SortExpression="RepositoryName"
                    NullDisplayText="N/A" />
                     <asp:BoundField DataField="MetaTemplateName" HeaderText="MetaTemplateName" SortExpression="MetaTemplateName"
                    NullDisplayText="N/A" />
                     <asp:BoundField DataField="FolderName" HeaderText="FolderName" SortExpression="FolderName"
                    NullDisplayText="N/A" />
                     <asp:BoundField DataField="CategoryName" HeaderText="CategoryName" SortExpression="CategoryName"
                    NullDisplayText="N/A" />


                     <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn" DataFormatString="{0:dd-MMM-yyyy}"
                    NullDisplayText="N/A" />
                     <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"
                    NullDisplayText="N/A" />
                     <asp:BoundField DataField="UpdatedOn" HeaderText="UpdatedOn" SortExpression="UpdatedOn"
                    NullDisplayText="N/A" />
                     <asp:BoundField DataField="UpdatedBy" HeaderText="UpdatedBy" SortExpression="UpdatedBy"
                    NullDisplayText="N/A" />
            </Columns>
        </asp:GridView>
               
            </td>
        </tr>
        <tr><td> <asp:ObjectDataSource
        id="srcDocumentStatus"
      
        Runat="server" />
        </td></tr>
    </table>
</asp:Content>
