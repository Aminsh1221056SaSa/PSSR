
g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');

var WBSTreeConfig = WBSTreeConfig || (function () {
    
    return {
        init: function (wbstypes) {
            initialization(wbstypes);
        },
        getProjectWorks: function() {
            getProjectWorks()
        },
        getProjectLocation: function() {
            getProjectLocation()
        },
        getProjectDesciplines: function() {
            getProjectDesciplines()
        },
        getProjectSystems: function() {
            getProjectSystems()
        },
        getProjectSubSystems: function() {
            getProjectSubSystems()
        },
        showEditWbsNode: function(id,wbsCode) {
            showEditWbsNode(id, wbsCode)
        },

        editWbsCode: function() {
            editWbsCode()
        },
        deleteWbsCode: function() {
            deleteWbsCode()
        },
        dropwbs: function() {
            dropwbs()
        },
        removeFromWbsType: function(id) {
            removeFromWbsType(id)
        },
        addToWbsType: function(id) {
            addToWbsType(id)
        },
        getProjectwbsTree: function(types) {
            getProjectwbsLine(types)
        },
        getSystemWBsTree: function(types,systemId) {
            getSystemWBsTree(types, systemId)
        },
    };

    //wbs tree

    function initialization(wbstypes) {
        g.setShowInput(0);
        g.setShowRes(0);
        g.setShowDur(0);
        g.setShowComp(1);
        g.setShowStartDate(0);
        g.setShowEndDate(0);
        g.setCaptionType('Resource');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)

        if (g) {

            if (g != null) {
                g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
                g.setShowInput(0);
                g.setShowRes(0);
                g.setShowDur(0);
                g.setShowComp(1);
                g.setShowStartDate(0);
                g.setShowEndDate(0);
                g.setCaptionType('Resource');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)
            }

            getProjectwbsWfTree(wbstypes);
        }
        else {
            alert("not defined");
        }
    }

    function getProjectwbsWfTree(wbstypes) {
        $('#wbs-creator-loading').show();

        $.ajax({
            type: "Get",
            url: "/APSE/WBS/GetProjectWBSTree",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {

                    if (val.Childeren.length) {
                        g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '8/15/2008', '8/24/2008', 'ff0000', '', 0, getLeverTypeString(val.Type), val.WF, 1, 0, 1, val.Name));
                        recuresiveChildsWf(val.Childeren, wbstypes);
                    }
                    else {
                        g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '8/15/2008', '8/24/2008', 'ff0000', '', 0, getLeverTypeString(val.Type), val.WF, 0, 0, 1, val.Name));
                    }
                });
                $('#wbs-creator-loading').hide();
                initializWfItems();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function recuresiveChildsWf(child,wbstypes) {
        $.each(child, function (j, val) {

            if (val.Childeren.length) {
                g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '7/20/2008', '7/20/2008', 'f600f6', '', 0, getLeverTypeString(val.Type), val.WF, 1, val.ParentId, 0, val.Name));

                recuresiveChildsWf(val.Childeren, wbstypes);
            }
            else {
                g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '7/20/2008', '7/20/2008', 'f600f6', '', 0, getLeverTypeString(val.Type), val.WF, 0, val.ParentId, 0, val.Name));

            }
        });
    }

    function initializWfItems() {
        g.Draw('WBS Code', 'Duration', 'Wf(%)', 'Start Date', 'End Date', 'Name','NextName','NextName');

        g.HidenRightside();

        $('#leftside').css('width', '100%');
        $('#theTable-firsttd').css('width', '100%');
        $('#theTable-second').css('width', '100%');

        $('#jsgantt-bottom').hide();
        var wh = window.innerHeight * 0.63;
        $('#theTable-second').DataTable(
            {
                scrollY: wh + "px",
                fixedHeader: true,
                colReorder: true,
                autoWidth: true
            });
    }

    function getLeverTypeString(level) {
        var str = "";
        switch (level) {
            case 1001:
                str = 'Project';
                break;
            case 1002:
                str = 'Work Package';
                break;
            case 1003:
                str = 'Location';
                break;
            case 1004:
                str = 'Descipline';
                break;
            case 1005:
                str = 'System';
                break;
            case 1006:
                str = 'SubSystem';
                break;
            case 1007:
                str = 'MDR';
                break;
        }
        return str;
    }

    //wbs line
    function getProjectwbsLine(types) {

        $.ajax({
            type: "Get",
            url: "/APSE/WBS/GetProjectWBSTree/",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    createProjectNode(val.Name, val.Id, val.WBSCode, types);
                    recuresiveChilds(val.Childeren, types);
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createProjectNode(name, id, wbsCode, types) {
        var targetId = "realnode-" + id;
        var $li = $('#first-li-wbs-real');
        var $a = $("<a id='" + targetId + "' data-wbsCode='" + wbsCode + "' class='drop-wbs'><p  style='width:60px;height:10px'>" + name + "</p><p class='html-wbsCode' style='width:60px;height:10px'>" + wbsCode + "</p></a>");
        var $ul = $(' <ul id="dr-ul-1001"></ul>');
        $li.append($a);
        $ul.append($ul);

        $a.droppable(
            {
                drop: function (event, ui) {
                    var id = ui.draggable.attr("id").split('-')[1];
                    var name = ui.draggable.attr("data-name");
                    var dropId = $(this).attr('id');

                    $('#wbs-edit-title').html("Add WBS Item (" + name + ")");

                    $('#selected-wbs-name').val(name);
                    $('#selected-wbs-targetid').val(id);

                    $('#drop-wbs-id').val(dropId);
                    createWbsChild(types);
                    //$('#modal-default-wbsEdit').modal('show');
                }
            }
        );
    }

    function recuresiveChilds(child, wbstypes) {

        $.each(child, function (j, item) {
            if (wbstypes.indexOf(item.Type) > -1) {
                var objParent = $("#realnode-" + item.ParentId + "");
                createTrWBSModel(item.Name, item.Id, item.WBSCode, objParent, item.Type, wbstypes);
                if (item.Childeren.length) {
                    recuresiveChilds(item.Childeren, wbstypes);
                }
            }
        });
    }

    function createTrWBSModel(name, id, wbsCode, dropItem, type, wbsTypes) {

        name = name.split('(')[0];
        var targetId = "realnode-" + id;

        var newParent = $(dropItem).parent();

        if (newParent.children('ul').length) {
            var parentUl = newParent.children('ul').first();

            var $li = $("<li></li>");
            var $a = $("<a id='" + targetId + "' data-wbsCode='" + wbsCode + "' class='drop-wbs'><p style='width:auto;height:10px'>" + name + "</p><p class='html-wbsCode text-center'  style='width:auto;height:10px'>" + wbsCode + "</p></a>");

            $li.append($a);
            if (type == 1005) {
                var $p = $("<div><p data-sbId='" + targetId + "' class='btn btn-default sh-subsystem'><span class= 'badge bg-yellow' ><i class='fa fa-arrow-down'></i></span ></p></div>");

                $li.append($p);
            }

            parentUl.append($li);
        }
        else {
            var $ul = $("<ul></ul>");
            var $li = $("<li style='margin-left:-45px;'></li>");
            var $a = $("<a id='" + targetId + "' class='drop-wbs' data-wbsCode='" + wbsCode + "'><p style='width:auto;height:10px'>" + name + "</p><p class='html-wbsCode text-center' style='width:auto;height:10px'>" + wbsCode + "</p></a>");
            $li.append($a);
            if (type == 1005 /*&& !(wbsTypes.indexOf(1006) > -1)*/) {
                var $p = $("<div><p data-sbId='" + targetId + "' class='btn btn-default sh-subsystem'><span class= 'badge bg-yellow' ><i class='fa fa-arrow-down'></i></span ></p></div>");

                $li.append($p);
            }

           
            $ul.append($li);

            newParent.append($ul);
        }

        $a.droppable(
            {
                classes: {
                    "ui-droppable-active": "ui-state-active",
                    "ui-droppable-hover": "ui-state-hover"
                },
                drop: function (event, ui) {
                    var id = ui.draggable.attr("id").split('-')[1];
                    var name = ui.draggable.attr("data-name");
                    var dropId = $(this).attr('id');

                    $('#wbs-edit-title').html("Add WBS Item (" + name + ")");

                    $('#selected-wbs-name').val(name);
                    $('#selected-wbs-targetid').val(id);

                    $('#drop-wbs-id').val(dropId);
                    createWbsChild(wbsTypes);
                    //$('#modal-default-wbsEdit').modal('show');
                }
            }
        );
    }


    function createWbsChild(types) {
        var parentNode = $('#drop-wbs-id').val();
        var parentId = $('#drop-wbs-id').val().split('-')[1];
        var dropItem = document.getElementById(parentNode);

        var name = $('#selected-wbs-name').val();
        var targetId = $('#selected-wbs-targetid').val();

        var type = $('#wbs-type').val();

        var model = { 'Type': type, 'TargetId': targetId, 'Wf': 0, 'Name': name, 'ParentId': parentId };

        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "Post",
            url: "/APSE/WBS/CreateProjectWBS",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    if (data.model != null) {
                        $.each(data.model, function (i, val) {
                            var objParent = $("#realnode-" + val.parentId + "");
                            if ((types.indexOf(val.type) > -1)) {
                                createTrWBSModel(val.name, val.id, val.wbsCode, objParent, val.type,types);
                            }
                        });
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
   
    //wbs line modal last childs
    function getSystemWBsTree(types, systemId) {
        var intId = parseInt(systemId);
        $.ajax({
            type: "Get",
            url: "/APSE/WBS/GetProjectWBSTree/",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    recuresiveChilds1(val.Childeren, types, intId);
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function recuresiveChilds1(child, wbstypes, intId) {

        $.each(child, function (j, item) {
            if (item.Type < 1006) {
                if (item.Id == intId) {
                    var claculateSize = 90;
                    createSubSystemNode(item.Name, item.Id, item.WBSCode);
                    $.each(item.Childeren, function (j, vt) {
                        var objParent = $("#sysId-" + vt.ParentId + "");
                        createTrWBSModel1(vt.Name, vt.Id, vt.WBSCode, objParent, vt.Type, wbstypes);
                        claculateSize += 90;
                    })

                    if (claculateSize < window.innerWidth) {

                        $('.modal-lg').width(claculateSize);
                    }
                    else {

                        $('.modal-lg').width(window.innerWidth-100);
                    }
                }
                else {
                    if (item.Childeren.length) {
                        recuresiveChilds1(item.Childeren, wbstypes, intId);
                    }
                }
            }
            else {
                if (item.Childeren.length) {
                    recuresiveChilds1(item.Childeren, wbstypes, intId);
                }
            }
        });
    }

    function createSubSystemNode(name, id, wbsCode) {
        name = name.split('(')[0];
        var targetId = "sysId-" + id;
        var $li = $('#first-li-wbs1');
        var $a = $("<a id='" + targetId + "' class='drop-wbs'><p  style='width:60px;height:10px'>" + name + "</p><p class='html-wbsCode text-center' style='width:auto;height:10px'>" + wbsCode + "</p></a>");
        var $ul = $('<ul id="pr-ul-1001"></ul>');
        $li.append($a);
        $ul.append($ul);
    }

    function createTrWBSModel1(name, id, wbsCode, dropItem, type, wbsTypes) {

        name = name.split('(')[0];
        var targetId = "sysId-" + id;

        var newParent = $(dropItem).parent();

        if (newParent.children('ul').length) {
            var parentUl = newParent.children('ul').first();

            var $li = $("<li></li>");
            var $a = $("<a id='" + targetId + "' data-wbsCode='" + wbsCode + "' class='drop-wbs'><p style='width:60px;height:10px'>" + name + "</p><p class='html-wbsCode text-center' style='width:60px;height:10px'>" + wbsCode + "</p></a>");
            if (type == 1005 && !(wbsTypes.indexOf(1006) > -1)) {
                var $p = $("<div><p data-sbId='" + targetId + "' class='btn btn-default sh-subsystem'><span class= 'badge bg-yellow' ><i class='fa fa-arrow-down'></i></span ></p></div>");
            }

            $li.append($a);
            $li.append($p);
            parentUl.append($li);
        }
        else {
            var $ul = $("<ul></ul>");
            var $li = $("<li></li>");
            var $a = $("<a id='" + targetId + "' class='drop-wbs' data-wbsCode='" + wbsCode + "'><p style='width:60px;height:10px'>" + name + "</p><p class='html-wbsCode text-center' style='width:60px;height:10px'>" + wbsCode + "</p></a>");
            if (type == 1005 && !(wbsTypes.indexOf(1006) > -1)) {
                var $p = $("<div><p data-sbId='" + targetId + "' class='btn btn-default sh-subsystem'><span class= 'badge bg-yellow' ><i class='fa fa-arrow-down'></i></span ></p></div>");
            }

            $li.append($a);
            $li.append($p);
            $ul.append($li);

            newParent.append($ul);
        }
    }

    function dropwbs(event, ui)
    {
        var id = ui.draggable.attr("id").split('-')[1];
        var name = ui.draggable.attr("data-name");
        var dropId = $(this).attr('id');

        $('#wbs-edit-title').html("Add WBS Item (" + name + ")");

        $('#selected-wbs-name').val(name);
        $('#selected-wbs-targetid').val(id);
        $('#drop-wbs-id').val(dropId);

        $('#modal-default-wbsEdit').modal('show');
    }

    //
    function getProjectWorks() {
        $.ajax({
            type: "Get",
            url: "/APSE/WorkPackage/GetRoadMaps",
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

    function getProjectLocation() {

        $.ajax({
            type: "Get",
            url: "/APSE/WorkPackage/GetLocations",
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

    function getProjectDesciplines() {
        $.ajax({
            type: "Get",
            url: "/APSE/Descipline/GetDesciplineList",
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

    function getProjectSystems() {
        $.ajax({
            type: "Get",
            url: "/APSE/ProjectSystem/GetProjectSystems",
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

    function getProjectSubSystems() {
        $.ajax({
            type: "Get",
            url: "/APSE/SubSystem/GetProjectSubSystems",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                releaseInitialization();
                $.each(data, function (i, val) {
                    createTrSubSystem(val, i);
                });
                elementInitialization();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createTr(model, count) {
        count = count + 1;
        var tName = model.title;
       
        if (model.description) {
            tName += "(" + model.description+")";
        }

        var tr = $("<tr id='tr-" + model.id + "' data-name='" + tName + "' ></tr>");
        //var tdCount = $("<td>" + count + "</td>");

        //tr.append(tdCount);
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");

        tr.append("<td>" + tName + "</td>");

        tr.draggable({
            appendTo: 'body',
            helper: function () {
                //drag selected items
                var selected = $(this);
                var container = $('<div/>');
                container.append(selected.clone());
                return container;
            }
        });

        $('#model-items').append(tr);
    }

    function createTrSubSystem(model, count) {
        count = count + 1;
        var tName = model.title;

        if (model.description) {
            tName += "(" + model.description + ")";
        }

        var tr = $("<tr id='tr-" + model.id + "' data-name='" + tName + "' ></tr>");
        //var tdCount = $("<td>" + count + "</td>");

        //tr.append(tdCount);
        //tr.append("<td><label><input type='checkbox' class='flat-red wbs-ch-type' id='wbs-ch-" + model.id + "'></label></td>");

        tr.append("<td>" + tName + "</td>");

        tr.draggable({
            appendTo: 'body',
            helper: function () {
                //drag selected items
                var selected = $(this);
                var container = $('<div/>');
                container.append(selected.clone());
                return container;
            }
        });

        $('#model-items').append(tr);
    }

    function showEditWbsNode(id, wbsCode) {
        var splitId = id.split('-')[1];
        $('#modal-default-overlay').modal('show');
        $.ajax({
            type: "Get",
            url: "/APSE/WBS/GetProjectWBS?wbsId=" + splitId,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                $('#delete-wbs-id').val(id);
                $('#Wbscodeedit1').val(data.wbsCode);
                $('#calType').val(data.calculationType);
                $('#modal-default-wbsDelete').modal('show');
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function editWbsCode() {
        var id = $('#delete-wbs-id').val().split('-')[1];
        var wbsCode = $('#Wbscodeedit1').val();
        var calType = $('#calType').val();

        if (wbsCode.length <= 0) {
            alert('wbs code is required!!!');
            return;
        }

        var model = {'id': id, 'WBSCode': wbsCode, 'CalculationType': calType};

        $('#modal-default-overlay').modal('show');

        $.ajax({
            type: "PUT",
            url: "/APSE/WBS/EditProjectWBS",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    $('#modal-default-wbsDelete').modal('toggle');
                    var targetId = 'realnode-' + id;
                    var item = $('#' + targetId).children('p').eq(1).html(wbsCode);
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


    function deleteWbsCode() {
        var id = $('#delete-wbs-id').val().split('-')[1];
      
        $('#modal-default-overlay').modal('show');
        $.ajax({
            type: "Delete",
            url: "/APSE/WBS/DeleteProjectWBS/"+id+"",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {
                    $('#modal-default-wbsDelete').modal('toggle');
                    var obj = $("#realnode-" + id).parent();
                    obj.remove();
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


    function releaseInitialization() {
        var datatablet = $('#wbsCreatorTable').DataTable();
        datatablet.clear().draw();
        datatablet.destroy();
    }

    function elementInitialization() {

        var datatablet = $('#wbsCreatorTable').DataTable(
            {
                "info": false,
                pagingType:'simple'
            });

        $('.dataTables_filter input[type="search"]').css(
            { 'width': '50px', 'display': 'inline-block' }
        );
    }

}());