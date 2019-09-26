
var person = person || (function () {

    return {
        init: function () {
            initialization();
        },
        getPersonList: function () {
            getPersonList()
        },
        getPerson: function (id) {
            return getPerson(id);
        },
        setCurrentIndex: function (index) 
        {
            setCurrentIndex(index);
        },
        removePerson: function (id) {
            removePerson(id);
        }
    };

    var _table = null;
    var _cIndex = 0;
    function setCurrentIndex(index) {
        _cIndex = index;
    }

    function initialization()
    {
        if (_table) {
            _table.clear().draw();
            _table.destroy();
        }

        $("form[name='personForm']").validate({
            // Specify validation rules
            rules: {
                firstName: "required",
                lastName: "required",
                nationalId: {
                    required: true,
                    minlength: 10,
                },
                mobileNumber: {
                    required: true,
                    minlength: 7,
                },
                projects: "required",
            },
            messages: {
                firstName: "Please enter Person First Name",
                lastName: "Please enter Person Last Name",
                projects: "Please select at least one project",
                nationalId:
                {
                    required: "Please enter Person National code",
                    minlength: "Person Natioanl Code must consist of at least 6 characters"
                },
                mobileNumber: {
                    required: "Please enter a mobile number",
                    minlength: "Person mobile number must consist of at least 7 characters"
                }
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
                var fname = $('input#firstName').val();
                var lname = $('input#lastName').val();
                var nid = $('input#nationalId').val();
                var mobile = $('input#mobileNumber').val();
                var projects = $("select#projects").select2("val");
                createPerson(fname, lname, nid, mobile, projects);
            }
        });

        $("form[name='personEditForm']").validate({
            // Specify validation rules
            rules: {
                firstName: "required",
                lastName: "required",
                nationalId: {
                    required: true,
                    minlength: 10,
                },
                mobileNumber: {
                    required: true,
                    minlength: 7,
                },
                projects: "required",
            },
            messages: {
                firstName: "Please enter Person First Name",
                lastName: "Please enter Person Last Name",
                projects: "Please select at least one project",
                nationalId:
                {
                    required: "Please enter Person National code",
                    minlength: "Person Natioanl Code must consist of at least 6 characters"
                },
                mobileNumber: {
                    required: "Please enter a mobile number",
                    minlength: "Person mobile number must consist of at least 7 characters"
                }
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
                var fname = $('input#editfirstName').val();
                var lname = $('input#editlastName').val();
                var nid = $('input#editnationalId').val();
                var mobile = $('input#editmobileNumber').val();
                var projects = $("select#editprojects").select2("val");
                var id = $('#current-personId').val();
                editPerson(fname, lname, nid, mobile, projects,id);
            }
        });
    }

    function getPersonList()
    {
        var content = $('#person-content');
        content.empty();
        $.ajax({
            type: "Get",
            url: "/APSE/Person/GetPersons",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var name = $("<td>" + val.name + "</td>");
                    var nationalId = $("<td>" + val.nationalId + "</td>");
                    var editBtn = $("<td><button  data-toggle='modal' data-target='#edit-personModal' data-id='" + val.id + "' class='edit-person btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>");
                    var deleteBtn = $("<td><button data-id='" + val.id + "' class='delete-person btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button></td>");
                    var row = $('<tr></tr>');
                    row.append(name).append(nationalId).append(editBtn).append(deleteBtn);
                    content.append(row);
                });
                _table = tableinit();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        })
    }

    function getPerson(id) {
      return  $.ajax({
            type: "Get",
          url: '/APSE/Person/GetPerson/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createPerson(firstName, lastName, nationalId, mobileNumber, projects)
    {
        var model =
        {
            'firstName': firstName, 'lastName': lastName, 'nationalId': nationalId, 'mobileNumber': mobileNumber, 'projectIds': projects
        };
        var name = firstName + " " + lastName;
        $.ajax({
            type: "Post",
            url: "/APSE/Person/CreatePerson",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row.add([
                            name,
                            nationalId,
                            "<button  data-toggle='modal' data-target='#edit-personModal' data-id='" + data.subject + "' class='edit-person btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-person btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
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

    function editPerson(firstName, lastName, nationalId, mobileNumber, projects,id) {
        var model =
        {
            'firstName': firstName, 'lastName': lastName, 'nationalId': nationalId, 'mobileNumber': mobileNumber, 'projectIds': projects
        };
        var name = firstName + " " + lastName;
        $.ajax({
            type: "PUT",
            url: "/APSE/Person/UpdatePerson/"+id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').data([
                            name,
                            nationalId,
                            "<button  data-toggle='modal' data-target='#edit-personModal' data-id='" + data.subject + "' class='edit-person btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-person btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]);
                        $('#edit-personModal').modal('toggle');
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

    function removePerson(id) {
      
        $.ajax({
            type: "Delete",
            url: "/APSE/Person/DeletePerson/" + id,
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
        var table = $('#personLst').DataTable({
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
                  filename: "Persons",
                  messageTop: "Persons",
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
              const ps = new PerfectScrollbar('.dataTables_scrollBody');
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