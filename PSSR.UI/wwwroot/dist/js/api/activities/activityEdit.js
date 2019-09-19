var activityEdit = (function ($) {

    return {
        init: function (activityId) {
            initialization(activityId);
            getActivityStatus(activityId);
            getActivityDocuments(activityId);
        },
        UpdateActivityStatus: function (id, acSummary) {
            UpdateActivityStatus(id, acSummary);
        }
    };

    function initialization(activityId) {
        $.ajax({
            type: "Get",
            url: "/poec/Activity/GetActivityWBSTree?activityWBsId=" + activityId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    if (val.parentId == null)
                    {
                        createProjectNode(val.name, val.id, val.wbsCode);
                    }
                    else
                    {
                        var objParent = $("#node-" + val.parentId + "");
                        createTrWBSModel(val.name, val.id, val.wbsCode, objParent);
                    }
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createProjectNode(name, id, wbsCode) {
        name = name.split('(')[0];
        var targetId = "node-" + id;
        var $li = $('#first-li-wbs');
        var $a = $("<a id='" + targetId + "' data-wbsCode='" + wbsCode + "' class='drop-wbs'><p  style='width:60px;height:10px'>" + name + "</p><p class='html-wbsCode' style='width:60px;height:10px'>" + wbsCode + "</p></a>");
        var $ul = $(' <ul id="dr-ul-1001"></ul>');
        $li.append($a);
        $ul.append($ul);
        
    }

    function createTrWBSModel(name, id, wbsCode, dropItem) {
        name = name.split('(')[0];
        var targetId = "node-" + id;

        var newParent = $(dropItem).parent();

        if (newParent.children('ul').length) {
            var parentUl = newParent.children('ul').first();

            var $li = $("<li></li>");
            var $a = $("<a id='" + targetId + "' data-wbsCode='" + wbsCode + "' class='drop-wbs'><p style='width:auto;height:10px'>" + name + "</p><p class='html-wbsCode' style='width:60px;height:10px'>" + wbsCode + "</p></a>");
            $li.append($a);
            parentUl.append($li);
        }
        else {
            var $ul = $("<ul></ul>");
            var $li = $("<li style='margin-left:-45px;'></li>");
            var $a = $("<a id='" + targetId + "' class='drop-wbs' data-wbsCode='" + wbsCode + "'><p style='width:auto;height:10px'>" + name + "</p><p class='html-wbsCode' style='width:60px;height:10px'>" + wbsCode + "</p></a>");
            $li.append($a);
            $ul.append($li);

            newParent.append($ul);
        }
    }

    function UpdateActivityStatus(id,acSummary) {
        
        var progress = $('#ac-st-progress').val();
        var status = $('#ac-status').val();
        var condition = $('#ac-conditon').val();
        var holdBy = $('#ac-holdBy').val();
        var description = $('#ac-st-description').val();

        var model = { 'Id': id, 'Status': status, 'HoldBy': holdBy, 'Progress': progress, 'Condition': condition };

        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "PUT",
            url: "/poec/activity/UpdateActivityStatus",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    initialization(id);
                    getActivityStatus(id);

                    acSummary.getActivitySummary();
                    acSummary.getActivityDetails(id);

                    $('#modal-default-overlay').modal('toggle');
                }
                else {
                    $('#global-error-message').html(data.value);
                    $('#modal-default-overlay').modal('toggle');
                    $('#modal-danger').modal('show');
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getActivityStatus(activityId) {
        $.ajax({
            type: "Get",
            url: "/poec/activity/GetActivityStatusHistory?activityId=" + activityId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization('activityStatusHisTable');
                $.each(data, function (i, val) {
                    createTr(val, i);
                });
                elementInitialization('activityStatusHisTable');
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getActivityDocuments(activityId) {
        $.ajax({
            type: "Get",
            url: "/poec/activity/GetActivityDocuments?activityId=" + activityId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization1();
                $.each(data, function (i, val) {
                    createDocumentTr(val, i);
                });
                elementInitialization1();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function activityDocumentManipulation(activityId) {
        var model = { 'Id': id, 'Status': status, 'HoldBy': holdBy, 'Progress': progress, 'Condition': condition };
        $.ajax({
            type: "PUT",
            url: "/poec/activity/CreateMainDocumentFile",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    initialization(id);
                    getActivityStatus(id);

                    acSummary.getActivitySummary();
                    acSummary.getActivityDetails(id);

                    $('#modal-default-overlay').modal('toggle');
                }
                else {
                    $('#global-error-message').html(data.value);
                    $('#modal-default-overlay').modal('toggle');
                    $('#modal-danger').modal('show');
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createTr(model, count)
    {
        count = count + 1;
        var tr = $("<tr id='tr-" + model.id + "'></tr>");
        tr.append("<td>" + count + "</td>");
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");

        tr.append("<td>" + model.description + "</td>");
        tr.append("<td>" + getHoldByTypeString(model.holdBy) + "</td>");
        tr.append("<td>" + model.createDate + "</td>");
        
        $('#activity-packages').append(tr);
    }

    function createDocumentTr(model, count) {
        count = count + 1;
        var tr = $("<tr id='tr-" + model.id + "'></tr>");
        tr.append("<td>" + count + "</td>");
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");

        tr.append("<td>" + model.createdDate + "</td>");
        tr.append("<td>" + model.description + "</td>");
        tr.append("<td>" + model.punchCode + "</td>");
        tr.append("<td><p class='btn btn-info activity-doc-class' data-docId='" + model.id + "'><i class='fa fa-file'></i></p></td>");

        $('#document-activity').append(tr);
    }

  function releaseInitialization(content) {
      var datatablet = $("#activityStatusHisTable").DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

  function elementInitialization() {
      var datatablet = $("#activityStatusHisTable").DataTable();
  }

    function releaseInitialization1(content) {
        var datatablet = $("#activityDocTable").DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization1() {
        var datatablet = $("#activityDocTable").DataTable();
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

    function getHoldByTypeString(level) {
        var str = "";
        switch (level) {
            case 0:
                str = 'Normal';
                break;
            case 1:
                str = 'HoldMaterial';
                break;
            case 2:
                str = 'HoldSequence';
                break;
            case 3:
                str = 'HoldOther';
                break;
        }
        return str;
    }
}(window.jQuery));