var g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');

var WBSWfActivityConfig = WBSWfActivityConfig || (function () {

    return {
        init: function (toProgress) {
            initialization(toProgress);
        },
        updateAllActivityWbsWf: function (calType) {
            updateAllActivityWbsWf(calType);
        },
        lastwbsChildClick: function (element) {
            lastwbsChildClick(element);
        }
    };

    function initialization(toProgress) {
        if (toProgress) {
            g.setShowProgress(1);
        }
        else {
            g.setShowProgress(0);
        }
       
        g.setShowInput(0);
        g.setShowActivityCount(1);
        g.setShowRes(0);
        g.setShowDur(0);
        g.setShowComp(1);
        g.setShowStartDate(0);
        g.setShowEndDate(0);
        g.setCaptionType('Resource');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)

        if (g) {

            if (g != null) {
                g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
                if (toProgress) {
                    g.setShowProgress(1);
                }
                else {
                    g.setShowProgress(0);
                }

                g.setShowInput(0);
                g.setShowActivityCount(1);
                g.setShowRes(0);
                g.setShowDur(0);
                g.setShowComp(1);
                g.setShowStartDate(0);
                g.setShowEndDate(0);
                g.setCaptionType('Resource');
            }

            getProjectwbsWfTree(toProgress);
        }
        else {
            alert("not defined");
        }
    }

    function getProjectwbsWfTree(toProgress) {
        $('#wbs-wfCalculate-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetProjectWBSTreeToProgress?toProgress=" + toProgress + "",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {

                    if (val.Childeren.length) {
                        g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '8/15/2008', '8/24/2008', 'ff0000', '', 0, getLeverTypeString(val.Type), val.WF, 1, 0, 1, val.Name, val.Progress,val.ActivityCount));
                        recuresiveChildsWf(val.Childeren);
                    }
                    else {
                        g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '8/15/2008', '8/24/2008', 'ff0000', '', 0, getLeverTypeString(val.Type), val.WF, 0, 0, 1, val.Name, val.Progress, val.ActivityCount));
                    }
                })

                $('#wbs-wfCalculate-loading').hide();
                initializWfItems();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function updateAllActivityWbsWf(calType) {
        $('#modal-default-overlay').modal('show');
        var model = { 'CalType': calType };

        $.ajax({
            type: "PUT",
            url: "/APSE/WBS/UpdateAllActivityWf",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(model),
            success: function (data, status, jqXHR) {
                $('#modal-default-overlay').modal('toggle');
                if (data.key == 200) {

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

    function recuresiveChildsWf(child) {

        $.each(child, function (j, val) {

            if (val.Childeren.length) {
                g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '7/20/2008', '7/20/2008', 'f600f6', '', 0, getLeverTypeString(val.Type), val.WF, 1, val.ParentId, 0, val.Name, val.Progress, val.ActivityCount));

                recuresiveChildsWf(val.Childeren);
            }
            else {
                g.AddTaskItem(new JSGantt.TaskItem(val.Id, val.WBSCode, '7/20/2008', '7/20/2008', 'f600f6', '', -100, getLeverTypeString(val.Type), val.WF, 0, val.ParentId, 0, val.Name, val.Progress, val.ActivityCount));
                //g.AddTaskItem(new JSGantt.TaskItem(val.Id*10, 'ddd', '7/20/2008', '7/20/2008', 'f600f6', '', 0, 'ddd', 0, 0, val.Id, 0, '0'));
            }
        });
    }

    function initializWfItems() {
        g.Draw('Level Type', 'Duration', 'Wf(%)', 'Start Date', 'End Date', 'Name', 'Progress(%)','Activity Count');
      
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
            default:
                "Activity"
        }
        return str;
    }
}());