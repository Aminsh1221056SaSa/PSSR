var wbsActivityList = wbsActivityList || (function () {
    return {
        init: function () {
        },
        lastwbsChildClick: function(element) {
            lastwbsChildClick(element);
        },
        getProjectPlanActivity: function (workId, locId, subsystemId, deciplineId, formId) {
            getProjectPlanActivity(workId, locId, subsystemId, deciplineId, formId)
        }
    };

    function lastwbsChildClick(element) {
        $('#modal-default-overlay').modal('show');
        var pint = parseInt(element);
        getProjectwbsWfTreeActivity(pint);
    }

    function getProjectwbsWfTreeActivity(lasId) {
        $.ajax({
            type: "Get",
            url: "/APSE/WBS/GetProjectWBSActivityTree?parentId=" + lasId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization();
                $.each(data, function (i, val) {
                    createTr(val, i);
                });
                elementInitialization();
                $('#modal-default-overlay').modal('toggle');
                $('#modal-wbs-actvivityList').modal('show');
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getProjectPlanActivity(workId, locId, subsystemId, deciplineId,formId) {
        $('#modal-default-overlay').modal('toggle');
        $.ajax({
            type: "Get",
            url: "/APSE/Project/GetProjectPlanActivity?workPackageId=" + workId + "&locationId=" + locId + "&subsystemId=" + subsystemId + "&desciplineId=" + deciplineId + "&formId=" + formId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization();
                $.each(data, function (i, val) {
                    createPlanTr(val, i);
                });
                elementInitialization();
                $('#modal-default-overlay').modal('toggle');
                $('#modal-wbs-actvivityPlanList').modal('show');
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createTr(model, count) {
        count = count + 1;
        var tr = $("<tr id='tr-" + model.id + "' data-name='name-" + model.name + "'></tr>");
        tr.append("<td>" + count + "</td>");
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");


        tr.append("<td>" + model.name + "</td>");

        tr.append("<td>" + model.wbsCode + "</td>");
        tr.append("<td>" + model.wf + "</td>");
        tr.append("<td>" + model.wf + "</td>");
        tr.append("<td>" + model.wf + "</td>");

        $('#wbs-acList').append(tr);
    }

    function createPlanTr(model, count) {
        count = count + 1;
        var tr = $("<tr id='tr-" + model.id + "' data-name='name-" + model.activityCode + "'></tr>");
        tr.append("<td>" + count + "</td>");
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");


        tr.append("<td>" + model.activityCode + "</td>");

        tr.append("<td>" + getStatusTypeString(model.status) + "</td>");
        tr.append("<td>" + model.wf + "</td>");
        tr.append("<td>" + model.startDate + "</td>");
        tr.append("<td>" + model.startTime + "</td>");
        tr.append("<td>" + model.endDate + "</td>");
        tr.append("<td>" + model.endTime + "</td>");
        $('#wbs-acList').append(tr);
    }

    function releaseInitialization() {
        var datatablet = $('#wbsCreatorTableList').DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization() {
        var datatablet = $('#wbsCreatorTableList').DataTable({
            dom: '<"row"<"col-sm-12 col-md-1"B><"col-sm-12 col-md-7"l><"col-sm-12 col-md-3  text-left"i>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-12"p>>',
            buttons: [
                {
                    text: '',
                    extend: 'excelHtml5',
                    collectionLayout: 'fixed two-column ',
                    className: 'btn btn-success btn-sm',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        },
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('dt-button');
                        $(node).append('<p class="" style="height:8px"><i style="font-size:16px;color:#FFF;" class="fa fa-file-excel-o"></i><p>');
                    },
                    extension: '.xlsx',
                    filename: "PlanActivityes",
                    messageTop: "Plan Activity",
                    customize: function (xlsx) {
                        var sheet = xlsx.xl.worksheets['sheet1.xml'];
                        var row = 0;

                        $('row', sheet).each(function (x) {

                            if (x > 2) {
                                if (x % 2 == 0) {
                                    $('row:nth-child(' + (x + 1) + ') c', sheet).attr('s', '10');
                                }
                                else {
                                    $('row:nth-child(' + (x + 1) + ') c', sheet).attr('s', '20');
                                }
                            }

                        });

                    }
                }

            ],
        });
    }

    function getStatusTypeString(level) {
        var str = "";
        switch (level) {
            case 1:
                str = 'NotStarted';
                break;
            case 2:
                str = 'Ongoing';
                break;
            case 3:
                str = 'Done';
                break;
            case 4:
                str = 'Reject';
                break;
        }
        return str;
    }
}());