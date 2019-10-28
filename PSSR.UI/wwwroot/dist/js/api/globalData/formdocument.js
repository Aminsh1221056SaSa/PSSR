
var formdocument = formdocument || (function () {

    return {
        init: function (enumDefinetion) {
            initialization(enumDefinetion);
        },
        getFormDictionaryList: function () {
            getFormDictionaryList()
        },
        getFormDictionary: function (id) {
            return getFormDictionary(id);
        },
        setCurrentIndex: function (index) {
            setCurrentIndex(index);
        },
        removeFormDictionary: function (id) {
            removeFormDictionary(id);
        }
    };
    var _enumDefine = null;
    var _table = null;
    var _cIndex = 0;
    function setCurrentIndex(index) {
        _cIndex = index;
    }

    function initialization(enumDefinetion) {
        _enumDefine = enumDefinetion;
        if (_table) {
            _table.clear().draw();
            _table.destroy();
        }

        $("form[name='formdictionaryForm']").validate({
            // Specify validation rules
            rules: {
                formdicFile: "required",
                formdicCode: "required",
                formdicType: "required",
                formdicWorkpackageId: "required",
                formdicPriority: {
                    required: true,
                    number: true
                },
                formdicMH: {
                    required: true,
                    number: true
                },
                formdicDescipline:"required"
            },
            messages: {
                formdicFile: "Please select a Document",
                formdicCode: "Please enter form code",
                formdicType: "Please select a type",
                formdicWorkpackageId: "Please select a workpackage",
                formdicPriority: { required: "Please Enter a priority", number: "Please enter numbers Only" },
                formdicMH: { required: "Please Enter a Man Hours", number: "Please enter numbers Only" },
                formdicDescipline: "Please select at least one descipline",
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
                var formData = new FormData();
                formData.append('file', $('#formdicFile')[0].files[0]);
                var code = $('input#formdicCode').val();
                var type = $('select#formdicType').val();
                var workp = $('select#formdicWorkpackageId').val();
                var priority = $('input#formdicPriority').val();
                var mh = $('input#formdicMH').val();
                var descipliens = $("select#formdicDescipline").select2("val");
                var description = $('input#formdicDescription').val();
                createformDictionary(formData, code, type, workp, priority, mh, descipliens, description);
            }
        });

        $("form[name='formDocumentEditForm']").validate({
            // Specify validation rules
            rules: {
                formdicFile: "required",
                formdicCode: "required",
                formdicType: "required",
                formdicWorkpackageId: "required",
                formdicPriority: {
                    required: true,
                    number: true
                },
                formdicMH: {
                    required: true,
                    number: true
                },
                formdicDescipline: "required"
            },
            messages: {
                formdicFile: "Please select a Document",
                formdicCode: "Please enter form code",
                formdicType: "Please select a type",
                formdicWorkpackageId: "Please select a workpackage",
                formdicPriority: { required: "Please Enter a priority", number: "Please enter numbers Only" },
                formdicMH: { required: "Please Enter a Man Hours", number: "Please enter numbers Only" },
                formdicDescipline: "Please select at least one descipline",
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
                var name = $('input#editformDictionaryname').val();
                var description = $('input#editformDictionaryDescription').val();
                var id = $('#current-formDictionaryId').val();
                editformDictionary(name, description, id);
            }
        });
    }

    function getFormDictionaryList() {
        var content = $('#formDictionary-content');
        content.empty();
        $.ajax({
            type: "Get",
            url: "/APSE/FormDictionary/GetFormDocuments",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var name = $("<td>" + val.code + "</td>");
                    var type = $("<td>" + _enumDefine.getFormDocumentType(val.type) + "</td>");
                    var workPackage = $("<td>" + val.wrokPackageName + "</td>");
                    var editBtn = $("<td><button  data-toggle='modal' data-target='#edit-formDictionaryModal' data-id='" + val.id + "' class='edit-formDictionary btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>");
                    var deleteBtn = $("<td><button data-id='" + val.id + "' class='delete-formDictionary btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button></td>");
                    var row = $('<tr></tr>');
                    row.append(name).append(type).append(workPackage).append(editBtn).append(deleteBtn);
                    content.append(row);
                });
                _table = tableinit();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        })
    }

    function getFormDictionary(id) {
        return $.ajax({
            type: "Get",
            url: '/APSE/FormDictionary/getFormDictionary/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {

            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createformDictionary(formData, code, type, workp, priority, mh, descipliens, description)
    {
        var model =
        {
            'Description': description, 'Code': code, 'Type': type, 'WorkPackageId': workp,
            'Priority': priority, 'Mh': mh, 'AvailableDesciplines': descipliens
        };

        formData.append('jsonString', JSON.stringify(model));
        var sType = _enumDefine.getFormDocumentType(parseInt(type));
        var workpackageName = $('select#formdicWorkpackageId').text();

        $.ajax({
            type: "Post",
            url: "/APSE/FormDictionary/CreateFormDocument",
            data: formData,
            processData: false,
            contentType: false,
            success: function (data, status, jqXHR) {
                var gdata = JSON.parse(data);
                if (gdata.key == 200) {
                    if (_table) {
                        _table.row.add([
                            name,
                            sType,
                            workpackageName,
                            "<button  data-toggle='modal' data-target='#edit-formDictionaryModal' data-id='" + data.subject + "' class='edit-formDictionary btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-formDictionary btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]).draw(false);
                    }
                }
                else {
                    alert(gdata.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function editformDictionary(name, description, id) {
        var model =
        {
            'Name': name, 'Description': description
        };
        $.ajax({
            type: "PUT",
            url: "/APSE/FormDictionary/UpdateFormDictionary/" + id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').data([
                            name,
                            "<button  data-toggle='modal' data-target='#edit-formDictionaryModal' data-id='" + data.subject + "' class='edit-formDictionary btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-formDictionary btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]);
                        $('#edit-formDictionaryModal').modal('toggle');
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

    function removeFormDictionary(id) {
        $.ajax({
            type: "Delete",
            url: "/APSE/FormDictionary/DeleteFormDictionary/" + id,
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
        var table = $('#formDictionaryLst').DataTable({
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