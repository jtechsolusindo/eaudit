$(document).off(EVENT_CLICK, '.modal-footer #btnSave_ModalAuditor');
$(document).on(EVENT_CLICK, '.modal-footer #btnCancel_ModalAuditor', function () {
    $('#kiModalContainer').modal('hide');
});

$(document).off(EVENT_CLICK, '#btnSave_ModalAuditor');
$(document).on(EVENT_CLICK, '#btnSave_ModalAuditor', function (e) {
    let saveUri = '/api/configuration/person/auditor/save';
    var id = $("#txtID").val()!=""?$("#txtID").val():'0';
    let personData = {
        ID:id,
        NPP:$("#txtNPP").val(),
        KODE: $("#txtKode").val(),
        PRODI: $("#txtProgramStudi").val(),
        EMAIL: $("#txtEmail").val(),
    };

    $.when(vmNet.ajax.post(saveUri, personData)).done(function (e) {
        if (e.result === "ok") {
            vmNet.notification.show(e.message, vmNet.message.type.success, RELOAD_NO_REPOST);
        } else {
            vmNet.notification.show(e.message, vmNet.message.type.error, BLANK_STRING);
        }
    });
})

