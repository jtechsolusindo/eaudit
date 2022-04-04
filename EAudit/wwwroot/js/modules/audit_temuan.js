var tableDOM = $("#grid");
var modalId = '#modalTemuan';


$(document).ready(function () {
    vmPenugasan.grid.init();
});


function clearForms() {
    $("#txtID").val(null);
    $("#formTambah").trigger('reset');
}

var filters = {}
let vmPenugasan = {
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

            var button_export =[];

            if (role == "Auditor") {
                button_export.push(
                    {
                        "text": 'Tambah Temuan Baru',
                        "attr": {
                            "class": "btn btn-primary btn-sm",
                        },
                        "action": function (e, dt, node, config) {
                            clearForms();
                            modalId = "#modalTemuan";
                            vmNet.modal.makeDraggable = true;
                            vmNet.modal.razorModal.show(modalId, 'Tambah Temuan');
                            get_auditor(id_auditor);
                            get_auditee();
                            get_standar_spmi();
                            // vmNet.goToURL("/configuration/person/auditee/add");
                        }
                    },
                );
            }
            var column_export = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            button_export.push(
                {
                    extend: 'excelHtml5',
                    title: "Data Tanggapan Audit",
                    text: 'Export Excel',
                    titleAttr: 'Excel',
                    className: 'btn-success',
                    exportOptions: { columns: column_export }
                }
            );
            button_export.push(
                {
                    extend: 'pdfHtml5',
                    title: "Data Tanggapan Audit",
                    text: 'Export Pdf',
                    titleAttr: 'Pdf',
                    className: 'btn-warning',
                    exportOptions: { columns: column_export }
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
                "serverSide": false,
                "responsive": false,
                "ajax": {
                    "url": "/api/audit/temuan/list",
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
                scrollCollapse: true,
                fixedColumns: {
                    left: 0,
                    right: 1
                },
                "processing": true,
                "columns": [
                    {
                        "width": "10%",
                        "data": "ID_TEMUAN"
                    },
                    {
                        "width": "10%",
                        "data": "NAMA_LENGKAP_GELAR"
                    }, {
                        "width": "30%",
                        "data": "KODE"
                    }, {
                        "width": "20%",
                        "data": "NAMA_UNIT"
                    },
                    {
                        "width": "20%",
                        "data": "SINGKATAN"
                    },
                    {
                        "width": "20%",
                        "data": "JENIS"
                    },
                    {
                        "width": "20%",
                        "data": "URAIAN"
                    },
                    {
                        "width": "20%",
                        "data": "NO_STANDAR"
                    },
                    {
                        "width": "20%",
                        "data": "PERNYATAAN"
                    },
                    {
                        "width": "20%",
                        "data": null,
                        render: function (data, type, row) {
                            console.log(row);
                            var status = "";
                            if (role == "Auditor") {
                                // jika sudah terkirim
                                if (row.SENT) {
                                    var tgl_terkirim = "";
                                    if (row.SENTDATE != null && row.SENTDATE != "") {
                                        let createdDate = row.SENTDATE;
                                        tgl_terkirim = WaktuIndo(new Date(createdDate));

                                    }
                                    status = "Temuan Audit Sudah Terkirim ke Auditee pada " + tgl_terkirim;
                                } else {
                                    status = "Temuan Audit Belum Terkirim";
                                }
                            }

                            if (role == "Auditee" || role=="Admin") {
                                // jika sudah ditanggapi
                                if (row.STATUSTANGGAPAN) {
                                    var tgl_tanggapan = "";
                                    if (row.TANGGALTANGGAPAN != null && row.TANGGALTANGGAPAN != "") {
                                        let createdDate = row.TANGGALTANGGAPAN;
                                        tgl_tanggapan = WaktuIndo(new Date(createdDate));

                                    }
                                    status = "Sudah ditanggapi pada tanggal " + tgl_tanggapan;
                                } else {
                                    status = " Temuan Audit belum ditanggapi";
                                }
                            }
                            
                            return status;
                        }
                    }, {
                        "searchable": false,
                        "orderable": false,
                        "width": "10%",
                        "data": null,
                        visible: role=="Admin"?false:true,
                        render: function (data, type, row) {
                            if (role == "Auditor") {
                                var actions = [
                                    {
                                        "class": "edit",
                                        "label": "",
                                        "color": "info",
                                        "icon": "pencil-alt",
                                        "url": null,
                                    },
                                    {
                                        "class": "delete",
                                        "label": "",
                                        "color": "danger",
                                        "icon": "trash-alt",
                                        "url": null,
                                    },
                                    {
                                        "class": "kirim_temuan",
                                        "label": "",
                                        "color": "success",
                                        "icon": "paper-plane",
                                        "url": null,
                                    }
                                ];

                                if (!row.SENT) {
                                    var btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                                    return btnLink;
                                } else {
                                    return "<p> Tidak ada Aksi </p>";
                                }
                            } else if (role == "Auditee") {
                                var actions = [
                                    {
                                        "class": "tanggapi",
                                        "label": "Tanggapi",
                                        "color": "info",
                                        "icon": "",
                                        "url": null,
                                    },

                                ];
                                if (!row.STATUSTANGGAPAN) {
                                    var btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                                    return btnLink;
                                } else {
                                    return "<p> Tidak ada Aksi </p>";
                                }
                            } else {
                                return "";
                            }
                           

                            
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
    vmPenugasan.grid.redraw();
}));

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmPenugasan.grid.redraw();
});


function doDelete(data) {
    var formData = new FormData();
    formData.append("id", data.id_temuan);
    $.ajax({
        type: "POST",
        url: '/api/audit/temuan/delete',
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
}

function doKirim(data) {
    var formData = new FormData();
    formData.append("id", data.id_temuan);
    $.ajax({
        type: "POST",
        url: '/api/audit/temuan/kirim',
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
}

$(document).off(EVENT_CLICK, '.delete');
$(document).on(EVENT_CLICK, '.delete', function (e) {
    let data = $(this).data();
    console.log(data);
    vmNet.dialog.message = "Apakah Anda yakin untuk menghapus temuan audit ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDelete, data);


});

$(document).off(EVENT_CLICK, '.kirim_temuan');
$(document).on(EVENT_CLICK, '.kirim_temuan', function (e) {
    let data = $(this).data();
    console.log(data);
    vmNet.dialog.message = "<b>Yakin Kirim audit ini ?</b> <br>  Dengan menekan kirim maka temuan audit akan dibuat dan tidak dapat diubah lagi setelahnya. Pastikan seluruh data temuan audit sudah diisi dengan benar.";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doKirim, data);
});

$(document).off(EVENT_CLICK, '.edit');
$(document).on(EVENT_CLICK, '.edit', function (e) {
    modalId = "#modalTemuan";
    vmNet.modal.razorModal.show(modalId, 'Edit Temuan Audit');
    clearForms();
    let data = $(this).data();
    var id = data.id_temuan;
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/audit/temuan/detail/" + id,
        success: function (res) {
            $("#txtID").val(res.ID_TEMUAN);
            $("#input_uraian").val(res.URAIAN);
            $("#input_jenis").val(res.JENIS).trigger('change');
            get_auditor(res.ID_AUDITOR);
            get_auditee(res.ID_AUDITEE);
            get_standar_spmi(res.ID_STANDAR);
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

$(document).off(EVENT_CLICK, '.tanggapi');
$(document).on(EVENT_CLICK, '.tanggapi', function (e) {
    modalId = "#modalTanggapan";
    vmNet.modal.razorModal.show(modalId, 'Tambah Tanggapan');
    let data = $(this).data();
    var id = data.id_temuan;
    $("#formEditTanggapan").trigger("reset");
    $("#tgp_id_temuan").val(id);
    get_akar_masalah();
});


$(document).off(EVENT_CLICK, '.modal-footer #btnCancel');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel', function () {
    vmNet.modal.razorModal.hide(modalId);
});

$("#formTambah").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    formData.append("id_auditor", id_auditor);
    if (!confirm('Apakah Anda yakin?')) return false;
    $.ajax({
        type: "POST",
        url: '/api/audit/temuan/save',
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

$("#formEditTanggapan").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    if (!confirm('Apakah Anda yakin?')) return false;
    $.ajax({
        type: "POST",
        url: '/api/audit/tanggapan/save',
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


function get_auditor(value = "") {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/auditor/list",
        success: function (res) {
            var results = [];
            for (var idx = 0; idx < res.length; idx++) {
                results.push({
                    id: res[idx].ID,
                    text: res[idx].NAMA_LENGKAP_GELAR,
                });
            }
            $.when($("#input_auditor").select2({ data: results })).done(function () {
                if (value != "") {
                    $("#input_auditor").val(value).trigger('change');
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

function get_auditee(value = "") {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/auditee/list",
        success: function (res) {
            var results = [];
            for (var idx = 0; idx < res.length; idx++) {
                results.push({
                    id: res[idx].ID,
                    text: res[idx].NAMA,
                });
            }
            $.when($("#input_auditee").select2({ data: results })).done(function () {
                if (value != "") {
                    $("#input_auditee").val(value).trigger('change');
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

function get_standar_spmi(value = "") {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/standar_spmi/list",
        success: function (res) {
            var results = [];
            for (var idx = 0; idx < res.length; idx++) {
                results.push({
                    id: res[idx].ID,
                    text: res[idx].NOSTANDAR +" - "+ res[idx].PERNYATAAN,
                });
            }
            $.when($("#input_no_standar").select2({ data: results })).done(function () {
                if (value != "") {
                    $("#input_no_standar").val(value).trigger('change');
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

$("#input_no_standar").change(function () {
    var id = $(this).val();
    get_detail_standar_spmi(id);
});

function get_detail_standar_spmi(id) {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/standar_spmi/detail/" + id,
        success: function (res) {
            console.log(res);
            $("#pernyataan").val(res.PERNYATAAN);
        },
        error: function (e) {
            show_error_ajax(e.status);
        },
        complete: function () {
            hide_loading();
        }
    });
}

function get_akar_masalah(value = "") {
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/unsur_manajemen/list",
        success: function (res) {
            var results = [];
            for (var idx = 0; idx < res.length; idx++) {
                results.push({
                    id: res[idx].ID_UNSUR_MANAJEMEN,
                    text: res[idx].DESKRIPSI,
                });
            }
            $.when($("#input_akar_masalah").select2({ data: results })).done(function () {
                if (value != "") {
                    $("#input_akar_masalah").val(value).trigger('change');
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

$('input[type=radio][name=jenis]').change(function () {
    if (this.value == 'doc') {
        $(".div_input_link").hide();
        $(".div_input_file").show();
    }
    else if (this.value == 'vid') {
        $(".div_input_file").hide();
        $(".div_input_link").show();
    }
});







