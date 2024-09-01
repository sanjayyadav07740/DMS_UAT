<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPageDashBoard.Master" AutoEventWireup="true" CodeBehind="NewDashBoard.aspx.cs" Inherits="DMS.Shared.NewDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">


    <script src="../bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->

    <script src="../bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- fastclick -->

<%--    <script src="../bower_components/fastclick/lib/fastclick.js"></script>--%>
    <!-- adminlte app -->

    <script src="../dist/js/adminlte.min.js"></script>


    <link href="../bower_components/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- Ionicons -->
    <link href="../bower_components/Ionicons/css/ionicons.min.css" rel="stylesheet" />
    <!-- jvectormap -->
   <%-- <link rel="stylesheet" href="bower_components/jvectormap/jquery-jvectormap.css"/>--%>
    <!-- Theme style -->
    <link href="../dist/css/AdminLTE.css" rel="stylesheet" />

    <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
    <style type="text/css">
        #fa .fa {
            margin-top: 20px;
        }

        .gridrow td, th {
            border: 1px solid #000000;
            color: #313131;
            font-size: 12px;
            font-weight: normal;
            vertical-align: middle;
            text-align: center !important;
            margin: 0px;
            height: 22px;
            padding: 5px;
        }
    </style>
    <%--  <script type="text/javascript" src="../JQ/jquery-1.4.2.min.js"></script>--%>
    <link href="../dist/css/skins/_all-skins.min.css" rel="stylesheet" />

    <%-- ============================================================================--%>

    <%--Added Code for Calendar--%>


   <%-- <!-- Font Awesome -->
    <link rel="stylesheet" href="../bower_components/font-awesome/css/font-awesome.min.css" />--%>
    <!-- Ionicons -->
    <link rel="stylesheet" href="../bower_components/Ionicons/css/ionicons.min.css" />
    <!-- fullCalendar -->
    <link rel="stylesheet" href="../bower_components/fullcalendar/dist/fullcalendar.min.css" />
    <link rel="stylesheet" href="../bower_components/fullcalendar/dist/fullcalendar.print.min.css" media="print" />
    <!-- Theme style -->

    <%--    <!-- jQuery 3 -->
    <script src="../bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="../bower_components/bootstrap/dist/js/bootstrap.min.js"></script>--%>
    <!-- jQuery UI 1.11.4 -->
   <script src="../bower_components/jquery-ui/jquery-ui.min.js"></script>
    <!-- Slimscroll -->
    <script src="../bower_components/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
     <%--<script src="../bower_components/fastclick/lib/fastclick.js"></script>--%>
    <!-- fullCalendar -->
    <script src="../bower_components/moment/moment.js"></script>
    <script src="../bower_components/fullcalendar/dist/fullcalendar.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            //debugger;
            //InitializeCalendar();
        });
        function InitializeCalendar() {
            // debugger;
            //-- start date and end date criteria.. you can get it from user input.. 
            var startDate = "1/1/2010 12:00:00 AM";
            var endDate = "12/31/2050 12:00:00 AM";
            //-- start date and end date criteria.. you can get it from user input.. 

            //-- ajax call to fetch calendar data from database
            $.ajax({
                type: "POST",
                //contentType: "application/json",
                contentType: 'application/json;charset=utf-8',
                data: "{ StartDate: '" + startDate + "', EndDate: '" + endDate + "' }",
                // url: "WebForm125.aspx/GetCalendarData",
                url: "NewDashBoard.aspx/GetCalendarData",
                dataType: "json",
                success: function (data) {
                    $('#calendar').empty();
                    $('#calendar').fullCalendar({
                        header: {
                            //left: 'prev,next today',
                            left: 'prev,next',
                            center: 'title',
                            // right: 'agendaDay,agendaWeek,month'
                            right: 'month'
                        },
                        defaultView: 'month',
                        // allDaySlot:false,
                        weekNumbers: false,
                        height: 400,
                        allDayText: 'Documents',
                        selectable: true,
                        overflow: 'auto',
                        editable: false,
                        // aspectRatio: 1.8,
                        minTime: '09:00',
                        maxTime: '20:00',
                        scrollTime: '09:00',
                        timezone: 'local',
                        firstDay: 7,
                        slotMinutes: 5,
                        //eventTextColor: 'Yellow',
                        //eventBackgroundColor: 'purple',
                        // businessHours: true,
                        // timezoneParam: '2013-10-20T02:00:00+09:00', 
                        //eventClick: function (calEvent, jsEvent, view) {
                        //    var modal = new DayPilot.Modal();
                        //    modal.top = 60;
                        //    modal.width = 900;
                        //    modal.opacity = 70;
                        //    modal.border = "10px solid #d0d0d0";
                        //    modal.closed = function () {
                        //        if (this.result == "OK") {
                        //            dpc.commandCallBack('refresh');
                        //        }
                        //        dpc.clearSelection();
                        //    };
                        //    modal.height = 550;
                        //    modal.width = 900;
                        //    modal.zIndex = 9999999;
                        //    //modal.showUrl("Followup.aspx?loc=0&Id=" + calEvent.id); 
                        //},
                        //eventRender: function(event, element) {
                        //    $(element).tooltip({ title: event.title,content: event.description});             
                        //},
                        eventRender: function (event, element) {
                            // $(element).popover({ title: event.title, content: event.description, trigger: 'hover', placement: 'auto bottom', delay: { "hide": 300} });
                            $(element).css({ "width": "35px", "margin-left": "12px", "margin-top": "10px", "zIndex": "9999999", "text-align": "Center" });
                        },
                        /*
                        ----On Hover---
                        
                        eventMouseover: function (calEvent, jsEvent, event) {
                            var tooltip = '<div class="tooltipevent" style="width:250px;height:auto;background:#fefefe;position:absolute;z-index:10001;border-radius:10px;text-align:center;padding:10px 0px 15px 0px;box-shadow:10px 10px 10px 0px #282828;"><div style="width:23px;height:23px;position:absolute;background:#fefefe; border-right-width:1px; border-right-style:solid; border-top-width:1px; border-top-style:solid; border-color:#eeeeee; top:-10px; left:108px; transform:rotate(-45deg); z-index:-1;display:none"></div><b style="color:#000;">' + calEvent.title + '<b/><br/><b style="color:#000;font-weight:normal;">' + calEvent.description + '</b></div>';
                            $("body").append(tooltip);
                            $(this).mouseover(function (e) {
                                $(this).css('z-index', 10000);
                                // $(this).css('width', 200);
                                $('.tooltipevent').fadeIn('500');
                                $('.tooltipevent').fadeTo('10', 1.9);
                            }).mousemove(function (e) {
                                $('.tooltipevent').css('top', e.pageY + 10);
                                $('.tooltipevent').css('left', e.pageX + 20);
                            });
                        },
                    */
                        eventMouseout: function (calEvent, jsEvent) {
                            $(this).css('z-index', 8);
                            $('.tooltipevent').remove();
                        },

                        events: $.map(data.d, function (item, i) {
                            //-- here is the event details to show in calendar view.. the data is retrieved via ajax call will be accessed here
                            var eventStartDate = new Date(parseInt(item.EventStartDate.substr(6)));
                            var eventEndDate = new Date(parseInt(item.EventEndDate.substr(6)));
                            var event = new Object();
                            event.id = item.EventId;
                            event.start = new Date(eventStartDate.getFullYear(), eventStartDate.getMonth(), eventStartDate.getDate(), eventStartDate.getHours(), 0, 0, 0);
                            event.end = new Date(eventEndDate.getFullYear(), eventEndDate.getMonth(), eventEndDate.getDate(), eventEndDate.getHours() + 1, 0, 0, 0);
                            event.title = item.Title;
                            event.description = item.EventDescription;
                            // event.allDay = item.AllDayEvent;
                            return event;
                        })
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //-- log error
                }
            });
        }

        $(document).ready(function () {
            $("#mycollapse").click(function () {
                $("#hidebox").slideToggle("slow");
                $("#myplus").toggleClass('active');
            });
        });

    </script>

    <%--============================================================================--%>


    <%--Added code for chart--%>

    <%--    <script src="../JQ/jquery-1.4.2.min.js"></script>--%>
    <%--       <script src="../JQ/jquery-1.8.1.min.js"></script>
    <script src="../JQ/jquery-ui-1.8.11.js"></script>--%>

    <script src="../JQ/highchart/highcharts.js"></script>

    <script type="text/javascript">

        function pageLoad(sender, args) {

            $.ajax({
                type: "POST",
                url: "NewDashBoard.aspx/GetYearsforDropdown",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    for (var i = 0; i < data.d.length; i++) {
                        $('#ddlYear').append(new Option(data.d[i], data.d[i], false, true));
                    }

                },
                failure: function (response) {

                }
            });


            InitialiseSettings();
            InitializeCalendar();
            $('#ddlYear').change(function () {
                InitialiseSettings();
            });
        }

        function InitialiseSettings() {
            //$.ajax({
            //    type: "POST",
            //    url: "NewDashBoard.aspx/monthwisechart",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (datachart) {


            //        var dataseries = [];

            //        var array = datachart.d.split(',');


            //        for (var i = 0; i < array.length; i++) {
            //            var itemsx = array[i];
            //            var itemsy = array[i + 1];

            //            //var Xcategories = items.status;
            //            var seriesdata = { name: itemsx, y: parseInt(itemsy) };
            //            // Xaxis.push(Xcategories);

            //            dataseries.push(seriesdata);
            //            i++;
            //        }
            //        //getdata(dataseries)
            //        //console.log(dataseries);

            //        getchartmonth(dataseries);


            //    },
            //    failure: function (response) {

            //    }
            //});

            //function generateData(cats, names, points) {
            //    var ret = {},
            //        ps = [],
            //        series = [],
            //        len = cats.length;

            //    //concat to get points
            //    for (var i = 0; i < len; i++) {
            //        ps[i] = {
            //            x: cats[i],
            //            y: points[i],
            //            n: names[i]
            //        };
            //    }
            //    names = [];
            //    //generate series and split points
            //    for (i = 0; i < len; i++) {
            //        var p = ps[i],
            //            sIndex = $.inArray(p.n, names);

            //        if (sIndex < 0) {
            //            sIndex = names.push(p.n) - 1;
            //            series.push({
            //                name: p.n,
            //                data: []
            //            });
            //        }
            //        series[sIndex].data.push(p);
            //    }
            //    return series;
            //}


            //function getchartmonth(dataseries) {
            //    debugger;

            //    $('#monthwise').highcharts({
            //        chart: {
            //            //renderTo: 'container1',
            //            type: 'column',
            //            options3d: {
            //                enabled: true,
            //                alpha: 10,
            //                beta: 25,
            //                depth: 70
            //            },
            //            backgroundColor: {
            //                linearGradient: [0, 0, 0, 300],
            //                stops: [
            //                    [0, '#FFFFFF'],
            //                    [1, '#FFFFFF']
            //                ]
            //            },
            //        },
            //        title: {
            //            text: 'File vs Month'
            //        },
            //        subtitle: {
            //            text: ' '
            //        },
            //        xAxis: {
            //            type: 'category',
            //            labels: {
            //                //rotation: -55,
            //            }
            //        },
            //        yAxis: {
            //            title: {
            //                text: 'Count'
            //            }

            //        },
            //        credits: {
            //            enabled: false
            //        },
            //        legend: {
            //            enabled: false
            //        },
            //        plotOptions: {
            //            series: {
            //                borderWidth: 0,
            //                dataLabels: {
            //                    enabled: true,
            //                    // format: '{point.y:.1f}%'
            //                    format: '{point.y}'
            //                }
            //            }
            //        },

            //        tooltip: {
            //            headerFormat: '<span style="font-size:13px">{series.name}</span><br>',
            //            //pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
            //            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b> <br/>'
            //        },
            //        exporting: {
            //            enabled: true
            //        },
            //        series: [{
            //            name: 'Status',
            //            colorByPoint: true,

            //            data: dataseries
            //        }],

            //    });
            //};



            $.ajax({
                type: "POST",
                url: "NewDashBoard.aspx/Metatemplatewisechart",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'year': $('#ddlYear').val() }),
                success: function (datachart) {


                    var dataseries = [];

                    var array = datachart.d.split(',');


                    for (var i = 0; i < array.length; i++) {
                        var itemsx = array[i];
                        var itemsy = array[i + 1];

                        //var Xcategories = items.status;
                        var seriesdata = { name: itemsx, y: parseInt(itemsy) };
                        // Xaxis.push(Xcategories);

                        dataseries.push(seriesdata);
                        i++;
                    }
                    //getdata(dataseries)
                    //console.log(dataseries);

                    getchart(dataseries);


                },
                failure: function (response) {

                }
            });


            function getchart(dataseries) {
                debugger;

                $('#metatemplatewise').highcharts({
                    chart: {
                        //renderTo: 'container1',
                        type: 'column',
                        options3d: {
                            enabled: true,
                            alpha: 10,
                            beta: 25,
                            depth: 70
                        },
                        backgroundColor: {
                            linearGradient: [0, 0, 0, 300],
                            stops: [
                                [0, '#FFFFFF'],
                                [1, '#FFFFFF']
                            ]
                        },
                    },
                    title: {
                        text: 'File vs MetaTemplate'
                    },
                    subtitle: {
                        text: ' '
                    },
                    xAxis: {
                        type: 'category',
                        labels: {
                            //rotation: -55,
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Count'
                        }

                    },
                    credits: {
                        enabled: false
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                // format: '{point.y:.1f}%'
                                format: '{point.y}'
                            }
                        }
                    },

                    tooltip: {
                        headerFormat: '<span style="font-size:13px">{series.name}</span><br>',
                        //pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b> <br/>'
                    },
                    exporting: {
                        enabled: true
                    },
                    series: [{
                        name: 'Status',
                        colorByPoint: true,

                        data: dataseries
                    }],

                });
            };


        };

    </script>

    <%-- ========================================================================--%>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphmain" runat="server">

    <div style="width: 100%; margin: 0px 15px 0px 15px; margin-left: 0px">
        <div style="background-color: #ECF0F5">
            <div class="content wrapper">
                <section class="content-header breadcrumb">
      <h1 style="color:#0F5EAA">
        Dashboard
        <small></small>
      </h1>
      <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Dashboard</li>
      </ol>
    </section>

                <div class="row" id="fa">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="info-box">
                            <span class="info-box-icon bg-secondary"><i class="fa fa-file"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">New Document</span>
                                <span class="info-box-number">
                                    <asp:Label ID="lblNewDoc" runat="server" Font-Size="Large" /></span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="info-box">
                            <span class="info-box-icon bg-teal"><i class="fa fa-file-o"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">Total Document</span>
                                <span class="info-box-number">
                                    <asp:Label ID="lbltotalDoc" runat="server" Font-Size="Large" /></span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->

                    <!-- fix for small devices only -->
                    <div class="clearfix visible-sm-block"></div>

                    <div class="col-md-6 col-sm-6 col-xs-12" style="display:none">
                        <div class="info-box">
                            <span class="info-box-icon bg-green"><i class="fa fa-sign-in"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">Pending Document</span>
                                <span class="info-box-number">
                                    <asp:Label ID="lblPendingDoc" runat="server" Font-Size="Large" /></span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                    <div class="col-md-3 col-sm-6 col-xs-12"  style="display:none">
                        <div class="info-box">
                            <span class="info-box-icon bg-yellow"><i class="fa fa-upload fa-lg"></i></span>

                            <div class="info-box-content">
                                <span class="info-box-text">Entry Completed</span>
                                <span class="info-box-number">
                                    <asp:Label ID="lblUploadDoc" runat="server" Font-Size="Large" /></span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                </div>
            </div>
        </div>
        <br />
        <%-- Added by Sanjay --%>
        <div class="row">

            <div class="col-md-6" style="display: none">

                <!-- TABLE: LATEST ORDERS -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">My WorkSpace</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div class="table-responsive">
                            <%-- <asp:GridView runat="server" ID="gridview1" Width="100%" AllowPaging="true" PageSize="7" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true">

           </asp:GridView>--%>
                            <asp:GridView ID="gvInprogressDocument" runat="server" AutoGenerateColumns="False" DataKeyNames="DocumentName,Documenttype,MetaTemplateName,HTStatus,UserName" Width="100%"
                                OnRowDataBound="gvInprogressDocument_RowDataBound" EmptyDataText="No Data to Display." OnRowCommand="gvInprogressDocument_RowCommand" AllowPaging="false" PageSize="10"
                                OnPageIndexChanging="gvInprogressDocument_PageIndexChanging"
                                ShowFooter="false" ShowHeaderWhenEmpty="True">
                                <Columns>

                                    <asp:TemplateField HeaderText="Document Name" ItemStyle-Width="150">
                                        <ItemTemplate>
                                            <%# (Eval("DocumentName").ToString().Length > 20) ? (Eval("DocumentName").ToString().Substring(0, 20) + "...") : Eval("DocumentName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File Type" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Image ID="imgIcon" runat="server" ImageAlign="Middle" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="MetaTemplateName" DataField="MetaTemplateName" />
                                    <asp:BoundField HeaderText="UserName" DataField="UserName" />
                                    <asp:BoundField HeaderText="Status" DataField="HTStatus" />
                                    <%-- <asp:ButtonField ButtonType="Image" CommandName="View" HeaderText="View Document" ItemStyle-HorizontalAlign="Center"
                        ImageUrl="~/Images/Icon-View.png">
                        <ControlStyle Width="25px"></ControlStyle>
                    </asp:ButtonField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <!-- /.table-responsive -->
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer clearfix">
                        <%-- <a href="javascript:void(0)" class="btn btn-sm btn-info btn-flat pull-left">Place New Order</a>--%>
                        <a href="../MetaData/ViewAllWorkspace.aspx" class="btn btn-sm btn-info btn-flat pull-right">View All </a>
                    </div>
                    <!-- /.box-footer -->
                </div>
                <!-- /.box -->
            </div>

            <div class="col-md-6">

                <!-- TABLE: LATEST ORDERS -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Recent Document</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div class="table-responsive">
                            <%-- <asp:GridView runat="server" ID="gridview1" Width="100%" AllowPaging="true" PageSize="7" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true">

           </asp:GridView>--%>
                            <asp:GridView ID="gvRecentDocument" AutoGenerateColumns="false" DataKeyNames="DocumentName,Documenttype,MetaTemplateName,CreatedOn" OnRowDataBound="gvRecentDocument_RowDataBound" Width="100%" EmptyDataText="No Comments Available."
                                ShowHeaderWhenEmpty="True" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="Document Name" ItemStyle-Width="150">
                                        <ItemTemplate>
                                            <%# (Eval("DocumentName").ToString().Length > 20) ? (Eval("DocumentName").ToString().Substring(0, 20) + "...") : Eval("DocumentName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File Type" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Image ID="imgIcon" runat="server" ImageAlign="Middle" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="MetaTemplateName" DataField="MetaTemplateName" />
                                    <asp:BoundField HeaderText="CreatedOn" DataField="CreatedOn" DataFormatString="{0:dd MMM yy}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <!-- /.table-responsive -->
                    </div>
                    <!-- /.box-body -->
                    <%--<div class="box-footer clearfix">
             
              <a href="javascript:void(0)" class="btn btn-sm btn-info btn-flat pull-right">View All</a>
            </div>--%>
                    <!-- /.box-footer -->
                </div>
                <!-- /.box -->
            </div>

            <div class="col-md-6" id="DivCalendar" visible="true" runat="server">

                <!-- TABLE: LATEST ORDERS -->
                <div class="box box-warning">
                    <div class="box-header with-border">
                        <h3 class="box-title">Calendar</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div class="table-responsive">
                            <div id="calendar"></div>
                        </div>
                        <!-- /.table-responsive -->
                    </div>

                </div>
                <!-- /.box -->
            </div>

        </div>

        <div class="row">

            <div class="col-md-7" id="DivDocChart" visible="false" style="display:none" runat="server">

                <!-- TABLE: LATEST ORDERS -->
                <div class="box box-warning">
                    <div class="box-header with-border">
                        <h3 class="box-title">Document Chart</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div class="text-center">
                            <label>Select Year :</label>
                            <select id="ddlYear" style="width: 100px"></select>
                        </div>
                        <div class="table-responsive">
                            <div class="graphoutbox" style="float: left;">
                                <div id="metatemplatewise" class="graphbg"></div>
                            </div>

                        </div>
                    </div>

                </div>

            </div>

            <!-- /.box -->
        </div>

        <div>

            <!-- Content Wrapper. Contains page content -->

            <!-- /.content-wrapper -->

        </div>
    </div>

</asp:Content>
