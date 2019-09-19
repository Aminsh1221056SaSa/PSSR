var punchCategoryReport = punchCategoryReport || (function () {

    return {
        getItems: function () {
            getItems();
        }
    };

    function getItems() {
        $('#nothing-plan').show();
        var sDate = $('#start-date').val();
        var enDate = $('#end-date').val();

        //$('#categoryPunch-table').DataTable().destroy();
        //$('#clarepunch-table').DataTable().destroy();
        //$('#clearPunch-table').DataTable().destroy();

        $.ajax({
            type: "Get",
            url: "/poec/Report/GetPunchCategoryReport?fromDate=" + sDate + "&toDate=" + enDate + "",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data != null) {
                    initCategory(data.categories);
                    $('#nothing-plan').hide();
                    //releaseInitialization();
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function releaseInitialization() {

        var wh = window.innerHeight * 0.63;
        $('#categoryPunch-table').DataTable(
            {
                "paging": false,
                "info": false,
                "searching": false,
            });

        //$('#clarepunch-table').DataTable(
        //    {
        //        language: {
        //            "zeroRecords": " "
        //        },
        //        "paging": false,
        //        "info": false,
        //        "searching": false,
        //    });

        //$('#clearPunch-table').DataTable(
        //    {
        //        "paging": false,
        //        "info": false,
        //        "searching": true
        //    });
    }

    function initCategory(items)
    {
        var bodyContent = $('#descipline-items');
        bodyContent.empty();

        $.each(items, function (j, cat) {
           
            var mtr = $('<tr></tr>');
            var mtd = $('<td></td>');
            var mTable = $('<table style="width:100%;margin-bottom:0px" class="table table-bordered"></table>');
            var mCogrip = $('<colgroup><colstyle="width:50%" /><col style="width:25%"/> <col style="width:25%" /></colgroup>');
            var mThead = $('<thead></thead>');
            var ntr = $("<tr><th colspan='1' rowspan='2' style='margin-top:-20px'>" + cat.name + "</th><th>Workable</th></tr><tr><th colspan='1'>Hold</th></tr>");

            mThead.append(ntr);
            mTable.append(mCogrip);
            mTable.append(mThead);
            mtd.append(mTable);
            mtr.append(mtd);

            var gtd = $('<td></td>');
            var gTable = $('<table style="width:100%;margin-bottom:0px" class="table table-bordered"></table>');
            var gThead = $('<thead></thead>');
            $.each(cat.totals, function (j, to) {
                var gtr1 = $("<tr style='border-bottom:1px solid #efefef'><td style='width:50px'>" + to.item1 + "</td></tr>");
                var gtr2 = $("<tr style='border-bottom:1px solid #efefef'><td style='width:50px'>" + to.item2 + "</td></tr>");

                gThead.append(gtr1).append(gtr2);
            });
            gTable.append(gThead);
            gtd.append(gTable);
            mtr.append(gtd);


            var ktd = $('<td></td>');
            var kTable = $('<table  style="width:100%;margin-top:-30px" class="table table-bordered"></table>');
            var kThead = $('<thead></thead>');
            var ktr = $('<tr></tr>');

            $.each(cat.punchDetails, function (p, to) {

                var lotd = $('<td></td>');
                var loTable = $('<table  border="1" style="width:100%;margin-bottom:0px" class="table table-bordered"></table>');
                var loThead = $('<thead></thead>');
              
                var lotr1 = $('<tr></tr>');
                var lotr2 = $('<tr></tr>');
                var lotr3 = $('<tr></tr>');
                $.each(to.value, function (u, toy) {
                    var lotd1 = $("<td style='background-color:#b6eee7'>" + toy.item1 + "</td>");
                    var lotd2 = $("<td>" + toy.item2 + "</td>");
                    var lotd3 = $("<td>" + toy.item3 + "</td>");
                    lotr1.append(lotd1);
                    lotr2.append(lotd2);
                    lotr3.append(lotd3)
                });
                var htr = $("<tr><th colspan='3'>" + to.key + "</th></tr>");
                loThead.append(htr);
             
                loThead.append(lotr1).append(lotr2).append(lotr3);
                loTable.append(loThead);
                lotd.append(loTable);
                ktr.append(lotd);
            });
            kThead.append(ktr);
            kTable.append(kThead);
            ktd.append(kTable);
            mtr.append(ktd);

            bodyContent.append(mtr);
        });
    }
}());