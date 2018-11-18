$(document).ready(function () {
    $('#changepassform').validate({
        rules: {
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
                    setTimeout(function () {
                        window.location.href = "/";
                    }, 800);
                }
                else {
                    swal("Thông báo", "Đổi mật khẩu thất bại", "error");
                    ResetForm();
                }
            },
        });
    });
});

var GetValueToObject = function () {
    var obj = {};
    obj.MatKhau = $("#MatKhau").val() || "";
    obj.MatKhauMoi = $("#MatKhauMoi").val() || "";
    return obj;
}

var ResetForm = function () {
    $("#MatKhau").val('');
    $('#MatKhauMoi').val('');
    $('#NhapLaiMatKhauMoi').val('');
}
