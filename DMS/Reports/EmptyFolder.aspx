<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="EmptyFolder.aspx.cs" Inherits="DMS.Folder.EmptyFolder" %>

<asp:Content ID="cphFolderViewHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>
<asp:Content ID="cphFolderViewMain" ContentPlaceHolderID="cphmain" runat="server">
  
     <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css"/>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.5.6/css/buttons.dataTables.min.css" />  

     <script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.js"></script>
     <script type="text/javascript" src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>

     
    <%--<script src="../js/jquery-1.9.0.min.js"></script>--%>
    
    <%--<script src="../js/jquery.dataTables.js"></script>--%>
  <%--  <script src="../js/jquery.dataTables.min.js"></script>
    
    <link href="../Css/jquery.dataTables.css" rel="stylesheet" />--%>
 <script type="text/javascript">

   <%--  $(document).ready(function () {

         $('#<%=gvrEmpFolder.ClientID %>').prepend($("<thead></thead>").append($('#<%=gvrEmpFolder.ClientID %>').find("tr:first"))).DataTable({

              });

          });
    --%>
     </script>
  <%--  <script type="text/javascript">
        $(function () {
            debugger;
            $("#gvrEmpFolder").DataTable(
            {
                bLengthChange: true,
                lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                bFilter: true,
                bSort: true,
                bPaginate: true
            });
        });
    </script>--%>

    <center>
        <asp:Label ID="lblTitle" runat="server" SkinID="Title">Folder Report</asp:Label>
    </center>
    <%--   <center>
        <asp:ImageButton ID="ibtnAddNew" runat="server" OnClick="ibtnAddNew_Click" SkinID="AddNewButton"
            ToolTip='<%$ Resources:Resource,AddNew %>' CausesValidation="false" />
    </center>--%>
    <center>
    <table width="931" cellpadding="0" cellspacing="3" border="0">
        <tr>
            <td align="left">
                <uc:EntityModule ID="emodModule" runat="server" DisplayCategory="false" />
            </td>
            </tr>
        <tr>
            <td align="left">
                <%--<asp:ImageButton ID="ibtnsubmit" runat="server" OnClick="ibtnsubmit_Click" ImageUrl="~/Images/DMSButton/submit.jpg" />--%>
                <div>
                    <asp:Label ID="lblcount" runat="server" Text="Document Count" Visible="true"></asp:Label>
                    <asp:TextBox ID="txtcount" runat="server" Visible="true" style="width:100px"></asp:TextBox> <asp:Button ID="btnsearch" runat="server" Text="Search" OnClick="btnsearch_Click"></asp:Button> <asp:Label ID="lbldoccount" runat="server" style="margin-top: -20px;margin-left: 800px"></asp:Label> <div></div>  </div> <span> </span>
               
                <br />
               
                 <asp:GridView ID="gvrEmpFolder" AutoGenerateColumns="false" Width="100%" AllowPaging="true" OnPageIndexChanging="gvrEmpFolder_PageIndexChanging" PageSize="2" DataKeyNames="RepositoryId,RepositoryName,MetaTemplateName,FolderLeastPath,DocCount"  runat="server">

                        <Columns>
                              <asp:TemplateField HeaderText="Sr." ControlStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RepositoryName" HeaderText="Repository Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="RepositoryName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="MetaTemplateName" HeaderText="MetaTemplate Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="MetaTemplateName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="FolderLeastPath" HeaderText="Folder Path" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="FolderLeastPath"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocCount"  HeaderText="Document Count" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
 SortExpression="Document Count"
                                NullDisplayText="N/A" />
                        </Columns>
                     <RowStyle VerticalAlign="Middle" ForeColor="#000066"></RowStyle>
                    </asp:GridView>
                <%--<div style="margin-left: 150px">--%>
                <br />
                <div>
                    <%--<asp:UpdatePanel ID="upanPanel" runat="server">--%>
                    <%--<ContentTemplate>--%>
                 
                    <%--<asp:Button ID="btnexport" runat="server" OnClick="btnexport_Click" Text="Export" />--%>
                    <asp:ImageButton ID="imgsubmit" runat="server" Width="100px" ImageUrl="~/Images/DMSButton/export.jpg" OnClick="imgsubmit_Click" />
                    <%--</ContentTemplate>--%>

                    <%--</asp:UpdatePanel>--%>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="middle">
                  
            </td>
        </tr>
    </table>
        </center>
</asp:Content>
