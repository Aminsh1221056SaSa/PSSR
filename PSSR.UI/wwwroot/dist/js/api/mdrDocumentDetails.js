var mdrDocumentDetails = mdrDocumentDetails || (function () {
    return {
        init: function () {
            initialization();
        },
        getMdrDocumentSummary: function() {
            getMdrDocumentSummary();
        },
        getMDRDetails: function(id) {
            getMDRDetails(id);
        },
        saveMDRComments: function (id, comment, title) {
            saveMDRComments(id, comment, title);
        },
        getCommentDetails: function(id) {
            getCommentDetails(id);
        },
        editMderComment: function (id, title, description, mdrId) {
            editMderComment(id, title, description, mdrId);
        },
        getIssuanceDescription: function (id)
        {
            getIssuanceDescription(id);
        },
        saveMDRIssuance: function (id, description, statusId, isConfirm, type, code) {
            saveMDRIssuance(id, description, statusId, isConfirm, type, code);
        }
    };

    function initialization() {

    }


 function getMdrDocumentSummary(urlAction) {
        var sortOption = $('#OrderByOptions').val();
        var filterType = $('#FilterBy').val();
        var filterVal = $('#filter-value-dropdown').val();
        var container = $('#tb-mdrSummary');

        container.empty();

        $.ajax({
            type: "Get",
            url: "/APSE/MDRDocument/GetListSummary?filterByOption=" + filterType + "&sortByOption=" + sortOption + "&filterValue=" + filterVal,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    createTr(val, urlAction);
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getMDRDetails(id) {
      
     $.ajax({
         type: "Get",
         url: "/APSE/MDRDocument/GetMDRDocumentDetails?id=" + id,
         contentType: 'application/json; charset=utf-8',
         dataType: "json",
         success: function (data, status, jqXHR) {
             initOnSelect(data);
             getMDRStatusHistory(id);
             getMDRComments(id);
           
         },
         error: function (jqXHR, status) {
             console.log(jqXHR);
         }
     });
 }
    function getMDRStatusHistory(id) {
       
        $.ajax({
            type: "Get",
            url: "/APSE/MDRDocument/GetMDRDocumentStatusHistory?id=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization();
                $.each(data, function (i, val) {
                    createTrStatus(val);
                });
                elementInitialization();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getMDRComments(id) {
     
     $.ajax({
         type: "Get",
         url: "/APSE/MDRDocument/GetMDRDocumentComments?id=" + id,
         contentType: 'application/json; charset=utf-8',
         dataType: "json",
         success: function (data, status, jqXHR) {
             releaseInitialization1();
             $.each(data, function (i, val) {
                 createTrComment(val);
             });

             elementInitialization1();
         },
         error: function (jqXHR, status) {
             console.log(jqXHR);
         }
     });
 }

    function getCommentDetails(id) {
        $('#modal-default-overlay').modal('show');
        $.ajax({
            type: "Get",
            url: "/APSE/MDRDocument/GetCommentDetails?commentId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                $('#mdrcomment-title-edit').val(data.title);
                $('#edit-commentId').val(data.id);
                $('#compose-textarea-edit').val(data.description);
                $('#modal-default-editComment').modal('show');
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getIssuanceDescription(id) {
        $.ajax({
            type: "Get",
            url: "/APSE/MDRDocument/GetIssuanceDescription?mdrId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    setIssuanceDetails(data);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getMDRCommentsTimeLine(id) {
        $('.timeline').empty();

        $.ajax({
            type: "Get",
            url: "/APSE/MDRDocument/GetMDRDocumentComments?id=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var fObj = val[0];
                    addTimeLabel(fObj, i);
                    $.each(val, function (j, obj) {
                        initTimeLine(obj);
                    });
                });

                var lastChild = $("<li><i class='fa fa-clock-o bg-gray'></i ></li>");
                $('.timeline').append(lastChild);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    //
    //function saveMDRIssuance(id, description, statusId,isConfirm,type,code) {
    //    var formData = new FormData();
    //    formData.append('file', $('#status-mdr-file')[0].files[0]);

    //    var model = {
    //        'MdrId': id, 'StatusId': statusId, 'Description': description, 'Type': type, 'Code': code, 'IsConfirmContractor': isConfirm
    //    };

    //    $('#modal-default-docIssuance').modal('toggle');
    //    formData.append('jsonString', JSON.stringify(model));
    //    $('#modal-default-overlay').modal('show');

    //    $.ajax({
    //        type: "PUT",
    //        url: "/APSE/MDRDocument/UpdateMDRIssuance",
    //        data: formData,
    //        processData: false,
    //        contentType: false,

    //        success: function (data, status, jqXHR) {
    //            $('#modal-default-overlay').modal('toggle');
    //            if (data.key == 200) {
    //                $('#status-mdr-file').val('');

    //                getMDRDetails(id);
    //            }
    //            else {
    //                $('#global-error-message').html(data.value);
    //                $('#modal-danger').modal('show');
    //            }
    //        },
    //        error: function (jqXHR, status) {
    //            console.log(jqXHR);
    //        }
    //    });
    //}

    function saveMDRComments(id, comment, title) {
        var formData = new FormData();
        formData.append('file', $('#file-comment')[0].files[0]);
        var model = { 'MDRDocumentId': id, 'Description': comment, 'Title': title };
        formData.append('jsonString', JSON.stringify(model));
        $('#modal-default-addComment').modal('toggle');
        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "Post",
            url: "/APSE/MDRDocument/AddMDRComment",
            data: formData,
            processData: false,
            contentType: false,

            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    $('#file-comment').val('');
                    getMDRDetails(id);
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

    function editMderComment(id, title, description,mdrId) {

        var model = {
            'id': id, 'title': title, 'description': description, 'MDRDocumentId': mdrId
        };
        $('#modal-default-editComment').modal('toggle');
        $('#modal-default-overlay').modal('show');
              $.ajax({
                type: "PUT",
                  url: "/APSE/MDRDocument/EditMdrComment",
                data: JSON.stringify(model),
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                  success: function (data, status, jqXHR) {
                      $('#modal-default-overlay').modal('toggle');
                      if (data.key == 200) {
                          getMDRComments(mdrId);
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

    function createTr(model, urlAction) {

        var container = $('#tb-mdrSummary');
        var trContent = $('<tr></tr>');

        trContent.append("<td>" + model.title + "</td>");
        trContent.append("<td>" + model.wbsName + "</td>");
        trContent.append("<td>" + model.lastStatusName + "</td>");
        var $pedit = $("<td><p class='btn btn-danger btn-sm edtit-summary-btn' id='ed-sm-" + model.id + "'><i class='fa fa-edit'></i></p></td>");
        trContent.append($pedit);

        container.append(trContent);
    }

    function createTrComment(model) {

        var container = $('#mdr-CommentTb');
        var trContent = $('<tr></tr>');

        trContent.append("<td>" + model.title + "</td>");
        trContent.append("<td>" + model.isClear + "</td>");
        trContent.append("<td>" + model.createDate + "</td>");
        trContent.append("<td><p class='btn btn-danger mdr-comment-details' data-comId='" + model.id + "'><i class='fa fa-reorder'></i></p></td>");

        var $td = $('<td></td>');

        if (model.hasFilePath != null)
        {
            $td.append("<p class='btn btn-info mdr-comment-file' data-comId='" + model.id + "'><i class='fa fa-file'></i></p>");
        }
        trContent.append($td);
        container.append(trContent);
    }

    function createTrStatus(model) {

        var container = $('#mdr-statusTb');
        var trContent = $('<tr></tr>');

        trContent.append("<td>" + model.createdDate + "</td>");
        trContent.append("<td>" + model.statusName + "</td>");
        if (model.isIFR) {
            trContent.append("<td><h4 class='text-danger'><i class='fa fa-check'></i></h4></td>");
        }
        else {
            trContent.append("<td><h4 class='text-danger'><i class='fa fa-remove'></i></h4></td>");
        }
       
        trContent.append("<td><p class='btn btn-info mdr-status-details' data-path='" + model.filePath + "' data-comId='" + model.id + "'><i class='fa fa-file'></i></p></td>");

        container.append(trContent);
    }

    function initOnSelect(obj) {
        if (obj.isCompleted) {
            $('#doc-issuance').hide();
            $('#doc-comment').hide();
            $('#edit-mdr-comment').hide();
        }
        else {
            $('#doc-issuance').show();
            $('#doc-comment').show();
            $('#edit-mdr-comment').show();
        }

        $('#current-mdr-id').val(obj.id);
        
        $('#current-mdr-title').html(obj.title);
        $('#current-mdr-code').html(obj.code);
        $('#mdr-status').html(obj.lastStatusName);
        $('#current-mdr-createDate').html(obj.createdDate);
        $('#current-mdr-issuance').html(obj.lastIssuanceDate);
        $('#current-mdr-progress').html(obj.progress + " %");
        $('#current-mdr-class').html(getclassTypeString(obj.type));

        //next status modal
        $('#next-status-code').val(obj.code);
        $('#next-status-mdrId').val(obj.id);
        $('#next-status-type').val(obj.type);
    }

    function setIssuanceDetails(data) {

        $('#next-status-id').val(data.nextStatusId);
        $('#mdr-nextstatus-Desc').val(data.nextStatusDescription);

        $('#mdr-status-issuance').html(data.lastStatus);
        $('#mdr-unClear-count').html(data.unclearComment);
        $('#mdr-Comment-lastDate').html(data.lastCommentDate);
        $('#mdr-nextstatus-Desc-desc').html(data.nextStatusDescription);
        
        if (data.type == '2')
        {
            $('#confirm-contractor-doc').hide();
        }
        else
        {
            $('#confirm-contractor-doc').show();
        }

        $('#modal-default-docIssuance').modal('show');
    }

 function addTimeLabel(val,j)
 {
     var container = $('.timeline');
     if (j % 2 == 0) {
         var $litime = $("<li class='time-label'><span class='bg-red'>" + val.createDate + "</span></li>");
     }
     else
     {
         var $litime = $("<li class='time-label'><span class='bg-green'>" + val.createDate + "</span></li>");
     }

     container.append($litime);
 }

 function initTimeLine(comments)
 {
     var container = $('.timeline');
     var $liComment = $("<li></li>");
     var bg = "bg-blue";
     var icon = "";

     if (comments.hStatus == 1001)
     {
         bg = "bg-yellow";
         icon = "fa-bell";
     }
     else if (comments.hStatus == 1002) {
         bg = "bg-purple";
         icon = "fa-bells";
     }
     else if (comments.hStatus == 1003) {
         bg = "bg-green";
         icon = "fa-bullhorn";
     }

     var tItem = $("<i class='fa " + icon + " " + bg + "'></i>");
     $liComment.append(tItem);
     if (comments.description.length > 0)
     {
         $liComment.append("<div class='timeline-item'><h3 class='timeline-header'>MDR Status: " + getStatusTypeString(comments.hStatus) + "</h3><div class='timeline-body'>" + comments.description + "</div></div>");
     }
     
     container.append($liComment);
 }

 function getStatusTypeString(level) {
        var str = "";
        switch (level) {
            case 1001:
                str = 'IFC';
                break;
            case 1002:
                str = 'IFA';
                break;
            case 1003:
                str = 'AFC';
                break;
        }
        return str;
    }

    function getclassTypeString(level) {
        var str = "";
        switch (level) {
            case 1:
                str = 'A';
                break;
            case 2:
                str = 'B';
                break;
        }
        return str;
    }

    function releaseInitialization() {
        var stTable = $("#mdrStatusTable").DataTable();
        stTable.clear().draw();
        stTable.destroy();
    }

    function elementInitialization() {
      $("#mdrStatusTable").DataTable();
    }

    function releaseInitialization1() {
        var commentTable = $("#mdrCommentTable").DataTable();
        commentTable.clear().draw();
        commentTable.destroy();
    }

    function elementInitialization1() {
       $("#mdrCommentTable").DataTable();
    }
}());