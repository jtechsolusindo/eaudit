var tableDOM = $("#gridAuditor");

var filters = {}

$(document).ready(function () {
    vmAuditor.grid.init();
});

function jsonObjectToSelect2(jsonObject) {
    console.log(jsonObject);
    let results = [];
    for (var idx = 0; idx < jsonObject.length; idx++) {
        results.push({
            id: jsonObject[idx].npp,
            text: jsonObject[idx].nama,
        });
    }
    return results;
    // console.log(results);
}
function clearForms() {
    $("#txtID").val(null);
    $("#txtNPP").val(null).trigger('change');
    $("#formTambah").trigger('reset');

}
let vmAuditor = {
    grid: {
        redraw: function () {
            filters = {}
            var formData = new FormData($("#frmFilter")[0]);
            for (var [key, value] of formData.entries()) {
                filters[key] = value;
            }
            tableDOM.DataTable().ajax.reload();
            tableDOM.DataTable().draw(true);
        },
        init: function () {
            filters = {}
            var formData = new FormData($("#frmFilter")[0]);
            for (var [key, value] of formData.entries()) {
                filters[key] = value;
            }
            dtTableExportButtons = [
                {
                    "text": 'Auditor Baru',
                    "attr": {
                        "class": "btn btn-primary btn-sm",
                        "data-target":"#kiModalContainer"
                    },
                    "action": function (e, dt, node, config) {
                        vmNet.modal.razorModal.show("#kiModalContainer", 'Tambah Auditor');
                        clearForms();
                        $('#kiModalContainer').modal('show');
                        get_pegawai_unassigned();
                        // vmNet.goToURL("/configuration/person/auditor/add");
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: "Data Auditor",
                    text: 'Export Sebagai Excel',
                    titleAttr: 'Excel',
                    exportOptions: { columns: [1, 2, 3, 4, 5, 6] }
                }
            ];
            
          
           
            let grid = tableDOM.DataTable({
                "ordering": true,
                "searching": false,
                "pageLength": 10,
                "lengthChange": false,
                "paging": true,
                "select": "single",
                "destroy": true,
                "scrollX": true,
                "responsive": true,
                "ajax": {
                    "url": "/api/auditor/list",
                    "type": "POST",
                    "dataSrc": function (data) {
                        return data;
                    },
                    'data': function (data) {
                        return filters;
                    }
                },

                'autoWidth': false,
                "dom": 'Bfrtip',
                "buttons": dtTableExportButtons,
                "order": [[0, "desc"]],
                "processing": true,
                "columns": [
                    {
                        "width": "5%",
                        "data": "ID"
                    },
                    {
                        "width": "10%",
                        "data": "NPP"
                    },
                    {
                        "width": "30%",
                        "data": "NAMA_LENGKAP_GELAR"
                    },
                    {
                        "width": "10%",
                        "data": "EMAIL"
                    },
                    {
                        "width": "10%",
                        "data": "KODE_UNIT"
                    },
                    {
                        "width": "20%",
                        "data": "NAMA_UNIT"
                    },
                    {
                        "width": "10%",
                        "data": "PRODI"
                    }, {
                        "searchable": false,
                        "orderable": false,
                        "width": "10%",
                        "data": null,
                        render: function (data, type, row) {
                            let actions = [
                                {
                                    "class": "edit",
                                    "label": "Edit",
                                    "color": "info",
                                    "icon": "pencil",
                                    "url": null,
                                },
                                {
                                    "class": "delete",
                                    "label": "Delete",
                                    "color": "danger",
                                    "icon": "pencil",
                                    "url": null,
                                }
                            ];

                            let btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                            return btnLink;
                        }
                    },
                ] // end fo columns
            });
            grid.on('order.dt search.dt', function () {
                grid.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            return grid;
        }
    }
};
$("#frmFilter").on('submit', (function (e) {
    e.preventDefault();
    vmAuditor.grid.redraw();
}));

function doDeleteAuditor(postData) {
    $.when(vmNet.ajax.post('/api/auditor/delete', postData)).done(function (e) {
        if (e.result === "ok") {
            vmNet.notification.show(e.message, vmNet.message.type.success, RELOAD_NO_REPOST);
        } else {
            vmNet.notification.show(e.message, vmNet.message.type.error, BLANK_STRING);
        }
    });
}

$(document).off(EVENT_CLICK, '.delete');
$(document).on(EVENT_CLICK, '.delete', function (e) {
    let data = $(this).data();
    let postData = {
        ID: data.id,
    };

    vmNet.dialog.message= "Apakah Anda yakin untuk menghapus Auditor ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDeleteAuditor, postData);

    
});

$(document).off(EVENT_CLICK, '.edit');
$(document).on(EVENT_CLICK, '.edit', function (e) {
    vmNet.modal.razorModal.show("#kiModalContainer", 'Edit Auditor');
    clearForms();
    let data = $(this).data();
    var id = data.id;
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/auditor/detail/"+id,
        success: function (res) {
            $("#txtID").val(res.ID);
            $("#txtKode").val(res.KODE);
            $("#txtProgramStudi").val(res.PRODI);
            $("#txtEmail").val(res.EMAIL);
            get_pegawai_unassigned(res.NPP, res.NAMA_LENGKAP_GELAR);
        },
        error: function (e) {
            show_error_ajax(e.status);
        },
        complete: function () {
            hide_loading();
        }
    });
    console.log(data);
});

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmAuditor.grid.redraw();
});



$(document).off(EVENT_CLICK, '.modal-footer #btnSave_ModalAuditor');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel_ModalAuditor', function () {
    $('#kiModalContainer').modal('hide');
});

$("#formTambah").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    if (!confirm('Apakah Anda yakin?')) return false;
    $.ajax({
        type: "POST",
        url: '/api/auditor/save',
        contentType: false,
        processData: false,
        data: formData,
        dataType: 'json',
        beforeSend: function () {
            show_loading();
        },
        success: function (res) {
            if (res.result == "ok") {
               vmNet.notification.show(res.message, vmNet.message.type.success, RELOAD_NO_REPOST);
            } else {
                vmNet.notification.show(res.message, vmNet.message.type.error, BLANK_STRING);
            }

        },
        error: function (e) {
            show_error_ajax(e.status);
        },
        complete: function () {
            hide_loading();
        }
    });
});

function get_pegawai_unassigned(value="",nama="") {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/pegawai_unassigned_opsi",
        success: function (res) {
            if (value != "") {
                res.push({
                    id: value,
                    text: nama,
                });
            }
           
            $.when($("#txtNPP").select2({ data: res })).done(function () {
                if (value != "") {
                    $("#txtNPP").val(value).trigger('change');
                }
               
            });
            
        },
        error: function (e) {
            show_error_ajax(e.status);
        },
        complete: function () {
            hide_loading();
        }
    });
}


