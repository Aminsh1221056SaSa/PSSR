
var contractor = contractor || (function () {

    return {
        init: function () {
            initialization();
        },
        getContractorList: function () {
            getContractorList()
        },
        getContractor: function (id) {
            return getContractor(id);
        },
        setCurrentIndex: function (index) {
            setCurrentIndex(index);
        },
        removeContractor: function (id) {
            removeContractor(id);
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

        $("form[name='contractorForm']").validate({
            // Specify validation rules
            rules: {
                name: "required",
                phone: {
                    required: true,
                    minlength: 7,
                }
            },
            messages: {
                name: "Please enter Contractor Name",
                phone: {
                    required: "Please enter a phone number",
                    minlength: "Your phone must consist of at least 7 characters"
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
                var name = $('input#name').val();
                var phone = $('input#phone').val();
                var address = $('textarea#address').val();
                createContractor(name, phone, address);
            }
        });

        $("form[name='contractorEditForm']").validate({
            // Specify validation rules
            rules: {
                editname: "required",
                editphone: {
                    required: true,
                    minlength: 7,
                    number: true
                }
            },
            messages: {
                name: "Please enter Contractor Name",
                phone: {
                    required: "Please enter a phone number",
                    minlength: "Your phone must consist of at least 7 characters"
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
                var name = $('input#editname').val();
                var phone = $('input#editphone').val();
                var address = $('textarea#editaddress').val();
                var id = $('#current-contractorId').val();
                editContractor(name, phone, address, id);
            }
        });
    }

    function getContractorList()
    {
        var content = $('#contractor-content');
        content.empty();
        $.ajax({
            type: "Get",
            url:"/APSE/Contractor/GetContractors",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                $.each(data, function (i, val) {
                    var name = $("<td>" + val.name + "</td>");
                    var phoneNumber = $("<td>" + val.phoneNumber + "</td>");
                    var address = $("<td>" + val.address + "</td>");
                    var editBtn = $("<td><button  data-toggle='modal' data-target='#edit-contractorModal' data-id='" + val.id + "' class='edit-contractor btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>");
                    var deleteBtn = $("<td><button data-id='" + val.id + "' class='delete-contractor btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button></td>");
                    var row = $('<tr></tr>');
                    row.append(name).append(phoneNumber).append(address).append(editBtn).append(deleteBtn);
                    content.append(row);
                });

                _table= tableinit();
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function getContractor(id) {
      return  $.ajax({
            type: "Get",
            url: '/APSE/Contractor/GetContractor/' + id,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                
            },
            error: function (jqXHR, status) {
                console.log(jqXHR);
            }
        });
    }

    function createContractor(name, phoneNumber, address)
    {
        var model =
        {
            'name': name, 'phoneNumber': phoneNumber, 'address': address
        };

        $.ajax({
            type: "Post",
            url: "/APSE/Contractor/CreateContractor",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row.add([
                            name,
                            phoneNumber,
                            address,
                            "<button  data-toggle='modal' data-target='#edit-contractorModal' data-id='" + data.subject + "' class='edit-contractor btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-contractor btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
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

    function editContractor(name, phoneNumber, address,id) {
        var model =
        {
            'name': name, 'phoneNumber': phoneNumber, 'address': address
        };
        $.ajax({
            type: "PUT",
            url: "/APSE/Contractor/UpdateContractor/"+id,
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.key == 200) {
                    if (_table) {
                        _table.row(':eq(' + _cIndex + ')').data([
                            name,
                            phoneNumber,
                            address,
                            "<button  data-toggle='modal' data-target='#edit-contractorModal' data-id='" + data.subject + "' class='edit-contractor btn btn-indigo btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-edit'></i></button></td>",
                            "<button  data-toggle='modal' data-id='" + data.subject + "' class='delete-contractor btn  btn-danger btn-icon' style='height:28px;min-height:28px'><i style='line-height:0.4;color: #FFF;' class='typcn typcn-delete'></i></button>"
                        ]);
                        $('#edit-contractorModal').modal('toggle');
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

    function removeContractor(id) {
      
        $.ajax({
            type: "Delete",
            url: "/APSE/Contractor/DeleteContractor/" + id,
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
      var table= $('#contractorLst').DataTable({
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
                  filename: "Contractors",
                  messageTop: "Contractors",
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
              //$('.dataTables_scrollBody').perfectScrollbar('destroy').perfectScrollbar();
              //ps.update();
              const ps = new PerfectScrollbar('.dataTables_scrollBody');
          }
      });
        return table;
    }

}());