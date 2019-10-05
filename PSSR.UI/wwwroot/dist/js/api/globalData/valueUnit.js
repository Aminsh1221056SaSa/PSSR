
var valueUnit = valueUnit || (function () {

    return {
        init: function (myTreeview) {
            initialization(myTreeview);
        },
        getValueUnitList: function () {
            getValueUnitList()
        },
        getValueUnit: function (id) {
            return getValueUnit(id);
        },
        removeValueUnit: function (id) {
            removeValueUnit(id);
        }
    };

    var _treeView = null;

    function initialization(myTreeview) {
        _treeView = myTreeview;

        $("form[name='valueUnitForm']").validate({
            // Specify validation rules
            rules: {
                valueUnitname: "required",
                mathType: "required",
                mathNum: "required",
            },
            messages: {
                valueUnitname: "Please enter ValueUnit Name",
                mathType: "Please enter math Type",
                mathNum: "Please enter math num",
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
                var name = $('input#valueUnitname').val();
                var mathNum = $('input#mathNum').val();
                var mathType = $('select#mathType').val();
                var cParent = $('input#current-valueUnitparent-id').val();
                createvalueUnit(name, mathType, mathNum, cParent);
            }
        });

        $("form[name='valueUnitEditForm']").validate({
            // Specify validation rules
            rules: {
                valueUnitname: "required",
                mathType: "required",
                mathNum: "required",
            },
            messages: {
                valueUnitname: "Please enter ValueUnit Name",
                mathType: "Please enter math Type",
                mathNum: "Please enter math num",
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
                var name = $('input#editvalueUnitname').val();
                var mathNum = $('input#editmathNum').val();
                var mathType = $('select#editmathType').val();
                var id = $('#current-valueUnitId').val();
                editvalueUnit(name, mathType, mathNum, id);
            }
        });
    }

    function getValueUnitList() {
        var content = $('#value-unit-treeItems');
        content.empty();
        $.get("/APSE/ValueUnit/GetValueUnitsTreeFormat", function (data) {
            content.append(data);
            _treeView.treed('.myTreeview');
        });
      
    }

    function getValueUnit(id) {
        return $.ajax({
            type: "Get",
            url: '/APSE/ValueUnit/GetValueUnit/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {

            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createvalueUnit(name, mathType, mathNum, cParent) {
        var gparent = null;
        if (parseInt(cParent) > 0) {
            gparent = cParent;
        }

        var model =
        {
            'Name': name, 'MathType': mathType, 'MathNum': mathNum, 'ParentId': gparent
        };
        $.ajax({
            type: "Post",
            url: "/APSE/ValueUnit/CreateValueUnit",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    getValueUnitList();
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

    function editvalueUnit(name, mathType, mathNum, id) {
        var model =
        {
            'Name': name, 'MathType': mathType, 'MathNum': mathNum
        };
        $.ajax({
            type: "PUT",
            url: "/APSE/ValueUnit/UpdateValueUnit/" + id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    getValueUnitList();
                    $('#edit-valueUnitModal').modal('toggle');
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

    function removeValueUnit(id) {
        $.ajax({
            type: "Delete",
            url: "/APSE/ValueUnit/DeleteValueUnit/" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    getValueUnitList();
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

}());