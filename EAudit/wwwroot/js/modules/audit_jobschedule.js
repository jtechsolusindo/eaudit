var tableDOM = $("#grid");
var modalId = '#modalPenugasan';

var filters = {
    search: _getVal('inputSearch'),
    id_auditor: id_auditor
}

$(document).ready(function () {
    vmPenugasan.grid.init();
});

function jsonObjectToSelect2(jsonObject) {
    console.log(jsonObject);
    let results = [];
    for (var idx = 0; idx < jsonObject.length; idx++) {
        results.push({
            id: jsonObject[idx].id,
            text: jsonObject[idx].nama,
        });
    }
    return results;
    // console.log(results);
}
function clearForms() {
    $("#txtID").val(null);
    $("#txtIdAuditee").val("").trigger('change');
    $("#txtJamMulai").val("08:00");
    $("#txtJamAkhir").val("08:00");
    $("#txtTglKegiatan").val("");
    $("#txtIdAuditor").val("").trigger('change');
}
let vmPenugasan = {
    grid: {
        redraw: function () {
            filters.search = _getVal('inputSearch');
            filters.id_auditor = id_auditor;
            tableDOM.DataTable().ajax.reload();
            tableDOM.DataTable().draw(true);
        },
        init: function () {
            var button_export = [];
            var is_action_visible = false;
            if (role == "Admin") {
                is_action_visible = true;
                button_export.push(
                    {
                        "text": 'Penugasan Baru',
                        "attr": {
                            "class": "btn btn-primary btn-sm",
                            "data-target": modalId
                        },
                        "action": function (e, dt, node, config) {
                            $.when(vmNet.ajax.post('/audit/job_schedule/add', null)).done(function (e) {
                                if (e.isValid === false) {
                                    vmNet.notification.show(e.message, MSG_ERROR, BLANK_STRING);
                                } else {
                                    vmNet.modal.makeDraggable = true;
                                    vmNet.modal.razorModal.show(modalId, 'Tambah Penugasan');

                                    $.when($("#txtIdAuditee").select2({ data: jsonObjectToSelect2(e.globalData.auditeeList) })).done(function (e) {
                                        clearForms();
                                    });

                                    $.when($("#txtIdAuditor").select2({ data: jsonObjectToSelect2(e.globalData.auditorList) })).done(function (e) {
                                        clearForms();
                                    });
                                }

                            });
                            // vmNet.goToURL("/configuration/person/auditee/add");
                        }
                    }
                );
            }
            button_export.push(
                {
                    extend: 'excelHtml5',
                    title: "Data Penugasan",
                    text: 'Export Sebagai Excel',
                    titleAttr: 'Excel',
                    exportOptions: { columns: [1, 2, 3, 4] }
                }
            );
            dtTableExportButtons = button_export;


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
                    "url": "/api/audit/scheduler/penugasan",
                    "type": "POST",
                    "dataType": "json",
                    "contentType": "application/json;charset=utf-8",
                    "dataSrc": function (data) {
                        return data;
                    },
                    'data': function (data) {
                        return JSON.stringify(filters);
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
                        "data": "TANGGAL"
                    },
                    {
                        "width": "10%",
                        "data": "WAWE"
                    }, {
                        "width": "30%",
                        "data": "NAMA_UNIT"
                    }, {
                        "width": "20%",
                        "data": "NAMA_LENGKAP"
                    },
                    {
                        "searchable": false,
                        "orderable": false,
                        "width": "10%",
                        "visible":is_action_visible,
                        "data": null,
                        render: function (data, type, row) {
                            let actions = [
                                {
                                    "class": "edit",
                                    "label": "Edit",
                                    "color": "info",
                                    "icon": "",
                                    "url": null,
                                },
                                {
                                    "class": "delete",
                                    "label": "Delete",
                                    "color": "danger",
                                    "icon": "",
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

$(document).off(EVENT_CLICK, '#btnFilterSearch');
$(document).on(EVENT_CLICK, '#btnFilterSearch', function (e) {

    vmPenugasan.grid.redraw();
})

function doDeleteJadwal(postData) {
    $.when(vmNet.ajax.post('/api/audit/scheduler/penugasan/delete', postData)).done(function (e) {
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

    vmNet.dialog.message = "Apakah Anda yakin untuk menghapus jadwal penugasan ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDeleteJadwal, postData);


});

$(document).off(EVENT_CLICK, '.edit');
$(document).on(EVENT_CLICK, '.edit', function (e) {
    let data = $(this).data();
    let postData = {
        ID: data.id
    };
    $.when(vmNet.ajax.post('/audit/job_schedule/edit', postData)).done(function (e) {
        vmNet.modal.razorModal.show(modalId, 'Edit Penugasan');

        let select2DataSource = {
            data: jsonObjectToSelect2(e.globalData.auditeeList)
        }
        let select2DataSource2 = {
            data: jsonObjectToSelect2(e.globalData.auditorList)
        }
        clearForms();
        var id_auditor = e.globalData.jadwalRow.iD_AUDITOR;
        var id_auditor_array = id_auditor.split("#");
        $("#txtIdAuditee").select2(select2DataSource);
        $("#txtIdAuditor").select2(select2DataSource2);
        $("#txtIdAuditee").val(e.globalData.jadwalRow.iD_AUDITEE).trigger('change');
        $("#txtIdAuditor").val(id_auditor_array).trigger('change');
        $("#txtID").val(e.globalData.jadwalRow.id);
        $("#txtJamMulai").val(e.globalData.jadwalRow.ws);
        $("#txtJamAkhir").val(e.globalData.jadwalRow.we);
        $("#txtTglKegiatan").val(e.globalData.jadwalRow.tanggal);

    });

    console.log(data);
});

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmPenugasan.grid.redraw();
});

$(document).off(EVENT_CLICK, '.modal-footer #btnCancel');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel', function () {
    vmNet.modal.razorModal.hide(modalId);
});




$("#formTambah").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    if (!confirm('Apakah Anda yakin?')) return false;

    let saveUri = '/api/audit/scheduler/penugasan/save';
    var id_auditor_array = $("#txtIdAuditor").val();
    console.log(id_auditor_array);
    var id_auditor_str = id_auditor_array.join("#");
    console.log(id_auditor_str);
    let personData = {
        ID: $("#txtID").val() == "" ? null : $("#txtID").val(),
        TANGGAL: $("#txtTglKegiatan").val(),
        ID_AUDITEE: parseInt($("#txtIdAuditee").val()),
        NAMA_UNIT: $("#txtIdAuditee option:selected").text(),
        WS: $("#txtJamMulai").val(),
        WE: $("#txtJamAkhir").val(),
        ID_AUDITOR: id_auditor_str
    };

    $.when(vmNet.ajax.post(saveUri, personData)).done(function (e) {
        if (e.result === "ok") {
            vmNet.notification.show(e.message, vmNet.message.type.success, RELOAD_NO_REPOST);
        } else {
            vmNet.notification.show(e.message, vmNet.message.type.error, BLANK_STRING);
        }
    });
});

