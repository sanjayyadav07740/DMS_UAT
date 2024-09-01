<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchMSIB.aspx.cs" Inherits="DMS.MHADA_Website_Search.SearchMSIB" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSIB</title>
    <script>
        // WRITE THE VALIDATION SCRIPT IN THE HEAD TAG.
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }
</script>
    <style type="text/css">
        .bg {
            background:url(../Images/bg-img-v1.jpg) no-repeat;
            width:100%;
            margin:0px;
        }
    #wrapper {
        width:1300px;
        margin:0 auto;
        font-family:Verdana, sans-serif;
        font-size:13px;
		padding: 0px 6px 8px 6px;
    }
    /*legend {
        color:#0481b1;
        font-size:16px;
        padding:0 10px;
        background:#fff;
        -moz-border-radius:4px;
        box-shadow: 0 1px 5px rgba(4, 129, 177, 0.5);
        padding:5px 10px;
        text-transform:uppercase;
        font-family:Helvetica, sans-serif;
        font-weight:bold;
    }
    fieldset {
        border-radius:4px;
        background: #fff; 
        background: -moz-linear-gradient(#fff, #f9fdff);
        background: -o-linear-gradient(#fff, #f9fdff);
        background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#fff), to(#f9fdff));
        background: -webkit-linear-gradient(#fff, #f9fdff);
        padding:20px;
        border-color:rgba(4, 129, 177, 0.4);
        display:block;
        margin: 0 0 30px;
    }*/
   fieldset {
    margin: 0px;
    padding: 0px 20px 0px 20px;
    border: solid orange;
    border-width: 1px 1px 1px 1px;
    overflow: hidden;
}

    legend {
        background-color: orange;
        line-height: 50px;
        padding-right: 95%;
        padding-left: 5%;
        float: left;
        margin-left: -30px;
        width:100%;
    }

    input,
    textarea,
    select {
        color: #373737;
        background: #fff;
        border: 1px solid #CCCCCC;
        color: #aaa;
        font-size: 14px;
        line-height: 1.2em;
        margin-bottom:15px;
        -moz-border-radius:4px;
        -webkit-border-radius:4px;
        border-radius:4px;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1) inset, 0 1px 0 rgba(255, 255, 255, 0.2);
    }
    input[type="text"],
    input[type="password"]{
        padding: 8px 6px;
        height: 22px;
        width:280px;
    }
    input[type="text"]:focus,
    input[type="password"]:focus {
        background:#f5fcfe;
        text-indent: 0;
        z-index: 1;
        color: #373737;
        /*-webkit-transition-duration: 400ms;
        -webkit-transition-property: width, background;
        -webkit-transition-timing-function: ease;
        -moz-transition-duration: 400ms;
        -moz-transition-property: width, background;
        -moz-transition-timing-function: ease;
        -o-transition-duration: 400ms;
        -o-transition-property: width, background;
        -o-transition-timing-function: ease;*/
        width: 280px;
        border-color:#ccc;
        box-shadow:0 0 5px rgba(4, 129, 177, 0.5);
        opacity:0.6;
    }
    input[type="submit"]{
        background: #222;
        border: none;
        text-shadow: 0 -1px 0 rgba(0,0,0,0.3);
        text-transform:uppercase;
        color: #eee;
        cursor: pointer;
        font-size: 15px;
        margin: 5px 0;
        padding: 5px 22px;
        -moz-border-radius: 4px;
        border-radius: 4px;
        /*-webkit-border-radius:4px;
        -webkit-box-shadow: 0px 1px 2px rgba(0,0,0,0.3);
        -moz-box-shadow: 0px 1px 2px rgba(0,0,0,0.3);
        box-shadow: 0px 1px 2px rgba(0,0,0,0.3);*/
    }
    textarea {
        padding:3px;
        }
    textarea:focus {
        background:#ebf8fd;
        text-indent: 0;
        z-index: 1;
        color: #373737;
        opacity:0.6;
        box-shadow:0 0 5px rgba(4, 129, 177, 0.5);
        border-color:#ccc;
    }
    select {
       padding: 8px 6px;
       height: 35px;
       width:280px;
    }
    select:focus {
        background:#ebf8fd;
        text-indent: 0;
        z-index: 1;
        color: #373737;
        opacity:0.6;
        box-shadow:0 0 5px rgba(4, 129, 177, 0.5);
        border-color:#ccc;
    }

    .small {
        line-height:14px;
        font-size:12px;
        color:#999898;
        margin-bottom:3px;
    }

</style>
</head>
<body style="margin:0 auto;width:100%;background-color:#e6ffff;">
    <form runat="server">
        <div style="width:100%;height:10px;background-color:#000000;"></div>
        <div style="height:100px;padding:0px 0px 0px 40px;background-color:#ffffff;"><img src="../Images/logo.jpg" /></div>
        <div>
            <div id="wrapper">
                <fieldset>
                    <legend>Search RTI</legend>
                    <table width="100%" style="padding-top: 15px;">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblZone" runat="server" Text="Zone*"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlZone" runat="server" Width="170px">
                                    <asp:ListItem>--Select--</asp:ListItem>
                                    <asp:ListItem>City</asp:ListItem>
                                    <asp:ListItem>East</asp:ListItem>
                                    <asp:ListItem>West</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlZone" ErrorMessage="Please Select Zone" runat="server" ForeColor="Red" InitialValue="--Select--"></asp:RequiredFieldValidator>
                            </td>

                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblAggNo" runat="server" Text="B1 Agreement Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAggNo" runat="server" Width="156px" MaxLength="4" onkeypress="javascript:return isNumber(event)"></asp:TextBox>
                                
                            </td>
                        </tr>
                         <tr>
                            <td align="right">
                                <asp:Label ID="lblSubName" runat="server" Text="Subject Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubName" runat="server" Width="272px" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" Text="Constituency/Area"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtArea" runat="server" Width="153px"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTotal" runat="server" Visible="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="txtSearch" runat="server" Text="Search" OnClick="txtSearch_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="txtCancel" runat="server" Text="Cancel" OnClick="txtCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:GridView ID="grvDocument" runat="server" AutoGenerateColumns="false" Width="503px" OnRowCommand="grvDocument_RowCommand" DataKeyNames="ID" AllowPaging="true" PageSize="10" OnPageIndexChanging="grvDocument_PageIndexChanging" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1  %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" Visible="false" DataField="id"  />
                                        <asp:BoundField HeaderText="Document Name" DataField="documentname" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="View" CommandArgument=' <%# Eval("id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </form>
</body>
</html>
