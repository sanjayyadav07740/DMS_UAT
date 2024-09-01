<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentDashBoard_SSL.aspx.cs" Inherits="DMS.Shared.DocumentDashBoard_SSL" MasterPageFile="~/MainMasterPage.Master" enableEventValidation ="false"%>

<asp:Content ID="cphDocumentDashBoardHead" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
     .hidden
        {
            display:none;
        }
        </style>
    <script type="text/javascript">
        var TotalChkBx;
        var Counter;

        window.onload = function () {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.grvUploadedDocs.Rows.Count %>');

    //Get total no. of checked CheckBoxes in side the GridView.
    Counter = 0;
}

function HeaderClick(CheckBox) {
    //Get target base & child control.
    var TargetBaseControl =
        document.getElementById('<%= this.grvUploadedDocs.ClientID %>');
    var TargetChildControl = "chkBxSelect";

    //Get all the control of the type INPUT in the base control.
    var Inputs = TargetBaseControl.getElementsByTagName("input");

    //Checked/Unchecked all the checkBoxes in side the GridView.
    for (var n = 0; n < Inputs.length; ++n)
        if (Inputs[n].type == 'checkbox' &&
                  Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
            Inputs[n].checked = CheckBox.checked;

    //Reset Counter
    Counter = CheckBox.checked ? TotalChkBx : 0;
}

function ChildClick(CheckBox, HCheckBox) {
    //get target control.
    var HeaderCheckBox = document.getElementById(HCheckBox);

    //Modifiy Counter; 
    if (CheckBox.checked && Counter < TotalChkBx)
        Counter++;
    else if (Counter > 0)
        Counter--;

    //Change state of the header CheckBox.
    if (Counter < TotalChkBx)
        HeaderCheckBox.checked = false;
    else if (Counter == TotalChkBx)
        HeaderCheckBox.checked = true;
}
</script>
</asp:Content>
<asp:Content ID="cphDocumentDashBoardMain" ContentPlaceHolderID="cphMain" runat="server">
    <center><asp:Label ID="lblTitle" runat="server" Text="Uploaded Document Report"  SkinID="Title"></asp:Label> </center>
<table align="center">
    <tr>
        <td>
            <asp:Label ID="lblDate" runat="server" Text="From Date"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
            <ajaxtoolkit:CalendarExtender ID="cleDate" runat="server" TargetControlID="txtDate"
                    Format="dd-MMM-yyyy">
                </ajaxtoolkit:CalendarExtender>
        </td>
        <td>
            <asp:Label ID="Label1" runat="server" Text="To Date"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
            <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                    Format="dd-MMM-yyyy">
                </ajaxtoolkit:CalendarExtender>
        </td>
    </tr>
    <tr>
        <td colspan="4" align="center">
            <asp:ImageButton ID="ibtnSubmit" runat="server" SkinID="SubmitButton" OnClick="ibtnSubmit_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="ibtnCancel" runat="server" SkinID="CancelButton" OnClick="ibtnCancel_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="ibtnExport" runat="server" SkinID="ExportButton" OnClick="ibtnExport_Click" />
             &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="ibtnDownload" runat="server" SkinID="Download" OnClick="ibtnDownload_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="4" align="center">
            <asp:GridView ID="grvUploadedDocs" runat="server" AutoGenerateColumns="false" DataKeyNames="DocumentName,DocumentPath,ID,MetaDataID,DocumentStatusID" OnRowCommand="grvUploadedDocs_RowCommand" OnRowCreated="grvUploadedDocs_RowCreated"
                >
                <Columns>
                     <asp:TemplateField HeaderText="Select">
         <ItemTemplate>
            <asp:CheckBox ID="chkBxSelect" runat="server" />
         </ItemTemplate>
         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
         <HeaderTemplate>
            <asp:CheckBox ID="chkBxHeader" 

                 onclick="javascript:HeaderClick(this);" runat="server" />
         </HeaderTemplate>
      </asp:TemplateField>
                     <asp:TemplateField HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
                    <asp:BoundField HeaderText="Uploaded On" DataField="CreatedOn" />
                    <asp:BoundField HeaderText="Document Path" DataField="DocumentPath" >
                     <ItemStyle CssClass="hidden"/>
                     <HeaderStyle CssClass="hidden"/>
                    </asp:BoundField>
                    <asp:ButtonField ButtonType="Image" CommandName="DocumentSearch" HeaderText="View Document"
                                ImageUrl="~/Images/DMSButton/view.jpg" ControlStyle-Width="80px" />  
                    <asp:ButtonField ButtonType="Image" CommandName="Download" HeaderText="Download Document" ImageUrl="~/Images/DMSButton/download.jpg"  ControlStyle-Width="80px" />
                    
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    </table>
    </asp:Content>
