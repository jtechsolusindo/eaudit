var tableDOM = $("#grid");
var modalId = '';


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

            var button_export = [];
            var column_export = [1, 2, 3, 4, 5, 6, 7,8,9];
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
                    modifier: {
                        // DataTables core
                        order: 'index', // 'current', 'applied',
                        //'index', 'original'
                        page: 'all', // 'all', 'current'
                        search: 'none' // 'none', 'applied', 'removed'
                    },
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
                    "url": "/api/audit/tanggapan/list",
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
                        "data": "ID_TANGGAPAN"
                    },
                    {
                        "width": "10%",
                        "data": "DESKRIPSI"
                    }, {
                        "width": "30%",
                        "data": "ANALISIS"
                    }, {
                        "width": "20%",
                        "data": "KOREKSI"
                    },
                    {
                        "width": "20%",
                        "data": "KOREKTIF"
                    },
                    {
                        "width": "20%",
                        "data": null,
                        render: function (data, type, row) {
                            link = "";
                            if (row.LINK != "") {
                                link = '<a href="' + row.LINK + '" target="_blank">' + row.NAMA_FILE + '</a>';
                            } else {
                                link = '<a href="/file_audit/' + row.DOKUMEN + '" target="_blank">' + row.NAMA_FILE + '</a>';
                            }
                            return link;
                        }
                    },
                  
                    {
                        "width": "20%",
                        "data": null,
                        "visible": role == "Auditee" || role == "Admin"?true:false,
                        render: function (data, type, row) {
                            console.log(row);
                            var status = "";
                            if (row.SENT) {
                                var tgl_tanggapan = "";
                                if (row.SENTDATE != null && row.SENTDATE != "") {
                                    let createdDate = row.SENTDATE;
                                    tgl_tanggapan = WaktuIndo(new Date(createdDate));

                                }
                                status = " Dikirim dan Disetujui pada " + tgl_tanggapan;
                            } else {
                                status = "Tanggapan Audit Belum Terkirim";
                            }

                            return status;
                        }
                    },
                    {
                        "width": "20%",
                        "data": null,
                        "visible": role == "Auditee" || role == "Admin"? true : false,
                        render: function (data, type, row) {
                            console.log(row);
                            var status = "";
                            if (role == "Auditee") {
                                if (row.SENT == "" || row.SENT == null) {
                                    status = "Belum di verifikasi";
                                } else {
                                    status = row.KONFIRMASI;
                                }
                            } else if (role == "Admin") {
                                status = row.KONFIRMASI;
                            }
                           
                            return status;
                        }
                    },
                   
                    {
                        "width": "20%",
                        "data": "NAMA_AUDITOR",
                        "visible": role == "Auditee" ? true : false,
                    },
                    {
                        "width": "20%",
                        "data": "NAMA_UNIT",
                        "visible": role == "Admin" || role == "Auditor" ? true : false,
                    },
                    {
                        "searchable": false,
                        "orderable": false,
                        "width": "10%",
                        "data": null,
                        render: function (data, type, row) {
                            var konfirmasi = row.KONFIRMASI;
                            var sent = row.SENT;
                            if (role == "Auditee") {
                                if (konfirmasi == null) {
                                    if (sent == false || sent == null) {
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
                                                "class": "kirim_tanggapan",
                                                "label": "",
                                                "color": "success",
                                                "icon": "paper-plane",
                                                "url": null,
                                            }
                                        ];
                                        var btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                                        return btnLink;
                                    }

                                } else if (konfirmasi == "Setuju") {
                                    return "<p> Tidak ada Aksi </p>";
                                } else {
                                    if (row.FINISHBYADMIN) {
                                        return " <p> Tidak ada Aksi <br/> diselesaikan oleh admin.</p>";
                                    } else {
                                        if (sent == false || sent == null) {
                                            var actions = [
                                                {
                                                    "class": "edit",
                                                    "label": "",
                                                    "color": "info",
                                                    "icon": "pencil-alt",
                                                    "url": null,
                                                },
                                                {
                                                    "class": "kirim_tanggapan",
                                                    "label": "",
                                                    "color": "success",
                                                    "icon": "paper-plane",
                                                    "url": null,
                                                }
                                            ];
                                            var btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                                            return btnLink;
                                        }
                                    }
                                }
                            } else if (role == "Auditor") {
                                var actions = [
                                    {
                                        "class": "verifikasi",
                                        "label": "Verifikasi",
                                        "color": "info",
                                        "icon": "",
                                        "url": null,
                                    },

                                ];
                                var btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                                return btnLink;
                            } else if (role == "Admin") {
                                if (row.FINISHBYADMIN) {
                                    return " <p> Tidak ada Aksi <br/> diselesaikan oleh admin.</p>";
                                } else {
                                    if (konfirmasi == "Setuju") {
                                        return "<p> Tidak ada Aksi </p>";
                                    } else {
                                        var actions = [
                                            {
                                                "class": "selesaikan",
                                                "label": "Selesaikan",
                                                "color": "info",
                                                "icon": "",
                                                "url": null,
                                            },
                                            {
                                                "class": "delete",
                                                "label": "",
                                                "color": "danger",
                                                "icon": "trash-alt",
                                                "url": null,
                                            },
                                        ];
                                        var btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                                        return btnLink;
                                    }
                                }
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
    formData.append("id", data.id_tanggapan);
    $.ajax({
        type: "POST",
        url: '/api/audit/tanggapan/delete',
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
    formData.append("id", data.id_tanggapan);
    $.ajax({
        type: "POST",
        url: '/api/audit/tanggapan/kirim',
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

function doSelesaikan(data) {
    var formData = new FormData();
    formData.append("id", data.id_tanggapan);
    $.ajax({
        type: "POST",
        url: '/api/audit/tanggapan/selesaikan',
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
    vmNet.dialog.message = "Apakah Anda yakin untuk menghapus tanggapan audit ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDelete, data);


});

$(document).off(EVENT_CLICK, '.kirim_tanggapan');
$(document).on(EVENT_CLICK, '.kirim_tanggapan', function (e) {
    let data = $(this).data();
    console.log(data);
    vmNet.dialog.message = "<b>Yakin Kirim tanggapan ini ?</b>";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doKirim, data);
});

$(document).off(EVENT_CLICK, '.selesaikan');
$(document).on(EVENT_CLICK, '.selesaikan', function (e) {
    let data = $(this).data();
    console.log(data);
    vmNet.dialog.message = "<b>Apakah Anda Yakin Ingin Menyelesaikan Data Tanggapan Tersebut ?</b>";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doSelesaikan, data);
});

$(document).off(EVENT_CLICK, '.edit');
$(document).on(EVENT_CLICK, '.edit', function (e) {
    modalId = "#modalTanggapan";
    vmNet.modal.razorModal.show(modalId, 'Ubah Tanggapan');
    $("#formEditTanggapan").trigger("reset");
    let data = $(this).data();
   
    var id = data.id_tanggapan;
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/audit/tanggapan/detail/" + id,
        success: function (res) {
            $("#tgp_id_edit").val(res.ID_TANGGAPAN);
            $("#tgp_id_temuan").val(res.ID_TEMUAN);
            $("#input_analisis").val(res.ANALISIS);
            $("#input_koreksi").val(res.KOREKSI);
            $("#input_korektif").val(res.KOREKTIF);
            $("#input_nama_file").val(res.NAMA_FILE);
            $("#input_link").val(res.LINK);
            var jenis = res.LINK == "" || res.LINK==null ? 'doc' : 'vid';
            $("input[name=jenis][value="+jenis+"]").prop('checked', true).trigger('change');

            $("#tgp_id_temuan").val(res.ID_TEMUAN);
            get_akar_masalah(res.ID_UNSUR_MANAJEMEN);
            if (res.JENIS == "OB") {
                $("#div_korektif").hide();
            } else {
                $("#div_korektif").show();
            }
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

$(document).on(EVENT_CLICK, '.verifikasi', function (e) {
    modalId = "#modalVerifikasi";
    vmNet.modal.razorModal.show(modalId, 'Verifikasi Auditor');
    $("#formVerifikasi").trigger("reset");
    let data = $(this).data();
    $("#id_tanggapan").val(data.id_tanggapan);
    console.log(data);

});




$(document).off(EVENT_CLICK, '.modal-footer #btnCancel');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel', function () {
    vmNet.modal.razorModal.hide(modalId);
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

$("#formVerifikasi").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    if (!confirm('Apakah Anda yakin?')) return false;
    $.ajax({
        type: "POST",
        url: '/api/audit/tanggapan/verifikasi',
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







