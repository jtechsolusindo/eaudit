var tableDOM = $("#gridAuditee");
var modalId = '#modalAuditee';

var filters = {
    searchType: 'Auditee',
    search: _getVal('inputSearch')
}

$(document).ready(function () {
    vmAuditee.grid.init();
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
    $("#txtUnitAuditee").val(null).trigger('change');
    $("#txtSingkatanResmi").val(null);
    $("#txtNamaAuditee").val(null);
}
let vmAuditee = {
    grid: {
        redraw: function () {
            filters.search = _getVal('inputSearch');
            filters.searchType = 'Auditee';

            tableDOM.DataTable().ajax.reload();
            tableDOM.DataTable().draw(true);
        },
        init: function () {
            dtTableExportButtons = [
                {
                    "text": 'Auditee Baru',
                    "attr": {
                        "class": "btn btn-primary btn-sm",
                        "data-target": modalId
                    },
                    "action": function (e, dt, node, config) {
                        $.when(vmNet.ajax.post('/configuration/person/auditee/add_modal', { 'mode': 'new' })).done(function (e) {
                            if (e.isValid === false) {
                                vmNet.notification.show(e.message, MSG_ERROR, BLANK_STRING);
                            } else {
                                vmNet.modal.makeDraggable = true;
                                vmNet.modal.razorModal.show(modalId, 'Tambah Auditee');

                                $.when($("#txtUnitAuditee").select2({ data: jsonObjectToSelect2(e.globalData.employeeUnassigned) })).done(function (e) {
                                    clearForms();
                                });
                            }
                        });
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
                    "url": "/api/configuration/person/auditee",
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

$(document).off(EVENT_CLICK, '#btnFilterSearch');
$(document).on(EVENT_CLICK, '#btnFilterSearch', function (e) {

    vmAuditee.grid.redraw();
})

function doDeleteauditee(postData) {
    $.when(vmNet.ajax.post('/api/configuration/person/auditee/delete', postData)).done(function (e) {
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
    let data = $(this).data();
    let postData = {
        'mode': 'edit',
        ID: data.id,
        NPP: data.npp
    };
    $.when(vmNet.ajax.post('/configuration/person/auditee/edit_modal', postData)).done(function (e) {
        vmNet.modal.razorModal.show(modalId, 'Edit Auditee');

        let select2DataSource = {
            data: jsonObjectToSelect2(e.globalData.employeeUnassigned)
        }
        clearForms();
        $("#txtUnitAuditee").select2(select2DataSource);
        $("#txtUnitAuditee").val(e.globalData.auditeeRow.iD_UNIT).trigger('change');
        $("#txtID").val(e.globalData.auditeeRow.id);
        $("#txtSingkatanResmi").val(e.globalData.auditeeRow.kode);
        $("#txtNamaAuditee").val(e.globalData.auditeeRow.nama);

    });

    console.log(data);
});

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmAuditee.grid.redraw();
});

$(document).off(EVENT_CLICK, '.modal-footer #btnCancel');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel', function () {
    vmNet.modal.razorModal.hide(modalId);
});

$(document).off(EVENT_CLICK, '#btnSave');
$(document).on(EVENT_CLICK, '#btnSave', function (e) {
    let saveUri = '/api/configuration/person/auditee/save';

    let personData = {
        ID: $("#txtID").val() == "" ? null : $("#txtID").val(),
        ID_UNIT: parseInt($("#txtUnitAuditee").val()),
        SINGKATAN: $("#txtSingkatanResmi").val(),
        NAMA: $("#txtNamaAuditee").val()
    };

    $.when(vmNet.ajax.post(saveUri, personData)).done(function (e) {
        if (e.result === "ok") {
            vmNet.notification.show(e.message, vmNet.message.type.success, RELOAD_NO_REPOST);
        } else {
            vmNet.notification.show(e.message, vmNet.message.type.error, BLANK_STRING);
        }
    });
});

