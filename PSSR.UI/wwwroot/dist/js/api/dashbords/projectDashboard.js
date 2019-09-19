var projectDashboard = projectDashboard || (function () {

    return {
        init: function () {
            initialization();
        },
        globalChart: function() {
            globalChart();
        },
        getWorkPackageContent: function(id) {
            getWorkPackageContent(id);
        },
        initWorkPakcageDropDown: function (cworkId, locid) {
            initWorkPakcageDropDown(cworkId, locid);
        },
        initLocationDropDown: function (locid) {
            initLocationDropDown(locid);
        }
    };

    function initialization() {
        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/ProjectDashboardInitialization",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    initProjectData(data.project);
                    initWorkpackageTab(data.workPackages);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function initProjectData(project) {
        $('#pr-d-0').text(project.description);
        $('#pr-d-1').text(project.startDate);
        $('#pr-d-2').text(project.endDate);
        $('#pr-d-3').text(project.elapsedDate);
        $('#pr-d-4').text(project.remainedDate);

        $('#pr-d-5').text(project.desciplinesCount);
        $('#pr-d-6').text(project.systemsCount);
        $('#pr-d-7').text(project.subSystemsCount);
        $('#pr-d-8').text(project.activitysCount);
    }

    function initWorkpackageTab(workPackages) {

        var cHeader = $('#tab-header');
        var cBody = $('#tab-body');

        $.each(workPackages, function (i, val) {
            if (i == 0) {
                var th = $("<li><a class='tab-header-wf' data-id='th-" + val.id + "'  href='#tab_wf_" + val.id + "' data-toggle='tab'>" + val.title + "</a></li>");
            }
            else {
                var th = $("<li><a class='tab-header-wf' data-id='th-" + val.id + "'  href='#tab_wf_" + val.id + "' data-toggle='tab'>" + val.title + "</a></li>");
            }

            var tb = $("<div class='tab-pane workpackage-pane' id='tab_wf_" + val.id + "'></div>");
            cHeader.append(th);
            cBody.append(tb);
        });

    }

    function initWorkPakcageDropDown(cworkId, locid)
    {
        var content = $('#workPackage-type');
        content.empty();
        content.append("<option value=0>...Select workPackage...</option>");

        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/ProjectDashboardInitialization",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data.workPackages, function (i, val) {
                    if (val.id == cworkId) {
                        content.append("<option value=" + val.id + " selected>" + val.title + "</option>");
                    }
                    else {
                        content.append("<option value=" + val.id + ">" + val.title + "</option>");
                    }
                });

                initLocationDropDown(locid);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function initLocationDropDown(locid) {
        var content = $('#location-type');
        content.empty();
        content.append("<option value=0>...Select Location..</option>");

        $.ajax({
            type: "Get",
            url: "/poec/WorkPackage/GetLocations",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    if (val.id == locid) {
                        content.append("<option value=" + val.id + " selected>" + val.title + "</option>");
                    }
                    else {
                        content.append("<option value=" + val.id + ">" + val.title + "</option>");
                    }
                });
                $('#location-type').trigger('change');
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function globalChart() {
        $('#gchart-loading').show();
        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/GetGlobalActivityDoneChart",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    paintingGlobalChart(data);
                }
                $('#gchart-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function paintingGlobalChart(model) {
        Highcharts.chart('globalChart', {
            chart: {
                type: 'spline'
            },
            title: {
                text: 'Daily Done Activity For Related Projects'
            },
            subtitle: {
                text: 'Source: PCMS Database'
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: { // don't display the dummy year
                    month: '%e. %b',
                    year: '%b'
                },
                title: {
                    text: 'Date'
                }
            },
            yAxis: {
                title: {
                    text: 'Activity'
                },
                min: 0
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.x:%e. %b}: {point.y}'
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: model.values
        });

        $('.highcharts-credits').css('visibility', 'hidden');
    }

}());