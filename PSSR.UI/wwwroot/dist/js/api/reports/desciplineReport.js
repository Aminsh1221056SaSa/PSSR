var desciplineReport = desciplineReport || (function () {

    return {
        paintingDesciplineActivityBarStatus: function (workPackageId, locationId, content) {
            paintingDesciplineActivityBarStatus(workPackageId, locationId, content);
        },
        paintingDesciplineActivityBarCondition: function() {
            paintingDesciplineActivityBarCondition();
        },
        getDesciplineCounter: function() {
            getDesciplineCounter();
        },
        getDesciplineDoneActivity: function() {
            getDesciplineDoneActivity();
        }
    };

    function paintingDesciplineActivityBarStatus(workPackageId, locationId, content) {
        $("#"+content).empty();
        $('#precom-chart-loading').show();
        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/TaskStatusPreCommByDesciplines?workPackageId=" + workPackageId + "&locationId=" + locationId + "&total=true",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designStatusDesciplineChart(data, content);
                    $('#precom-chart-loading').hide();
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function paintingDesciplineActivityBarCondition() {

        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/TaskConditionByDesciplines",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designConditionDesciplineChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getDesciplineCounter() {

        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/TaskCounterByDesciplines",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    paintDesciplineCounterChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getDesciplineDoneActivity() {

        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/TaskDoneByDesciplines",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    spLineDesciplineDateChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function designStatusDesciplineChart(model, content) {
        Highcharts.chart(content, {
            chart: {
                type: 'bar'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: 'Precommissioning Chart'
            },
            xAxis: {
                categories: model.desciplines,
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Number',
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' '
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: false
                    }
                }
            },
            credits: {
                enabled: false
            },
            exporting: {
                buttons: {
                    contextButton: {
                        enabled: true
                    },
                    customButton: {
                        text: 'Details',
                        onclick: function () {
                            showDesciplineActivityDetails();
                        },
                        theme: {
                            'stroke-width': 0.5,
                            stroke: 'silver',
                            r: 0,
                            states: {
                                hover: {
                                    fill: '#a4edba'
                                },
                                select: {
                                    stroke: '#039',
                                    fill: '#a4edba'
                                }
                            }
                        }
                    }
                }
            },
            series: model.values
        });
    }


    function showDesciplineActivityDetails() {
        var workPackageId = $('#cu-workDes-id').val();
        var locationId = $('#cu-des-id').val();
        $('#modal-default-overlay').modal('toggle');
        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/TaskStatusPreCommByDesciplines?workPackageId=" + workPackageId + "&locationId=" + locationId + "&total=false",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designStatusDesciplineDetailsChart(data, 'desciplineStatusComDetails');
                    $('#modal-default-overlay').modal('toggle');
                    $('#modal-details-Deschart').modal('show');
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }


    function designStatusDesciplineDetailsChart(model, content) {
        Highcharts.chart(content, {
            chart: {
                type: 'bar'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: 'Precommissioning Chart'
            },
            xAxis: {
                categories: model.desciplines,
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Number',
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' '
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: false
                    }
                }
            },
            credits: {
                enabled: false
            },
            series: model.values
        });
    }

    function designConditionDesciplineChart(model) {
        Highcharts.chart('desciplineCondition', {
            chart: {
                type: 'bar'
            },
            title: {
                text: 'Activities Condition'
            },
            subtitle: {
                text: 'Source: <a>PCMS Database</a>'
            },
            xAxis: {
                categories: model.desciplines,
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: "Number",
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' '
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            credits: {
                enabled: false
            },
            series: model.values
        });
    }

    function paintDesciplineCounterChart(model) {
        // Build the chart
        Highcharts.chart('pieDesciplineCounter', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'Activities Counter'
            },
            subtitle: {
                text: 'Source: PCMS Database'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.y}',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        },
                        connectorColor: 'silver'
                    }
                }
            },
            series: [{
                name: 'percentage',
                data: model.values
            }]
        });

        $('.highcharts-credits').css('visibility', 'hidden');
    }

    function spLineDesciplineDateChart(model) {

        Highcharts.chart('splineDesciplineDoneTask', {
            chart: {
                type: 'spline'
            },
            title: {
                text: 'Done Daily Activities vs. Date (Last Mounth)'
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
                    text: 'number Activity'
                },
                min: 0
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.x:%e. %b}: {point.y}'
            },

            plotOptions: {
                spline: {
                    marker: {
                        enabled: true
                    }
                }
            },

            series: model.values
        });

        $('.highcharts-credits').css('visibility', 'hidden');
    }
}());