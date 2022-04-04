var tableDOM = $("#gridAuditee");
var modalId = '#modalAuditee';

var filters = {}
$(document).ready(function () {
    vmAuditee.grid.init();
});

function clearForms() {
    $("#txtID").val(null);
    $("#formTambah").trigger('reset');
}

let vmAuditee = {
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
                    "text": 'Auditee Baru',
                    "attr": {
                        "class": "btn btn-primary btn-sm",
                        "data-target": modalId
                    },
                    "action": function (e, dt, node, config) {
                        clearForms();
                        vmNet.modal.makeDraggable = true;
                        vmNet.modal.razorModal.show(modalId, 'Tambah Auditee');
                        get_auditee_unassigned();
                        
                        // vmNet.goToURL("/configuration/person/auditee/add");
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: "Data Auditee",
                    text: 'Export Sebagai Excel',
                    titleAttr: 'Excel',
                    exportOptions: { columns: [1, 2, 3, 4] }
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
                    "url": "/api/auditee/list",
                    "type": "POST",
                    "dataType": "json",
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
                    }, {
                        "width": "30%",
                        "data": "NAMA_UNIT"
                    }, {
                        "width": "20%",
                        "data": "KODE"
                    }, {
                        "width": "30%",
                        "data": "NAMA"
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
    vmAuditee.grid.redraw();
}));
$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmAuditee.grid.redraw();
});

function doDeleteauditee(postData) {
    $.when(vmNet.ajax.post('/api/auditee/delete', postData)).done(function (e) {
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

    vmNet.dialog.message = "Apakah Anda yakin untuk menghapus auditee ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDeleteauditee, postData);


});

$(document).off(EVENT_CLICK, '.edit');
$(document).on(EVENT_CLICK, '.edit', function (e) {
    vmNet.modal.razorModal.show(modalId, 'Edit Auditee');
    clearForms();
    let data = $(this).data();
    var id = data.id;
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/auditee/detail/" + id,
        success: function (res) {
            $("#input_id_edit").val(res.ID);
            $("#input_singkatan").val(res.KODE);
            $("#input_nama_auditee").val(res.NAMA);
            get_auditee_unassigned(res.ID_UNIT, res.NAMA_UNIT);
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


$(document).off(EVENT_CLICK, '.modal-footer #btnCancel');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel', function () {
    vmNet.modal.razorModal.hide(modalId);
});


$("#formTambah").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    if (!confirm('Apakah Anda yakin?')) return false;
    $.ajax({
        type: "POST",
        url: '/api/auditee/save',
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



function get_auditee_unassigned(value = "", nama = "") {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/auditee_unassigned_opsi",
        success: function (res) {
            if (value != "") {
                res.push({
                    id: value,
                    text: nama,
                });
            }

            $.when($("#input_id_unit").select2({ data: res })).done(function () {
                if (value != "") {
                    $("#input_id_unit").val(value).trigger('change');
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


