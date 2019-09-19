var g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
var planWorkFlow = planWorkFlow || (function () {

    return {
        grantinitialization: function () {
            grantinitialization();
        }
    }

    function grantinitialization() {
        g.setShowRes(1); // Show/Hide Responsible (0/1)
        g.setShowDur(1); // Show/Hide Duration (0/1)
        g.setShowComp(1); // Show/Hide % Complete(0/1)
        g.setCaptionType('Resource');  // Set to Show Caption
        g.setShowStartDate(1);
        g.setShowEndDate(1);
        if (g) {

            if (g != null) {
                g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
                g.setShowRes(1); // Show/Hide Responsible (0/1)
                g.setShowDur(1); // Show/Hide Duration (0/1)
                g.setShowComp(1); // Show/Hide % Complete(0/1)
                g.setCaptionType('Resource');  // Set to Show Caption
                g.setShowStartDate(1);
                g.setShowEndDate(1);
            }

            getPlanHirechary();
        }
        else {
            alert("not defined");
        }
    }

    function getPlanHirechary() {
        var filterTypes = [];

        if ($('#gr-1000').iCheck('update')[0].checked) {
            filterTypes.push(1000);
        }

        if ($('#gr-1001').iCheck('update')[0].checked) {
            filterTypes.push(1001);
        }

        if ($('#gr-1002').iCheck('update')[0].checked) {
            filterTypes.push(1002);
        }

        if ($('#gr-1003').iCheck('update')[0].checked) {
            filterTypes.push(1003);
        }
        if ($('#gr-1004').iCheck('update')[0].checked) {
            filterTypes.push(1004);
        }
        $.ajax({
            type: "Get",
            url: "/APSE/Project/GetPlanHirechary?filterTypes=" + filterTypes,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.length > 0) {
                   
                    $.each(data, function (i, val) {
                        if (val.childeren.length) {
                            g.AddTaskItem(new JSGantt.TaskItem(val.id, val.title, '', '', 'ff0000', '', 0, val.title, 0, 1, 0, 1));
                            recuresiveChildsPlane(val.childeren);
                        }
                        else {
                            g.AddTaskItem(new JSGantt.TaskItem(val.id, val.title, '', '', 'ff0000', '', 0, val.title, 0, 1, 0, 0));
                        }
                    });
                    initializWfItems();
                }
                $('#nothing-plan').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function recuresiveChildsPlane(child) {

        $.each(child, function (j, val) {
            var comp = parseFloat(((val.done / val.total) * 100).toFixed(2));
            var color = 'e61a2e';
            if (comp > 0 && comp <= 30) {
                var color = 'ff9900';
            }
            else if (comp > 30 && comp <= 50) {
                var color = 'ffff66';
            }
            else if (comp > 50 && comp <= 75) {
                var color = '33ccff';
            }
            else if (comp > 75 && comp <= 95) {
                var color = '66ff99';
            }
            else if (comp > 95) {
                var color = '00cc00';
            }

            if (val.childeren.length) {
                g.AddTaskItem(new JSGantt.TaskItem(val.id, val.title, val.startDate, val.endDate, color, '', 0, val.title, comp, 1, val.parentId, 1));
                recuresiveChildsPlane(val.childeren);
            }
            else {
                g.AddTaskItem(new JSGantt.TaskItem(val.id, val.title, val.startDate, val.endDate, color, '', 0, val.resources, comp, 0, val.parentId,0));
            }
        });
    }

    function initializWfItems() {
        g.Draw();
        g.DrawDependencies();

        $('#leftside').css('width', '100%');
        $('#theTable-firsttd').css('width', '100%');
        $('#theTable-second').css('width', '100%');
        $('#jsgantt-bottom').hide();
    }

}());
