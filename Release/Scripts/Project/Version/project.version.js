var dateFormat = 'dd/MM/yyyy';
var table = null;

$(document).ready(function () {
    $('.date-deadline').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true
    });
    BindDataVersion();
    $('#modalVersion').on('hidden.bs.modal', function () {
        resetModal();
    });
});

$(document).on('click', '#btnThem', function () {
    resetModal();
    $('#modalVersion').modal('show');
})

$(document).on('click', '#btnLuu', function () {
    var model = getModel();
    var formdata = new FormData();
    formdata.append("model", JSON.stringify(model));
    $.ajax({
        url: '/Project/Version/InsUpdVersion',
        type: 'POST',
        data: formdata,
        contentType: false,
        processData: false,
        success: function (response) {
            if (parseInt(response) > 0) {
                swal("Thông báo", "Tạo phiên bản thành công", "success");
            }
            else {
                swal("Thông báo", "Tạo phiên bản khoản thất bại", "error");
            }
            resetModal();
            BindDataVersion();
            $('#modalVersion').modal('hide');
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
});

$(document).on('click', '#btnThoat', function () {
    resetModal();
})

$(document).on('click', '.edit-version', function () {
    $('#modalVersion').modal('show');
    var id = $(this).data('id');
    var data = table.rows().data();
    var model = getObjectFromArray(data, id);
    setModal(model);
});

var getObjectFromArray = function (array,key) {
    for (var i in array) {
        if (array[i].Id == key) {
            return array[i];
        }
    }
    return {};
}

var BindDataVersion = function () {
    $.ajax({
        url: '/Project/Version/GetAllVersion/' + $('#idProject').val(),
        type: 'GET',
        dataType:'json',
        success: function (response) {
            LoadDataTable(response);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
}

var LoadDataTable = function (data) {
    if (table != null) {
        table.clear();
        table.destroy();
    }
    table = $('#myTable').DataTable({
        "data": data,
        "columns": [
            { "data": "TenPhienBan","width":"15%" },
            { "data": "MoTa", "width": "25%" },
            { "data": "NgayBatDau", "width": "15%" },
            { "data": "NgayKetThuc", "width": "15%" },
            {
                "render": function (data, type, full, row) {
                    return full.TinhTrang == true 
                        ? '<span class="label label-success">Đang hoạt động</span>'
                        : '<span class="label label-danger">Không hoạt động</span>';
                },"width":"20%"
            },
            {
                "render": function (data, type, full, row) {
                    return '<input type="button" class="btn btn-info edit-version" data-id="'+full.Id+'" value="Sửa">';
                },"width":"10%"
            }
        ],
        "searching": true,
        "ordering": false,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
    });
}

var resetModal = function () {
    $('#idVersion').val('');
    $('#tenPhienBan').val('');
    $('#moTa').val('');
    $('#ngayBatDau').datepicker('setDate', new Date());
    $('#ngayKetThuc').datepicker('setDate', new Date());
    $('#trangThai').prop('checked', true);
}

var getModel = function () {
    var obj = {
        Id: $('#idVersion').val() == "" ? null : $('#idVersion').val(),
        TenPhienBan: $('#tenPhienBan').val(),
        MoTa: $('#moTa').val(),
        DuAnID: $('#idProject').val(),
        NgayBatDau: new moment($('#ngayBatDau').val(), dateFormat.toUpperCase()).format("MM/DD/YYYY"),
        NgayKetThuc: new moment($('#ngayKetThuc').val(),dateFormat.toUpperCase()).format("MM/DD/YYYY"),
        TinhTrang: $('#trangThai').is(':checked')
    };
    return obj;
}

var setModal = function (data) {
    $('#idVersion').val(data.Id);
    $('#tenPhienBan').val(data.TenPhienBan);
    $('#moTa').val(data.MoTa);
    $('#ngayBatDau').datepicker('setDate', data.NgayBatDau);
    $('#ngayKetThuc').datepicker('setDate', data.NgayKetThuc);
    $('#trangThai').prop('checked', data.TinhTrang);
}