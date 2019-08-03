$(document).ready(function () {
    let Address = {
        ProvinceId: 0,
        Province : "",
        DistrictId: 0,
        District: "",
        ExclusiveId: 0,
        Exclusive: "",
        Edit: {
            ProvinceId: 0,
            Province: "",
            DistrictId: 0,
            District: "",
            ExclusiveId: 0,
            Exclusive: ""
        }
    };
    let ExclusiveId;

    let provinceAddSlc2 = $('#rateprcice-add-province').select2({
        placeholder: "เลือกจังหวัด",
        ajax: {
            url: '/Address/ProvinceList',
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
                    items.push({ id: val.ProvinceId, text: val.Province });
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
        Address.ProvinceId = e.params.data.id;
        Address.Province = e.params.data.text;
        $('#rateprcice-add-district').removeAttr('disabled');
        districtAddSlc2.empty().append('<option value="0">เลือกอำเภอ</option>').val(null).trigger('change');
    });

    let districtAddSlc2 = $('#rateprcice-add-district').select2({
        placeholder: "เลือกอำเภอ",
        ajax: {
            url: '/Address/DistrictList',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                params.term = params.term || '';
                params.page = params.page || 1;
                params.length = params.length || 10;
                return {
                    search: { value: params.term },
                    start: (params.page - 1) * params.length,
                    length: params.length,
                    ProvinceId: Address.ProvinceId
                };
            },
            processResults: function (data, params) {
                let items = [];

                $.each(data.data, function (idx, val) {
                    items.push({ id: val.DistrictId, text: val.District });
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
        Address.DistrictId = e.params.data.id;
        Address.District = e.params.data.text;
    });

    let exclusiveAddSlc2 = $('#rateprice-add-exclusive-slc').select2({
        placeholder: "เลือกบริษัท",
        ajax: {
            url: '/ExclusiveCompany/List',
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
                    items.push({ id: val.ExclusiveId, text: val.Company });
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
        Address.ExclusiveId = e.params.data.id;
        Address.Exclusive = e.params.data.text;
    });

    $('#rateprcice-add-btn').on('click', function () {
        let ExclusiveId = Address.ExclusiveId;
        let Exclusive = Address.Exclusive;
        let DistrictId = Address.DistrictId;
        let District = Address.District;
        let ProvinceId = Address.ProvinceId;
        let Province = Address.Province;
        let RatePrice = $('#rateprcice-add-rate').val();

        if (DistrictId == 0 || ProvinceId == 0 || RatePrice == null || RatePrice == "") {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        let param = new Object();
        param.ExclusiveId = ExclusiveId;
        param.Exclusive = Exclusive;
        param.ProvinceId = ProvinceId;
        param.Province = Province;
        param.DistrictId = DistrictId;
        param.District = District;
        param.RatePrice = RatePrice;

        $('#rateprcice-add-modal').modal('hide');

        $.ajax({
            type: "POST",
            url:"/Settings/Add",
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
                clearAddData();
                ratePriceTable.ajax.reload(null, false);
            }
        });
    });

    $('#rateprice-show-add-modal').on('click', function () {
        $('#rateprcice-add-modal').modal('show');
    });

    let ratePriceTable = $('#rateprcice-table').DataTable({
        "dom": '<"clear">ftp',
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "ordering": true,
        "responsive": true,
        "ajax": {
            "url": "/Settings/List",
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                d.ExclusiveId = ExclusiveId;
            }
        },
        "aoColumns": [
            { "mData": "Province" },
            { "mData": "District" },
            { "mData": "RatePrice" },
            { "mData": "Exclusive" },
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
                outStr += '<button class="btn btn-info rateprcice-edit" RateId="' + row.RateId + '" ExclusiveId="' + row.ExclusiveId + '" ProvinceId="' + row.ProvinceId + '"Province="' + row.Province + '"DistrictId="' + row.DistrictId + '"District="' + row.District + '"RatePrice="' + row.RatePrice + '"ExclusiveId="' + row.ExclusiveId + '"Exclusive="' + row.Exclusive+'">แก้ไขข้อมูล</button>';
                outStr += '<button class="btn btn-danger rateprcice-dlt" RateId="' + row.RateId + '">ลบ</button>';
                return outStr;
            }, 'aTargets': [4]
        }]
    });

    let clearAddData = function () {
        Address.ExclusiveId = 0;
        Address.ProvinceId = 0;
        Address.Province = "";
        Address.DistrictId = 0;
        Address.District = "";
        provinceAddSlc2.empty().append('<option value="0">เลือกจังหวัด</option>').val(null).trigger('change');
        districtAddSlc2.empty().append('<option value="0">เลือกอำเภอ</option>').val(null).trigger('change');
        exclusiveAddSlc2.empty().append('<option value="0">เลือกบริษัท</option>').val(null).trigger('change');
        $('#rateprcice-add-rate').val('');
        
    };

    $(document).on('click', '.rateprcice-edit', function () {
        $('#rateprcice-edit-rate-id').val($(this).attr('RateId'));
        $('#rateprcice-edit-rate').val($(this).attr('RatePrice'));

        $("rateprice-edit-exclusive-slc select").val($(this).attr('ExclusiveId'));

        Address.Edit.ExclusiveId = $(this).attr('ExclusiveId');
        Address.Edit.Exclusive = $(this).attr('Exclusive');
        Address.Edit.ProvinceId = $(this).attr('ProvinceId');
        Address.Edit.Province = $(this).attr('Province');
        Address.Edit.DistrictId = $(this).attr('DistrictId');
        Address.Edit.District = $(this).attr('District');

        $('#rateprcice-edit-province').empty().append('<option value="' + Address.Edit.ProvinceId + '">' + Address.Edit.Province + '</option>').val(Address.Edit.ProvinceId).trigger('change');

        $('#rateprcice-edit-district').empty().append('<option value="' + Address.Edit.DistrictId + '">' + Address.Edit.District + '</option>').val(Address.Edit.DistrictId).trigger('change');
        
        $('#rateprice-edit-exclusive-slc').empty().append('<option value="' + Address.Edit.ExclusiveId + '">' + Address.Edit.Exclusive + '</option>').val(Address.Edit.ExclusiveId).trigger('change');

        $('#rateprcice-edit-modal').modal('show');
    });

    let provinceEditSlc2 = $('#rateprcice-edit-province').select2({
        placeholder: "เลือกจังหวัด",
        ajax: {
            url: '/Address/ProvinceList',
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
                    items.push({ id: val.ProvinceId, text: val.Province });
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
        Address.Edit.ProvinceId = e.params.data.id;
        Address.Edit.Province = e.params.data.text;
    });

    let districtEditSlc2 = $('#rateprcice-edit-district').select2({
        placeholder: "เลือกอำเภอ",
        ajax: {
            url: '/Address/DistrictList',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                params.term = params.term || '';
                params.page = params.page || 1;
                params.length = params.length || 10;
                return {
                    search: { value: params.term },
                    start: (params.page - 1) * params.length,
                    length: params.length,
                    ProvinceId: Address.Edit.ProvinceId
                };
            },
            processResults: function (data, params) {
                let items = [];

                $.each(data.data, function (idx, val) {
                    items.push({ id: val.DistrictId, text: val.District });
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
        Address.Edit.DistrictId = e.params.data.id;
        Address.Edit.District = e.params.data.text;
    });

    let exclusiveEditSlc2 = $('#rateprice-edit-exclusive-slc').select2({
        placeholder: "เลือกบริษัท",
        ajax: {
            url: '/ExclusiveCompany/List',
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
                    items.push({ id: val.ExclusiveId, text: val.Exclusive });
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
        Address.Edit.ExclusiveId = e.params.data.id;
        Address.Edit.Exclusive = e.params.data.text;
    });

    let clearEditData = function () {
        Address.Edit.ExclusiveId = 0;
        Address.Edit.Exclusive = "";
        Address.Edit.ProvinceId = 0;
        Address.Edit.Province = "";
        Address.Edit.DistrictId = 0;
        Address.Edit.District = "";
        provinceEditSlc2.empty().append('<option value="0">เลือกจังหวัด</option>').val(null).trigger('change');
        districtEditSlc2.empty().append('<option value="0">เลือกอำเภอ</option>').val(null).trigger('change');
        exclusiveEditSlc2.empty().append('<option value="0">เลือกบริษัท</option>').val(null).trigger('change');
        $('#rateprcice-edit-rate').val('');

    };

    //Edit RatePrice
    $('#rateprcice-edit-btn').on('click', function () {
        let rateId = $('#rateprcice-edit-rate-id').val();
        let ExclusiveId = Address.Edit.ExclusiveId;
        let Exclusive = Address.Edit.Exclusive;
        let DistrictId = Address.Edit.DistrictId;
        let District = Address.Edit.District;
        let ProvinceId = Address.Edit.ProvinceId;
        let Province = Address.Edit.Province;
        let RatePrice = $('#rateprcice-edit-rate').val();

        if (DistrictId == 0 || ProvinceId == 0 || RatePrice == null || RatePrice == "") {
            swal("กรุณาใส่ข้อมูลให้ครบ");
            return;
        }

        let param = new Object();
        param.RateId = rateId;
        param.ExclusiveId = ExclusiveId;
        param.Exclusive = Exclusive;
        param.ProvinceId = ProvinceId;
        param.Province = Province;
        param.DistrictId = DistrictId;
        param.District = District;
        param.RatePrice = RatePrice;

        $('#rateprcice-edit-modal').modal('hide');

        $.ajax({
            type: "POST",
            url: "/Settings/Edit",
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
                clearEditData();
                ratePriceTable.ajax.reload(null, false);
            }
        });
    });

    // Remove Delete
    $(document).on('click', '.rateprcice-dlt', function () {
        let rateId = $(this).attr('RateId');
        swal({
            title: "ลบข้อมูลเรทราคา",
            text: "คุณต้องการลบข้อมูลเรทราคาแถวนี้ใช่หรือไม่?",
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
                    param.RateId = rateId;

                    $.ajax({
                        type: "POST",
                        url: "/Settings/Remove",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(param),
                        async: true,
                        success: function (resp) {

                        },
                        error: function (xhr, status, error) {

                        },
                        complete: function () {
                            ratePriceTable.ajax.reload(null, false);
                        }
                    });

                    swal("สำเร็จ", "ลบข้อมูลเรทราคาเรียบร้อยแล้ว", "success");
                } else {
                    swal.close();
                }
            });
    });

    let exclusiveSlc2 = $('#rateprice-exclusive-slc').select2({
        placeholder: "เลือกบริษัท",
        ajax: {
            url: '/ExclusiveCompany/List',
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
                    items.push({ id: val.ExclusiveId, text: val.Company });
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
        ExclusiveId = e.params.data.id;
        ratePriceTable.ajax.reload(null, true);
    });
    

    $(document).on('click', '#rateprice-exclusive-clear', function () {
        ExclusiveId = 0;
        exclusiveSlc2.empty().append('<option value="0">เลือกบริษัท</option>').val(null).trigger('change');
        ratePriceTable.ajax.reload(null, true);
    });
});