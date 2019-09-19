var punchListSummary = punchListSummary || (function () {

    return {
        init: function () {
            initialization();
        },
        getPunchSummary: function () {
            getPunchSummary();
        }
    };

    function initialization() {
        //
    }

    function getPunchSummary() {
        var sortOption = $('#OrderByOptions').val();
        var filterType = $('#FilterBy').val();
        var filterVal = $('#filter-value-dropdown').val();

        var pgNum = 1;
        if ($('#pg-num').val()) {
            pgNum = $('#pg-num').val();
        }

        var pgSize = 10;
        if ($('#pg-size-dropdown').val()) {
            pgSize = $('#pg-size-dropdown').val();
        }
        var container = $('#tb-punchSummary');
        var prevCheckState = $('#prevCheckState').val();
        var query = $('#queryFilter').val();

        container.empty();
        $.ajax({
            type: "Get",
            url: "/poec/ActivityPunch/PunchListSummary?filterByOption=" + filterType + "&sortByOption=" + sortOption + "&filterValue=" + filterVal
                + "&pageNum=" + pgNum + "&pageSize=" + pgSize + "&query=" + query + "&prevCheckState=" + prevCheckState,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data.items, function (i, val) {
                    createTr(val);
                });

                setOption(data.option);
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createTr(model) {
        var container = $('#tb-punchSummary');
        var trContent = $('<tr></tr>');

        trContent.append("<td>" + model.code + "</td>");
        trContent.append("<td>" + model.type + "</td>");
        var $ptd = $("<td>" + model.orginatedDate + "</td>");

        trContent.append($ptd);

        var $pedit = $("<td><p class='btn btn-danger edtit-summary-btn btn-sm' id='ed-sm-" + model.id + "'><i class='fa fa-edit'></i></p></td>");

        trContent.append($pedit);

        container.append(trContent);
    }

    function setOption(model) {
        $('#pg-num').val(model.pageNum);
        $('#current-page').text("of " + model.numPages + "");

        $("#pg-size-dropdown").empty();
        $.each(model.pageSizes, function (i, val) {
            $("#pg-size-dropdown").append("<option value=" + val + ">" + val + "</option>");
        });
        $("#pg-size-dropdown").val(model.pageSize);
        $('#prevCheckState').val(model.prevCheckState);
    }
}());