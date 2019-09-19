
var activitySummary = activitySummary || (function () {
    return {
        init: function () {
            initialization();
        },
        getActivityList: function (tbHeight) {
            getActivityList(tbHeight);
        },
        getActivitySummary: function() {
            getActivitySummary();
        },
        getActivitySummaryFilterByDescipline: function (workId,desId) {
            getActivitySummaryFilterByDescipline(workId,desId);
        },
        getActivityDetails: function(id) {
            getActivityDetails(id);
        },
        initDropDownonStatus: function(status,condition) {
            initDropDownonStatus(status,condition);
        }
    };
   
    function initialization()
    {
     //
    }

    function getActivitySummary() {
        var sortOption = $('#OrderByOptions').val();
        var filterType = $('#FilterBy').val();
        var filterVal = $('#filter-value-dropdown').val();

        var pgNum = 1;
        if ($('#pg-num').val()) {
            pgNum = $('#pg-num').val();
        }

        var pgSize = 10;
        if ($('#pg-size-dropdown').val()) {
            pgSize=  $('#pg-size-dropdown').val();
        }
        var container = $('#tb-acSummary');
        var prevCheckState = $('#prevCheckState').val();
        var query = $('#queryFilter').val();
        container.empty();
        $.ajax({
            type: "Get",
            url: "/poec/activity/ActivityListSummary?filterByOption=" + filterType + "&sortByOption=" + sortOption + "&filterValue=" + filterVal
                + "&page=" + pgNum + "&pageSize=" + pgSize + "&search=" + query + "&prevCheckState=" + prevCheckState,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data.items, function (i, val) {
                    createTr(val, container);
                    });

                setOption(data.option);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getActivitySummaryFilterByDescipline(workId, desId) {

        var sortOption = $('#OrderByOptions').val();
        var filterType = $('#FilterBy').val();
        var filterVal = $('#filter-value-dropdown').val();

        var pgNum = 1;
        if ($('#pg-num').val()) {
            pgNum = $('#pg-num').val();
        }

        var pgSize = 10;
        if ($('#pg-size-dropdown').val()) {
            pgSize = $('#pg-size-dropdown').val();
        }
        var container = $('#tb-acSummary');
        var prevCheckState = $('#prevCheckState').val();
        var query = $('#queryFilter').val();
       
        container.empty();
        $.ajax({
            type: "Get",
            url: "/poec/activity/ActivityListSummaryByWorkDescipline?workId=" + workId+"&desId=" + desId + "&filterByOption=" + filterType + "&sortByOption=" + sortOption + "&filterValue=" + filterVal
                + "&pageNum=" + pgNum + "&pageSize=" + pgSize + "&query=" + query + "&prevCheckState=" + prevCheckState,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data.items, function (i, val) {
                    createTrVr1(val);
                });

                setOption(data.option);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function setOption(model) {
        $('#pg-num').val(model.pageNum);
        $('#current-page').text("of " + model.numPages + "");

        $("#pg-size-dropdown").empty();
        $.each(model.pageSizes, function (i, val) {
            $("#pg-size-dropdown").append("<option value=" + val + ">" + val + "</option>");
        });
        $("#pg-size-dropdown").val(model.pageSize);
        $('#prevCheckState').val(model.prevCheckState);
    }

    function createTr(model, container)
    {
        var trContent = $('<tr></tr>');

        trContent.append("<td>" + model.tagNumber + "</td>");
        trContent.append("<td>" + getStatusTypeString(model.status) + "</td>");
        var $ptd = $("<td>" + model.weightFactor + "</td>");

        trContent.append($ptd);

        var $pedit = $("<td><p class='btn btn-warning btn-sm edtit-summary-btn' id='ed-sm-" + model.id + "'><i class='fa fa-edit'></i></p></td>");

        trContent.append($pedit);

        container.append(trContent);
    }


    function createTrVr1(model) {
        var container = $('#tb-acSummary');
        var trContent = $('<tr></tr>');

        trContent.append("<td>" + model.activityCode + "</td>");
        trContent.append("<td>" + model.tagNumber + "</td>");
        trContent.append("<td>" + getStatusTypeString(model.status) + "</td>");
        trContent.append("<td>" + model.planStartDate + "</td>");
        trContent.append("<td>" + model.planEndDate + "</td>");
        container.append(trContent);
    }

    function getActivityDetails(id) {

        $.ajax({
            type: "Get",
            url: "/poec/activity/GetActivityDetails?id=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                iniCurrentState(data);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function iniCurrentState(model) {

        $('#ac-st-prPr').removeClass();

        var pclass = "progress-bar-red";
        if (model.progress <= 25) {
            pclass = "progress-bar-red";
        }
        else if (model.progress > 25 && model.progress <= 65) {
            pclass = "progress-bar-yellow";
        }
        else if (model.progress > 65 && model.progress <= 95) {
            pclass = "progress-bar-aqua";
        }
        else if (model.progress > 95) {
            pclass = "progress-bar-green";
        }

        $('#ac-st-id').val(model.id);
        $('#ac-st-progress').val(model.progress);

        $('#ac-st-prPr').addClass('progress-bar').addClass(pclass).css('width', model.progress + "%");
        $('#ac-st-prtx').html(model.progress + " %");
        $('#cr-st-ac').html(getStatusTypeString(model.status));
        $('#cr-st-co').html(getConditionString(model.condition));

        $('#wo-st').html(model.workPackageName);
        $('#lo-st').html(model.locationName);
        $('#ds-st').html(model.desciplineName);
        $('#sy-st').html(model.systemName);
        $('#sb-st').html(model.subSystemName);

        $('#fr-co').html(model.formCode);
        $('#fr-ty').html(model.formType);
        $('#fr-dsc').html(model.formDescription);
        $('#wo-st-step').html(model.workPackageStepName);

        $('#selected-activity-c').html('Activity Code : ' + model.activityCode);

        if (model.status == 4) {
            $('#edit-ac-st').hide();
            $('#add-punch-show').hide();
        }
        else {
            $('#edit-ac-st').show();
            $('#add-punch-show').show();
        }
        initDropDownonStatus(model.status, model.condition);
    }


    function initDropDownonStatus(status, condition) {

        if (condition == 3002) {
            $('#ac-holdBy').prop('disabled', false);
        }
        else if (condition == 3000
            || condition == 3003) {
            $('#ac-holdBy').prop('disabled', true);
        }


        if (status == 1) {
            $('#ac-conditon').prop('disabled', false);
        }
        else if (status == 2) {
            $('#ac-conditon').prop('disabled', false);
        }
        else if (status == 3
            || status == 4) {
            $('#ac-conditon').prop('disabled', true);
            $('#ac-holdBy').prop('disabled', true);
        }

        $("#ac-conditon").val(condition);
        $("#ac-status").val(status);
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

    function getConditionString(level) {
        var str = "";
        switch (level) {
            case 3000:
                str = 'Normal';
                break;
            case 3002:
                str = 'Hold';
                break;
            case 3003:
                str = 'Front';
                break;
        }
        return str;
    }

}());