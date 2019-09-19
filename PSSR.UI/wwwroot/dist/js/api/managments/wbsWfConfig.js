var g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');

var WBSWfConfig = WBSWfConfig || (function () {


    return {
        init: function () {
            initialization();
        },
        updatewbswf: function (obj) {
            updatewbswf(obj)
        }
    };

    function initialization() {
        g.setShowActivityCount(1);
        g.setShowInput(1);
        g.setShowRes(0);
        g.setShowDur(0);
        g.setShowComp(1);
        g.setShowStartDate(0);
        g.setShowEndDate(0);
        g.setCaptionType('Resource');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)

        if (g) {

            if (g != null) {
                g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
                g.setShowActivityCount(1);
                g.setShowInput(1);
                g.setShowRes(1);
                g.setShowDur(0);
                g.setShowComp(1);
                g.setShowStartDate(0);
                g.setShowEndDate(0);
                g.setCaptionType('Resource');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)
            }

            getProjectwbsWfTree();
        }
        else {
            alert("not defined");
        }
    }

    function getProjectwbsWfTree() {
        $.ajax({
            type: "Get",
            url: "/poec/Dashboard/GetProjectWBSTreeToProgress?toProgress=false",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {

                    if (val.Childeren.length) {
                        g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '8/15/2008', '8/24/2008', 'ff0000', '', 0, getLeverTypeString(val.Type), val.WF, 1, 0, 1, val.Name, 0, val.ActivityCount));
                        recuresiveChildsWf(val.Childeren);
                    }
                    else {
                        g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '8/15/2008', '8/24/2008', 'ff0000', '', 0, getLeverTypeString(val.Type), val.WF, 0, 0, 1, val.Name, 0, val.ActivityCount));
                    }
                })
                initializWfItems();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function recuresiveChildsWf(child) {

        $.each(child, function (j, val) {

            if (val.Childeren.length) {
                g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '7/20/2008', '7/20/2008', 'f600f6', '', 0, getLeverTypeString(val.Type), val.WF, 1, val.ParentId, 0, val.Name, 0, val.ActivityCount));
                recuresiveChildsWf(val.Childeren);
            }
            else {
                g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '7/20/2008', '7/20/2008', 'f600f6', '', 0, getLeverTypeString(val.Type), val.WF, 0, val.ParentId, 0, val.Name, 0, val.ActivityCount));

            }
        });
    }

    function initializWfItems() {
        g.Draw('WBS Code', 'Duration', 'Wf(%)', 'Start Date', 'End Date', 'Name', 'Type', 'Activity Count');

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

    function updatewbswf(obj) {

        var val = $(obj).val();
        var id = $(obj).attr('id').split('-')[2];
        obj.addClass('inputLoading');

        var model = { 'key': id, 'value': val };

        $.ajax({
            type: "PUT",
            url: "/poec/WBS/EditProjectWBSWF",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    obj.removeClass('inputLoading');
                }
                else {
                    obj.removeClass('inputLoading');
                    $('#global-error-message').html(data.value);
                    $('#modal-danger').modal('show');
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
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


}());