var dailyTaskFormTypeReport = dailyTaskFormTypeReport || (function () {

    return {
        getItems: function (id) {
            getItems(id);
        }
    };

    function getItems(id) {

        $('#body-content').empty();
        $('#nothing-plan').show();
        var sDate = $('#start-date').val();
        var enDate = $('#end-date').val();

        var wh = window.innerHeight * 0.63;

        $.ajax({
            type: "Get",
            url: "/APSE/Report/GetDailyTaskFormTypeReport?fromDate=" + sDate + "&toDate=" + enDate + "&workId="+id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {

                    initHeadersType(data.punchType);
                    initContents(data.items);
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
                sScrollX: "0%",
                scrollX: true,
                fixedHeader: true,
                "bDestroy": true
            });
    }

    function initHeadersType(items) {

        var tag = '  <table border="1" style="width: 100%" class="table table-bordered" id="clearPunch-table">'+
             '        <colgroup>  ' +
            '                           <col style="width: 30%" />  ' +
            '                           <col style="width: 23%" />  ' +
            '                           <col style="width: 23%" />  ' +
            '                           <col style="width: 23%" />  ' +
            '                       </colgroup>  ' +
            '                       <thead>  ' +
            '                           <tr>  ' +
            '                               <th rowspan="2" colspan="1" style="text-align:center;font-weight:bold;background-color:#b6eee7">Description</td>  ' +
            '                               <th>Total</th>  ' +
            '                               <th>Performed</th>  ' +
            '                               <th>Remain</th>  ' +
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
            '                               <td colspan="1" style="padding:0">  ' +
            '                                   <table style="width: 100%;margin-bottom:0px" class="table table-bordered" id="remainpunch-table">  ' +
            '                                       <thead>  ' +
            '                                           <tr id="remainpunch-table-tr">  ' +
            '                                               <th></th>  ' +
            '                                           </tr>  ' +
            '                                       </thead>  ' +
            '                                   </table>  ' +
            '                               </td>  ' +
            '                           </tr>  ' +
            '     ' +
            '                       </thead>  ' +
            '                       <tbody id="descipline-items">  ' +
            '                            ' +
            '                      </tbody>  </table>';
        $('#body-content').append(tag);

        var totalContent = $('#total-table-tr');
        var clearContent = $('#clarepunch-table-tr');
        var remainContent = $('#remainpunch-table-tr');
        totalContent.empty();
        clearContent.empty();
        remainContent.empty();

        $.each(items, function (j, ds) {
            var $th = $("<th style='width:50px'>" + ds.name + "</th>");
            var $th1 = $("<th style='width:50px'>" + ds.name + "</th>");
            var $th2 = $("<th style='width:50px'>" + ds.name + "</th>");
            totalContent.append($th);
            clearContent.append($th1);
            remainContent.append($th2);
        });
    }

    function initContents(items) {
        var desContent = $('#descipline-items');
        desContent.empty();

        $.each(items, function (j, ds) {
            var $tr = $("<tr></tr>");
            var $th = $("<th style='width:50px' id=" + ds.id + ">" + ds.desciplineName + "</th>");
            var $tdLeft = $("<td></td>");
            var $tdRight = $("<td></td>");
            var $tdRight1 = $("<td></td>");

            var tableLeft = $("<table border='1' style='width:100 %;margin-bottom:0px' class='table table-bordered'></table>");
            var tHeadLeft = $("<thead></thead>");
            var trLeft = $("<tr></tr>");
            $.each(ds.totals, function (i, ks) {
                var $tdk = $("<td style='width:50px'>" + ks + "</td>");
                trLeft.append($tdk);
            });

            tHeadLeft.append(trLeft);
            tableLeft.append(tHeadLeft);
            $tdLeft.append(tableLeft);

            var tableRight = $("<table border='1' style='width:100 %;margin-bottom:0px' class='table table-bordered'></table>");
            var tHeadRight = $("<thead></thead>");
            var trRight = $("<tr></tr>");

            $.each(ds.clearPunch, function (i, cs) {
                var $tdk = $("<td style='width:50px'>" + cs + "</td>");
                trRight.append($tdk);
            });
         
            tHeadRight.append(trRight);
            tableRight.append(tHeadRight);
            $tdRight.append(tableRight);

            var tableRight1 = $("<table border='1' style='width:100 %;margin-bottom:0px' class='table table-bordered'></table>");
            var tHeadRight1 = $("<thead></thead>");
            var trRight1 = $("<tr></tr>");
            $.each(ds.remain, function (i, rs) {
                var $tdk1 = $("<td style='width:50px'>" + rs + "</td>");
                trRight1.append($tdk1);
            });

            tHeadRight1.append(trRight1);
            tableRight1.append(tHeadRight1);
            $tdRight1.append(tableRight1);

            $tr.append($th);
            $tr.append($tdLeft);
            $tr.append($tdRight);
            $tr.append($tdRight1);

            desContent.append($tr);
        });
    }
}());