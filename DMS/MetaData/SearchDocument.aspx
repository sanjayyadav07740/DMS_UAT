<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true"
    CodeBehind="SearchDocument.aspx.cs" Inherits="DMS.Shared.SearchDocument" %>

<asp:Content ID="cphSearchDocumentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="cphSearchDocumentMain" ContentPlaceHolderID="cphMain" runat="server">
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


        function DeleteConfirm(msg) {
            debugger;
            if (document.contains(document.getElementById("hdnconfirm_value"))) {
                document.getElementById("hdnconfirm_value").remove();
            }
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.id = "hdnconfirm_value"
            confirm_value.name = "confirm_value";
            if (confirm(msg)) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

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

        function validateCheckBoxes() {
            var valid = false;
            var gv = document.getElementById('<%= this.gvwDocument.ClientID %>');
            for (var i = 0; i < gv.getElementsByTagName("input").length; i++) {
                var node = gv.getElementsByTagName("input")[i];
                if (node != null && node.type == "checkbox" && node.checked) {
                    valid = true;
                    break;
                }
            }
            if (!valid) {
                alert("Please Select a Checkbox to Download Documents.");
            }
            return valid;
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
                                        <asp:Label ID="lblCriteria" runat="server" Text="Criteria : "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlCriteria" runat="server">
                                            <asp:ListItem Text="EQUAL" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LIKE" Value="2" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left"></td>
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
                                        <asp:RadioButtonList ID="rdblSearchBy" runat="server" AutoPostBack="true" TextAlign="Right" OnSelectedIndexChanged="rdblSearchBy_SelectedIndexChanged">
                                            <%--<asp:ListItem Text="MetaTemplate Field" Value="1"></asp:ListItem>--%>
                                            <asp:ListItem Text="Document Tag" Value="2" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Document Content" Value="3"></asp:ListItem>
                                            <%--<asp:ListItem Value="4">Search By Folder Name</asp:ListItem>
                                            <asp:ListItem Text="Field Search" Value="5" ></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>

                                <tr runat="server" id="fieldtr" visible="false">
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

                                 <tr id="trDataToSearch" runat="server" visible="false">
                                    <td align="left" id="tdsearch" runat="server">
                                        <asp:Label ID="lblDataToSearch" runat="server" Text="Data To Search : " Visible="false"></asp:Label>
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
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="931px"
                        DataKeyNames="ID,MetaDataID,DocumentStatusID,DocumentName,DocumentType"
                        OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand"
                        OnRowDataBound="gvwDocument_RowDataBound" OnSorting="gvwDocument_Sorting"
                        ShowFooter="True" OnRowCancelingEdit="gvwDocument_RowCancelingEdit" OnRowEditing="OnRowEditing"
                        OnRowUpdating="gvwDocument_RowUpdating" OnRowDeleting="gvwDocument_RowDeleting">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeader" runat="server" />
                                    <%--OnCheckedChanged="chkHeader_CheckedChanged"--%>
                                    <%-- onclick = "checkAll(this);--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkChild" runat="server" />
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
                            <asp:TemplateField HeaderText="Sr." ControlStyle-Width="20px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                NullDisplayText="N/A" />
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A" ReadOnly="true" />
                            <%--<asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />--%>

                            <asp:TemplateField HeaderText="Document Name" SortExpression="DocumentName">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DocumentName" runat="server" Text='<%# Eval("documentname") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_DocumentName" runat="server" Text='<%# System.IO.Path.GetFileNameWithoutExtension(Eval("documentname").ToString())  %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- <asp:TemplateField HeaderText="DocumentName" SortExpression="DocumentName">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDocName" runat="server" Text='<%# Bind("DocumentName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDocName" runat="server" Text='<%# Bind("DocumentName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" NullDisplayText="N/A"
                                Visible="false" ReadOnly="true" />
                            <asp:BoundField DataField="DocumentType" HeaderText="DocumentType" SortExpression="DocumentType"
                                Visible="false" NullDisplayText="N/A" ReadOnly="true" />
                            <asp:BoundField DataField="DocumentStatus" HeaderText="DocumentStatus" SortExpression="DocumentStatus"
                                NullDisplayText="N/A" ReadOnly="true" />
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A" ReadOnly="true" />
                            <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px">

                                <ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Rename Document">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" ControlStyle-Width="80px" CommandArgument="<%# Container.DataItemIndex %>" CommandName="Edit" ImageUrl="~/Images/DMSButton/edit.jpg" CssClass="centerbtn" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibtnUpdate" runat="server" ControlStyle-Width="80px" OnClick="OnUpdate" ImageUrl="~/Images/DMSButton/update.jpg" />
                                    <asp:ImageButton ID="ImageButton1" runat="server" ControlStyle-Width="80px" OnClick="OnCancel" ImageUrl="~/Images/DMSButton/cancel.jpg" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:ButtonField ButtonType="Image" CommandName="Merge" HeaderText="Merge Document" ImageUrl="~/Images/DMSButton/Merge.jpg" ControlStyle-Width="80px" />

                            <asp:ButtonField ButtonType="Image" CommandName="Split" HeaderText="Split Document" ImageUrl="~/Images/DMSButton/Split.jpg" ControlStyle-Width="80px" />

                            <%--  <asp:ButtonField ButtonType="Image" CommandName="Delete" HeaderText="Delete Document" ImageUrl="~/Images/DMSButton/delete.jpg" ControlStyle-Width="80px" />--%>

                            <asp:TemplateField HeaderStyle-Width="8px" HeaderText="Delete Document" ItemStyle-HorizontalAlign="center" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="Delete" OnClientClick="DeleteConfirm('Are you sure you want to Delete this Document ?')" />
                               
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>
                    <br />

                    <asp:ImageButton ID="ibtnDownloadFiles" Visible="false" runat="server" OnClientClick="javascript:return validateCheckBoxes();" CausesValidation="false" OnClick="ibtnDownloadFiles_Click" SkinID="Download"></asp:ImageButton>
                </td>
            </tr>
        </table>

        <div id="divSplit" runat="server" visible="false">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblRange" runat="server" Text="Enter Page Range :"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRange" placeholder="eg.1-10" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:ImageButton ID="ibtnSplit" runat="server" SkinID="SplitButton" OnClick="ibtnSplit_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnBack" runat="server" SkinID="CancelButton" OnClick="ibtnBack_Click" />
                    </td>
                </tr>
            </table>
        </div>

        <div id="divMerge" runat="server" visible="false">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblDrop" runat="server" Text="Where to Merge.?"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMerge" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMerge_SelectedIndexChanged"
                            Enabled="false">

                            <asp:ListItem Text="Merge Initial" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Merge In-Between" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Merge End" Value="2" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblBrowse" runat="server" Text="Browse the document to be merged"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:FileUpload ID="flUpload" runat="server" text="Select File" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPageNo" runat="server" Text="Page No"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPageRange" placeholder="ex: 15" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" align="center">
                        <asp:ImageButton ID="ibtnMerge" runat="server" SkinID="MergeButton" OnClick="ibtnMerge_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" OnClick="ibtnCancel_Click" />
                    </td>
                </tr>
            </table>
        </div>

    </center>
    
</asp:Content>
