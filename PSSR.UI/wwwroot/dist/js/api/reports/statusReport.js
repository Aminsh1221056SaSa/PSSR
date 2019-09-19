var statusReport = statusReport || (function () {

    return {
        getItems: function (id, workName) {
            getItems(id, workName);
        }
    };

    function getItems(id, workName) {

        $('#body-content').empty();
        $('#nothing-plan').show();

        var wh = window.innerHeight * 0.63;

        $.ajax({
            type: "Get",
            url: "/APSE/Report/GetStatusReport?workId=" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    initHeadersType(workName);
                    initContents(data);
                    $('#nothing-plan').hide();
                    releaseInitialization(wh);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function releaseInitialization(wh) {
        $('#clearPunch-table').DataTable(
            {
                "paging": false,
                "info": false,
                "searching": false,
                responsive: true,
                scrollY: wh + "px",
                fixedHeader: true,
                autoWidth: true,
                "bDestroy": true
            });

        $('.inner-table').DataTable(
            {
                "language": {
                    "emptyTable": null,
                    "zeroRecords": null
                },
                "paging": false,
                "info": false,
                "searching": false,
                "bDestroy": true
            });
        
    }

    function initHeadersType(workName) {

        var st = "RFC";
        if (workName == 'Commissioning')
        {
            st = "RFSU";
        }

        var tag = '<table border="1" style="width: 100%;" class="table table-bordered" id="clearPunch-table">' +
            '                        <colgroup><col style="width:10%"><col style="width: 10%"><col style="width: 10%"><col style="width:50%"><col style="width:10%"></colgroup>' +
            '                        <thead>' +
            '                            <tr>' +
            '                                <th colspan="1"><p style="font-size:13px">Priority</p></th>' +
            '                                <th colspan="1"><p style="font-size:13px">SubSystem</p></th>' +
            '                                <th colspan="1"><p style="font-size:13px">SubSystem Desc</p></th>' +
            '                                <th colspan="1" style="padding:0;">' +
            '                                    <table style="width: 100%;margin-bottom:0;border-bottom:none" class="table  inner-table">' +
            '                                        <colgroup>' +
            '                                            <col style="width:20%">' +
            '                                            <col style="width:20%">' +
            '                                            <col style="width:20%">' +
            '                                            <col style="width:20%">' +
            '                                        </colgroup>' +
            '                                        <thead style="margin-top:10px">' +
            '                                            <tr>' +
            '                                                <th style="border-bottom:none">Descipline</th>' +
            '                                                <th style="border-bottom:none">Remain Sheet</th>' +
            '                                                <th style="border-bottom:none" id="rm-1-desck"></th>' +
            '                                                <th style="border-bottom:none" id="status-desc">DAC Status</th>' +
            '                                            </tr>' +
            '                                        </thead>' +
            '                                    </table>' +
            '                                </th>' +
            '                                <th id="status-work" colspan="1">' + st+'</th>' +
            '                            </tr>' +
            '                        </thead>' +
            '                        <tbody id="status-items"></tbody>' +
            '                    </table>';

        $('#body-content').append(tag);
    }

    function initContents(items) {
        var desContent = $('#status-items');
        desContent.empty();

        $.each(items, function (j, ds) {
            var $tr = $("<tr></tr>");
            var $td = $("<td>" + ds.priority + "</td>");
            var $td2 = $("<td>" + ds.subSystemDesc + "</td>");
            var $td3 = $("<th>" + ds.subSystemCode + "</th>");
            var $td4 = $("<td>" + ds.statusDesc + "</td>");
            $tr.append($td);
            $tr.append($td3);
            $tr.append($td2);

            var $td5 = $("<th></th>");

            var tableLeft = $("<table border='1' style='width:100 %;margin-bottom:0px' class='table table-bordered'></table>");
            var coGroup = $('<colgroup><col style ="width:20%"><col style="width:20%"><col style="width:20%"><col style="width:20%"></colgroup>');
            var tbody = $('<tbody></tbody>');
            tableLeft.append(coGroup);

            $.each(ds.statusDetails, function (i, ks) {
                var $trinto = $("<tr></tr>");
                var $tdk = $("<td style='width:50px'>" + ks.descipline + "</td>");
                var $tdk2 = $("<td style='width:50px'>" + ks.remainSheet + "</td>");
                var $tdk3 = $("<td style='width:50px'>" + ks.remainPunchNum1 + "</td>");
                var $tdk5 = $("<td style='width:50px'>" + ks.statusDesc + "</td>");

                if (i == 0)
                {
                    $('#rm-1-desck').html(ks.remainPunchDesc1);
                }

                $trinto.append($tdk).append($tdk2).append($tdk3).append($tdk5);
                tbody.append($trinto);
            });

            tableLeft.append(tbody);
            $td5.append(tableLeft);
            $tr.append($td5);
            $tr.append($td4);
            desContent.append($tr);
        });
    }
}());