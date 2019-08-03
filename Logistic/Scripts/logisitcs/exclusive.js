$(document).ready(function () {
    let userId = $('#userid').val();
    let companyTable = $('#company-table').DataTable({
        "dom": '<"clear">fltp',
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "ordering": true,
        "responsive": true,
        "ajax": {
            "url": "/ExclusiveCompany/List",
            "type": "GET",
            "datatype": "json"
        },
        "aoColumns": [
            { "mData": "Company" },
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
                outStr += '<a href="/Item/Index/' + userId + '/' + row.ExclusiveId + '"><button type="button" class="btn btn-primary">รายการสินค้า</button></a>';
                outStr += '<button class="btn btn-info company-edit" ExclusiveId="' + row.ExclusiveId + '" Company="' + row.Company + '">แก้ไขข้อมูล</button>';
                outStr += '<button class="btn btn-danger company-delete" ExclusiveId="' + row.ExclusiveId + '">ลบ</button>';
                return outStr;
            }, 'aTargets': [1]
        }]
    });

    //Edit
    $(document).on('click', '.company-edit', function () {
        let ExclusiveId = $(this).attr('ExclusiveId');
        let Company = $(this).attr('Company');
        $('#company-edit-id').val(ExclusiveId);
        $('#company-edit-name').val(Company);
        $('#company-edit-modal').modal('show');
    });

    $('#company-edit-btn').on('click', function () {
        let ExclusiveId = $('#company-edit-id').val();
        let Company = $('#company-edit-name').val();
        if (Company == "" || Company == null) {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        $('#company-edit-modal').modal('hide');

        let params = new Object();
        params.ExclusiveId = ExclusiveId;
        params.Company = Company;

        $.ajax({
            type: "POST",
            url: "/ExclusiveCompany/Edit",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            async: true,
            success: function (resp) {
                if (resp.status == 'fail') {
                    swal("มีข้อมูลอยู่แล้ว");
                }
            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                companyTable.ajax.reload(null, false);
            }
        });
    });

    // Remove
    $(document).on('click', '.company-delete', function () {
        let ExclusiveId = $(this).attr('ExclusiveId');
        swal({
            title: "ลบข้อมูลบริษัทของสินค้า",
            text: "คุณต้องการลบข้อมูลบริษัทของสินค้าแถวนี้ใช่หรือไม่?",
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
                    param.ExclusiveId = ExclusiveId;

                    $.ajax({
                        type: "POST",
                        url: "/ExclusiveCompany/Remove",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(param),
                        async: true,
                        success: function (resp) {

                        },
                        error: function (xhr, status, error) {

                        },
                        complete: function () {
                            companyTable.ajax.reload(null, false);
                        }
                    });

                    swal("สำเร็จ", "ลบข้อมูลบริษัทเรียบร้อยแล้ว", "success");
                } else {
                    swal.close();
                }
            });
    });

    $('#company-show-add-modal').on('click', function () {
        $('#company-add-modal').modal('show');
    });

    $('#company-add-btn').on('click', function () {
        let Company = $('#company-add-name').val();
        if (Company == "" || Company == null) {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        $('#company-add-modal').modal('hide');

        let params = new Object();
        params.Company = Company;

        $.ajax({
            type: "POST",
            url: "/ExclusiveCompany/Add",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            async: true,
            success: function (resp) {
                if (resp.status == 'fail') {
                    swal("มีข้อมูลอยู่แล้ว");
                }
            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                companyTable.ajax.reload(null, false);
            }
        });
    });
});