<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MutiFieldSearch.aspx.cs" Inherits="DMS.Shared.MutiFieldSearch" MasterPageFile="~/MainMasterPage.Master" %>

<asp:Content ID="cphUploadDocumentsHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 392px;
        }
    </style>
</asp:Content>
<asp:Content ID="cphUploadDocumentsMain" ContentPlaceHolderID="cphMain" runat="server">
     <center>
        <asp:Label ID="lblTitle" runat="server" Text="Search Document" SkinID="Title"></asp:Label>
    </center>
    <table border="0" cellpadding="0" cellspacing="3" width="931">
        <tr>
           <%-- <td align="left" colspan="3">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
            </td>--%>

           <td align="left" valign="top">
                  <table cellpadding="0" cellspacing="3" border="0">
                      <tr>
                          <td>
                              <asp:Label ID="lblMrNo" runat="server" Text="MR. No."></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtMrNo" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblSeizureWise" runat="server" Text="Sr No Seizure Wise"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtSeizureWise" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblSDeedNo" runat="server" Text="S Deed NO"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtSDeedNo" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblDetailsOfBuyer" runat="server" Text="Details Of Buyer As Per Sale Deed"></asp:Label>
                           </td>
                          <td>
                              <asp:TextBox ID="txtDetailsOfBuyer" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblDetailsOfSeller" runat="server" Text="Details Of Seller"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtDetailsOfSeller" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblD_O_P" runat="server" Text="D.O.P"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtD_O_P" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblArea" runat="server" Text="Area"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtArea" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Width="125px"></asp:DropDownList>
                              <%--<asp:TextBox ID="txtState" runat="server"></asp:TextBox>--%>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblDistt" runat="server" Text="District"></asp:Label>
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlDistt" runat="server" Width="125px"></asp:DropDownList>
                              <%--<asp:TextBox ID="txtDistt" runat="server"></asp:TextBox>--%>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblTehsil" runat="server" Text="Tehsil"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtTehsil" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblVillage" runat="server" Text="Village"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtVillage" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblSyNo" runat="server" Text="SY NO"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtSyNo" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblMode" runat="server" Text="Mode Cash/Cheque"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtMode" runat="server"></asp:TextBox>
                          </td>
                      </tr>
                      </table>
               </td>
        </tr>
      
        </table>
    <table border="0" cellpadding="0" cellspacing="3" width="931">
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnShow" runat="server" SkinID="showbutton" OnClick="ibtnShow_Click" />
                <%--&nbsp;&nbsp;&nbsp
                <asp:ImageButton ID="ibtnExport" runat="server" SkinID="ExportButton"/>--%>
            </td>
        </tr>
    </table>
    <table>
        <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowSorting="True" 
                        AllowPaging="True" DataKeyNames="DOCUMENTID"
                        
                        ShowFooter="True" OnRowCommand="gvwDocument_RowCommand" OnPageIndexChanging="gvwDocument_PageIndexChanging">
                        <Columns>
                            
                            <%--<asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                               
                            </asp:TemplateField>--%>   
                            <asp:BoundField HeaderText="MR No" DataField="MR_NO" />
                            <%--<asp:BoundField HeaderText="SRNO_SEIZURE_WISE" DataField="SRNO_SEIZURE_WISE" >
                           <HeaderStyle Width="50px" HorizontalAlign="Left" Wrap="False"></HeaderStyle>


                            </asp:BoundField>
                            <asp:BoundField HeaderText="SDEEDNO_AGREEMENT" DataField="SDEEDNO_AGREEMENT" />--%>
                            <asp:BoundField HeaderText="DETAILS_OF_BUYER" DataField="DETAILS_OF_BUYER" />
                            <asp:BoundField HeaderText="DETAILS_OF_SELLER" DataField="DETAILS_OF_SELLER" />
                           <%-- <asp:BoundField HeaderText="D_O_P" DataField="D_O_P" />
                            <asp:BoundField HeaderText="AMOUNT" DataField="AMOUNT" />--%>
                            <asp:BoundField HeaderText="AREA" DataField="AREA" />
                            <asp:BoundField HeaderText="STATE" DataField="STATE" />
                            <asp:BoundField HeaderText="DISTT" DataField="DISTT" />
                            <asp:BoundField HeaderText="TEHSIL" DataField="TEHSIL" />
                            <asp:BoundField HeaderText="VILLAGE" DataField="VILLAGE" />
                            <asp:BoundField HeaderText="SYNo_KhaasraNo" DataField="SYNo_KhaasraNo" />
                            <asp:BoundField HeaderText="REMARKS" DataField="REMARKS" NullDisplayText="N/A" />
                          <%--  <asp:BoundField HeaderText="MODE_CASH_CHEQUE" DataField="MODE_CASH_CHEQUE" />--%>
                            
                           <%-- <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" Visible="false" />
                            <asp:BoundField DataField="DocumentName" HeaderText="DocumentName" SortExpression="DocumentName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Size" HeaderText="Size(in KB)" SortExpression="Size" NullDisplayText="N/A" Visible="false"/>
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                          
                            <asp:BoundField DataField="PAGECOUNT" HeaderText="PageCount" SortExpression="PAGECOUNT" NullDisplayText="N/A" />
                            <asp:BoundField DataField="CreatedOn" HeaderText="Uploaded On" SortExpression="CreatedOn" NullDisplayText="N/A" />--%>
                          <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" >  
<ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>
                                  
                           
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
    </table>
      
</asp:Content>