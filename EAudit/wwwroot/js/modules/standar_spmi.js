var tableDOM = $("#gridStandarSPMI");
var modalId = '#modalStandarSPMI';

var filters = {
    search: _getVal('inputSearch')
}

$(document).ready(function () {
    vmStandarSPMI.grid.init();
});

function clearForms() {
    $("#txtId").val(null);
    $("#txtNoStandar").val(null).trigger('change');
    $("#txtPernyataan").val(null);
    $("#txtIndikator").val(null);
}
let vmStandarSPMI = {
    grid: {
        redraw: function () {
            filters.search = _getVal('inputSearch');

            tableDOM.DataTable().ajax.reload();
            tableDOM.DataTable().draw(true);
        },
        init: function () {
            dtTableExportButtons = [
                {
                    "text": 'Standar SPMI Baru',
                    "attr": {
                        "class": "btn btn-primary btn-sm",
                        "data-target": modalId
                    },
                    "action": function (e, dt, node, config) {
                        $.when(vmNet.ajax.post('/configuration/lookup/standar_spmi/add', null)).done(function (e) {
                            if (e.isValid === false) {
                                vmNet.notification.show(e.message, MSG_ERROR, BLANK_STRING);
                            } else {
                                vmNet.modal.makeDraggable = true;
                                vmNet.modal.razorModal.show(modalId, 'Tambah Standar SPMI');
                                clearForms();
                            }
                        });
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: "Data Standar SPMI",
                    text: 'Export Sebagai Excel',
                    titleAttr: 'Excel',
                    exportOptions: { columns: [1, 2, 3] }
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
                    "url": "/api/configuration/lookup/standar_spmi",
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
                        "width": "15%",
                        "data": "NOSTANDAR"
                    }, {
                        "width": "40%",
                        "data": "PERNYATAAN"
                    }, {
                        "width": "30%",
                        "data": "INDIKATOR"
                    },
                    {
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

    vmStandarSPMI.grid.redraw();
})

$(document).off(EVENT_CLICK, '.delete');
$(document).on(EVENT_CLICK, '.delete', function (e) {
    let data = $(this).data();
    let postData = {
        ID: data.id,
    };

    vmNet.dialog.message = "Apakah Anda yakin untuk menghapus Standar SPMI ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDeleteStandarSPMI, postData);
});

function doDeleteStandarSPMI(postData) {
    $.when(vmNet.ajax.post('/api/configuration/lookup/standar_spmi/delete', postData)).done(function (e) {
        if (e.result === "ok") {
            vmNet.notification.show(e.message, vmNet.message.type.success, RELOAD_NO_REPOST);
        } else {
            vmNet.notification.show(e.message, vmNet.message.type.error, BLANK_STRING);
        }
    });
}


$(document).off(EVENT_CLICK, '.edit');
$(document).on(EVENT_CLICK, '.edit', function (e) {
    let data = $(this).data();
    let postData = {
        'mode': 'edit',
        ID: data.id
    };
    $.when(vmNet.ajax.post('/configuration/lookup/standar_spmi/edit', postData)).done(function (e) {
        vmNet.modal.razorModal.show(modalId, 'Edit Standar SPMI');

        clearForms();
        $("#txtId").val(e.globalData.spmi.id);
        $("#txtNoStandar").val(e.globalData.spmi.nostandar);
        $("#txtIndikator").val(e.globalData.spmi.indikator);
        $("#txtPernyataan").val(e.globalData.spmi.pernyataan);
    });
});

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmStandarSPMI.grid.redraw();
});

$(document).off(EVENT_CLICK, '.modal-footer #btnCancel');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel', function () {
    vmNet.modal.razorModal.hide(modalId);
});


$("#formTambah").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    if (!confirm('Apakah Anda yakin?')) return false;
    let saveUri = '/api/configuration/lookup/standar_spmi/save';

    let postData = {
        ID: $("#txtId").val() == "" ? null : parseInt($("#txtId").val()),
        NOSTANDAR: $("#txtNoStandar").val(),
        PERNYATAAN: $("#txtPernyataan").val(),
        INDIKATOR: $("#txtIndikator").val()
    };

    $.when(vmNet.ajax.post(saveUri, postData)).done(function (e) {
        if (e.result === "ok") {
            vmNet.notification.show(e.message, vmNet.message.type.success, RELOAD_NO_REPOST);
        } else {
            vmNet.notification.show(e.message, vmNet.message.type.error, BLANK_STRING);
        }
    });
});
