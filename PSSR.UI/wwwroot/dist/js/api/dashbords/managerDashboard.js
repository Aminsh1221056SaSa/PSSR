var managerDashboard = managerDashboard || (function () {

    return {
        init: function (desciplineReport, workPackageStepReport) {
            Initialization(desciplineReport, workPackageStepReport);
            getProjectMdrSummary();
        }
    };

    function Initialization(desciplineReport, workPackageStepReport) {
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/ManagerDashboardInitializationW1",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    if (data.workPackages != null) {
                        initProjectData(data.project);
                        var wok1 = data.workPackages[0];
                        if (wok1 != null) {
                            initWorkPackagesPane1Title(wok1);
                            var loc1 = selectWhere(data.locations, "parentId", wok1.id);
                            initLocationW1Tab(loc1, wok1);
                            initW1Details(wok1.id);
                            var w1content = "desciplineStatusPreCom-" + 0 + "";
                            desciplineReport.paintingDesciplineActivityBarStatus(wok1.id, 0, w1content);
                            $('#cu-workDes-id').val(wok1.id);
                            $('#cu-des-id').val(0);
                        }

                        var wok2 = data.workPackages[1];

                        if (wok2 != null) {
                            initWorkPackagesPane2Title(wok2);

                            var loc2 = selectWhere(data.locations, "parentId", wok2.id);
                            initLocationW2Tab(loc2, wok2);
                            initW2Details(wok2.id);
                            var w2content = "desciplineStatusCom-" + 0 + "";
                            workPackageStepReport.paintingWorkStepActivityBarStatus(wok2.id, 0, w2content);

                            $('#cu-workSt-id').val(wok2.id);
                            $('#cu-wst-id').val(0);
                        }
                    }
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getProjectMdrSummary()
    {
        var content = $('#model-managerMdr');
        content.empty();

        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetProjectMDRSummary",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var tr = $('<tr></tr>');
                    var name = $("<td><a href='" + val.link + "'><p>" + val.name + "</p></a></td>");
                    var total = $("<td><p>" + val.total + "</p></td>");
                    var done = $("<td><p>" + val.done + "</p></td>");
                    tr.append(name).append(total).append(done);
                    content.append(tr);
                });
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function initProjectData(project) {
        $('#pr-d-0').text(project.description);
        $('#pr-d-1').text(project.startDate);
        $('#pr-d-2').text(project.endDate);
        $('#pr-d-3').text(project.elapsedDate);
        $('#pr-d-4').text(project.remainedDate);

        $('#pr-d-5').text(project.desciplinesCount);
        $('#pr-d-6').text(project.systemsCount);
        $('#pr-d-7').text(project.subSystemsCount);
        $('#pr-d-8').text(project.activitysCount);
    }

    //w1

    function initWorkPackagesPane1Title(workPackage) {
        var content = $('#wd-1');
        content.css('color', '#FFF');
        var innercontent = $('#wd-title-1');
        content.attr('data-workId', workPackage.id);
        innercontent.html(workPackage.title + " Database");

        var url = content.attr("href");
        url = url.replace("dummyDate", workPackage.id);
        content.attr("href", url);
    }

    function initLocationW1Tab(locations) {

        var cHeader = $('#w1-header');
        var cBody = $('#w1-body');
        var mainContent = 'desciplineStatusPreCom-0';

        var th = $("<li class='active'><a data-wId='th-" + 0 + "' class='tab-header-w1' data-id='th-w1-" + 0 + "'  href='#tab_w1_" + 0 + "' data-toggle='tab'>All</a></li>");
        var tb = $("<div class='tab-pane active location-tab-pane' id='tab_w1_" + 0 + "'><div id=" + mainContent + " style='height: 300px; margin: 0 auto'></div></div>");

        cHeader.append(th);
        cBody.append(tb);

        $.each(locations, function (i, val) {
            if (i == 0) {
                var th = $("<li><a class='tab-header-w1'  data-id='th-w1-" + val.id + "'  href='#tab_w1_" + val.id + "' data-toggle='tab'>" + val.name + "</a></li>");
            }
            else {
                var th = $("<li><a class='tab-header-w1' data-id='th-w1-" + val.id + "'  href='#tab_w1_" + val.id + "' data-toggle='tab'>" + val.name + "</a></li>");
            }

            var tb = $("<div class='tab-pane location-tab-pane' id='tab_w1_" + val.id + "'><div id='desciplineStatusPreCom-" + val.id + "' style='height: 300px; margin: 0 auto'></div></div>");
            cHeader.append(th);
            cBody.append(tb);
        });
       
    }

    function initW1Details(workPackageId) {
        $('#w1-actual-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetActivityDetailsByWorkPackage?workPackageId=" + workPackageId +"&groupType="+1 ,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.actualReport != null) {
                    var content = $('#w1-actual-report');
                    content.empty();

                    $.each(data.actualReport, function (i, val) {
                        if (val.isBolded == true) {
                            var bgColor = "bg-red"
                            if (val.value > 0 && val.value <= 30) {
                                bgColor = "bg-orange"
                            }
                            else if (val.value > 30 && val.value <= 50) {
                                bgColor = "bg-yellow"
                            }
                            else if (val.value > 50 && val.value <= 90) {
                                bgColor = "bg-blue"
                            }
                            else if (val.value > 90) {
                                bgColor = "bg-green"
                            }

                            content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge " + bgColor + "'>" + val.value.toFixed(2) + " %</span></a></li>");
                        }
                        else {
                            if (val.isTitle == true) {
                                content.append("<li><a style='font-weight:bold' href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-red'>" + val.value + "</span></a></li>");
                            }
                            else {
                                content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-gray'>" + val.value + "</span></a></li>");
                            }
                        }
                    })
                }

                if (data.planeReport != null)
                {
                    var content = $('#w1-plane-report');
                    content.empty();
                    $.each(data.planeReport, function (i, val) {
                        if (val.isBolded == true) {
                            var bgColor = "bg-red"
                            if (val.value > 0 && val.value <= 30) {
                                bgColor = "bg-orange"
                            }
                            else if (val.value > 30 && val.value <= 50) {
                                bgColor = "bg-yellow"
                            }
                            else if (val.value > 50 && val.value <= 90) {
                                bgColor = "bg-blue"
                            }
                            else if (val.value > 90) {
                                bgColor = "bg-green"
                            }

                            content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge " + bgColor + "'>" + val.value.toFixed(2) + " %</span></a></li>");
                        }
                        else {
                            if (val.isTitle == true) {
                                content.append("<li><a style='font-weight:bold' href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-green'>" + val.value + "</span></a></li>");
                            }
                            else {
                                content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-gray'>" + val.value + "</span></a></li>");
                            }
                        }
                    })
                }

                if (data.groupReport != null)
                {
                    var content = $('#DACCheckerTable');
                    content.empty();

                    var donSum = 0;
                    var totalSum = 0;

                    $.each(data.groupReport, function (i, val) {
                        var $tr = $('<tr></tr>');
                        $tr.append("<td><a href='" + val.link + "'><p>" + val.title + " DAC</p></a></td>");
                        $tr.append("<td><p>" + val.total + "</p></td>");
                        $tr.append("<td><p>" + val.done + "</p></td>");
                        donSum += val.done;
                        totalSum += val.total;
                        content.append($tr);
                    });
                 
                    var footerContent = $('#DACCheckerTable-footer');
                    footerContent.empty();
                    var $ftr = $('<tr></tr>');
                    $ftr.append("<td><p>Total RFC</p></td>");
                    $ftr.append("<td><p>" + totalSum + "</p></td>");
                    $ftr.append("<td><p>" + donSum+ "</p></td>");
                    footerContent.append($ftr);
                }
                $('#w1-actual-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }


    //w2

    function initWorkPackagesPane2Title(workPackage) {
        var content = $('#wd-2');
        content.css('color', '#FFF');
        var innercontent = $('#wd-title-2');
        content.attr('data-workId', workPackage.id);
        innercontent.html(workPackage.title + " Database");

        var url = content.attr("href");
        url = url.replace("dummyDate", workPackage.id);
        content.attr("href", url);
    }

    function initLocationW2Tab(locations) {
        var cHeader = $('#w2-header');
        var cBody = $('#w2-body');
        var mainContent = 'desciplineStatusCom-0';

        var th = $("<li class='active'><a data-wId='th-" + 0 + "' class='tab-header-w2' data-id='th-w2-" + 0 + "'  href='#tab_w2_" + 0 + "' data-toggle='tab'>All</a></li>");
        var tb = $("<div class='tab-pane active location-tab-pane' id='tab_w2_" + 0 + "'><div id=" + mainContent + " style='height: 300px; margin: 0 auto'></div></div>");

        cHeader.append(th);
        cBody.append(tb);

        $.each(locations, function (i, val) {
            if (i == 0) {
                var th = $("<li><a class='tab-header-w2'  data-id='th-w2-" + val.id + "'  href='#tab_w2_" + val.id + "' data-toggle='tab'>" + val.name + "</a></li>");
            }
            else {
                var th = $("<li><a class='tab-header-w2' data-id='th-w2-" + val.id + "'  href='#tab_w2_" + val.id + "' data-toggle='tab'>" + val.name + "</a></li>");
            }

            var tb = $("<div class='tab-pane location-tab-pane' id='tab_w2_" + val.id + "'><div id='desciplineStatusCom-" + val.id + "' style='height: 300px; margin: 0 auto'></div></div>");
            cHeader.append(th);
            cBody.append(tb);
        });
    }

    function initW2Details(workPackageId) {
        $('#w2-actual-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetActivityDetailsByWorkPackage?workPackageId=" + workPackageId + "&groupType=" + 2,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.actualReport != null) {
                    var content = $('#w2-actual-report');
                    content.empty();

                    $.each(data.actualReport, function (i, val) {
                        if (val.isBolded == true) {
                            var bgColor = "bg-red"
                            if (val.value > 0 && val.value <= 30) {
                                bgColor = "bg-orange"
                            }
                            else if (val.value > 30 && val.value <= 50) {
                                bgColor = "bg-yellow"
                            }
                            else if (val.value > 50 && val.value <= 90) {
                                bgColor = "bg-blue"
                            }
                            else if (val.value > 90) {
                                bgColor = "bg-green"
                            }

                            content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge " + bgColor + "'>" + val.value.toFixed(2) + " %</span></a></li>");
                        }
                        else {
                            if (val.isTitle == true) {
                                content.append("<li><a style='font-weight:bold' href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-red'>" + val.value + "</span></a></li>");
                            }
                            else {
                                content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-gray'>" + val.value + "</span></a></li>");
                            }
                        }
                    })
                }

                if (data.planeReport!= null) {
                    var content = $('#w2-plane-report');
                    content.empty();
                    $.each(data.planeReport, function (i, val) {
                        if (val.isBolded == true) {
                            var bgColor = "bg-red"
                            if (val.value > 0 && val.value <= 30) {
                                bgColor = "bg-orange"
                            }
                            else if (val.value > 30 && val.value <= 50) {
                                bgColor = "bg-yellow"
                            }
                            else if (val.value > 50 && val.value <= 90) {
                                bgColor = "bg-blue"
                            }
                            else if (val.value > 90) {
                                bgColor = "bg-green"
                            }

                            content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge " + bgColor + "'>" + val.value.toFixed(2) + " %</span></a></li>");
                        }
                        else {
                            if (val.isTitle == true) {
                                content.append("<li><a style='font-weight:bold' href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-green'>" + val.value + "</span></a></li>");
                            }
                            else {
                                content.append("<li><a href=" + val.link + ">" + val.title + " <span class='pull-right badge bg-gray'>" + val.value + "</span></a></li>");
                            }
                        }
                    })
                }

                if (data.groupReport != null)
                {
                    var content = $('#systemRFCTable');
                    content.empty();

                    var donSum = 0;
                    var totalSum = 0;

                    $.each(data.groupReport, function (i, val) {
                        var $tr = $('<tr></tr>');
                        $tr.append("<td><a href='" + val.link + "'><p>" + val.title + "</p></a></td>");
                        $tr.append("<td><p>" + val.total + "</p></td>");
                        $tr.append("<td><p>" + val.done + "</p></td>");
                        donSum += val.done;
                        totalSum += val.total;
                        content.append($tr);
                    });
                    var footerContent = $('#systemRFCTable-footer');
                    footerContent.empty();
                    var $ftr = $('<tr></tr>');
                    $ftr.append("<td><p>Total RFS</p></td>");
                    $ftr.append("<td><p>" + totalSum + "</p></td>");
                    $ftr.append("<td><p>" + donSum + "</p></td>");
                    footerContent.append($ftr);
                }

                $('#w2-actual-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    //global
    function selectWhere(data, propertyName, equality) {
        var obj=[];
        for (var i = 0; i < data.length; i++) {
            if (data[i][propertyName] == equality) {
                obj.push(data[i]);
            }
        }
        return obj;
    }
}());