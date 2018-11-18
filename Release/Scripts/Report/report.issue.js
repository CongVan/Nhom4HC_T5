var table = null;
var tableMember = null;
$(document).ready(function () {
    getAllProject();
});

$(document).on('change', '#sel-duan', function () {
    var val = $(this).val();
    $.ajax({
        url: '/Report/Issue/ThongKeVanDe',
        type: 'GET',
        data: {duanId : val},
        dataType: 'json',
        success: function (response) {
            LoadDataTable(response);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
});

$(document).on('click', '.sknadExExcel', function () {
    var duanId = $('#sel-duan').val();
    var tenduan = $('#sel-duan option:selected').text();
    $.ajax({
        url: '/Report/Issue/ExcelVanDe',
        type: 'GET',
        data: {duanId : duanId , tenDuAn : tenduan},
        dataType: 'json',
        success: function (data) {
            if (data === "1") {
                swal("Thông báo", "Không có dữ liệu!", "error");
            } else if (data === "2") {
                swal("Thông báo", "Không có biểu mẫu!", "error");
            } else {
                var bort = window.location.port;
                var stringbort = ':' + bort;
                if (bort === "" || bort === null || bort === undefined)
                    stringbort = "";
                var url = window.location.protocol + '//' + document.location.hostname + stringbort + '//' + data;
                setTimeout(function () {
                    window.open(url);
                }, 300);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
});

$(document).on('click', '.sknadExPDF', function () {
    var duanId = $('#sel-duan').val();
    var tenduan = $('#sel-duan option:selected').text();
    $.ajax({
        url: '/Report/Issue/WordPdfVanDe',
        type: 'GET',
        data: {duanId : duanId , tenDuAn : tenduan , format: "pdf"},
        dataType: 'json',
        success: function (data) {
            if (data === "1") {
                swal("Thông báo", "Không có dữ liệu!", "error");
            } else if (data === "2") {
                swal("Thông báo", "Không có biểu mẫu!", "error");
            } else {
                var bort = window.location.port;
                var stringbort = ':' + bort;
                if (bort === "" || bort === null || bort === undefined)
                    stringbort = "";
                var url = window.location.protocol + '//' + document.location.hostname + stringbort + '//' + data;
                setTimeout(function () {
                    window.open(url);
                }, 300);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
});

$(document).on('click', '.sknadExWord', function () {
    var duanId = $('#sel-duan').val();
    var tenduan = $('#sel-duan option:selected').text();
    $.ajax({
        url: '/Report/Issue/WordPdfVanDe',
        type: 'GET',
        data: { duanId: duanId, tenDuAn: tenduan, format: "doc" },
        dataType: 'json',
        success: function (data) {
            if (data === "1") {
                swal("Thông báo", "Không có dữ liệu!", "error");
            } else if (data === "2") {
                swal("Thông báo", "Không có biểu mẫu!", "error");
            } else {
                var bort = window.location.port;
                var stringbort = ':' + bort;
                if (bort === "" || bort === null || bort === undefined)
                    stringbort = "";
                var url = window.location.protocol + '//' + document.location.hostname + stringbort + '//' + data;
                setTimeout(function () {
                    window.open(url);
                }, 300);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
});

var LoadDataTable = function (data) {
    if (table != null) {
        table.clear();
        table.destroy();
    }
    table = $('#myTable').DataTable({
        "data": data,
        "columns": [
            {
                "render": function (data, type, full, row) {
                    return '<a href="#" onclick="GetReportMember('+ full.TaiKhoanID +')">' + full.HoTen + '</a>';
                }, "width": "40%"
            },
            {
                "render": function (data, type, full, row) {
                    return '<span class="label label-primary">' + full.ChuaXacNhan + '</span>';
                },"width":"15%", "className": "text-center"
            },
            {
                "render": function (data, type, full, row) {
                    return '<span class="label label-info">' + full.XacNhan + '</span>';
                }, "width": "15%", "className": "text-center"
            },
            {
                "render": function (data, type, full, row) {
                    return '<span class="label label-warning">' + full.DangXuLy + '</span>';
                }, "width": "15%", "className": "text-center"
            },
            {
                "render": function (data, type, full, row) {
                    return '<span class="label label-success">' + full.DaXuLy + '</span>';
                }, "width": "15%", "className": "text-center"
            }
        ],
        "searching": true,
        "ordering": false,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
    });
}

var LoadDataTableMember = function (data) {
    if (tableMember != null) {
        tableMember.clear();
        tableMember.destroy();
    }
    tableMember = $('#myTableMember').DataTable({
        "data": data,
        "columns": [
            {
                "data": "HoTen",
            },
            {
                "data": "TenVanDe",
            },
            {
                "data": "TenLoaiVanDe",
            },
            {
                "render": function (data, type, full, row) {
                    return getTextByTrangThai(full.TrangThai);
            }
            },
        ],
        "searching": false,
        "ordering": false,
        "bLengthChange": false,
    });
}

var GetReportMember = function (taikhoanId) {
    var duanid = $('#sel-duan').val();
    $.ajax({
        url: '/Report/Issue/VanDeThanhVien',
        type: 'GET',
        data: {duanId : duanid , taikhoanID : taikhoanId},
        dataType: 'json',
        success: function (response) {
            $('#modalMember').modal('show');
            LoadDataTableMember(response);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
}

var getTextByTrangThai = function (trangthai) {
    switch (trangthai)
    {
        case 1:
            return '<span class="label label-primary">Chưa xác nhận</span>';
            break;
        case 2:
            return '<span class="label label-info">xác nhận</span>';
            break;
        case 3:
            return '<span class="label label-warning">Đang xử lý</span>';
            break;
        case 4:
            return '<span class="label label-success">Đã xử lý</span>';
            break;
    }
}

var getAllProject = function () {
    $.ajax({
        url: '/Report/Issue/GetDuAnByUserID',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response != "") {
                BindSelect(response);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
}

var BindSelect = function(arr){
    var $sel = $('#sel-duan');
    if (arr.length > 0) {
        var html = '<option value="">Chọn dự án</option>';
        for (var i in arr) {
            html += '<option value=' + arr[i].ID + '>' + arr[i].TenDuAn + '</option>';
        }
        $sel.empty().append(html);
    }
}