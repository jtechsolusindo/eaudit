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
                    title: "Log History",
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
                    "url": "/api/log/list",
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
                        "data": "ID_LOG"
                    },
                    {
                        "width": "30%",
                        "data": "TANGGAL_WAKTU"
                    },
                    {
                        "width": "30%",
                        "data": "KETERANGAN"
                    },
                    {
                        "width": "30%",
                        "data": "NPP"
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

$(document).off(EVENT_CLICK, '#btnFilterClear');
$(document).on(EVENT_CLICK, '#btnFilterClear', function (e) {
    _setVal('inputSearch', null);
    vmAuditor.grid.redraw();
});


