$(document).ready(function () {

    $('#changepassform').validate({
        rules: {
            TenDangNhap: {
                required: true,
                minlength: 5,
            },
            MatKhau: {
                required: true,
                minlength: 6,
            },
            MatKhauMoi: {
                required: true,
                minlength: 6,
            },
            NhapLaiMatKhau: {
                equalTo: "#MatKhauMoi"
            }
        },
        messages: {
            TenDangNhap: {
                required: "Điền thông tin vào trường này",
                minlength: "Tài khoản ít nhật 5 ký tự",
            },
            MatKhau: {
                required: "Điền thông tin vào trường này",
                minlength: "Mật khẩu ít nhất 6 ký tự",
            },
            MatKhauMoi: {
                reqired: "Điền thông tin vào trường này",
                minlength: "Mật khẩu mới ít nhất 6 ký tự"
            },
            NhapLaiMatKhau: {
                equalTo: "Mật khẩu nhập lại không khớp"
            }
        }
    });
    $('#btn-changePass').on('click', function () {
        var idForm = '#changepassform';
        var objmodel = {};
        objmodel = GetValueToObject();
        var formdata = new FormData();
        formdata.append("usermodel", JSON.stringify(objmodel));
        $.ajax({
            url: '/User/ChangePass/ChangePassUser',//User/ResetPass/ResetPassUser @@
            type: 'POST',
            data: formdata,
            contentType: false,
            processData: false,
            success: function (response) {
                if (parseInt(response) > 0) {
                    swal("Thông báo", "Đổi mật khẩu thành công", "success");
                    ResetForm();
                }
                else {
                    swal("Thông báo", "Đổi mật khẩu thất bại", "error");
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('JAVASCRIPT ERROR !!!');
            }
        });
    });
});

var GetValueToObject = function () {
    var obj = {};
    obj.TenDangNhap = $('#TenDangNhap').val() || "";
    obj.MatKhau = $("#MatKhau").val() || "";
    obj.MatKhauMoi = $("#MatKhauMoi").val() || "";
    return obj;
}

var ResetForm = function () {
    $("#TenDangNhap").val('');
    $("#MatKhau").val('');
    $('#MatKhauMoi').val('');
    $('#NhapLaiMatKhauMoi').val('');
}


