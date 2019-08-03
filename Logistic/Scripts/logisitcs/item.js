$(document).ready(function () {
    let userId = $('#userid').val();
    let ExclusiveId = $('#exclusiveId').val();
    let itemTable = $('#item-table').DataTable({
        "dom": '<"clear">fltp',
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "ordering": true,
        "responsive": true,
        "ajax": {
            "url": "/Item/List",
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                d.ExclusiveId = ExclusiveId;
            }
        },
        "aoColumns": [
            { "mData": "Name" },
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
                outStr += '<button class="btn btn-info item-edit" ItemId="' + row.ItemId + '" Name="' + row.Name + '">แก้ไขข้อมูล</button>';
                outStr += '<button class="btn btn-danger item-delete" ItemId="' + row.ItemId + '">ลบ</button>';
                return outStr;
            }, 'aTargets': [1]
        }]
    });

    //Edit
    $(document).on('click', '.item-edit', function () {
        let ItemId = $(this).attr('ItemId');
        let Name = $(this).attr('Name');
        $('#item-edit-id').val(ItemId);
        $('#item-edit-name').val(Name);
        $('#item-edit-modal').modal('show');
    });

    $('#item-edit-btn').on('click', function () {
        let ItemId = $('#item-edit-id').val();
        let Name = $('#item-edit-name').val();
        if (Name == "" || Name == null) {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        $('#item-edit-modal').modal('hide');

        let params = new Object();
        params.ItemId = ItemId;
        params.ExclusiveId = ExclusiveId;
        params.Name = Name;

        $.ajax({
            type: "POST",
            url: "/Item/Edit",
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
                itemTable.ajax.reload(null, false);
            }
        });
    });

    // Remove
    $(document).on('click', '.item-delete', function () {
        let ItemId = $(this).attr('ItemId');
        swal({
            title: "ลบข้อมูลสินค้า",
            text: "คุณต้องการลบข้อมูลสินค้าแถวนี้ใช่หรือไม่?",
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
                    param.ItemId = ItemId;
                    param.ExclusiveId = ExclusiveId;

                    $.ajax({
                        type: "POST",
                        url: "/Item/Remove",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(param),
                        async: true,
                        success: function (resp) {

                        },
                        error: function (xhr, status, error) {

                        },
                        complete: function () {
                            itemTable.ajax.reload(null, false);
                        }
                    });

                    swal("สำเร็จ", "ลบข้อมูลสินค้าเรียบร้อยแล้ว", "success");
                } else {
                    swal.close();
                }
            });
    });

    $('#item-show-add-modal').on('click', function () {
        $('#item-add-modal').modal('show');
    });

    $('#item-add-btn').on('click', function () {
        let Name = $('#item-add-name').val();
        if (Name == "" || Name == null) {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        $('#item-add-modal').modal('hide');

        let params = new Object();
        params.ExclusiveId = ExclusiveId;
        params.Name = Name;

        $.ajax({
            type: "POST",
            url: "/Item/Add",
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
                itemTable.ajax.reload(null, false);
            }
        });
    });
});