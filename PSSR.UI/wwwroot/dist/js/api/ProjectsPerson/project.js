﻿
var project = project || (function () {

    return {
        init: function (enumDefinetion) {
            initialization(enumDefinetion);
        },
        getProjectList: function () {
            return initProjectList();
        },
        getProject: function (id) {
            return getProject(id);
        },
        setCurrentIndex: function (index) {
            setCurrentIndex(index);
        },
        removeProject: function (id) {
            removeProject(id);
        }
    };

    var _enumDefine = null;
    var _table = null;
    var _cIndex = 0;
    function setCurrentIndex(index) {
        _cIndex = index;
    }

    function initialization(enumDefinetion)
    {
        _enumDefine = enumDefinetion;
        if (_table) {
            _table.clear().draw();
            _table.destroy();
        }

        $("form[name='projectForm']").validate({
            // Specify validation rules
            rules: {
                pDescription: "required",
                type: "required",
                contractorId: "required"
            },
            messages: {
                pDescription: "Please enter  Name",
                type: "Please select a Type",
                contractorId: "Please select a Contractor"
            },
            errorElement: "em",
            errorPlacement: function (error, element) {
                // Add the `invalid-feedback` class to the error element
                error.addClass("invalid-feedback");

                if (element.prop("type") === "checkbox") {
                    error.insertAfter(element.next("label"));
                } else {
                    error.insertAfter(element);
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            // Make sure the form is submitted to the destination defined
            // in the "action" attribute of the form when valid
            submitHandler: function (form) {
                var description = $('input#pDescription').val();
                var type = $('select#type').val();
                var contractorId = $('select#contractorId').val();
                var cName = $('#contractorId option:selected').text();
                var startDate = $('input#startDate').val();
                var endDate = $('input#endDate').val();
                createProject(description, type, contractorId, startDate, endDate, cName);
            }
        });

        $("form[name='projectEditForm']").validate({
            // Specify validation rules
            rules: {
                pDescription: "required",
                type: "required",
                contractorId: "required"
            },
            messages: {
                pDescription: "Please enter Name",
                type: "Please select a Type",
                contractorId: "Please select a Contractor"
            },
            errorElement: "em",
            errorPlacement: function (error, element) {
                // Add the `invalid-feedback` class to the error element
                error.addClass("invalid-feedback");

                if (element.prop("type") === "checkbox") {
                    error.insertAfter(element.next("label"));
                } else {
                    error.insertAfter(element);
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            // Make sure the form is submitted to the destination defined
            // in the "action" attribute of the form when valid
            submitHandler: function (form) {
                var description = $('input#editPDescription').val();
                var type = $('select#edittype').val();
                var contractorId = $('select#editcontractorId').val();
                var cName = $('#contractorId option:selected').text();
                var startDate = $('input#editstartDate').val();
                var endDate = $('input#editendDate').val();
                var id = $('#current-projectId').val();
                editProject(description, type, contractorId, startDate, endDate, id, cName);
            }
        });
    }

    function getProjectList()
    {
        return $.ajax({
            type: "Get",
            url: "/APSE/Project/GetProjects",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
               
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        })
    }

    async function initProjectList() {
        var content = $('#project-content');
        content.empty();
        var data = await getProjectList();
        $.each(data, function (i, val) {
            var name = $("<td>" + val.name + "</td>");
            var contractorName = $("<td>" + val.contractorName + "</td>");
            var type = $("<td>" + _enumDefine.getProjectType(val.type) + "</td>");
            var editBtn = $("<td><button  data-toggle='modal' data-target='#edit-projectModal' data-id='" + val.id + "' class='edit-project btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>");
            var deleteBtn = $("<td><button data-id='" + val.id + "' class='delete-project btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button></td>");
            var row = $('<tr></tr>');
            row.append(name).append(contractorName).append(type).append(editBtn).append(deleteBtn);
            content.append(row);
        });
        _table = tableinit();
    }

    function getProject(id) {
      return  $.ajax({
            type: "Get",
          url: '/APSE/Project/GetProject/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createProject(description, type, contractorId, startDate, endDate,contractorName)
    {
        var model =
        {
            'description': description, 'startDate': startDate, 'endDate': endDate, 'contractorId': contractorId, 'type': type
        };
        var sType = _enumDefine.getProjectType(parseInt(type));
        $.ajax({
            type: "Post",
            url: "/APSE/Project/CreateProject",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row.add([
                            description,
                            contractorName,
                            sType,
                            "<button  data-toggle='modal' data-target='#edit-projectModal' data-id='" + data.subject + "' class='edit-project btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-project btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]).draw(false);
                    }
                }
                else {
                    alert(data.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function editProject(description, type, contractorId, startDate, endDate, id, contractorName) {
        var model =
        {
            'description': description, 'startDate': startDate, 'endDate': endDate, 'contractorId': contractorId, 'type': type
        };
        var sType = _enumDefine.getProjectType(parseInt(type));
        $.ajax({
            type: "PUT",
            url: "/APSE/Project/UpdateProject/"+id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').data([
                            description,
                            contractorName,
                            sType,
                            "<button  data-toggle='modal' data-target='#edit-projectModal' data-id='" + data.subject + "' class='edit-project btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-project btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]);
                        $('#edit-projectModal').modal('toggle');
                    }
                }
                else {
                    alert(data.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function removeProject(id) {
      
        $.ajax({
            type: "Delete",
            url: "/APSE/Project/DeleteProject/" + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').remove().draw(false);
                    }
                }
                else {
                    alert(data.value);
                }
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function tableinit() {

        var hh = window.innerHeight * 0.45;
      var table= $('#projectLst').DataTable({
          responsive: true,
          FixedHeader: true,
          scrollY: hh+"px",
          dom: 'Bfrtip',
          buttons: [
              {
                  text: '',
                  extend: 'excelHtml5',
                  className: 'btn btn-success btn-sm',
                  exportOptions: {
                      modifier: {
                          page: 'all'
                      },
                  },
                  init: function (api, node, config) {
                      $(node).removeClass('dt-button');
                      $(node).append('<p class="" style="height:1px;"><i style="font-size:20px;color:#FFF;" class="fa fa-file-excel"></i><p>');
                  },
                  extension: '.xlsx',
                  filename: "Projects",
                  messageTop: "Projects",
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
              {
                  extend: 'print',
                  text: '',
                  className: 'btn btn-warning btn-sm',
                  exportOptions: {
                      modifier: {
                          page: 'all'
                      }
                  },
                  init: function (api, node, config) {
                      $(node).removeClass('dt-button');
                      $(node).append('<p class="" style="height:1px;"><i style="font-size:20px;color:#FFF;" class="fa fa-print"></i><p>');
                  }
              }
          ],
          scrollCollapse: !0,
          scroller: !0,
          "fnInitComplete": function () {
              //$('.dataTables_scrollBody').perfectScrollbar();
          },
          //on paginition page 2,3.. often scroll shown, so reset it and assign it again
          "fnDrawCallback": function (oSettings) {
              $('.dataTables_scrollBody').each(function () {
                  if (!$(this).hasClass('ps')) {
                      const ps = new PerfectScrollbar($(this)[0]);
                  }
                  $(this)[0].scrollTop = 0;
              });
          }
      });
        return table;
    }

}());