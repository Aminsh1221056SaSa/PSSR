var contractor = contractor || (function () {

    return {
        init: function () {
            initialization();
        }
    };

    function initialization()
    {
        $('#contractorLst').DataTable({
            responsive: true,
            language: {
                searchPlaceholder: 'Search...',
                sSearch: '',
                lengthMenu: '_MENU_ items/page',
            },
            scrollY: 200 + "px",
            sScrollX: "0%",
            scrollX: true,
            fixedHeader: true,
            orderCellsTop: true,
            autoWidth: false,
          
            scrollCollapse: true,
            "fnInitComplete": function () {
                //$('.dataTables_scrollBody').perfectScrollbar();
                const ps = new PerfectScrollbar('.dataTables_scrollBody');
            },
            //on paginition page 2,3.. often scroll shown, so reset it and assign it again
            "fnDrawCallback": function (oSettings) {
                //const ps= $('.dataTables_scrollBody').perfectScrollbar('destroy').perfectScrollbar();
                //ps.update();
                const ps = new PerfectScrollbar('.dataTables_scrollBody');
            },
            columns: [
                { "width": "20%" },
                { "width": "20%" },
                { "width": "60%" },
            ],
        });

    }
}());