<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDocument.aspx.cs" Inherits="DMS.MHADA_Website_Search.ViewDocument" %>

<%@ Register Src="~/UserControl/DocumentViewer_MHADA.ascx" TagPrefix="uc" TagName="DocumentViewer_Mhada" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download Document</title>
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
        width:96%;
        height:100px;
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
    <form id="form1" runat="server">
        <div style="width:100%;height:10px;background-color:#000000;"></div>
        <div style="height:100px;padding:0px 0px 0px 40px;background-color:#ffffff;"><img src="../Images/logo.jpg" /></div>
        <div style="width:1300px;height:100%;">
            <uc:DocumentViewer_Mhada ID="dvDocumentViewer" runat="server" />
        </div>
    </form>
</body>
</html>
