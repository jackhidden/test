$(document).ready(function () {
    $('#employee-add-modal-show').on('click', function () {
        $('#employee-username').val("");
        $('#employee-password').val("");
        $('#employee-name').val("");
        $('#employee-surname').val("");

    });

    $('#employee-add-btn').on('click', function () {
        let username = $('#employee-username').val();
        let password = $('#employee-password').val();
        let name = $('#employee-name').val();
        let surname = $('#employee-surname').val();

        if (username == "" || username == null || password == "" || password == null || name =="" || name == null) {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        let param = new Object();
        param.Username = username;
        param.Password = password;
        param.Name = name;
        param.Surname = surname;

        $('#employee-add-modal').modal('hide');

        $.ajax({
            type: "POST",
            url:"/Employee/Register",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            async: true,
            success: function (resp) {
                if (resp.status == 'fail') {
                    swal("มีข้อมูลอยู่แล้ว");
                }
            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                employeeTable.ajax.reload(null, false);
            }
        });
    });

    let employeeTable = $('#employee-table').DataTable({
        "dom": '<"clear">ftp',
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "ordering": true,
        "responsive": true,
        "ajax": {
            "url": "/Employee/List",
            "type": "GET",
            "datatype": "json"
        },
        "aoColumns": [
            { "mData": "Username" },
            { "mData": "Password" },
            { "mData": "Name" },
            { "mData": "Surname" },
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
                if (row.UserId != 1) {
                    outStr += '<button class="btn btn-info employee-edit" userId="' + row.UserId + '" username="' + row.Username + '" password="' + row.Password + '" name="' + row.Name + '" surname="' + row.Surname + '">แก้ไขข้อมูล</button>';
                    outStr += '<button class="btn btn-danger employee-dlt" userId="' + row.UserId + '">ลบ</button>';
                } else {
                    outStr += '<button class="btn btn-info employee-edit" userId="' + row.UserId + '" username="' + row.Username + '" password="' + row.Password + '" name="' + row.Name + '" surname="' + row.Surname + '">แก้ไขข้อมูล</button>';
                }

                return outStr;
            }, 'aTargets': [4]
        }]
    });

    $(document).on('click', '.employee-edit', function () {
        $('#employee-edit-user-id').val($(this).attr('userId'));
        $('#employee-edit-username').val($(this).attr('username'));
        $('#employee-edit-password').val($(this).attr('password'));
        $('#employee-edit-name').val($(this).attr('name'));
        $('#employee-edit-surname').val($(this).attr('surname'));

        $('#employee-edit-modal').modal('show');
    });

    $('#employee-edit-btn').on('click', function () {
        let userId = $('#employee-edit-user-id').val();
        let username = $('#employee-edit-username').val();
        let password = $('#employee-edit-password').val();
        let name = $('#employee-edit-name').val();
        let surname = $('#employee-edit-surname').val();

        if (username == "" || username == null || password == "" || password == null) {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        let param = new Object();
        param.UserId = userId;
        param.Username = username;
        param.Password = password;
        param.Name = name;
        param.Surname = surname;

        $('#employee-edit-modal').modal('hide');

        $.ajax({
            type: "POST",
            url: "/Employee/Edit",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            async: true,
            success: function (resp) {
                if (resp.status == 'fail') {
                    swal("มีข้อมูลอยู่แล้ว");
                }
            },
            error: function (xhr, status, error) {

            },
            complete: function () {
                employeeTable.ajax.reload(null, false);
            }
        });
    });

    $(document).on('click', '.employee-dlt', function () {
        let userId = $(this).attr('userId');
        swal({
            title: "ลบรหัสพนักงาน",
            text: "คุณต้องการลบรหัสพนักงานใช่หรือไม่?",
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
                    param.UserId = userId;

                    $.ajax({
                        type: "POST",
                        url: "/Employee/Remove",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(param),
                        async: true,
                        success: function (resp) {

                        },
                        error: function (xhr, status, error) {

                        },
                        complete: function () {
                            employeeTable.ajax.reload(null, false);
                        }
                    });
                
                    swal("สำเร็จ", "ลบพนักงานเรียบร้อยแล้ว", "success");
                } else {
                    swal.close();
                }
            });
    });
});