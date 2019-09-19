var projectSystemReport = projectSystemReport || (function () {

    return {
        paintingSystemActivityBarStatus: function() {
            paintingSystemActivityBarStatus();
        },
        paintingSystemActivityBarCondition: function() {
            paintingSystemActivityBarCondition();
        },
        getSystemCounter: function() {
            getSystemCounter();
        },
        getSystemDoneActivity: function() {
            getSystemDoneActivity();
        }
    };

    function paintingSystemActivityBarStatus() {

        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/TaskStatusBySystem",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designStatusSystemChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function paintingSystemActivityBarCondition() {

        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/TaskConditionBySystem",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designConditionSystemChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getSystemCounter() {

        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/TaskCounterBySystem",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    paintSystemCounterChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getSystemDoneActivity() {
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/TaskDoneBySystem",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    spLineSystemDateChart(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function designStatusSystemChart(model) {
        Highcharts.chart('SystemStatus', {
            chart: {
                type: 'bar'
            },
            title: {
                text: 'Activity Status By System'
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
                        enabled: true
                    }
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 80,
                floating: true,
                borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            credits: {
                enabled: false
            },
            series: model.values
        });
    }

    function designConditionSystemChart(model) {
        Highcharts.chart('SystemCondition', {
            chart: {
                type: 'bar'
            },
            title: {
                text: 'Activity Condition By System'
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
                        enabled: true
                    }
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 80,
                floating: true,
                borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            credits: {
                enabled: false
            },
            series: model.values
        });
    }

    function paintSystemCounterChart(model) {
        // Build the chart
        Highcharts.chart('pieSystemCounter', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'Activity Counter By System'
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

    function spLineSystemDateChart(model) {

        Highcharts.chart('splineSystemDoneTask', {
            chart: {
                type: 'spline'
            },
            title: {
                text: 'Done Activity Per Day For Last month By System'
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