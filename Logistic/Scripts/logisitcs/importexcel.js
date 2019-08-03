$(document).ready(function () {
    $('#datepicker').datepicker({
        autoclose: true
    });

    $("#datepicker").change(function () {
        let a = $('#datepicker').val();
        let b = new Date(a);
        let day = b.getDate();
        if (day.toString().length === 1) {
            day = "0" + day;
        }
        let month = b.getMonth() + 1;
        if (month.toString().length === 1) {
            month = "0" + month;
        }
        let year = b.getFullYear();
        console.log(b);
        console.log(a);
        console.log();
        date = year + "/" + month + "/" + day;
    });

    $('#filter').on('click', function () {
        excelFileTable.ajax.reload(null, true);
    });

    $('#clear').on('click', function () {
        date = "";
        UserId = 0;
        $('#datepicker').val("");
        $('#excel-employee-slc2').empty().append('<option value="0">เลือกพนักงาน</option>').val(null).trigger('change');
        excelFileTable.ajax.reload(null, true);
    });

    let date = "";
    let UserId = 0;
    let userId = $('#userid').val();
    let excelFileTable = $('#excel-file-table').DataTable({
        "dom": '<"clear">fltp',
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "ordering": true,
        "responsive": true,
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Excel/ImportList",
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                d.Date = date;
                d.UserId = UserId;
            }
        },
        "aoColumns": [
            { "mData": "ExcelId" },
            { "mData": "NameFile" },
            { "mData": "UploadTime" },
            { "mData": "Employee" },
            null
        ],
        "aoColumnDefs": [{
            "bSortable": false,
            "aTargets": ["no-sort"]
        },
        {
            'orderable': false,
            'mRender': function (data, type, row) {
                let outStr = '';
                outStr += '<a href="/ExcelDownload/Download/' + userId + '/' + row.ExcelId + '"><button type="button" class="btn btn-primary">ดาว์นโหลดข้อมูล</button></a>';
                outStr += '<button class="btn btn-danger excel-file-dlt" ExcelId="' + row.ExcelId + '">ลบ</button>';
                return outStr;
            }, 'aTargets': [4]
        }]
    }); 

    $(document).on('click', '.excel-file-dlt', function () {
        let excelId = $(this).attr('ExcelId');
        swal({
            title: "ลบข้อมูลExcel",
            text: "คุณต้องการลบลบข้อมูลExcelใช่หรือไม่?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "ตกลง",
            cancelButtonText: "ยกเลิก",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    let param = new Object();
                    param.ExcelId = excelId;

                    $.ajax({
                        type: "POST",
                        url: "/Excel/Remove",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(param),
                        async: true,
                        success: function (resp) {

                        },
                        error: function (xhr, status, error) {

                        },
                        complete: function () {
                            excelFileTable.ajax.reload(null, false);
                        }
                    });

                    swal("สำเร็จ", "ลบข้อมูลExcelเรียบร้อยแล้ว", "success");
                } else {
                    swal.close();
                }
            });
    });

    $("#excel-upload-btn").click(function (event) {
        $.blockUI({ message: '<h1>กำลังอัพโหลดไฟล์...</h1>' });
        //stop submit the form, we will post it manually.
        event.preventDefault();

        // Get form
        let form = $('#excel-upload-form')[0];

        // Create an FormData object 
        let data = new FormData(form);

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "/Excel/ImportExcel",
            data: data,
            processData: false,
            contentType: false,
            cache: false,
            timeout: 600000,
            success: function (resp) {
                if (resp.status === "success") {


                    }
            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                $('#excelfile').val('');
                excelFileTable.ajax.reload(null, false);
                $.unblockUI(); 
            }
        });

    });

    let employeeSlc2 = $('#excel-employee-slc2').select2({
        placeholder: "เลือกพนักงาน",
        ajax: {
            url: '/Employee/List',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                params.term = params.term || '';
                params.page = params.page || 1;
                params.length = params.length || 10;
                return {
                    search: { value: params.term },
                    start: (params.page - 1) * params.length,
                    length: params.length
                };
            },
            processResults: function (data, params) {
                let items = [];

                $.each(data.data, function (idx, val) {
                    items.push({ id: val.UserId, text: val.Name +" "+ val.Surname });
                });

                params.page = params.page || 1;
                params.length = params.length || 10;
                return {
                    results: items,
                    pagination: {
                        more: (params.page * params.length) < data.recordsFiltered
                    }
                };
            },
            cache: true
        },
        minimumInputLength: 0
    }).on('select2:select', function (e) {
        UserId = e.params.data.id;
    });
});