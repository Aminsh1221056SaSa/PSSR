
var pssrlocation = pssrlocation || (function () {

    return {
        init: function () {
            initialization();
        },
        getLocationList: function () {
            getLocationList()
        },
        getLocation: function (id) {
            return getLocation(id);
        },
        setCurrentIndex: function (index) {
            setCurrentIndex(index);
        },
        removeLocation: function (id) {
            removeLocation(id);
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

        $("form[name='locationForm']").validate({
            // Specify validation rules
            rules: {
                locationname: "required",
            },
            messages: {
                locationname: "Please enter Location Name",
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
                var name = $('input#locationname').val();
                createLocation(name);
            }
        });

        $("form[name='locationEditForm']").validate({
            // Specify validation rules
            rules: {
                locationname: "required",
            },
            messages: {
                locationname: "Please enter Location Name"
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
                var name = $('input#editlocationname').val();
                var id = $('#current-locationId').val();
                editLocation(name, id);
            }
        });
    }

    function getLocationList() {
        var content = $('#location-content');
        content.empty();
        $.ajax({
            type: "Get",
            url: "/APSE/Location/GetLocations",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var name = $("<td>" + val.title + "</td>");
                    var editBtn = $("<td><button  data-toggle='modal' data-target='#edit-locationModal' data-id='" + val.id + "' class='edit-location btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>");
                    var deleteBtn = $("<td><button data-id='" + val.id + "' class='delete-location btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button></td>");
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

    function getLocation(id) {
        return $.ajax({
            type: "Get",
            url: '/APSE/Location/GetLocation/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {

            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createLocation(name) {

        var model =
        {
            'Title': name
        };
        $.ajax({
            type: "Post",
            url: "/APSE/Location/CreateLocation",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row.add([
                            name,
                            "<button  data-toggle='modal' data-target='#edit-locationModal' data-id='" + data.subject + "' class='edit-location btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-location btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
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

    function editLocation(name, id) {
        var model =
        {
            'Title': name,
        };
        $.ajax({
            type: "PUT",
            url: "/APSE/Location/UpdateLocation/" + id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').data([
                            name,
                            "<button  data-toggle='modal' data-target='#edit-locationModal' data-id='" + data.subject + "' class='edit-location btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-location btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]);
                        $('#edit-locationModal').modal('toggle');
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

    function removeLocation(id) {
        $.ajax({
            type: "Delete",
            url: "/APSE/Location/DeleteLocation/" + id,
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
        var table = $('#locationLst').DataTable({
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