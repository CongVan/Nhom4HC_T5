$(document).ready(function () {
    $('.datepicker').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    });
});

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

            if (data.ResultID == 1)
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
    if (TenVanDe.length < 0) {
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
