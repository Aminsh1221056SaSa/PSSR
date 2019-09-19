var workPackageReport = workPackageReport || (function () {
    return {
        init: function () {
            
        },
        getWorkPackageContent: function(id,locId) {
            getWorkPackageContent(id, locId);
        },
        getWorkPackageDesciplineReport: function (id, locId) {
            getWorkPackageDesciplineReport(id, locId);
        },
        getSystemWorkPackageReport: function (id, locId) {
            getSystemWorkPackageReport(id, locId);
        },
        getWorkPackageWrokStepReport: function (id, locId) {
            getWorkPackageWrokStepReport(id, locId);
        }
    };

    function getWorkPackageContent(id, locId) {
        var dUrl = $('#tab-body').attr('data-pathUrl');
        var pane = $("#tab_report");
        pane.empty();
        $.get(dUrl + "?wId=" + id + "&locId=" + locId, function (data) {
            pane.append(data);
        });
    }

    function getWorkPackageDesciplineReport(id, locId) {
        $('#descipline-summary-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetDesciplineWorkPackageReport?wId="+id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                initLocationsTable(data, id, locId,"DesciplineId");
                $('#descipline-summary-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getWorkPackageWrokStepReport(id, locId) {
        $('#descipline-summary-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetWorkPackageStepProgress?wId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                initLocationsTable(data, id, locId,"WorkPackageStepId");
                $('#descipline-summary-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getSystemWorkPackageReport(id, locId) {
        $('#system-summary-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetSystemWorkPackageReport?wId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                initLocationsTableSystem(data, id, locId);
                $('#system-summary-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    //
    function initLocationsTable(data, wid,locId,target)
    {
        var workSelected = $('#workPackage-type option:selected').text();

        var mx = Math.max.apply(Math, $.map(data, function (el) { return el.items.length }));
        if (mx <= 4) {
            mx = 4;
        }
        var mh = mx * 40;
        $(".progress-top").css({ 'height': mh + "px" });
        $.each(data, function (i, val) {
            var content = $("#body-loc-" + val.locationId + "");

            var twf = 0;
            var tprog = 0;
            var tpct = 0;
            content.empty();
            $.each(val.items, function (j, ds) {
                var wf = parseFloat(ds.wf).toFixed(2);
                var progress = parseFloat(ds.progress).toFixed(2);
                var pct = parseFloat(ds.pct).toFixed(2);

                twf += ds.wf;
                tprog += ds.progress;
                tpct += ds.pct;

                var tr = $("<tr></tr>");
                var td = $("<td><a href='/ProjectManagment/Activity/ActivityList?WorkPackageId=" + wid + "&LocationId=" + locId + "&" + target + "=" + ds.id + "'>" + ds.name + "</a></td>");
                var td1 = $("<td>" + wf + "</td>");
                var td2 = $("<td>" + progress + "</td>");
                var td3 = $("<td>" + pct + "</td>");

                tr.append(td).append(td1).append(td2).append(td3);
                content.append(tr);
            });

            var totalContent = $('#footLoc-' + val.locationId);
            totalContent.empty();

            var tr = $("<tr></tr>");
            var td = $("<td>Total</td>");
            var td1 = $("<td>" + parseFloat(twf).toFixed(2) + "</td>");
            var td2 = $("<td>" + parseFloat(tprog).toFixed(2) + "</td>");
            var td3 = $("<td>" + parseFloat(tpct / val.items.length).toFixed(2) + "</td>");

            tr.append(td).append(td1).append(td2).append(td3);
            totalContent.append(tr);

            //datatable initaliz
            var table = $('#tb-des-' + val.locationId);
            var name = table.attr('data-name');
            var headerstr = workSelected + ' Summary Report For ' + name;
            initDataTable(table, "SummaryReport-" + val.locationId, headerstr);
        })
    }

    function initLocationsTableSystem(data, wid, locId) {
        var workSelected = $('#workPackage-type option:selected').text();
        var mx = Math.max.apply(Math, $.map(data, function (el) { return el.items.length }));
        if (mx <= 4) {
            mx = 4;
        }
        var mh = mx * 40;
        $(".progress-bottom").css({ 'height': mh + "px" });
     
        $.each(data, function (i, val) {
            var content = $("#body-loc-sys-" + val.locationId + "");
            content.empty();

            var twf = 0;
            var tprog = 0;
            var tpct = 0;

            $.each(val.items, function (j, ds) {

                var wf = parseFloat(ds.wf).toFixed(2);
                var progress = parseFloat(ds.progress).toFixed(2);
                var pct = parseFloat(ds.pct).toFixed(2);

                twf += ds.wf;
                tprog += ds.progress;
                tpct += ds.pct;

                var tr = $("<tr></tr>");
                var td = $("<td><a href='/ProjectManagment/Activity/ActivityList?WorkPackageId=" + wid + "&LocationId=" + locId + "&SystemId=" + ds.id + "'>" + ds.name + "</a></td>");
                var td1 = $("<td>" + wf + "</td>");
                var td2 = $("<td>" + progress + "</td>");
                var td3 = $("<td>" + pct + "</td>");

                tr.append(td).append(td1).append(td2).append(td3);
                content.append(tr);
            });

            var totalContent = $('#footSys-' + val.locationId);
            totalContent.empty();

            var tr = $("<tr></tr>");
            var td = $("<td>Total</td>");
            var td1 = $("<td>" + parseFloat(twf).toFixed(2) + "</td>");
            var td2 = $("<td>" + parseFloat(tprog).toFixed(2) + "</td>");
            var td3 = $("<td>" + parseFloat(tpct / val.items.length).toFixed(2) + "</td>");

            tr.append(td).append(td1).append(td2).append(td3);
            totalContent.append(tr);

            var table = $('#tb-sys-' + val.locationId);
            var name = table.attr('data-name');
            var headerstr = workSelected + ' Summary Report For ' + name;
            initDataTable(table, "SummaryReport-" + val.locationId, headerstr);
        })
    }

    function initDataTable(table,excelFilename,header)
    {
        table.DataTable().destroy();
        var table = table.DataTable(
            {
                "paging": false,
                "info": false,
                "searching": false,
                dom: 'Bfrtip',
                buttons: [
                    //{
                    //    text: '',
                    //    extend: 'copyHtml5',
                    //    collectionLayout: 'fixed two-column ',
                    //    className: 'btn btn-info btn-sm',
                    //    postfixButtons: ['colvisRestore'],
                    //    init: function (api, node, config) {
                    //        $(node).removeClass('dt-button');
                    //        $(node).append('<p class="" style="height:8px"><i style="font-size:16px;color:#FFF;" class="fa fa-copy"></i><p>');
                    //    }
                    //},
                    {
                        text: '',
                        extend: 'excelHtml5',
                        collectionLayout: 'fixed two-column ',
                        className: 'btn btn-success btn-sm',
                        exportOptions: {
                            modifier: {
                                page: 'current'
                            },
                        },
                        init: function (api, node, config) {
                            $(node).removeClass('dt-button');
                            $(node).append('<p class="" style="height:8px"><i style="font-size:16px;color:#FFF;" class="fa fa-file-excel-o"></i><p>');
                        },
                        extension: '.xlsx',
                        filename: excelFilename,
                        messageTop: header,
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
                    //{
                    //    text: '',
                    //    extend: 'pdfHtml5',
                    //    collectionLayout: 'fixed two-column ',
                    //    className: 'btn btn-info btn-sm',
                    //    postfixButtons: ['colvisRestore'],
                    //    init: function (api, node, config) {
                    //        $(node).removeClass('dt-button');
                    //        $(node).append(' <p class="" style="height:8px"><i style="font-size:16px;color:#FFF;" class="fa fa-eye "></i><p>');
                    //    }
                    //}
                ]
            });
    }
}());