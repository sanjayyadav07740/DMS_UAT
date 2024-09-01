<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ShareDocument.aspx.cs" Inherits="DMS.Shared.ShareDocument" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"
 TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphDocument" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">



     
    <script src="../js/jquery.min.js"></script>
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

        <asp:Label ID="lblTitle" runat="server" Text="Share Document" SkinID="Title"></asp:Label>
    </center>
    <center>
        <table border="0" cellpadding="0" cellspacing="3" width="931">
         
          <tr >
               
            <td colspan="3">
                <uc:EntityModule ID="emodModule" runat="server" DisplayMetaDataCode="false" DisplayCategory="false" />
            </td>
            
          </tr>
             <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>
               <tr>
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblstartdate" runat="server" Text="Start Date :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFrom" runat="server" Width="400px" AutoComplete="off"></asp:TextBox>
                    <ajaxtoolkit:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtFrom"
                        PopupButtonID="txtFrom" Format="yyyy/MM/dd">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvstartdate" runat="server" ControlToValidate="txtFrom" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                   <%--<td align="left">
                       <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFrom" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                           ErrorMessage="Invalid date format." ValidationGroup="Group1" Display="Dynamic" />
                   </td>--%>
                                
            </tr>
             <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>
            <tr>
                <td align="left" style="width: 150px;">
                    <asp:Label ID="Label1" runat="server" Text="To Date :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtend" runat="server" Width="400px" AutoComplete="off"></asp:TextBox>
                    <ajaxtoolkit:CalendarExtender ID="calenddate" runat="server" TargetControlID="txtend"
                        PopupButtonID="txtend" Format="yyyy/MM/dd">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvenddate" runat="server" ControlToValidate="txtend" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                     <asp:CompareValidator ID="cvDateValidate" ValidationGroup="Date" ForeColor="Red" runat="server" ControlToValidate="txtFrom"
                                             ControlToCompare="txtend" Operator="LessThanEqual" Type="Date" ErrorMessage="End date must be Greater than From date."
                                             Display="Dynamic" ></asp:CompareValidator>
                </td>
                <%--<td align="left">
                       <asp:RegularExpressionValidator runat="server" ControlToValidate="txtend" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                           ErrorMessage="Invalid date format." ValidationGroup="Group1" Display="Dynamic" />
                    
                   </td>--%>
                
            </tr>

             <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>
            <tr>
                <td align="left" style="width: 150px;">
                    <asp:Label ID="Label4" runat="server" Text="Share with :"></asp:Label>
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rbtnsharewith" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnsharewith_SelectedIndexChanged">
                        <asp:ListItem Value="InternalUser">Internal User</asp:ListItem>
                        <asp:ListItem Value="ExternalUser">External User</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqfv" runat="server" ControlToValidate="rbtnsharewith" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                     
                </td>
                <%--<td align="left">
                       <asp:RegularExpressionValidator runat="server" ControlToValidate="txtend" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                           ErrorMessage="Invalid date format." ValidationGroup="Group1" Display="Dynamic" />
                    
                   </td>--%>
                
            </tr>
            <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>


            <tr id="divuseremail" runat="server">
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblemail" runat="server" Text="User Email :"></asp:Label>
                </td>
                <td align="left" runat="server" ID="divExternaluser" >
                    <asp:TextBox ID="txtemailId" runat="server" Width="400px" AutoComplete="off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvemailid" runat="server" ControlToValidate="txtemailId" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmailId"
                                 runat="server" ErrorMessage="Please Enter Valid Email ID"
                                 ControlToValidate="txtemailId" Display="Dynamic"
                                 ForeColor="Red"
                                 ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                             </asp:RegularExpressionValidator>
                             <ajaxtoolkit:ValidatorCalloutExtender ID="vceEmailid" runat="server" TargetControlID="revEmailId"></ajaxtoolkit:ValidatorCalloutExtender>
                </td>

                <td align="left" runat="server" id="divInternalUser">
                    <asp:DropDownList ID="ddluserlist" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqfvinternaluser" runat="server" ControlToValidate="ddluserlist" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    
                </td>
                
                <td style="width: 26px" align="left"></td>

            </tr>

             <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>

            <tr>
                <td align="left" style="width: 150px;">
                    <asp:Label ID="Label3" runat="server" Text="Access :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlaccesstype" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvaccesstype" runat="server" ControlToValidate="ddlaccesstype" InitialValue="-1" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
               
                <td style="width: 26px" align="left"></td>


            </tr>

            <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>

            <tr style="display:none">
                <td align="left" style="width: 150px;">
                    <asp:Label ID="Label2" runat="server" Text= "Share Folder :"></asp:Label>
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkboxselectedfolder" AutoPostBack="true" runat="server" OnCheckedChanged="chkboxselectedfolder_CheckedChanged" />
                </td>
                <td style="width: 26px" align="left"></td>


            </tr>

            <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>


            <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="gvwDocument" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="931px"
                        DataKeyNames="ID,MetaDataID,DocumentStatusID,DocumentName,DocumentPath"
                        OnPageIndexChanging="gvwDocument_PageIndexChanging" OnRowCommand="gvwDocument_RowCommand"
                        OnRowDataBound="gvwDocument_RowDataBound" OnSorting="gvwDocument_Sorting" 
                        ShowFooter="True"                          >
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" runat="server" /> <%--OnCheckedChanged="chkHeader_CheckedChanged"--%>
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
                              <asp:BoundField DataField="DocumentPath" HeaderText="DocumentPath" SortExpression="Doc Id" Visible="false"
                                NullDisplayText="N/A" />    
                            <asp:BoundField DataField="MetaDataCode" HeaderText="MetaDataCode" SortExpression="MetaDataCode"
                                NullDisplayText="N/A"  readonly="true" />
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />
                     
                            <asp:BoundField DataField="Tag" HeaderText="Tag" SortExpression="Tag" NullDisplayText="N/A"  readonly="true" />
                            <%--  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" NullDisplayText="N/A" />--%>
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" >
                             

<ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>
                        </Columns>

                    </asp:GridView>
                     <br />
                      <%--<asp:ImageButton ID="ibtnDownloadFiles" Visible="false" runat="server" OnClientClick="javascript:return validateCheckBoxes();" CausesValidation="false" OnClick="ibtnDownloadFiles_Click" SkinID="Download"></asp:ImageButton>--%>
                </td>
            </tr>

             <tr>
                <td align="center" colspan="2">
                    <asp:ImageButton ID="ibtnShow" Visible="false" runat="server" ToolTip='<%$ Resources:Resource,AddtoCart %>'
                        SkinID="cartButton" CausesValidation="true"  Width="17px" OnClick="ibtnShow_Click" />
                </td>
            </tr>

             <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>

             
            <tr>               
                <td align="center" colspan="2">
                    <asp:GridView ID="gvwcart" runat="server" AutoGenerateColumns="False" AllowSorting="True" Width="931px" ShowFooter="True"    
                         DataKeyNames="ID,DocumentName" >
                        <Columns>                         
                            <asp:TemplateField HeaderText="Sr." ControlStyle-Width="20px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID" HeaderText="DocId" SortExpression="Doc Id" Visible="false"
                                NullDisplayText="N/A" /> 
                                                         
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />
                      <asp:BoundField DataField="DocumentPath" HeaderText="DocumentPath" SortExpression="DocumentPath"
                                NullDisplayText="N/A"  readonly="true" />
                            
                            <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="Action"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" >
                             

<ControlStyle Width="80px"></ControlStyle>
                            </asp:ButtonField>
                        </Columns>

                    </asp:GridView>
                     <br />
                      <%--<asp:ImageButton ID="ibtnDownloadFiles" Visible="false" runat="server" OnClientClick="javascript:return validateCheckBoxes();" CausesValidation="false" OnClick="ibtnDownloadFiles_Click" SkinID="Download"></asp:ImageButton>--%>
                </td>
            </tr>

              <tr style="height: 8px">
                <td colspan="3"></td>
            </tr>


              <tr>
                <td align="center" colspan="2">
                    <asp:ImageButton ID="IbtnSubmit" runat="server" ToolTip='<%$ Resources:Resource,share %>'
                        SkinID="ShareButton" CausesValidation="true"  Width="17px" OnClick="IbtnSubmit_Click" />
                </td>
            </tr>

        </table>

        

        </body>
       
    </center>

    



</asp:Content>
