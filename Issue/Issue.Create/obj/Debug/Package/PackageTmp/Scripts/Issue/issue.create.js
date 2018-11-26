$(document).ready(function () {
    $('.datepicker').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    });
    
    //setTimeout(function () {
        LayDanhSachDuAn();
    //    setTimeout(function () {
    //        LayDanhSachLoaiVanDe();
    //        setTimeout(function () {
    //            LayDanhSachTaiKhoan();
    //            setTimeout(function () {
    //                var VanDeID = $('#txtVanDeID').val();
    //                if (VanDeID > 0) {
    //                    LayThongTinVanDe(VanDeID);
    //                }
    //            }, 1000);
    //        }, 1000);
    //    }, 1000);
    //}, 1000); 
});

function LayThongTinVanDe(VanDeID) {
    $.ajax({
        url: "/Issue/Create/LayThongTinVanDe",
        type: "POST",
        data: { "VanDeID": VanDeID }
    }).done(function (data) {
        if (data.DuAnID > 0) {
            $('#DuAnID').val(data.DuAnID);
            $('#txtLoaiVanDe').val(data.LoaiVanDeID);
            $('#txtTenVanDe').val(data.TenVanDe);
            $('#txtMoTa').val(data.MoTa);
            $('#txtTrangThai').val(data.TrangThai);
            $('#txtNguoiThucHien').val(data.NguoiThucHien);
            $('#txtNgayBatDau').val(data.NgayBatDau);
            $('#txtNgayKetThuc').val(data.NgayKetThuc);
            $('#txtSoGioDuKien').val(data.SoGioDuKien);
            $('#txtSoGioThucTe').val(data.SoGioThucTe);
        }       
    }).fail(function (xhr, status, err) {
        console.log(status);
        console.log(err);
    });
}

function LayDanhSachDuAn() {
    $.ajax({
        url: "/Issue/Create/LayDanhSachDuAn",
        type: "POST",
        //data: { "VanDeID": VanDeID }
    }).done(function (data) {
        var source = document.getElementById('DuAnID-template').innerHTML;
        var template = Handlebars.compile(source);
        var html = template(data);        
        $('#lstDuAn').html(html);
        LayDanhSachLoaiVanDe();
    }).fail(function (xhr, status, err) {
        console.log(status);
        console.log(err);
    });
}

function LayDanhSachLoaiVanDe() {
    $.ajax({
        url: "/Issue/Create/LayDanhSachLoaiVanDe",
        type: "POST",
        //data: { "VanDeID": VanDeID }
    }).done(function (data) {

        var source = document.getElementById('LoaiVanDe-template').innerHTML;
        var template = Handlebars.compile(source);
        var html = template(data);
        $('#lstLoaiVanDe').html(html);
        LayDanhSachTaiKhoan();
    }).fail(function (xhr, status, err) {
        console.log(status);
        console.log(err);
    });
}


function LayDanhSachTaiKhoan() {
    $.ajax({
        url: "/Issue/Create/LayDanhSachTaiKhoan",
        type: "POST",
        //data: { "VanDeID": VanDeID }
    }).done(function (data) {

        var source = document.getElementById('NguoiThucHien-template').innerHTML;
        var template = Handlebars.compile(source);
        var html = template(data);
        $('#lstNguoiThucHien').html(html);
        var VanDeID = $('#txtVanDeID').val();
        if (VanDeID > 0) {
            LayThongTinVanDe(VanDeID);
        }
    }).fail(function (xhr, status, err) {
        console.log(status);
        console.log(err);
    });
}

$("#btnCapNhat").on('click', function () {
    var VanDeID = $('#txtVanDeID').val();
    var DuAnID = $('#DuAnID').val();
    var LoaiVanDe = $('#txtLoaiVanDe').val();
    var TenVanDe = $('#txtTenVanDe').val();
    var MoTa = $('#txtMoTa').val();
    var TrangThai = $('#txtTrangThai').val();
    var NguoiThucHien = $('#txtNguoiThucHien').val();
    var NgayBatDau = $('#txtNgayBatDau').val();
    var NgayKetThuc = $('#txtNgayKetThuc').val();
    var SoGioDuKien = $('#txtSoGioDuKien').val();
    var SoGioThucTe = $('#txtSoGioThucTe').val();

    if (VerifyInfo(TenVanDe, NguoiThucHien, NgayBatDau, NgayKetThuc, SoGioDuKien)) {
        var objToPost = {
            "VanDeID": VanDeID,
            "DuAnID": DuAnID,
            "LoaiVanDe": LoaiVanDe,
            "TenVanDe": TenVanDe,
            "MoTa": MoTa,
            "TrangThai": TrangThai,
            "NguoiThucHien": NguoiThucHien,
            "NgayBatDau": parseDateSQL(NgayBatDau),
            "NgayKetThuc": parseDateSQL(NgayKetThuc),
            "SoGioDuKien": SoGioDuKien,
            "SoGioThucTe": SoGioThucTe
        }

        //var jsonToPost = new FormData();
        //jsonToPost.append("IssueModel", JSON.stringify(objToPost));

        $.ajax({
            url: "/Issue/Create/CapNhatVanDe",
            type: "POST",
            data: objToPost
        }).done(function (data) {
            console.log(data);

            if (data.ResultID > 0)
                type = "success";
            else
                type = "error";

            swal("Thông báo", data.ResultDesc, type);

            if (type == "success") {
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }

        }).fail(function (xhr, status, err) {
            console.log(status);
            console.log(err);
        });
    }
});

function VerifyInfo(TenVanDe, NguoiThucHien, NgayBatDau, NgayKetThuc, SoGioDuKien) {
    if (TenVanDe.length <= 0) {
        swal("Thông báo", "Vui lòng nhập Tên vấn đề", "error");
        $('#txtTenVanDe').focus();
        return false;
    }
    if (NguoiThucHien <= 0) {
        swal("Thông báo", "Vui lòng chọn Người thực hiện", "error");
        $('#txtNguoiThucHien').focus();
        return false;
    }
    if (SoGioDuKien <= 0) {
        swal("Thông báo", "Vui lòng nhập Số giờ dự kiến", "error");
        $('#txtSoGioDuKien').focus();
        return false;
    }
    if (NgayBatDau.length <= 0) {
        swal("Thông báo", "Vui lòng nhập Ngày bắt đầu", "error");
        $('#txtNgayBatDau').focus();
        return false;
    }
    if (NgayKetThuc.length <= 0) {
        swal("Thông báo", "Vui lòng nhập Ngày kết thúc", "error");
        $('#txtNgayBatDau').focus();
        return false;
    }

    var startDate = parseDateJAVA(NgayBatDau).getTime();
    var endDate = parseDateJAVA(NgayKetThuc).getTime();

    if (startDate > endDate) {
        swal("Thông báo", "Ngày bắt đầu phải nhỏ hơn Ngày kết thúc", "error");
        $('#txtNgayBatDau').focus();
        return false;
    }
    return true;
}

function parseDateJAVA(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[1], mdy[0]);
}
function parseDateSQL(str) {
    var mdy = str.split('/');
    return mdy[2] + "/" + mdy[1] + "/" + mdy[0];
}
