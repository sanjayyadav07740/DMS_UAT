 <%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="MergeTiffDocument.aspx.cs" Inherits="DMS.Shared.MergeTiffDocument" %>
<asp:Content ID="cphSearchTiffDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 18px;
        }
    </style>
</asp:Content>

<asp:Content ID="cphSearchTiffDocumentMain" ContentPlaceHolderID="cphmain" runat="server">

      <center>
        <asp:Label ID="lblTitle" runat="server" Text="Merge Document" SkinID="Title"></asp:Label>
    </center>
    <center>
        
        <table width="931" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
                </td>
              <%--<td align="left" valign="top">
                  <table cellpadding="0" cellspacing="3" border="0">
                      <tr>
                          <td>
                              <asp:Label ID="lblFileName" runat="server" Text="File Name"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtFileName" runat="server" TextMode="MultiLine"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblTypeOfFile" runat="server" Text="Type Of File"></asp:Label>
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlTypeOfFile" runat="server">
                                  <asp:ListItem>--Select--</asp:ListItem>
                                  <%--<asp:ListItem>CONNECTED FILE</asp:ListItem>
                                  <asp:ListItem>CORRESPONDENCE</asp:ListItem>
                                  <asp:ListItem>BALANCE SHEET</asp:ListItem>--%>
                                <%-- <asp:ListItem>CORRESPONDENCE</asp:ListItem>
                                  <asp:ListItem>NOTINGS</asp:ListItem>
                                  <asp:ListItem>DOCUMENT</asp:ListItem>
                                   <asp:ListItem>BALANCE SHEET</asp:ListItem>
                                  <asp:ListItem>BRIEF PARTICULARS</asp:ListItem>
                                  <asp:ListItem>CIRCULATION</asp:ListItem>
                                   <asp:ListItem>CONNECTED</asp:ListItem>
                                  <asp:ListItem>MEMORANDUM</asp:ListItem>
                                  <asp:ListItem>MISCELLANEOUS</asp:ListItem>
                                   <asp:ListItem>ROUTINE CORRESPONDENCE</asp:ListItem>
                                   <asp:ListItem>INSURANCES</asp:ListItem>
                                  <asp:ListItem>VERSUS</asp:ListItem>
                                  <asp:ListItem>ARC</asp:ListItem>
                                  <asp:ListItem>PERSONAL FILE</asp:ListItem>
                              </asp:DropDownList>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                               <ajaxtoolkit:CalendarExtender ID="cleFromDate" runat="server" TargetControlID="txtFromDate"
                                            Format="MM/dd/yyyy">
                                        </ajaxtoolkit:CalendarExtender>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                          </td>
                          <td>
                              <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                               <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                            Format="MM/dd/yyyy">
                                        </ajaxtoolkit:CalendarExtender>
                          </td>
                      </tr>
                      </table>
                  </td>--%>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upanPanel" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblField" runat="server" Text="Field : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlField" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlField_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left">
                                        <asp:RequiredFieldValidator ID="rfvField" runat="server" ControlToValidate="ddlField"
                                            Enabled="false" InitialValue="-1" ToolTip='<%$ Resources:Resource,Field  %>'></asp:RequiredFieldValidator>
                                        <asp:UpdateProgress ID="uprsProgessField" runat="server" DisplayAfter="1000">
                                            <ProgressTemplate>
                                                <img src="../Images/Loading.gif" height="25px" width="25px" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblCriteria" runat="server" Text="Criteria : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlCriteria" runat="server">
                                            <asp:ListItem Text="EQUAL" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="LIKE" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left"></td>
                                </tr>
                                <tr id="trDataToSearch" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblDataToSearch" runat="server" Text="Data To Search : "></asp:Label>
                                    </td>
                                    <td align="left" id="tdDataToSearch" runat="server"></td>
                                    <td align="left">
                                        <asp:UpdateProgress ID="upanDataToSearch" runat="server" DisplayAfter="1000">
                                            <ProgressTemplate>
                                                <img src="../Images/Loading.gif" height="25px" width="25px" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </td>
                                </tr>
                                <tr id="trFromDate" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                        <ajaxtoolkit:MaskedEditExtender ID="meeFromDate" runat="server" TargetControlID="txtFromDate"
                                            Mask="99/99/9999" MaskType="Date">
                                        </ajaxtoolkit:MaskedEditExtender>
                                        <ajaxtoolkit:MaskedEditValidator ID="mevFromDate" runat="server" ControlExtender="meeFromDate"
                                            ControlToValidate="txtFromDate" InvalidValueMessage="<br/>Invalid Date (mm/dd/yyyy)"></ajaxtoolkit:MaskedEditValidator>
                                    </td>
                                    <td align="left">
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                            Text="*"></asp:RequiredFieldValidator>
                                        <ajaxtoolkit:CalendarExtender ID="cleFromDate" runat="server" TargetControlID="txtFromDate"
                                            Format="MM/dd/yyyy">
                                        </ajaxtoolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="trToDate" runat="server" visible="false">
                                    <td align="left">
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                        <ajaxtoolkit:MaskedEditExtender ID="meeToDate" runat="server" TargetControlID="txtToDate"
                                            Mask="99/99/9999" MaskType="Date">
                                        </ajaxtoolkit:MaskedEditExtender>
                                        <ajaxtoolkit:MaskedEditValidator ID="mevToDate" runat="server" ControlExtender="meeToDate"
                                            ControlToValidate="txtToDate" InvalidValueMessage="<br/>Invalid Date (mm/dd/yyyy)"></ajaxtoolkit:MaskedEditValidator>
                                    </td>
                                    <td align="left">
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                            Text="*"></asp:RequiredFieldValidator>
                                        <ajaxtoolkit:CalendarExtender ID="cleToDate" runat="server" TargetControlID="txtToDate"
                                            Format="MM/dd/yyyy">
                                        </ajaxtoolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblSearchBy" runat="server" Text="Search By : "></asp:Label>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:RadioButtonList ID="rdblSearchBy" runat="server" TextAlign="Right">
                                            <asp:ListItem Text="MetaTemplate Field" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Document Tag" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Document Content" Value="3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblTextToSeach" runat="server" Text="Text To Search : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTextToSeach" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
             </tr>
        <tr>
                <td align="center" colspan="2">
                    <asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnShow_Click" />
                </td>
            </tr>
            <tr>
                <%--<td align="center" colspan="2">
                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true" width="931"
                        PageSize="5" AllowPaging="true" DataKeyNames="ID,MetaDataID,DocumentStatusID"
                        OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand"
                        OnRowDataBound="gvwDocument_RowDataBound" OnSorting="gvwDocument_Sorting" ShowFooter="true">
                        <Columns>
                            
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentName" HeaderText="DocumentName" SortExpression="DocumentName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A"
                                Visible="false" />
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                          <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                           <%-- <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />      
                            <asp:ButtonField ButtonType="Image" CommandName="Merge" HeaderText="Merge Document" ImageUrl="~/Images/DMSButton/Merge.jpg" ControlStyle-Width="80px" />
                            <asp:ButtonField ButtonType="Image" CommandName="Deleting" HeaderText="Delete Pages" ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-width="80px" Visible="false"/>
                        </Columns>
                    </asp:GridView>
                </td>--%>
                <td align="center" colspan="2">
                    <asp:GridView ID="gvwTiffDocument" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                        PageSize="5" AllowPaging="true" DataKeyNames="ID,MetaDataID,DocumentStatusID"
                        OnPageIndexChanging="gvwTiffDocument_PageIndexChanging" OnRowCommand="gvwTiffDocument_RowCommand"
                        OnRowDataBound="gvwTiffDocument_RowDataBound" OnSorting="gvwTiffDocument_Sorting" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField>
                                <FooterTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFilterGrid" runat="server" Text="Filter By : "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFilterGrid" runat="server">
                                                    <asp:ListItem Text="DocumentName" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Tag" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFilterGrid" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtnFilterGrid" runat="server" CausesValidation="false" OnClick="ibtnFilterGrid_Click" SkinID="ViewButton"></asp:ImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sr.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentName" HeaderText="DocumentName" SortExpression="DocumentName"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A"
                                Visible="false" />
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" />
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />
                            <asp:ButtonField ButtonType="Image" CommandName="Merge" HeaderText="Merge Document" ImageUrl="~/Images/DMSButton/Merge.jpg" ControlStyle-Width="80px" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblBrowse" runat="server" Text="Browse the document to be merged"></asp:Label>
                </td>

                <td align="left">
                    <%-- <asp:FileUpload ID="flUpload" runat="server" />--%>
                    <asp:FileUpload ID="flUpload" text="Select File" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMergePosition" runat="server" Text="Enter page number from where to be Merge" Width="495px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMergePosition" runat="server" Text="0" Enabled="False"></asp:TextBox>
                </td>
            </tr> 
            <%--<tr>
                <td>
                <asp:Label ID="lblPageNo" runat="server" Text="Page No"></asp:Label>
                    </td>
                <td align="left">
                    <asp:TextBox ID="txtPageNo" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td colspan="2" align="center" class="auto-style1">
                    <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" OnClick="ibtnSubmit_Click"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" />
                </td>
            </tr>
        </table>
    </center>

</asp:Content>
