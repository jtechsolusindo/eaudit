var tableDOM = $("#grid");

var filters = {}

$(document).ready(function () {
    vmAuditor.grid.init();
});



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
                    extend: 'excelHtml5',
                    title: "Riwayat Verifikasi Auditor",
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
                    "url": "/api/audit/verifikasi/list",
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
                        "data": "ID_VERIFIKASI"
                    },
                    {
                        "width": "30%",
                        "data": "URAIAN_TEMUAN"
                    },
                    {
                        "width": "30%",
                        "data": "NAMA_AUDITOR"
                    },
                    {
                        "width": "30%",
                        "data": "NAMA_UNIT"
                    },
                    {
                        "width": "10%",
                        "data": "KONFIRMASI"
                    },
                    {
                        "width": "30%",
                        "data": "CATATAN"
                    },
                    {
                        "width": "30%",
                        "data": "URAIAN"
                    }, {
                        "searchable": false,
                        "orderable": false,
                        "width": "10%",
                        "data": null,
                        render: function (data, type, row) {
                            let actions = [
                                {
                                    "class": "edit",
                                    "label": "Detail",
                                    "color": "info",
                                    "icon": "pencil",
                                    "url": "verifikasi/detail/"+row.ID_VERIFIKASI,
                                },
                                
                            ];

                            let btnLink = vmNet.grid.generateActionLink(actions, row, BLANK_STRING);
                            return btnLink;
                        }
                    }
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

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmAuditor.grid.redraw();
});


