
var modalId = '#modalForm';


function clearForms() {
    $("#txtID").val(null);
    $("#formTambah").trigger('reset');
}

function show_modal_tambah() {
    clearForms();
    vmNet.modal.makeDraggable = true;
    vmNet.modal.razorModal.show(modalId, 'Tambah Pengumuman');
}


function doDeleteauditee(formData) {
    $.ajax({
        type: "POST",
        url: '/api/dashboard/delete',
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


function delete_data(id) {
    var postData = new FormData();
    postData.append('id', id);
    vmNet.dialog.message = "Apakah Anda yakin untuk menghapus dashboard ini ?";
    vmNet.dialog.confirmation(BUTTON_YES, BUTTON_NO, doDeleteauditee, postData);
}
function show_modal_edit(id) {
    vmNet.modal.razorModal.show(modalId, 'Edit Dashboard');
    clearForms();
    show_loading();
    $.ajax({
        dataType: "json",
        url: "/api/dashboard/detail/" + id,
        success: function (res) {
            $("#input_id_edit").val(id);
            $("#input_judul").val(res.JUDUL);
            $("#input_keterangan").val(res.KETERANGAN);
            $("#input_tanggal_pembuatan").val(res.TANGGAL).trigger("keyup");
        },
        error: function (e) {
            show_error_ajax(e.status);
        },
        complete: function () {
            hide_loading();
        }
    });
    console.log(data);
}

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
        url: '/api/dashboard/save',
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



