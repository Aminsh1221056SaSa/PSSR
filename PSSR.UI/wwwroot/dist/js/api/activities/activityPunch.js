var activityPunch = activityPunch || (function () {

    return {
        init: function (activityId) {
        },
        getallPunches: function() {
            getallPunches();
        },
        getallPunchCategory: function () {
            getallPunchCategory();
        },
        getActivityPunchs: function(acId) {
            getActivityPunchs(acId);
        },
        savePunch: function(id,objsummary) {
            addPunch(id, objsummary);
        },
        deletePunch: function(id, acId, objsummary) {
            deletePunch(id, acId, objsummary);
        },
        getpunchDetails: function(id)
        {
            getPunchDetails(id);
        },
        editPunch: function(id, acid) {
            editPunch(id,acid);
        },
        showGoPunchModal: function(id) {
            showGoPunchDetails(id);
        },
        editPUnchGo: function(id, acid, objsummary) {
            editPUnchGo(id, acid, objsummary);
        },
        getPunchDocuments: function (punchId) {
            getPunchDocuments(punchId);
        }
    };

    function getallPunches() {
        $.ajax({
            type: "Get",
            url: "/poec/PunchType/GetAllPunchTypes",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    $("#punch-type-dropdown").append("<option value=" + val.id + ">" + val.name + "</option>");
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getallPunchCategory() {
        $.ajax({
            type: "Get",
            url: "/poec/PunchCategory/GetPunchCategoryes",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    $("#punch-category-dropdown").append("<option value=" + val.id + ">" + val.name + "</option>");
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getActivityPunchs(activityId) {
        $.ajax({
            type: "Get",
            url: "/poec/activityPunch/GetActivityPunchs?activityId=" + activityId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization();
                $.each(data, function (i, val) {
                    createpunchTr(val, i);
                });
                elementInitialization();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function showGoPunchDetails(id) {

        $.ajax({
            type: "Get",
            url: "/poec/activityPunch/GetPunchGoDetails?punchId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                setgoPunchDetails(data);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getPunchDetails(id)
    {
        $.ajax({
            type: "Get",
            url: "/poec/activityPunch/GetPunchDetails?punchId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                setPunchDetails(data);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getPunchDocuments(punchId) {
        $.ajax({
            type: "Get",
            url: "/poec/ActivityPunch/GetPunchDocuments?punchId=" + punchId,
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

    function deletePunch(id, acId, objsummary) {
        $('#modal-default-overlay').modal('show');
        $.ajax({
            type: "Delete",
            url: "/poec/activityPunch/DeletePunch/" + id + "",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    getActivityPunchs(acId);
                    objsummary.getActivityDetails(acId);
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

    function addPunch(acId, objsummary) {
        var typeId = $('#punch-type-dropdown').val();
        var catId = $('#punch-category-dropdown').val();
     
        var code = $('#pCode').val();
        var ogBy = $('#pOrginated').val();
        var pCreatedBy = $('#pCreatedBy').val();
        var pCheckBy = $('#pCheckBy').val();
        var pApproveBy = $('#pApproveBy').val();

        var pEstimateMh = $('#pEstimateMh').val();
        var pActualMh = $('#pActualMh').val();
        var pClearPlan = $('#pClearPlan').val();
        var pOrginatedDate = $('#pOrginatedDate').val();
        var pCheckDate = $('#pCheckDate').val();

        var pClearDate = $('#pClearDate').val();
        var pVendorName = $('#pVendorName').val();
        var pDefect = $('#pDefect').val();

        var vendorRequired = $('#pVendor').iCheck('update')[0].checked;
        var materialRequired = $('#pMaterial').iCheck('update')[0].checked;
        var enginerigRequired = $('#pEnginerig').iCheck('update')[0].checked;

        var model = {
            'Code': code, 'OrginatedBy': ogBy, 'CreatedBy': pCreatedBy,
            'CheckBy': pCheckBy, 'ApproveBy': pApproveBy, 'EstimateMh': pEstimateMh,
            'ActualMh': pActualMh, 'ClearPlan': pClearPlan, 'OrginatedDate': pOrginatedDate,
            'CheckDate': pCheckDate, 'ClearDate': pClearDate, 'VendorName': pVendorName,
            'DefectDescription': pDefect, 'activityId': acId, 'PunchTypeId': typeId, "VendorRequired": vendorRequired,
            "MaterialRequired": materialRequired, "EnginerigRequired": enginerigRequired, "CategoryId": catId
        };

        $('#modal-default-Addpunch').modal('toggle');
        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "POST",
            url: "/poec/activityPunch/AddPUnchToActivity",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    getActivityPunchs(acId);
                    objsummary.getActivityDetails(acId);
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

    function editPunch(id,acid) {

        var code = $('#pCode').val();
        var ogBy = $('#pOrginated').val();
        var pCreatedBy = $('#pCreatedBy').val();
        var pCheckBy = $('#pCheckBy').val();
        var pApproveBy = $('#pApproveBy').val();

        var pEstimateMh = $('#pEstimateMh').val();
        var pActualMh = $('#pActualMh').val();
        var pClearPlan = $('#pClearPlan').val();
        var pOrginatedDate = $('#pOrginatedDate').val();
        var pCheckDate = $('#pCheckDate').val();

        var pClearDate = $('#pClearDate').val();
        var pVendorName = $('#pVendorName').val();
        var pDefect = $('#pDefect').val();

        var vendorRequired = $('#pVendor').iCheck('update')[0].checked;
        var materialRequired = $('#pMaterial').iCheck('update')[0].checked;
        var enginerigRequired = $('#pEnginerig').iCheck('update')[0].checked;

        var model = {"Id":id,
            'Code': code, 'OrginatedBy': ogBy, 'CreatedBy': pCreatedBy,
            'CheckBy': pCheckBy, 'ApproveBy': pApproveBy, 'EstimateMh': pEstimateMh,
            'ActualMh': pActualMh, 'ClearPlan': pClearPlan, 'OrginatedDate': pOrginatedDate,
            'CheckDate': pCheckDate, 'ClearDate': pClearDate, 'VendorName': pVendorName,
            'DefectDescription': pDefect, "VendorRequired": vendorRequired,
            "MaterialRequired": materialRequired, "EnginerigRequired": enginerigRequired
        };

        $('#modal-default-Addpunch').modal('toggle');
        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "PUT",
            url: "/poec/activityPunch/UpdatePUnchToActivity",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    if (acid <= 0) {
                        getPunchDetails(id);
                    }
                    else {
                        getActivityPunchs(acid);
                    }
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

    function editPUnchGo(id, acid, objsummary) {
        var approveBy = $('#paprovBy').val();
        var checkBy = $('#pCheckBy').val();
        var clearBy = $('#pClearBy').val();
        var checkDate = $('#pCheckDate').val();
        var clearDate = $('#pClearDate').val();

        var model = {
            "Id": id, "ApproveBy": approveBy, "CheckBy": checkBy
            , "ClearBy": clearBy, "CheckDate": checkDate, "ClearDate": clearDate
        };

        $('#modal-default-punchModify').modal('toggle');
        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "PUT",
            url: "/poec/activityPunch/UpdatePUnchGo",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    if (objsummary == null) {
                        getPunchDetails(id);
                        showGoPunchDetails(id);
                    }
                    else {
                        getActivityPunchs(acid);
                        objsummary.getActivityDetails(acid);
                    }
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

    function createpunchTr(model, count) {
        var tr = $("<tr id='tr-" + model.id + "'></tr>");
        tr.append("<td>" + model.code + "</td>");
        tr.append("<td>" + model.type + "</td>");
        tr.append("<td>" + model.orginatedBy + "</td>");
        tr.append("<td>" + model.orginatedDate + "</td>");

        var $pedit = $("<td></td>");
        var $pdelete = $("<td></td>");

        $pedit.append("<p class='btn btn-info edtit-punch-btn btn-sm' id='ed-sm-" + model.id + "'><i class='fa fa-edit'></i></p>");
        if (!model.isEditable) {
            $pdelete.append("<p class='btn btn-danger delete-punch-btn btn-sm' id='dl-sm-" + model.id + "'><i class='fa fa-remove'></i></p>");
        }
        else {
            $('#punch-edit-btns-pane').hide();
        }

        if (model.isApprove) {

            var $pgo = $("<td><p class='btn btn-success go-punch-btn btn-sm' id='go-sm-" + model.id + "'><i class='fa fa-check'></i></p></td>");
        }
        else {
            var $pgo = $("<td><p class='btn btn-warning go-punch-btn btn-sm' id='go-sm-" + model.id + "'><i class='fa fa-arrow-right'></i></p></td>");
        }

        tr.append($pedit);
        tr.append($pdelete);
        tr.append($pgo);

        $('#punch-activity').append(tr);
    }

    function createDocumentTr(model, count) {
        count = count + 1;
        var tr = $("<tr id='tr-" + model.id + "'></tr>");
        tr.append("<td>" + count + "</td>");
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");

        tr.append("<td>" + model.createdDate + "</td>");
        tr.append("<td>" + model.description + "</td>");
        tr.append("<td><p class='btn btn-info btn-sm activity-doc-class' data-docId='" + model.id + "'><i class='fa fa-file'></i></p></td>");

        $('#document-punch').append(tr);
    }

    function setPunchDetails(data)
    {
        $('#pu-st-id').val(data.id);
        $('#ac-st-id').val(data.activityId);
        $('#punch-type-dropdown').val(data.punchTypeId);
        $('#pCode').val(data.code);
        $('#pOrginated').val(data.orginatedBy);
        $('#pCreatedBy').val(data.createdBy);
        $('#pCheckBy').val(data.checkBy);
        $('#pApproveBy').val(data.approveBy);

        $('#pEstimateMh').val(data.estimateMh);
        $('#pActualMh').val(data.actualMh);
        $('#pClearPlan').val(data.clearPlan);
        $('#pOrginatedDate').val(data.orginatedDate);
        $('#pCheckDate').val(data.checkDate);

        $('#pClearDate').val(data.clearDate);
        $('#pVendorName').val(data.vendorName);
        $('#pDefect').val(data.defectDescription);

        $('#punch-type-dropdown').attr('disabled', true);
        $('#save-punch').css('display', 'none');
        $('#edit-punch').css('display', 'block');
        $('#edit-punch').attr('data-editid', data.id);

        $('#pVendor').iCheck('uncheck');
        $('#pMaterial').iCheck('uncheck');
        $('#pEnginerig').iCheck('uncheck');

        if (data.vendorRequired == true) {
            $('#pVendor').iCheck('check');
        }

        if (data.materialRequired == true) {
            $('#pMaterial').iCheck('check');
        }

        if (data.enginerigRequired == true) {
            $('#pEnginerig').iCheck('check');
        }

        var pclass = "progress-bar-red";
        if (data.progress <= 25) {
            pclass = "progress-bar-red";
        }
        else if (data.progress > 25 && data.progress <= 65) {
            pclass = "progress-bar-yellow";
        }
        else if (data.progress > 65 && data.progress <= 95) {
            pclass = "progress-bar-aqua";
        }
        else if (data.progress > 95) {
            pclass = "progress-bar-green";
        }
        $('#ac-st-prPr').removeClass();
        $('#selected-punch-c').html('Punch Code : ' + data.code);
        $('#ac-st-prPr').addClass('progress-bar').addClass(pclass).css('width', data.progress + "%");
        $('#ac-st-prtx').html(data.progress + " %");
        $('#cr-st-co').html(getConditionString(data.condition));
        $('#cr-co-ac').html(data.activityCode);
        if (data.isEditable) {
            $('#editable-punch').hide();
            $('#editable-go-punch').hide();
        }
        else {
            $('#editable-punch').show();
            $('#editable-go-punch').show();
        }
    }

    function setgoPunchDetails(data) {

        $('#pClearBy').val(data.createdBy);
        $('#pClearDate').val(data.clearDate);

        $('#paprovBy').val(data.approveBy);
        $('#pCheckBy').val(data.checkBy);
        $('#pCheckDate').val(data.checkDate);

        if (data.isApprove == true && data.isClear == true)
        {
            $('#pClearBy').attr('disabled', true);
            $('#pClearDate').attr('disabled', true);

            $('#paprovBy').attr('disabled', true);
            $('#pCheckBy').attr('disabled', true);
            $('#pCheckDate').attr('disabled', true);
        }
        else if (data.isApprove == false && data.isClear == true)
        {
            
            $('#pClearBy').attr('disabled', true);
            $('#pClearDate').attr('disabled', true);

            $('#paprovBy').attr('disabled', false);
            $('#pCheckBy').attr('disabled', false);
            $('#pCheckDate').attr('disabled', false);
        }
        else if (data.isApprove == false && data.isClear == false)
        {
            $('#pClearBy').attr('disabled', false);
            $('#pClearDate').attr('disabled', false);

            $('#paprovBy').attr('disabled', true);
            $('#pCheckBy').attr('disabled', true);
            $('#pCheckDate').attr('disabled', true);
        }

        $('#save-go-punch').attr('data-gopunch-id', data.id);

        $('#modal-default-punchModify').modal('show');
    }

    function releaseInitialization() {
        var datatablet = $('#activitypunchTable').DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization() {
        var datatablet = $('#activitypunchTable').DataTable({
            "columnDefs": [
                { "orderable": false, "targets": [4, 5, 6] }
            ]
        });
    }

    function releaseInitialization1(content) {
        var datatablet = $("#punchDocTable").DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization1() {
        var datatablet = $("#punchDocTable").DataTable();
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