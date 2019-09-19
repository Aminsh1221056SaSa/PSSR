var workPackageStepReport = workPackageStepReport || (function () {

    return {
        paintingWorkStepActivityBarStatus: function (workPackageId, locationId, content) {
            paintingWorkStepActivityBarStatus(workPackageId, locationId, content);
        }
    };

    function paintingWorkStepActivityBarStatus(workPackageId, locationId, content) {
        $("#" + content).empty();
        $('#com-chart-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/TaskStatusPreCommByWorkStep?workPackageId=" + workPackageId + "&locationId=" + locationId + "&total=true",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designStatusWorkStepChart(data, content);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function designStatusWorkStepChart(model, content) {
        Highcharts.chart(content, {
            chart: {
                type: 'bar'
            },
            title: {
                text:"",
            },
            subtitle: {
                text: "Commissioning Chart",
               
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
                        _titleKey: 'some_key',
                        onclick: function () {
                            showDetails();
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

    function showDetails() {
        var workPackageId = $('#cu-workSt-id').val();
        var locationId = $('#cu-wst-id').val();
        $('#modal-default-overlay').modal('toggle');
       
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/TaskStatusPreCommByWorkStep?workPackageId=" + workPackageId + "&locationId=" + locationId + "&total=false",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    designStatusDetailsWorkStepChart(data, 'workPackageStepStatusComDetails');
                    $('#modal-default-overlay').modal('toggle');
                    $('#modal-details-Stepchart').modal('show');
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }


    function designStatusDetailsWorkStepChart(model, content) {
        Highcharts.chart(content, {
            chart: {
                type: 'bar'
            },
            title: {
                text: "",
            },
            subtitle: {
                text: "Commissioning Chart",

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
            credits: {
                enabled: false
            },
            series: model.values
        });
    }

}());