<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="DocumentSearch.aspx.cs" Inherits="DMS.Shared.DocumentSearch" %>

<asp:Content ID="cphSearchDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphSearchDocumentMain" ContentPlaceHolderID="cphMain" runat="server">
    <style>
        .popupextender {
    /* opacity: 0.3; */
    filter: alpha(opacity=30);
    background-color: #eee1e1;
}
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[id*=chkHeader]").live("click", function () {
        var chkHeader = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkHeader.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr")).addClass("selected");
            } else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");
            }
        });
    });
    $("[id*=chkChild]").live("click", function () {
        var grid = $(this).closest("table");
        var chkHeader = $("[id*=chkHeader]", grid);
        if (!$(this).is(":checked")) {
            $("td", $(this).closest("tr")).removeClass("selected");
            chkHeader.removeAttr("checked");
        } else {
            $("td", $(this).closest("tr")).addClass("selected");
            if ($("[id*=chkChild]", grid).length == $("[id*=chkChild]:checked", grid).length) {
                chkHeader.attr("checked", "checked");
            }
        }
    });
</script>
    <script type="text/javascript">
        function checkAll(objRef) {

            //var GridView = objRef;
            //alert(GridView)

            var GridView = document.getElementById('<%=gvwDocument.ClientID %>');
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;
                //alert(inputList.length);
                //alert(inputList[i].type);
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {

                        //If the header checkbox is checked

                        //check all checkboxes

                        //and highlight all rows

                        //row.style.backgroundColor = "aqua";

                        inputList[i].checked = true;

                    }

                    else {

                        //If the header checkbox is checked

                        //uncheck all checkboxes

                        //and change rowcolor back to original

                        //if (row.rowIndex % 2 == 0) {

                        //    //Alternating Row Color

                        //    row.style.backgroundColor = "#C2D69B";

                        //}

                        //else {

                        //    row.style.backgroundColor = "white";

                        //}

                        inputList[i].checked = false;

                    }

                }

            }

        }
        function Check_Click(objRef) {

            //Get the Row based on checkbox

            var row = objRef.parentNode.parentNode.parentNode;

            //  the reference of GridView

            //if (objRef.checked) {

            //       //If checked change color to Aqua

            //       //row.style.backgroundColor = "aqua";

            //   }

            //   else {

            //       //If not checked change back to original color

            //       if (row.rowIndex % 2 == 0) {

            //           //Alternating Row Color

            //           //row.style.backgroundColor = "#C2D69B";

            //       }

            //       else {

            //           //row.style.backgroundColor = "white";

            //       }

            //   }



            //Get     var GridView = row.parentNode;



            //Get all input elements in Gridview
            var GridView = document.getElementById('<%=gvwDocument.ClientID %>');
            var inputList = GridView.getElementsByTagName("input");
            //var inputList = GridView.getElementsByTagName("input");



            for (var i = 0; i < inputList.length; i++) {

                //The First element is the Header Checkbox

                var headerCheckBox = inputList[0];



                //Based on all or none checkboxes

                //are checked check/uncheck Header Checkbox

                var checked = true;

                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                    if (!inputList[i].checked) {

                        checked = false;

                        break;

                    }

                }

            }

            headerCheckBox.checked = checked;



        }
    </script>
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="Document Search" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table width="931" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left">
                    <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="true" />
                </td>
                <td align="left" valign="top">
                    <asp:UpdatePanel ID="upanPanel" runat="server">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="3" border="0">
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
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblSearchBy" runat="server" Text="Search By :"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:RadioButtonList ID="rdblSearchBy" runat="server" TextAlign="Right">
                                            <asp:ListItem Text="MetaTemplate Field" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Document Tag" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Document Content" Value="3"></asp:ListItem>
                                            <asp:ListItem Value="4">Search By Folder Name</asp:ListItem>
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
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:ImageButton ID="ibtnShow" runat="server" ToolTip='<%$ Resources:Resource,Show %>'
                        SkinID="SubmitButton" CausesValidation="true" OnClick="ibtnShow_Click" Width="17px" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="931px"
                        DataKeyNames="ID,MetaDataID,DocumentStatusID,DocumentName"
                        OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand"
                        OnRowDataBound="gvwDocument_RowDataBound" OnSorting="gvwDocument_Sorting" 
                        ShowFooter="True" OnRowCancelingEdit="gvwDocument_RowCancelingEdit" OnRowEditing="OnRowEditing"
                         OnRowUpdating="gvwDocument_RowUpdating" OnRowCreated="gvwDocument_RowCreated">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" runat="server"/> <%--OnCheckedChanged="chkHeader_CheckedChanged"--%>
                                   <%-- onclick = "checkAll(this);--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkChild" runat="server"  />
                                            <%--onclick = "Check_Click(this)"--%>
                                        </ItemTemplate>
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
                            <asp:BoundField DataField="ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                NullDisplayText="N/A" />    
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A"  readonly="true" />
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />
                           <%-- <asp:TemplateField HeaderText="DocumentName" SortExpression="DocumentName">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDocName" runat="server" Text='<%# Bind("DocumentName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDocName" runat="server" Text='<%# Bind("DocumentName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A"
                                Visible="false"  readonly="true" />
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A"  readonly="true" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A"   readonly="true"/>
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A"  readonly="true" />
                            <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" >
                             

                            <ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>

                            <asp:ButtonField ButtonType="Image" CommandName="RenameDocument" HeaderText="Rename Document"
                                ImageUrl="~/Images/DMSButton/rename.jpg" ControlStyle-Width="80px" >
                             

                            <ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>

                            <asp:ButtonField ButtonType="Image" CommandName="DeleteDocument" HeaderText="Delete Document"
                                ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="80px" >
                             

                            <ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>

     <%--                           <asp:ButtonField ButtonType="Image" CommandName="DownloadDocument" HeaderText="Download"
                                ImageUrl="~/Images/DMSButton/download.jpg" ControlStyle-Width="80px" >
                             

                            <ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>--%>

                             <asp:TemplateField HeaderText="Download">
                            <ItemTemplate>
                <%--        <asp:LinkButton ID="DownloadButton" runat="server" Text="Download"
                            CommandName="DownloadDocument" CommandArgument='<%# Eval("Id") %>' />--%>
                                
                     <asp:ImageButton ID="ibtndownloadgrd" runat="server" CommandName="DownloadDocument"  CommandArgument='<%# Eval("Id") %>' SkinID="Download" ></asp:ImageButton>

                    </ItemTemplate>
                </asp:TemplateField>


                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />           
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                     <br />
                      <asp:ImageButton ID="ibtnDownloadFiles" Visible="false" runat="server" CausesValidation="false" OnClick="ibtnDownloadFiles_Click" SkinID="Download"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Button ID="btnDummy" runat="server" Style="display: none;" />
                    <ajaxtoolkit:ModalPopupExtender ID="GridViewDetails" runat="server" TargetControlID="btnDummy"
                    PopupControlID="pnlGridViewDetails" BackgroundCssClass="modalBackground" />
                    <asp:Panel ID="pnlGridViewDetails" runat="server"  Style="display: none;" CssClass="popupextender">
                     <table>
                         <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl" runat="server" Text="Document to be renamed : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="HFDocID" runat="server" />
                                        <asp:Label ID="lbloldoc" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                         </tr>
                         <tr>

                                    <td align="left">
                                        <asp:Label ID="lblnewDoc" runat="server" Text="Enter New Name : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDocname" runat="server" ValidationGroup="rename" ></asp:TextBox>
                                    </td>
                                    <td>
                                            <asp:RequiredFieldValidator ID="rfvDocname" runat="server" ControlToValidate="txtDocname"
                                            Text="*" ValidationGroup="rename"></asp:RequiredFieldValidator>
                                    </td>
                            </tr>
                         <tr align="center">
                            <td>
                                <asp:ImageButton ID="ibtnsubmitrename" runat="server"  OnClick="ibtnsubmitrename_Click" SkinID="SubmitButton" ValidationGroup="rename"></asp:ImageButton>
                            </td>
                             <td>
                                  <asp:ImageButton ID="btnclose" runat="server"  OnClick="btnclose_Click" SkinID="CancelButton"></asp:ImageButton>
                            </td>
                         </tr>
                     </table>


                     
                    </asp:Panel>
                </td>
            </tr>
                        <tr>
                <td>
                     <asp:Button ID="btntst" runat="server" Style="display: none;" />
                    <ajaxtoolkit:ModalPopupExtender ID="mpopDelete" runat="server" TargetControlID="btntst"
                    PopupControlID="pnldelete" BackgroundCssClass="modalBackground" />
                    <asp:Panel ID="pnldelete" runat="server"  Style="display: none;" CssClass="popupextender">
                     <table>
                         <tr align="center">
                                <td align="left" colspan="2">
                                     <asp:Label ID="lbl1" runat="server" Text="Do you want to delete "></asp:Label><asp:Label ID="lbldocname" runat="server" Text=""></asp:Label>
                                 </td>
                             <asp:HiddenField ID="HFdociddelete" runat="server" />
                                 <td>&nbsp;</td>
                            </tr>
                         <tr align="center">
                            <td>
                                <asp:ImageButton ID="ibtnYes" runat="server" OnClick="ibtnYes_Click"  SkinID="YesButton" ></asp:ImageButton>
                            </td>
                             <td>
                                  <asp:ImageButton ID="ImageButton2" runat="server"  SkinID="NoButton"></asp:ImageButton>
                            </td>
                         </tr>
                     </table>


                     
                    </asp:Panel>
                </td>
            </tr>


        </table>
    </center>
</asp:Content>
