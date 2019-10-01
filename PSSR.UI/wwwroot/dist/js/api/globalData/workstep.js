
var workstep = workstep || (function () {

    return {
        init: function () {
            initialization();
        },
        getWorkstepList: function () {
            getWorkstepList()
        },
        getWorkstep: function (id) {
            return getWorkstep(id);
        },
        setCurrentIndex: function (index) {
            setCurrentIndex(index);
        },
        removeWorkstep: function (id) {
            removeWorkstep(id);
        }
    };

    var _table = null;
    var _cIndex = 0;
    function setCurrentIndex(index) {
        _cIndex = index;
    }

    function initialization() {
        if (_table) {
            _table.clear().draw();
            _table.destroy();
        }

        $("form[name='workstepForm']").validate({
            // Specify validation rules
            rules: {
                workstepname: "required",
                workpackageId: "required",
            },
            messages: {
                workstepname: "Please enter Workstep Name",
                workpackageId: "Please select a WorkPackage"
            },
            errorElement: "em",
            errorPlacement: function (error, element) {
                // Add the `invalid-feedback` class to the error element
                error.addClass("invalid-feedback");

                if (element.prop("type") === "checkbox") {
                    error.insertAfter(element.next("label"));
                } else {
                    error.insertAfter(element);
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            // Make sure the form is submitted to the destination defined
            // in the "action" attribute of the form when valid
            submitHandler: function (form) {
                var name = $('input#workstepname').val();
                var description = $('input#workstepDescription').val();
                var workpackageId = $('select#workpackageId').val();
                createworkstep(name, description, workpackageId);
            }
        });

        $("form[name='workstepEditForm']").validate({
            // Specify validation rules
            rules: {
                workstepname: "required",
                workpackageId: "required",
            },
            messages: {
                workstepname: "Please enter Workstep Name",
                workpackageId: "Please select a WorkPackage"
            },
            errorElement: "em",
            errorPlacement: function (error, element) {
                // Add the `invalid-feedback` class to the error element
                error.addClass("invalid-feedback");

                if (element.prop("type") === "checkbox") {
                    error.insertAfter(element.next("label"));
                } else {
                    error.insertAfter(element);
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            // Make sure the form is submitted to the destination defined
            // in the "action" attribute of the form when valid
            submitHandler: function (form) {
                var name = $('input#editworkstepname').val();
                var description = $('input#editworkstepDescription').val();
                var workpackageId = $('select#editworkpackageId').val();
                var id = $('#current-workstepId').val();
                editworkstep(name, description, workpackageId, id);
            }
        });
    }

    function getWorkstepList() {
        var content = $('#workstep-content');
        content.empty();
        $.ajax({
            type: "Get",
            url: "/APSE/WorkPackageStep/GetWorkPackageSteps",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var name = $("<td>" + val.title + "</td>");
                    var editBtn = $("<td><button  data-toggle='modal' data-target='#edit-workstepModal' data-id='" + val.id + "' class='edit-workstep btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>");
                    var deleteBtn = $("<td><button data-id='" + val.id + "' class='delete-workstep btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button></td>");
                    var row = $('<tr></tr>');
                    row.append(name).append(editBtn).append(deleteBtn);
                    content.append(row);
                });
                _table = tableinit();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        })
    }

    function getWorkstep(id) {
        return $.ajax({
            type: "Get",
            url: '/APSE/WorkPackageStep/GetWorkPackageStep/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {

            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createworkstep(name, description, workpackageId) {

        var model =
        {
            'Title': name, 'Description': description, 'WorkpackageId': workpackageId
        };
        $.ajax({
            type: "Post",
            url: "/APSE/WorkPackageStep/CreateWorkPackageStep",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row.add([
                            name,
                            "<button  data-toggle='modal' data-target='#edit-workstepModal' data-id='" + data.subject + "' class='edit-workstep btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-workstep btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]).draw(false);
                    }
                }
                else {
                    alert(data.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function editworkstep(name, description, workpackageId, id) {
        var model =
        {
            'Title': name, 'Description': description, 'workpackageId': workpackageId
        };
        $.ajax({
            type: "PUT",
            url: "/APSE/WorkPackageStep/UpdateWorkPackageStep/" + id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').data([
                            name,
                            "<button  data-toggle='modal' data-target='#edit-workstepModal' data-id='" + data.subject + "' class='edit-workstep btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-workstep btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]);
                        $('#edit-workstepModal').modal('toggle');
                    }
                }
                else {
                    alert(data.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function removeWorkstep(id) {
        $.ajax({
            type: "Delete",
            url: "/APSE/WorkPackageStep/DeleteWorkPackageStep/" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').remove().draw(false);
                    }
                }
                else {
                    alert(data.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function tableinit() {
        var hh = window.innerHeight * 0.45;
        var table = $('#workstepLst').DataTable({
            responsive: true,
            FixedHeader: true,
            scrollY: hh + "px",
            dom: 'Bfrtip',
            buttons: [
                {
                    text: '',
                    extend: 'excelHtml5',
                    className: 'btn btn-success btn-sm',
                    exportOptions: {
                        modifier: {
                            page: 'all'
                        },
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('dt-button');
                        $(node).append('<p class="" style="height:1px;"><i style="font-size:20px;color:#FFF;" class="fa fa-file-excel"></i><p>');
                    },
                    extension: '.xlsx',
                    filename: "Persons",
                    messageTop: "Persons",
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
                },
                {
                    extend: 'print',
                    text: '',
                    className: 'btn btn-warning btn-sm',
                    exportOptions: {
                        modifier: {
                            page: 'all'
                        }
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('dt-button');
                        $(node).append('<p class="" style="height:1px;"><i style="font-size:20px;color:#FFF;" class="fa fa-print"></i><p>');
                    }
                }
            ],
            scrollCollapse: !0,
            scroller: !0,
            "fnInitComplete": function () {
                //$('.dataTables_scrollBody').perfectScrollbar();
                const ps = new PerfectScrollbar('.dataTables_scrollBody');
            },
            //on paginition page 2,3.. often scroll shown, so reset it and assign it again
            "fnDrawCallback": function (oSettings) {
                $('.dataTables_scrollBody').each(function () {
                    if (!$(this).hasClass('ps')) {
                        const ps = new PerfectScrollbar($(this)[0]);
                    }
                    $(this)[0].scrollTop = 0;
                });
            }
        });
        return table;
    }

}());