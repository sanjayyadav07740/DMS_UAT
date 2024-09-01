<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentViewer_MHADA.ascx.cs" Inherits="DMS.UserControl.DocumentViewer_MHADA" %>


 <script type="text/javascript" src="../UserControl/JavaScript/jquery.js"></script>
    <script type="text/javascript" src="../UserControl/JavaScript/jquery-latest.pack.js"></script>
    <script type="text/javascript" src="../UserControl/JavaScript/jquery.easing.1.3.js"></script>
    <script type="text/javascript" src="../UserControl/JavaScript/jquery.csstransform.1.0.pack.js"></script>
    <script type="text/javascript">

        var actualWidth = 900;
        var actualHeight = 700;

        var screenWidth = 0;
        var screenHeight = 0;

        var w = 0;
        var h = 0;

        var panTool = false;

        $(document).ready(function () {
            $(window).load(function () {
                $(".viewer").css({ 'width': actualWidth, 'height': actualHeight });
                $(".viewerToolBar").css({ 'width': actualWidth });
                w = actualWidth;
                h = actualHeight;
                $("#imgLoading").css({ 'width': (w / 100) * 66, 'margin-left': (w / 100) * 16 });
                screenWidth = $(document).width();
                screenHeight = $(document).height();
            });
        });

        $(window).bind('resize', function () {
            screenWidth = $(document).width();
            screenHeight = $(document).height();
        });

        function zoomIn() {
            document.getElementById('<%= imgImageView.ClientID %>').style.width = (parseInt(w) + 50).toString() + 'px'; w = parseInt(w) + 50; document.getElementById('<%= imgImageView.ClientID %>').style.height = (parseInt(h) + 50).toString() + 'px'; h = parseInt(h) + 50;
        }

        function zoomOut() {
            if (parseInt(w) > 0) { document.getElementById('<%= imgImageView.ClientID %>').style.width = (parseInt(w) - 50).toString() + 'px'; w = parseInt(w) - 50; } if (parseInt(h) > 0) { document.getElementById('<%= imgImageView.ClientID %>').style.height = (parseInt(h) - 50).toString() + 'px'; h = parseInt(h) - 50; }
        }

        function rotateImage(rotateID) {
            var _css = null;
            if (rotateID == 1) {
                _css = { rotate: '-=90' };
            }
            else if (rotateID == 2) {
                _css = { rotate: '+=90' };
            }
            $('#' + '<%= imgImageView.ClientID %>').animate(_css, 0.8 * 1000, 'swing');
        }

        function viewLarge(pageID) {
            $("#divLoading").show();
            $("#imgLoading").show();

            if (document.getElementById('<%= imgImageView.ClientID %>') != null) {
                document.getElementById('<%= imgImageView.ClientID %>').src = "../UserControl/PdfHandler.ashx?PageID=" + pageID;
                return false;
            }
            return false;
        }

        function viewerEvent(e) {

            var ee = e.keyCode ? e.keyCode : e.which;

            if (ee == 43) { //+
                zoomIn();
            }
            else if (ee == 45) { //-
                zoomOut();
            }
            else if (e.ctrlKey && ee == 37) //Left
            {
                rotateImage(1);
            }
            else if (e.ctrlKey && ee == 38) //Up
            {
                rotateImage(1);
            }
            else if (e.ctrlKey && ee == 39) //Right
            {
                rotateImage(2);
            }
            else if (e.ctrlKey && ee == 40) //Down
            {
                rotateImage(2);
            }
            else if (ee == 33) //PageUp
            {
                previousPage();
            }
            else if (ee == 34) //PageDown
            {
                nextPage();
            }
            else if (e.shiftKey && ee == 37) //Left
            {
                document.getElementById('<%= divImageView.ClientID %>').scrollLeft = document.getElementById('<%= divImageView.ClientID %>').scrollLeft - 50;
            }
            else if (e.shiftKey && ee == 39) //Right
            {
                document.getElementById('<%= divImageView.ClientID %>').scrollLeft = document.getElementById('<%= divImageView.ClientID %>').scrollLeft + 50;
            }
            else if (e.shiftKey && ee == 38) //Up
            {
                document.getElementById('<%= divImageView.ClientID %>').scrollTop = document.getElementById('<%= divImageView.ClientID %>').scrollTop - 50;
            }
            else if (e.shiftKey && ee == 40) //Down
            {
                document.getElementById('<%= divImageView.ClientID %>').scrollTop = document.getElementById('<%= divImageView.ClientID %>').scrollTop + 50;
            }
}

function previousPage() {
    var pageNumber = document.getElementById('<%= hdfCurrentPageNumber.ClientID %>').value;
            if (parseInt(pageNumber) > 1) {
                pageNumber = parseInt(pageNumber) - 1;
                document.getElementById('<%= hdfCurrentPageNumber.ClientID %>').value = pageNumber;
                document.getElementById('<%= txtCurrentPageNumber.ClientID %>').value = pageNumber;
                viewLarge(pageNumber);
            }
        }

        function nextPage() {
            var pageNumber = document.getElementById('<%= hdfCurrentPageNumber.ClientID %>').value;
            if (parseInt(pageNumber) < parseInt(document.getElementById('<%= hdfTotalPageNumber.ClientID %>').value)) {
                pageNumber = parseInt(pageNumber) + 1;
                document.getElementById('<%= hdfCurrentPageNumber.ClientID %>').value = pageNumber;
                document.getElementById('<%= txtCurrentPageNumber.ClientID %>').value = pageNumber;
                viewLarge(pageNumber);
            }
        }

        function goToPage(pageNumber) {
            try {
                pageNumber = parseInt(pageNumber);
                if (pageNumber != 0 && pageNumber <= parseInt($('#' + '<%= hdfTotalPageNumber.ClientID %>').val())) {
                    document.getElementById('<%= hdfCurrentPageNumber.ClientID %>').value = pageNumber;
                    document.getElementById('<%= txtCurrentPageNumber.ClientID %>').value = pageNumber;
                    $("#bMessage").hide();
                    viewLarge(pageNumber);
                }
                else {
                    $("#bMessage").text('Invalid page number');
                    $("#bMessage").fadeOut('slow', '2000');
                    $("#bMessage").fadeOut('2000');
                }
            }
            catch (ex) {
                $("#bMessage").text('Invalid page number');
                $("#bMessage").fadeOut('slow');
                $("#bMessage").fadeOut('2000');
            }
        }

        function maximize() {
            $("#imageViewer").css({ 'position': 'absolute' });
            $("#imageViewer").css({ 'z-index': '103', 'left': '0', 'top': '0', 'position': 'absolute' });
            $(".viewer").css({ 'width': screenWidth, 'height': screenHeight });
            $(".viewerToolBar").css({ 'width': screenWidth });
            w = screenWidth;
            h = screenHeight;
            $("#imgLoading").css({ 'width': (w / 100) * 66, 'margin-left': (w / 100) * 16 });
        }

        function minimize() {
            $("#imageViewer").css({ 'position': 'relative' });
            $("#imageViewer").css({ 'z-index': '103', 'left': '0', 'top': '0', 'position': 'relative' });
            $(".viewer").css({ 'width': actualWidth, 'height': actualHeight });
            $(".viewerToolBar").css({ 'width': actualWidth });
            w = actualWidth;
            h = actualHeight;
            $("#imgLoading").css({ 'width': (w / 100) * 66, 'margin-left': (w / 100) * 16 });
        }

        function downloadFile() {
            $("#ifrmDownloader").attr("src", "../UserControl/PdfHandler.ashx?PageID=download");
        }

        $(document).ready(function () {
            $("#imgHelp").mouseover(function (event) {
                $("#divUserManualBody").css('left', $("#imgHelp").position().left);
                $("#divUserManualBody").animate({ width: '+400', height: '+100', opacity: 1 }, 'slow');
                $("#divUserManualBody").fadeIn(100);

            });
        });

        $(document).ready(function () {
            $("#imgHelp").mouseout(function (event) {
                $("#divUserManualBody").animate({ width: '0', height: '0', opacity: 0.1 }, 'slow');
                $("#divUserManualBody").fadeOut(100);
            });
        });

        $(document).ready(function () {
            $('#' + '<%= imgImageView.ClientID %>').load(function () {
                $("#divLoading").hide();
                $("#imgLoading").hide();
            });
        });

            $(document).ready(function () {
                $("body").keypress(function (event) {
                    viewerEvent(event);
                });
            });

            $(document).ready(function () {
                $("body").keydown(function (event) {
                    viewerEvent(event);
                });
            });

            $(document).ready(function () {
                $('#' + '<%= txtCurrentPageNumber.ClientID %>').keydown(function (event) {
                var ee = event.keyCode ? event.keyCode : event.which;
                if (ee == 13) //Enter
                {
                    event.preventDefault();
                    var pageNumber = $('#' + '<%= txtCurrentPageNumber.ClientID %>').val();
                    goToPage(pageNumber);
                }

            });
        });

        $(document).ready(function () {
            $('#' + '<%= divImageView.ClientID %>').bind('mousewheel', function (event) {
                if (event.shiftKey) {
                    event.preventDefault();
                    if (event.wheelDelta == 120) {
                        zoomIn();
                    }
                    else if (event.wheelDelta == -120) {
                        zoomOut();
                    }
                }
            });
        });

        $(document).ready(function () {
            $('#' + '<%= divImageView.ClientID %>').bind('mousewheel  DOMMouseScroll MozMousePixelScroll', function (event) {
                if (event.shiftKey) {
                    event.preventDefault();
                    if (event.detail == -3) {
                        zoomIn();
                    }
                    else if (event.detail == 3) {
                        zoomOut();
                    }
                }
            });
        });

        var x, y;
        $(document).ready(function () {
            $('#' + '<%= divImageView.ClientID %>').mousedown(function (event) {
                $('#' + '<%= imgImageView.ClientID %>').css('cursor', 'move');
                x = event.clientX;
                y = event.clientY;
                panTool = true;
            });

            $('#' + '<%= divImageView.ClientID %>').mouseup(function (event) {
                $('#' + '<%= imgImageView.ClientID %>').css('cursor', 'default');
                x = 0;
                y = 0;
                panTool = false;
            });

            $('#' + '<%= divImageView.ClientID %>').mousemove(function (event) {
                if (panTool == true) {
                    event.preventDefault();
                    if (event.clientY < y) {
                        $('#' + '<%= divImageView.ClientID %>').scrollTop($('#' + '<%= divImageView.ClientID %>').scrollTop() - 50);
                    }
                    else {
                        $('#' + '<%= divImageView.ClientID %>').scrollTop($('#' + '<%= divImageView.ClientID %>').scrollTop() + 50);
                    }

                    if (event.clientX < x) {
                        $('#' + '<%= divImageView.ClientID %>').scrollLeft($('#' + '<%= divImageView.ClientID %>').scrollLeft() - 50);
                    }
                    else {
                        $('#' + '<%= divImageView.ClientID %>').scrollLeft($('#' + '<%= divImageView.ClientID %>').scrollLeft() + 50);
                    }
                }
            });
        });

        $(document).ready(function () {
            $(".toolBarIcon").mouseenter(function () {
                $("#divToolTip").css('left', $(this).position().left - 90);
                $("#divToolTip").html('<br/><b>' + $(this).attr('title') + '</b>');
                $("#divToolTip").fadeIn("slow");
            });
        });

        $(document).ready(function () {
            $(".toolBarIcon").mouseout(function () {
                $("#divToolTip").css('left', $(this).position().left - 90);
                $("#divToolTip").fadeOut("slow");
            });
        });
    </script>
 <center>
    <table id="imageViewer">
            <tr>
                <td colspan="2" style="width: 600px;" valign="top" align="center" class="viewerToolBar">
                    <div style="text-align: center; width: 600px; background-image: url('../UserControl/Image/ToolBarBack1.gif');
                        background-repeat: repeat-x; vertical-align: middle;" class="viewerToolBar">
                        <img src="../UserControl/Image/Help.png" id="imgHelp" style="width: 20px;" />
                        <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="../UserControl/Image/Save.png" OnClientClick="javascript:downloadFile();" Width="20px" Height="20px" OnClick="ibtnSave_Click"  />
                        <!--<img src="../UserControl/Image/Save.png" style="width: 20px;" onclick="javascript:downloadFile();" class="toolBarIcon" title="Save File"/>-->
                        <div id="divUserManualBody" style="display: none; position: absolute; text-align: left;
                             border: 1px solid black;z-index:102;color:White; background-color: Gray;">
                            <ul>
                                <li>"+" ZoomIn And "-" ZoomOut</li>
                                <li>"Ctrl" + "Arrow Keys" for rotating image</li>
                                <li>"Arrow Keys" for navigating image</li>
                                <li>"Page Up And Page Down" for moving through image</li>
                            </ul>
                        </div>
                        <div id="divToolTip" style="background-image:url('../UserControl/Image/ToolTip.png');vertical-align:middle;position: absolute;z-index:103;display:none;background-repeat:no-repeat;position:absolute;width:180px;height:45px;"></div>
                        &nbsp;
                        <img src="../UserControl/Image/First.png" style="width: 20px;" onclick="javascript:goToPage(1);" class="toolBarIcon" title="Go to first page"/>
                        <img src="../UserControl/Image/Previous.png" style="width: 20px;" onclick="javascript:previousPage();" class="toolBarIcon" title="Go to previous page"/>
                        <img src="../UserControl/Image/Next.png" style="width: 20px;" onclick="javascript:nextPage();" class="toolBarIcon" title="Go to next page"/>
                        <img src="../UserControl/Image/Last.png" style="width: 20px;" onclick="javascript:goToPage($('#' + '<%= hdfTotalPageNumber.ClientID %>').val());"  class="toolBarIcon" title="Go to last page"/>
                        &nbsp;
                        <img src="../UserControl/Image/ZoomIn.png" style="width: 20px;" onclick="javascript:zoomIn();"  class="toolBarIcon" title="Zoom In"/>
                        <img src="../UserControl/Image/ZoomOut.png" style="width: 20px;" onclick="javascript:zoomOut();"  class="toolBarIcon" title="Zoom Out"/>
                        &nbsp;
                        <img src="../UserControl/Image/LeftRotate.png" style="width: 20px;" onclick="javascript:rotateImage(1);"  class="toolBarIcon" title="Rotate left"/>
                        <img src="../UserControl/Image/RightRotate.png" style="width: 20px;" onclick="javascript:rotateImage(2);"  class="toolBarIcon" title="Rotate right"/>
                        &nbsp;
                        <img id="imgMaximize" src="../UserControl/Image/Maximize.png" style="width: 20px;" onclick="javascript:maximize();"  class="toolBarIcon" title="Maximize viewer"/>
                        <img id="imgMinimize" src="../UserControl/Image/Minimize.png" style="width: 20px;" onclick="javascript:minimize();"  class="toolBarIcon" title="Minimize viewer"/>
                        &nbsp;
                        <img src="../UserControl/Image/Refresh.png" style="width: 20px;" onclick="javascript:goToPage($('#' + '<%= hdfCurrentPageNumber.ClientID %>').val());"  class="toolBarIcon" title="Refresh page"/>
                        <input type="text" id="txtCurrentPageNumber" runat="server" maxlength="5" style="width: 50px;" />
                        <b>/ </b><b id="bTotalPageNumber" runat="server" style="color: Black;"></b>&nbsp;&nbsp;
                        <b id="bMessage" style="color: Black;"></b>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2"  valign="top" align="center" class="viewer">
                    <div id="divImageView" runat="server" style="width: 600px; border: 1px solid black;
                        vertical-align: top; height: 600px; overflow: auto;" class="viewer" >
                         <div id="divLoading" style="display: none; width: 600px; height: 600px; z-index: 100;
                            position: absolute; background-color: Green; opacity: 0.3; filter: alpha(opacity=30);" class="viewer">
                        </div>
                        <img id="imgLoading" src="../UserControl/Image/Loading.gif" style="display: none; width: 400px;
                            z-index: 101; position: absolute; margin-left: 100px;"/>
                        <img id="imgImageView" runat="server" style="width: 600px; height: 600px; cursor: hand;" class="viewer"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width: 600px" valign="top" align="center">
                    <iframe id="ifrmDownloader" style="display:none;" target="_blank"></iframe>
                    <input type="hidden" id="hdfCurrentPageNumber" runat="server" />
                    <input type="hidden" id="hdfTotalPageNumber" runat="server" />
                </td>
            </tr>
             <tr>
            <td colspan="2" align="center">
                
                <asp:ImageButton ID="ImageButton1" runat="server"  CausesValidation="false"  SkinID="CancelButton"
                    ToolTip='<%$ Resources:Resource,Back %>' style="height: 16px"  />
                    &nbsp;

                <%-- <asp:ImageButton ID="imgAddRemarks" runat="server" CausesValidation="false" SkinID="AddRemarks" OnClick="imgAddRemarks_Click" />--%>
                <%--<asp:ImageButton ID="imgSendMail" runat="server" CausesValidation="false" 
                    SkinID="ApproveButton" onclick="imgSendMail_Click" Width="16px" />--%>
            </td>

        </tr>
        <%--<tr>
            <td  style="vertical-align:top;">
                <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                </td>
                <td align="left"><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
            </td>
        </tr>--%>
        </table>
</center>