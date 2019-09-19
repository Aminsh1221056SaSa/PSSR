var activityDataTable = (function ($) {
    return {
        init: function () {
            initialization();
        },
        getActivityList: function (tbHeight, filterBy, filterValue, workId, locationId, desiplineId, systemId, subSystemId, workpackageStepId) {
            acListInitialization(tbHeight, filterBy, filterValue, workId, locationId, desiplineId, systemId, subSystemId, workpackageStepId);
        },
    };

    function initialization() {
      
    }

    function acListInitialization(tbHeight, filterBy, filterValue, workId, locationId, desiplineId, systemId, subSystemId, workpackageStepId) {

        var statusNum = null;
        var conditionNum = null;

        switch (filterBy) {
            case 'ByStatus':
                if (filterValue == 'NotStarted') {
                    statusNum = 1;
                }
                break;
            case 'ByCondition':
                if (filterValue == 'Normal') {
                    conditionNum = 3000;
                }
                else if (filterValue == 'Hold') {
                    conditionNum = 3002;
                }
                else if (filterValue == 'Front') {
                    conditionNum = 3003;
                }
                break;
        }

        if ($('#datatable-acList').length !== 0) {

            var equalityoptionsstring = [
                {
                    id: "co",
                    name: "()"
                },
                {
                    id: "eq",
                    name: "=="
                }
            ]

            var equalityoptionsnumber = [
                {
                    id: "eq",
                    name: "=="
                },
                {
                    id: "gt",
                    name: "<"
                },
                {
                    id: "gte",
                    name: "<="
                },
                {
                    id: "lt",
                    name: ">"
                },
                {
                    id: "lte",
                    name: ">="
                }
            ]

            var dtColumns =
                [{
                    data: "id", render: function (data, type, row) {
                        var content = '<div><a href="/ProjectManagment/Activity/EditActivity?id=' + data + '" class="col-md-6" style="padding-left:0px"><p class="btn btn-success edtit-btn btn-sm"><i class="fa fa-reorder"></i></p></a><a style="padding-left:0px" href="/ProjectManagment/Activity/EditActivityDetails?id=' + data + '" class="col-md-6"><p class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></p></a></div>';
                      
                        return content;
                    }, visible: true, searchable: false, name: "co", searchable: true, orderable: false, search: { value: "", regex: false } },
            { data: "activityCode", name: "co", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "tagNumber", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    {
                        data: "status", render: function (data, type, row) {
                            switch (data) {
                                case 'Done':
                                    return '<div class="text-green">' + data + '</div>';
                                case 'NotStarted':
                                    return '<div class="text-orange">' + data + '</div>';
                                case 'Ongoing':
                                    return '<div class="text-blue">' + data + '</div>';
                                case 'Delete':
                                    return '<div class="text-red">' + data + '</div>';
                                default:
                                    return '<div class="text-red">' + data + '</div>';
                            }
                        }, name: "eq", searchable: true, orderable: true, search: { value: statusNum, regex: false }
                    },
                    { data: "condition", visible: true, name: "eq", searchable: true, orderable: true, search: { value: conditionNum, regex: false } },
                    {
                        data: "progress", render: function (data, type, row) {
                            var pclass = "progress-bar-red";

                            if (data <= 25) {
                                pclass = "progress-bar-red";
                            }
                            else if (data > 25 && data <= 65) {
                                pclass = "progress-bar-yellow";
                            }
                            else if (data > 65 && data <= 95) {
                                pclass = "progress-bar-aqua";
                            }
                            else if (data > 95) {
                                pclass = "progress-bar-green";
                            }

                            var myvar = '<div class="progress-group">' +
                                '                           <span class="progress-text" style="font-size:12px">' + data + '%</span>' +
                                '                            <span class="progress-number"><b></b></span>' +
                                '                            <div class="progress sm">' +
                                '                                <div class="progress-bar ' + pclass + '" style="width:' + data + '%"></div>' +
                                '                            </div>' +
                                '                        </div>';
                            return myvar;

                        },
                        visible: true, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false }
                    },
                    { data: "workPackage", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "workPackageId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: workId, regex: false } },
                    { data: "location", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "locationId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: locationId, regex: false } },
                    { data: "descipline", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "desciplineId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: desiplineId, regex: false } },
                    { data: "system", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "systemId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: systemId, regex: false } },
                    { data: "subSystem", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "subsytemId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: subSystemId, regex: false } },
            { data: "formDictionary", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
            { data: "formType", visible: false, visible: true, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
            { data: "formDictionaryId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "workPackageStep", visible: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "workPackageStepId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: workpackageStepId, regex: false } },
                 
            { data: "weightFactor", name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "estimateMh", visible: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "actualMh", visible: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "punchCount", visible: false, name: "eq", searchable: false, orderable: false, search: { value: "", regex: false } },
            {
                data: "planStartDate", render: function (data, type, row) {
                    if (data)
                        return window.moment(data).format("DD/MM/YYYY");
                    else
                        return null;
                }, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false }
            },
                    {
                        data: "planEndDate", visible: false, render: function (data, type, row) {
                            if (data)
                                return window.moment(data).format("DD/MM/YYYY");
                            else
                                return null;
                        }, name: "eeq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    {
                        data: "actualStartDate", visible: false, render: function (data, type, row) {
                            if (data)
                                return window.moment(data).format("DD/MM/YYYY");
                            else
                                return null;
                        }, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    {
                        data: "actualEndDate", visible: false, render: function (data, type, row) {
                            if (data)
                                return window.moment(data).format("DD/MM/YYYY");
                            else
                                return null;
                        }, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "valueUnit", visible: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
            { data: "valueUnitId", visible: false, searchable: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } },
                    { data: "valueUnitNum", visible: false, name: "eq", searchable: true, orderable: true, search: { value: "", regex: false } }
               
                   
                   ]

            function capitalizeFirstLetterToUpper(string) {
                return string.charAt(0).toUpperCase() + string.slice(1);
            }

            function capitalizeFirstLetterToLowerCase(string) {
                return string.charAt(0).toLowerCase() + string.slice(1);
            }

            function changename(data, name) {
                for (var i in dtColumns) {
                    if (dtColumns[i].data == data) {
                        dtColumns[i].name = name;
                        break; //Stop this loop, we found it!
                    }
                }
            }

            function changevalue(data, value) {
                for (var i in dtColumns) {
                    if (dtColumns[i].data == data) {
                        dtColumns[i].search.value = value;
                        break; //Stop this loop, we found it!
                    }
                }
            }

            function changeNameAndValue(data, name, value) {
                for (var i in dtColumns) {
                    if (dtColumns[i].data == data) {
                        dtColumns[i].name = name;
                        dtColumns[i].search.value = value;
                        break; //Stop this loop, we found it!
                    }
                }
            }

            function getequalitynumberDropDown(value) {

                var cval = '';
                for (var i in dtColumns) {
                    if (dtColumns[i].data == value) {
                        cval = dtColumns[i].name;
                        break; //Stop this loop, we found it!
                    }
                }

                var select = $('<select style="width:33%;font-size:12px;marigin:0;pading:0;height:30px"></select>');
                for (var i in equalityoptionsnumber) {
                    if (cval === equalityoptionsnumber[i].id) {
                        select.append('<option value="' + equalityoptionsnumber[i].id + '" selected>' + equalityoptionsnumber[i].name + '</option>')
                    }
                    else {
                        select.append('<option value="' + equalityoptionsnumber[i].id + '">' + equalityoptionsnumber[i].name + '</option>');
                    }
                }
                return select;
            }

            function getequalitystringDropDown(value) {
                var cval = '';
                for (var i in dtColumns) {
                    if (dtColumns[i].data == value) {
                        cval = dtColumns[i].name;
                        break; //Stop this loop, we found it!
                    }
                }
                var select = $('<select style="width:33%;font-size:12px;marigin:0;pading:0;height:30px"></select>');
                for (var i in equalityoptionsstring) {
                    if (cval === equalityoptionsstring[i].id) {
                        select.append('<option value="' + equalityoptionsstring[i].id + '" selected>' + equalityoptionsstring[i].name + '</option>')
                    }
                    else {
                        select.append('<option value="' + equalityoptionsstring[i].id + '">' + equalityoptionsstring[i].name + '</option>');
                    }
                }
                return select;
            }

            //$("#datatable-acList").DataTable().destroy();
            var table = $("#datatable-acList").DataTable({
                language: {
                    processing: '<div><div class= "loading-one" ><div class="bounceball"></div></div></div>',
                    buttons: {
                        colvis: ''
                    },
                    paginate: {
                        next: '<span class="glyphicon glyphicon-menu-right"></span>',
                        previous: '<span class="glyphicon glyphicon-menu-left"></span>'
                    },
                    "info": " _START_ to _END_ of _TOTAL_ Tasks",
                    "lengthMenu": "Display _MENU_ Tasks"
                },
                scrollY: tbHeight + "px",
                sScrollX: "0%",
                scrollX: true,
                fixedHeader: true,
                colReorder: true,
                processing: true,
                serverSide: true,
                orderCellsTop: true,
                stateSave: true,
                autoWidth: true,
                deferRender: true,
                lengthMenu: [10, 18, 30,50, 100],
                pageLength: 18,
                dom: '<"row"<"col-sm-12 col-md-4"B><"col-sm-12 col-md-6"l><"col-sm-12 col-md-2  text-left"i>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-12"p>>',
                buttons: [
                    {
                        text: '',
                        className: 'btn btn-warning btn-sm',
                        action: function (e, dt, node, config) {
                            table.state.clear();
                            window.location ="/ProjectManagment/Activity/CreateActivity";
                        },
                        init: function (api, node, config) {
                            $(node).removeClass('dt-button');
                            $(node).append('<p style="height:8px">Add Task  &nbsp;<i style="font-size:16px;color:#FFF;" class="fa fa-plus"></i><p>');
                        }
                    },
                    {
                        extend: 'colvis',
                        columns: ':not(.noVis)',
                        collectionLayout: 'fixed two-column ',
                        className: 'btn btn-info btn-sm',
                        postfixButtons: ['colvisRestore'],
                        init: function (api, node, config) {
                            $(node).removeClass('dt-button');
                            $(node).append('<p class="" style="height:8px">Change Column&nbsp;<i style="font-size:16px;color:#FFF;" class="fa fa-eye "></i><p>');
                        }
                    },
                    {
                        text: '',
                        className: 'btn btn-success  btn-sm',
                        action: function (e, dt, node, config) {
                            window.location.href = "/Report/GetActivityExportExcel";
                        },
                        init: function (api, node, config) {
                            $(node).removeClass('dt-button');
                            $(node).append('<p style="height:8px">Export &nbsp;<i style="font-size:16px;color:#FFF;" class="fa fa-file-excel-o"></i><p>');
                        }
                    },
                    
                    {
                        text: '',
                        className: 'btn btn-danger btn-sm',
                        action: function (e, dt, node, config) {
                            table.state.clear();
                            window.location.reload();
                        },
                        init: function (api, node, config) {
                            $(node).removeClass('dt-button');
                            $(node).append('<p style="height:8px"><i style="font-size:16px;color:#FFF;" class="fa fa-filter"></i><i style="font-size:16px;color:#FFF;" class="fa fa-remove"></i><p>');
                        }
                    },
                ],
                ajax: {
                    type: "POST",
                    url: '/APSE/activity/GetActivityDataTable',
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    headers: {
                        "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    data: function (data) {
                        //let additionalValues = [];
                        //additionalValues[0] = "Additional Parameters 1";
                        //additionalValues[1] = "Additional Parameters 2";
                        //data.AdditionalValues = additionalValues;
                        data.columns = dtColumns;
                        //console.log(dtColumns);
                        return JSON.stringify(data);
                    }
                },
                columnDefs: [
                    {
                        "defaultContent": "[N/A]",
                        "targets": "_all"
                    },
                    {
                        targets: -1,
                        visible: false
                    },
                    {
                        targets: 0,
                        className: 'noVis'
                    },
                    {
                        targets: 4,
                        className: 'noVis'
                    },
                    {
                        targets: 6,
                        className: 'noVis'
                    },
                    {
                        targets: 8,
                        className: 'noVis'
                    },
                    {
                        targets: 10,
                        className: 'noVis'
                    },
                    {
                        targets: 12,
                        className: 'noVis'
                    },
                    {
                        targets: 14,
                        className: 'noVis'
                    },
                    {
                        targets: 17,
                        className: 'noVis'
                    },
                    {
                        targets: 19,
                        className: 'noVis'
                    }
                ],
                columns: dtColumns,
                initComplete: function () {
                    var nRow = $('.dataTables_scrollHeadInner table thead tr')[1];
                    
                    $.each(nRow.cells, function (i, v) {
                      
                        if (v.innerText === 'ActivityCode')
                        {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm" autocomplete="off">');

                            val.append($input);
                            var $select = getequalitystringDropDown('activityCode');

                            val.append($select);
                            $(v).append(val);

                             $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('activityCode', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('activityCode', $(this).val());
                            });
                        }
                        else if (v.innerText === 'TagNumber') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm" autocomplete="off">');

                            val.append($input);
                            var $select = getequalitystringDropDown('tagNumber');

                            val.append($select);
                            $(v).append(val);

                            $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('tagNumber', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('tagNumber', $(this).val());
                            });
                        }
                        else if (v.innerText === 'Progress') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm" autocomplete="off">');

                            val.append($input);
                            var $select = getequalitynumberDropDown('progress');

                            val.append($select);
                            $(v).append(val);

                            $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('progress', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('progress', $(this).val());
                            });
                        }
                        else if (v.innerText === 'WeightFactor') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm" autocomplete="off">');

                            val.append($input);
                            var $select = getequalitynumberDropDown('weightFactor');

                            val.append($select);
                            $(v).append(val);

                            $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('weightFactor', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('weightFactor', $(this).val());
                            });
                        }
                        else if (v.innerText === 'EstimateMh') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm">');

                            val.append($input);
                            var $select = getequalitynumberDropDown('estimateMh');

                            val.append($select);
                            $(v).append(val);

                            $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('estimateMh', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('estimateMh', $(this).val());
                            });
                        }
                        else if (v.innerText === 'ActualMh') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm">');

                            val.append($input);
                            var $select = getequalitynumberDropDown('actualMh');

                            val.append($select);
                            $(v).append(val);

                            $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('actualMh', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('actualMh', $(this).val());
                            });
                        }
                        else if (v.innerText === 'PlanStartDate') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var ni = $('<div class="input-group-addon"><i class="fa fa-calendar"></i></div>');
                            var $picker = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm datepicker" autocomplete="off">');
                            val.append(ni);
                            val.append($picker);
                            var $select = getequalitynumberDropDown('planStartDate');

                            val.append($select);
                            $(v).append(val);

                            $picker.change(function () {
                                changevalue('planStartDate', this.value);
                                table.column(i + 1).search(this.value).draw();
                            })

                            $select.change(function () {
                                changename('planStartDate', $(this).val());
                            });
                        }
                        else if (v.innerText === 'PlanEndDate') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var ni = $('<div class="input-group-addon"><i class="fa fa-calendar"></i></div>');
                            var $picker = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm datepicker" autocomplete="off">');
                            val.append(ni);
                            val.append($picker);
                            var $select = getequalitynumberDropDown('planEndDate');

                            val.append($select);
                            $(v).append(val);

                            $picker.change(function () {
                                changevalue('planEndDate', this.value);
                                table.column(i + 1).search(this.value).draw();
                            })

                            $select.change(function () {
                                changename('planEndDate', $(this).val());
                            });
                        }
                        else if (v.innerText === 'ActualStartDate') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var ni = $('<div class="input-group-addon"><i class="fa fa-calendar"></i></div>');
                            var $picker = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm datepicker" autocomplete="off">');
                            val.append(ni);
                            val.append($picker);
                            var $select = getequalitynumberDropDown('actualStartDate');

                            val.append($select);
                            $(v).append(val);

                            $picker.change(function () {
                                changevalue('actualStartDate', this.value);
                                table.column(i + 1).search(this.value).draw();
                            })

                            $select.change(function () {
                                changename('actualStartDate', $(this).val());
                            });
                        }
                        else if (v.innerText === 'ActualEndDate') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var ni = $('<div class="input-group-addon"><i class="fa fa-calendar"></i></div>');
                            var $picker = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm datepicker" autocomplete="off">');
                            val.append(ni);
                            val.append($picker);
                            var $select = getequalitynumberDropDown('actualEndDate');

                            val.append($select);
                            $(v).append(val);

                            $picker.change(function () {
                                changevalue('actualEndDate', this.value);
                                table.column(i + 1).search(this.value).draw();
                            })

                            $select.change(function () {
                                changename('actualEndDate', $(this).val());
                            });
                        }
                        else if (v.innerText === 'ValueUnit') {
                            var select = $('<select class="form-control form-control-sm"><option value="">...</option></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('valueUnitId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/ValueUnit/GetValueUnits", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        select.append('<option value="' + item[i].id + '">' + item[i].name + '</option>')
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'ValueUnitNum') {
                            $(v).empty()
                            var val = $('<div class="input-group "></div>');
                            var $input = $('<input style="width:67%;" id = "pCheckDate" type = "text" class= "form-control form-control-sm" autocomplete="off">');

                            val.append($input);
                            var $select = getequalitynumberDropDown('valueUnitNum');

                            val.append($select);
                            $(v).append(val);

                            $input.on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue('valueUnitNum', this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });

                            $select.change(function () {
                                changename('valueUnitNum', $(this).val());
                            });
                        }
                        else if (v.innerText === 'Status') {
                            var select = $('<select class="form-control form-control-sm"><option value="">...</option></select>');
                            if (statusNum == 1) {
                                select.append('<option value="1" selected>NotStarted</option>');
                            }
                            else {
                                select.append('<option value="1">NotStarted</option>');
                            }
                           
                            select.append('<option value="2">Ongoing</option>');
                            select.append('<option value="3">Done</option>');
                            select.append('<option value="4">Reject</option>');
                            select.append('<option value="5">Delete</option>');

                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('status', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });
                        }
                        else if (v.innerText === 'Condition') {
                            var select = $('<select class="form-control form-control-sm"><option value="">...</option></select>');
                            if (conditionNum == 3000) {
                                select.append('<option value="3000" selected>Normal</option>');
                                select.append('<option value="3002">Hold</option>');
                                select.append('<option value="3003">Front</option>');
                            }
                            else if (conditionNum == 3002) {
                                select.append('<option value="3000" >Normal</option>');
                                select.append('<option value="3002" selected>Hold</option>');
                                select.append('<option value="3003">Front</option>');
                            }
                            else if (conditionNum == 3003) {
                                select.append('<option value="3000" >Normal</option>');
                                select.append('<option value="3002">Hold</option>');
                                select.append('<option value="3003" selected>Front</option>');
                            }
                            else {
                                select.append('<option value="3000" >Normal</option>');
                                select.append('<option value="3002">Hold</option>');
                                select.append('<option value="3003">Front</option>');
                            }

                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('condition', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });
                        }
                        else if (v.innerText === 'FormType') {
                            var select = $('<select class="form-control form-control-sm"><option value="">...</option></select>');
                            select.append('<option value="1">Test</option>')
                            select.append('<option value="2">Check</option>')

                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('formType', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });
                        }
                        if (v.innerText === 'WorkPackage') {
                            var select = $('<select class="form-control form-control-sm" style="width:90%"><option value="">...</option></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('workPackageId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/WorkPackage/GetRoadMaps", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        if (item[i].id == workId) {
                                            select.append('<option value="' + item[i].id + '" selected>' + item[i].title + '</option>')
                                        }
                                        else {
                                            select.append('<option value="' + item[i].id + '">' + item[i].title + '</option>')
                                        }
                                        
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'Location') {
                            var select = $('<select class="form-control form-control-sm" style="width:90%"><option value="">...</option></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('locationId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/WorkPackage/GetLocations", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        if (item[i].id == locationId) {
                                            select.append('<option value="' + item[i].id + '" selected>' + item[i].title + '</option>')
                                        }
                                        else {
                                            select.append('<option value="' + item[i].id + '">' + item[i].title + '</option>')
                                        }
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'Descipline') {
                            var select = $('<select class="form-control form-control-sm" style="width:90%"><option value="">...</option></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('desciplineId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/Descipline/GetDesciplineList", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        if (item[i].id == desiplineId) {
                                            select.append('<option value="' + item[i].id + '" selected>' + item[i].title + '</option>')
                                        }
                                        else {
                                            select.append('<option value="' + item[i].id + '">' + item[i].title + '</option>')
                                        }
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'System') {
                            var select = $('<select class="form-control form-control-sm" style="width:90%"><option value="">...</option></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('systemId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/ProjectSystem/GetProjectSystems", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        if (item[i].id == systemId) {
                                            select.append('<option value="' + item[i].id + '" selected>' + item[i].title + '</option>')
                                        }
                                        else {
                                            select.append('<option value="' + item[i].id + '">' + item[i].title + '</option>')
                                        }
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'SubSystem') {
                            var select = $('<select class="form-control form-control-sm select2" style="width:90%"></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('subsytemId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/SubSystem/GetProjectSubSystems", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    select.append('<option value="">...</option>');
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        if (item[i].id == subSystemId) {
                                            select.append('<option value="' + item[i].id + '" selected>' + item[i].title + '</option>')
                                        }
                                        else {
                                            select.append('<option value="' + item[i].id + '">' + item[i].title + '</option>')
                                        }
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'FormDictionary') {
                            var select = $('<select class="form-control form-control-sm select2" style="width:90%"></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('formDictionaryId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/FormDictionary/GetformDictionaryies", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    select.append('<option value="">...</option>');
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        select.append('<option value="' + item[i].id + '">' + item[i].code + '</option>')
                                    }
                                }
                            });
                        }
                        else if (v.innerText === 'WorkPackageStep') {
                            var select = $('<select class="form-control form-control-sm select2" style="width:90%"></select>');
                            select.appendTo($(v).empty())

                            select.on('click', function (e) {
                                e.stopPropagation();
                            });

                            select.on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                changeNameAndValue('workPackageStepId', "eq", val);
                                table.column(i + 1).search(val, true, false).draw();
                            });

                            $.ajax({
                                url: "/APSE/WorkPackageStep/GetWorkPackageSteps", contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    select.append('<option value="">...</option>');
                                    var item = JSON.parse(result);
                                    for (i = 0; i <= item.length; i++) {
                                        if (item[i].id == workpackageStepId) {
                                            select.append('<option value="' + item[i].id + '" selected>' + item[i].title + '</option>');
                                        }
                                        else {
                                            select.append('<option value="' + item[i].id + '">' + item[i].title + '</option>');
                                        }
                                    }
                                }
                            });
                        }
                        else {
                            $('#datatable-acList thead tr:last th:eq(' + (i) + ') input').on('keyup',
                                function (e) {
                                    if (e.keyCode === 13) {
                                        changevalue(capitalizeFirstLetterToLowerCase(v.innerText), this.value);
                                        table.column($(this).parent().index() + ':visible').search(this.value).draw();
                                    }
                                });
                        }

                        $('.select2').select2();
                        $('.datepicker').datepicker({
                            autoclose: true
                        });
                    });

                },
            });
        }
    }
}(window.jQuery));