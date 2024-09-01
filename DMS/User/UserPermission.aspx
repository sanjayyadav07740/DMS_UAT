<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="UserPermission.aspx.cs" Inherits="DMS.User.UserPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script language="javascript" type="text/javascript">

        //Below javascript function is used to checked all child nodes if parent checked and check parent node at lease one child node is checked otherwise unchecked 
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            var t = GetParentByTagName("table", src);
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1) {
                    if (nxtSibling.tagName.toLowerCase() == "div") {
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if (parentNodeTable) {
                var checkUncheckSwitch;
                var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                if (isAllSiblingsChecked) {
                    checkUncheckSwitch = true;
                }
                else {
                    checkUncheckSwitch = false;
                }
                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            var k = 0;
            for (var i = 0; i < childCount; i++) {

                if (parentDiv.childNodes[i].nodeType == 1) {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {

                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false       
                        if (prevChkBox.checked) {
                            //add each selected node one value    
                            k = k + 1;
                        }
                    }
                }
            }
            //Finally check any one of child node is select if selected yes then return ture parent node check           
            if (k > 0) {

                return true;
            }
            else {

                return false;
            }
        }


        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <center>
        <asp:Label ID="lblTitle" runat="server" Text="User Module Permission" SkinID="Title"></asp:Label>
    </center>
    <table border="0" cellpadding="0" cellspacing="0" width="931">
        <tr>
            <td align="left">
                <asp:Label ID="lblPermissionforUser" runat="server" Text="Permission for User :" SkinID="SubTitle"></asp:Label>
                <asp:Label ID="lblPermissionforRole" runat="server" Text="Permission for Role :" SkinID="SubTitle"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:TreeView ID="tvwUserPermission" ShowCheckBoxes="All" runat="server" EnableTheming="false"
                    ExpandDepth="0" Font-Underline="False"
                    Font-Names="Verdana"
                    Font-Size="8pt"
                    Font-Italic="False"
                    Font-Bold="False"
                    ImageSet="MSDN"
                    ForeColor="#000000"
                    NodeIndent="16">
                    <SelectedNodeStyle
                        BorderColor="#999999"
                        BackColor="#FFFFFF"
                        Font-Underline="False"
                        Font-Italic="False"
                        Font-Bold="False"
                        BorderWidth="1px"
                        BorderStyle="Solid"></SelectedNodeStyle>
                    <HoverNodeStyle
                        BorderColor="#999999"
                        BackColor="#C7C7C7"
                        Font-Underline="True"
                        Font-Italic="False"
                        Font-Bold="False"
                        BorderWidth="1px"
                        BorderStyle="Solid"></HoverNodeStyle>
                    <RootNodeStyle Font-Underline="False" Font-Italic="False" Font-Bold="False"></RootNodeStyle>
                    <ParentNodeStyle Font-Underline="False" Font-Italic="False" Font-Bold="False"></ParentNodeStyle>
                    <LeafNodeStyle Font-Underline="False" Font-Italic="False" Font-Bold="False"></LeafNodeStyle>

                    <NodeStyle ForeColor="Blue"
                        NodeSpacing="1"
                        Font-Underline="False"
                        Font-Names="Verdana"
                        Font-Size="8pt"
                        Font-Italic="False"
                        Font-Bold="False"></NodeStyle>

                </asp:TreeView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ImageButton ID="ibtnSave" runat="server" OnClick="ibtnSave_Click" SkinID="SubmitButton" />&nbsp;
                <asp:ImageButton ID="ibtnCancel" runat="server" OnClick="ibtnCancel_Click" SkinID="CancelButton" />
            </td>
        </tr>
    </table>

</asp:Content>
