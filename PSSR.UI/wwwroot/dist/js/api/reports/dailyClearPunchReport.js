var dailyClearPunchReport = dailyClearPunchReport || (function () {

    return {
        getItems: function () {
            getItems();
        }
    };

    function getItems() {
        $('#body-content').empty();
        $('#nothing-plan').show();
        var sDate = $('#start-date').val();
        var enDate = $('#end-date').val();

        $('#clearPunch-table').DataTable().destroy();

        $.ajax({
            type: "Get",
            url: "/poec/Report/GetDailyPunchClearReport?fromDate=" + sDate + "&toDate=" + enDate + "",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {

                    initHeadersType(data.punchType);
                    initContents(data.items);
                    $('#nothing-plan').hide();
                    releaseInitialization();
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function releaseInitialization() {

        var wh = window.innerHeight * 0.63;
        
        $('#clearPunch-table').DataTable(
            {
                "paging": false,
                "info": false,
                "searching": false,
                responsive: true,
                scrollY: wh + "px",
                sScrollX: "0%",
                scrollX: true,
                fixedHeader: true,
                "bDestroy": true
            });
    }

    function initHeadersType(items)
    {
        var tag = '<table border="1" style="width: 100%" class="table table-bordered" id="clearPunch-table">  ' +
            '                       <colgroup>  ' +
            '                           <col style="width: 31.5%" />  ' +
            '                           <col style="width: 38.5%" />  ' +
            '                           <col style="width: 30%" />  ' +
            '                       </colgroup>  ' +
            '                       <thead>  ' +
            '                           <tr>  ' +
            '                               <td rowspan="2" style="text-align:center;font-weight:bold;background-color:#b6eee7">Description</td>  ' +
            '                               <th colspan="1">Total</th>  ' +
            '                               <th colspan="1">Clear Punch</th>  ' +
            '                           </tr>  ' +
            '                           <tr>  ' +
            '                               <td colspan="1" style="padding:0">  ' +
            '                                   <table style="width: 100%;margin-bottom:0px" class="table table-bordered" id="total-table">  ' +
            '                                       <thead>  ' +
            '                                           <tr id="total-table-tr">  ' +
            '                                               <th></th>  ' +
            '                                           </tr>  ' +
            '                                       </thead>  ' +
            '                                   </table>  ' +
            '                               </td>  ' +
            '                               <td colspan="1" style="padding:0">  ' +
            '                                   <table style="width: 100%;margin-bottom:0px" class="table table-bordered" id="clarepunch-table">  ' +
            '                                       <thead>  ' +
            '                                           <tr id="clarepunch-table-tr">  ' +
            '                                               <th></th>  ' +
            '                                           </tr>  ' +
            '                                       </thead>  ' +
            '                                   </table>  ' +
            '                               </td>  ' +
            '                           </tr>  ' +
            '     ' +
            '                       </thead>  ' +
            '                       <tbody id="descipline-items"></tbody>  ' +
            '                  </table>  ';
        $('#body-content').append(tag);
        var totalContent = $('#total-table-tr');
        var clearContent = $('#clarepunch-table-tr');
        totalContent.empty();
        clearContent.empty();

        $.each(items, function (j, ds) {
            var $th = $("<th style='width:50px' id=" + ds.id + ">" + ds.name + "</th>");
            var $th1 = $("<th style='width:50px' id=" + ds.id + ">" + ds.name + "</th>");
            totalContent.append($th);
            clearContent.append($th1);
        });
    }

    function initContents(items)
    {
        var desContent = $('#descipline-items');
        desContent.empty();

        $.each(items, function (j, ds) {
            var $tr = $("<tr></tr>");
            var $th = $("<th style='width:50px' id=" + ds.id + ">" + ds.desciplineName + "</th>");
            var $tdLeft = $("<td></td>");
            var $tdRight = $("<td></td>");

            var tableLeft = $("<table border='1' style='width:100%;margin-bottom:0px' class='table table-bordered'></table>");
            var tHeadLeft = $("<thead></thead>");
            var trLeft = $("<tr></tr>");
            $.each(ds.totals, function (i, ks) {
                var $tdk = $("<td style='width:50px'>" + ks + "</td>");
                trLeft.append($tdk);
            });

            tHeadLeft.append(trLeft);
            tableLeft.append(tHeadLeft);
            $tdLeft.append(tableLeft);

            var tableRight = $("<table border='1' style='width:100%;margin-bottom:0px' class='table table-bordered'></table>");
            var tHeadRight = $("<thead></thead>");
            var trRight = $("<tr></tr>");

            $.each(ds.clearPunch, function (i, cs) {
                var $tdk = $("<td style='width:50px'>" + cs + "</td>");
                trRight.append($tdk);
            });

            tHeadRight.append(trRight);
            tableRight.append(tHeadRight);
            $tdRight.append(tableRight);

            $tr.append($th);
            $tr.append($tdLeft);
            $tr.append($tdRight);

            desContent.append($tr);
        });
    }
}());