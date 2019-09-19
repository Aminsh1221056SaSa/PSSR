
var desciplinePlane = desciplinePlane || (function () {

    return {
        init: function () {
            getWorkPackages();
            getLocations();
            getProjectSystems();
            getProjectSubSystems(0);
        },
        getDesciplineGroup: function (subSystemId, systemId, locId, workId) {
            getDesciplineGroup(subSystemId, systemId, locId, workId);
        },
        getActivityGroupedByPlanDate: function (workId, locationId, subSystemId, desId) {
            getActivityGroupedByPlanDate(workId, locationId, subSystemId, desId);
        },
        SetDesciplinePlane: function (workId,locationId,subSystemId, desId, startDate, endDate) {
            SetDesciplinePlane(workId, locationId, subSystemId, desId, startDate, endDate);
        },
        getProjectSubSystems: function (systemId) {
            getProjectSubSystems(systemId);
        }
    };

    function getWorkPackages() {

        var content = $('#workPackage-type');
        content.empty();

        $.ajax({
            type: "Get",
            url: "/poec/WorkPackage/GetRoadMaps",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    content.append("<option value=" + val.id + ">" + val.title + "</option>");
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }


    function getLocations() {
        var content = $('#location-type');
        content.empty();

        $.ajax({
            type: "Get",
            url: "/poec/WorkPackage/GetLocations",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    content.append("<option value=" + val.id + ">" + val.title + "</option>");
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getProjectSystems() {
        var content = $('#system-type');
        content.empty();
        content.append("<option value=0>...Select system...</option>");

        $.ajax({
            type: "Get",
            url: "/poec/ProjectSystem/GetProjectSystems",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    content.append("<option value=" + val.id + ">" + val.title + "</option>");
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getProjectSubSystems(systemId) {

        var content = $('#subSystem-type');
        content.empty();
        content.append("<option value=0>...Select SubSystem...</option>");

        $.ajax({
            type: "Get",
            url: "/poec/SubSystem/GetSubSystemBySystem?systemId=" + systemId+"",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    content.append("<option value=" + val.id + ">" + val.code + "</option>");
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    //
    function getDesciplineGroup(subSystemId, systemId, locId, workId)
    {
        $.ajax({
            type: "Get",
            url: "/poec/Project/GetDesciplineActivityGroupList?workPackageId=" + workId + "&locationId=" + locId + "&systemId=" + systemId + "&subsystemId=" + subSystemId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization();
                $.each(data, function (i, val) {
                    createTr(val, i);
                });
                elementInitialization();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getActivityGroupedByPlanDate(workId,locationId,subSystemId,desId) {

        $.ajax({
            type: "Get",
            url: "/poec/Project/GetFormDictionaryGroupedByPlanDate?workId=" + workId + "&locationId=" + locationId + "&subSystemId=" + subSystemId + "&desId=" + desId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization1();
                $.each(data, function (i, val) {
                    createTr1(val, i);
                });
                elementInitialization1();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function SetDesciplinePlane(workId, locationId, subSystemId, desId, startDate, endDate) {

        var model = {
            'StartDate': startDate, 'EndDate': endDate, 'DesciplineId': desId,
            'WorkPackageId': workId, 'SubSystemId': subSystemId, 'LocationId': locationId
        };

        $('#modal-default-overlay').modal('show');
        $.ajax({
            type: "PUT",
            url: "/poec/Activity/UpdateActivityPlane",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    getDesciplineGroup(subSystemId, 0, locationId, workId);
                    getActivityGroupedByPlanDate(workId, locationId, subSystemId, desId);
                }
                else {
                    $('#global-error-message').html(data.value);
                    $('#modal-danger').modal('show');
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createTr(model)
    {
        var tName = model.title;

        var tr = $("<tr id='tr-" + model.id + "' ></tr>");
        tr.append("<td>" + tName + "</td>");
        var tdTotal = $("<td>" + model.total + "</td>");
        var tdDone = $("<td>" + model.done + "</td>");

        tr.append(tdTotal);
        tr.append(tdDone);
        tr.append("<td>" + model.startDate + "</td>");
        tr.append("<td>" + model.endDate + "</td>");
        tr.append("<td><p class='btn btn-warning plan-edit-class'  data-name='" + tName + "'  id='des-it-" + model.id + "'>Plan</p></td>");

        $('#model-items').append(tr);
    }

    function createTr1(model) {
        var tName = model.title;

        var tr = $("<tr id='tr-" + model.id + "' ></tr>");
        var tdresources = $("<td>" + model.title + "</td>");
        var tdTotal = $("<td>" + model.total + "</td>");
        var tdDone = $("<td>" + model.done + "</td>");

        tr.append(tdresources);
        tr.append(tdTotal);
        tr.append(tdDone);
        tr.append("<td>" + model.startDate + "</td>");
        tr.append("<td>" + model.startTime + "</td>");
        tr.append("<td>" + model.endDate + "</td>");
        tr.append("<td>" + model.endTime + "</td>");
        tr.append("<td><p title='Activities' class='btn btn-success descipline-plan-class'  id='plan-des-" + model.id + "'><i  class='fa fa-reorder'></i></p></td>");

        $('#model-items1').append(tr);
    }

    function releaseInitialization() {
        var datatablet = $('#wbsCreatorTable').DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization() {

        var datatablet = $('#wbsCreatorTable').DataTable();
    }

    function releaseInitialization1() {
        var datatablet = $('#activityGroupPlane').DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization1() {
        var tbh = window.innerHeight * 0.44;
        var datatablet = $('#activityGroupPlane').DataTable(
            {
                scrollY: tbh + "px",
               
                fixedHeader: true,
                dom: '<"row"<"col-sm-12 col-md-1"B><"col-sm-12 col-md-7"l><"col-sm-12 col-md-3  text-left"i>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-12"p>>',
                "columnDefs": [
                    { "orderable": true, "targets": [3,5] },
                    {"sType": "date", "targets": [3,5]  },
                ],
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
                        filename: "PlanConfiguration",
                        messageTop: "Plan",
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
}());

