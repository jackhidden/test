$(document).ready(function () {
    $('#datepicker').datepicker({
        autoclose: true
    });

    let userId = $("#userid").val();
    let ExcelId = $('#excel-data-excelfile-id').val();
    if (userId == "1") {
        let excelDataTableAdmin = $('#excel-data-table-admin').DataTable({
            "dom": '<"clear">Bfltp',
            "autoWidth": false,
            "processing": true,
            "serverSide": true,
            "ordering": true,
            "responsive": true,
            lengthMenu: [[25, 100, -1], [25, 100, "All"]],
            pageLength: 25,
            buttons: [
                'excel'
            ],
            "ajax": {
                "url": "/ExcelDownload/DownloadList",
                "type": "GET",
                "datatype": "json",
                "data": function (d) {
                    d.ExcelId = ExcelId;
                }
            },
            "aaSorting": [[1, 'asc'], [9, 'asc'], [10, 'asc'], [11, 'asc']],
            "aoColumns": [
                { "mData": "No" },
                { "mData": "Crossdock" },
                { "mData": "ProductName" },
                { "mData": "Po_No" },
                { "mData": "InvoiceNo" },
                { "mData": "APC_Order" },
                { "mData": "DeliverId" },
                { "mData": "StoreId" },
                { "mData": "DateTime" },
                { "mData": "Province" },
                { "mData": "District" },
                { "mData": "StoreName" },
                { "mData": "ProductId" },
                { "mData": "ProductDetail" },
                { "mData": "Box" },
                { "mData": "Piece" },
                { "mData": "CBM" },
                { "mData": "Weight" },
                { "mData": "Category" },
                { "mData": "Note" },
                { "mData": "Week" },
                { "mData": "Amount" }
            ],
            "aoColumnDefs": [{
                "bSortable": false,
                "aTargets": ["no-sort"]
            },
            {
                'orderable': false,
                'aTargets': [2]
            },
            {
                'orderable': false,
                'aTargets': [3]
            },
            {
                'orderable': false,
                'aTargets': [4]
            },
            {
                'orderable': false,
                'aTargets': [5]
            },
            {
                'orderable': false,
                'aTargets': [6]
            },
            {
                'orderable': false,
                'aTargets': [7]
            },
            {
                'orderable': false,
                'aTargets': [8]
            },
            //{
            //    'orderable': false,
            //    'aTargets': [9]
            //},
            //{
            //    'orderable': false,
            //    'aTargets': [10]
            //},
            {
                'orderable': false,
                'aTargets': [11]
            },
            {
                'orderable': false,
                'aTargets': [12]
            },
            {
                'orderable': false,
                'aTargets': [13]
            }
                ,
            {
                'orderable': false,
                'aTargets': [14]
            }
                ,
            {
                'orderable': false,
                'aTargets': [15]
            },
            {
                'orderable': false,
                'aTargets': [16]
            },
            {
                'orderable': false,
                'aTargets': [17]
            },
            {
                'orderable': false,
                'aTargets': [18]
            },
            {
                'orderable': false,
                'aTargets': [19]
            },
            {
                'orderable': false,
                'aTargets': [20]
            },
            {
                'orderable': false,
                'aTargets': [21]
            }
            ]
        });
    } else {
        let excelDataTable = $('#excel-data-table').DataTable({
            "dom": '<"clear">Bfltp',
            "autoWidth": false,
            "processing": true,
            "serverSide": true,
            "ordering": true,
            "responsive": true,
            lengthMenu: [[25, 100, -1], [25, 100, "All"]],
            pageLength: 25,
            buttons: [
                'excel'
            ],
            "ajax": {
                "url": "/ExcelDownload/DownloadList",
                "type": "GET",
                "datatype": "json",
                "data": function (d) {
                    d.ExcelId = ExcelId;
                }
            },
            "aaSorting": [[1, 'asc'], [9, 'asc'], [10, 'asc'], [11, 'asc']],
            "aoColumns": [
                { "mData": "No" },
                { "mData": "Crossdock" },
                { "mData": "ProductName" },
                { "mData": "Po_No" },
                { "mData": "InvoiceNo" },
                { "mData": "APC_Order" },
                { "mData": "DeliverId" },
                { "mData": "StoreId" },
                { "mData": "DateTime" },
                { "mData": "Province" },
                { "mData": "District" },
                { "mData": "StoreName" },
                { "mData": "ProductId" },
                { "mData": "ProductDetail" },
                { "mData": "Box" },
                { "mData": "Piece" },
                { "mData": "CBM" },
                { "mData": "Weight" },
                { "mData": "Category" },
                { "mData": "Note" },
                { "mData": "Week" }
            ],
            "aoColumnDefs": [{
                "bSortable": false,
                "aTargets": ["no-sort"]
            },
            {
                'orderable': false,
                'aTargets': [2]
            },
            {
                'orderable': false,
                'aTargets': [3]
            },
            {
                'orderable': false,
                'aTargets': [4]
            },
            {
                'orderable': false,
                'aTargets': [5]
            },
            {
                'orderable': false,
                'aTargets': [6]
            },
            {
                'orderable': false,
                'aTargets': [7]
            },
            {
                'orderable': false,
                'aTargets': [8]
            },
            //{
            //    //'orderable': false,
            //    'aTargets': [9]
            //},
            //{
            //    //'orderable': false,
            //    'aTargets': [10]
            //},
            {
                'orderable': false,
                'aTargets': [11]
            },
            {
                'orderable': false,
                'aTargets': [12]
            },
            {
                'orderable': false,
                'aTargets': [13]
            }
                ,
            {
                'orderable': false,
                'aTargets': [14]
            }
                ,
            {
                'orderable': false,
                'aTargets': [15]
            },
            {
                'orderable': false,
                'aTargets': [16]
            },
            {
                'orderable': false,
                'aTargets': [17]
            },
            {
                'orderable': false,
                'aTargets': [18]
            },
            {
                'orderable': false,
                'aTargets': [19]
            },
            {
                'orderable': false,
                'aTargets': [20]
            }
            ]
        });
    }
});