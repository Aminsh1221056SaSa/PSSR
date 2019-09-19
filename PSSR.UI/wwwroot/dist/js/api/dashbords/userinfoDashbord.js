var userinfoDashbord = userinfoDashbord || (function () {
    return {
        init: function () {
            getallCurrentUserprojects();
        }
    };

    function getallCurrentUserprojects() {
        $('#project-loading').show();
        $.ajax({
            type: "Get",
            url: "/APSE/Dashboard/GetallCurrentUserprojects",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                createprojectList(data);
                $('#project-loading').hide();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createprojectList(model) {
        var offshorecontent = $('#cu-offshore-lst');
        var onshorecontent = $('#cu-onshore-lst');
        var otherscontent = $('#cu-others-lst');

        offshorecontent.empty();
        onshorecontent.empty();
        otherscontent.empty();

        $.each(model, function (i, val) {
            var menuModel = '<ul class="dropdown-menu" role="menu">' +
                "<li><a href='/Dashboard/ManagerDashboard?pid=" + val.id + "'>Manager Dashboard</a></li>" +
                "<li><a href='/Dashboard/ProjectWBS?pid=" + val.id + "'>Progress Dashboard</a></li>" +
                "<li><a href='/Dashboard/summaryworkpackagereport?pid=" + val.id + "'>Summary Report</a></li>" +
                '                <li class="divider"></li>' +
                "<li><a href='/ProjectManagment/Activity/ActivityList?pid=" + val.id + "'>Task</a></li>" +
                "<li><a href='/ProjectManagment/ActivityPunch/PunchList?pid=" + val.id + "'>Punch</a></li>" +
                "<li><a href='/ProjectManagment/Project/DesciplinePlane?pid=" + val.id + "'>Plan</a></li>" +
                '                                </ul > ';

            var rootDiv = $('<div class="col-lg-6 col-md-12 col-sm-6 col-xs-12"></div>');
            var myvar = '<div class="box-header with-border" style="padding:0;padding-bottom:8px;padding-top:13px;margin-right:-9px;background:#f7ad27"><div style="top:-2px" class="box-tools pull-right"><div class="btn-group">' +
                '                                <button type="button" class="btn btn-box-tool dropdown-toggle" data-toggle="dropdown">' +
                '                                    <i class="fa fa-gear" style="color:#FFF"></i>' +
                '                                </button>' +
                menuModel +
                '                                </div></div></div>';

            var topLink = $("<a href='/Dashboard/ManagerDashboard?pid=" + val.id + "'></a>");
            rootDiv.append(myvar);
            if (val.type === 1003) {
                var mdive = '<div   id="Clouds">' +
                    '  <div class="Cloud Foreground"></div>' +
                    '  <div class="Cloud Background"></div>' +
                    '  <div class="Cloud Foreground"></div>' +
                    '  <div class="Cloud Background"></div>' +
                    '  <div class="Cloud Foreground"></div>' +
                    '  <div class="Cloud Background"></div>' +
                    '  <div class="Cloud Background"></div>' +
                    '  <div class="Cloud Foreground"></div>' +
                    '  <div class="Cloud Background"></div>' +
                    '  <div class="Cloud Background"></div>' +
                    '<!--  <svg viewBox="0 0 40 24" class="Cloud"><use xlink:href="#Cloud"></use></svg>-->' +
                    '</div>' +
                    '' +
                    '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"' +
                    '	 width="40px" height="24px" viewBox="0 0 40 24" enable- xml:space="preserve">' +
                    '  <defs>' +
                    '    <path id="Cloud" d="M33.85,14.388c-0.176,0-0.343,0.034-0.513,0.054c0.184-0.587,0.279-1.208,0.279-1.853c0-3.463-2.809-6.271-6.272-6.271' +
                    '	c-0.38,0-0.752,0.039-1.113,0.104C24.874,2.677,21.293,0,17.083,0c-5.379,0-9.739,4.361-9.739,9.738' +
                    '	c0,0.418,0.035,0.826,0.084,1.229c-0.375-0.069-0.761-0.11-1.155-0.11C2.811,10.856,0,13.665,0,17.126' +
                    '	c0,3.467,2.811,6.275,6.272,6.275c0.214,0,27.156,0.109,27.577,0.109c2.519,0,4.56-2.043,4.56-4.562' +
                    '	C38.409,16.43,36.368,14.388,33.85,14.388z"/>' +
                    '  </defs>' +
                    '</svg>';
                var childDiv = $('<div class="small-box bg-orange product-image-onsure"></div>');
                childDiv.append(mdive);
                var iconDiv = $('<div><div><i></i></div></div>');
                var h3 = $("<h3 class='text-center' style='height:13px;'></h3>");
            }
            else {
                var childDiv = $('<div class="small-box bg-orange product-image"></div>');
                var iconDiv = $('<div class="wawewrapper"><div class="icon wave"><i></i></div></div>');
                var h3 = $("<h3 class='text-center' style='height:43px;'></h3>");
            }
            var innerDiv = $('<div class="inner"></div>');
            var p = $("<h3 class='product-desc'>" + val.description + "</h3>");

            innerDiv.append(h3);
            innerDiv.append(p);

           
            var link = $("<a class='small-box-footer product-bottom' style='height:28px;background:none'></a>");

            childDiv.append(innerDiv);
            childDiv.append(iconDiv);
            childDiv.append(link);
            topLink.append(childDiv);
            rootDiv.append(topLink);

            if (val.type == 1001 || val.type == 1002) {
                offshorecontent.append(rootDiv);
            }
            else if (val.type == 1003){
                onshorecontent.append(rootDiv);
            }
            else{
                otherscontent.append(rootDiv);
            }

        });
    }

}());