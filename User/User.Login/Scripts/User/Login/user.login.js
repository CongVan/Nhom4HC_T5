var dateFormat = 'dd/mm/yyyy';
var existsUser = false;
$(document).ready(function () {
    $('#loginform').validate({
        rules: {
            TenDangNhap:{
                required: true,
                minlength: 5,
            },
            MatKhau: {
                required: true,
                minlength:6,
            }
        },
        messages: {
            TenDangNhap: {
                required: "Vui lòng nhập tài khoản đăng nhập",
                minlength: "Tên đăng nhập ít nhất 5 ký tự",
            },
            MatKhau: {
                required: "Vui lòng nhập password",
                minlength: "Mật khẩu ít nhất 6 ký tự",
            }
        }
    });
});
$('#btn-login').on('click', function () {
    var idForm = '#loginform';
    if ($(idForm).valid()) {
        var objmodel = {};
        objmodel = GetValueToObject();
        var formdata = new FormData();
        formdata.append("usermodel", JSON.stringify(objmodel));
        $.ajax({
            url: '/User/Login/LoginUser',//User/Login/LoginUser @@
            type: 'POST',
            data: formdata,
            contentType: false,
            processData: false,
            success: function (response) {
                if (parseInt(response) > 0) {
                    swal("Thông báo", "Đăng nhập thành công bấm \n Sẽ chuyển qua trang chủ trong 3s", "success");
                    function Redirect() {
                        window.location.href = "/";
                    }
                    setTimeout(5000);
                    Redirect();
                }
                else {
                    swal("Thông báo", "Đăng nhập thất bại", "error");
                    ResetForm();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('JAVASCRIPT ERROR !!!');
            }
        });
    }
});

var GetValueToObject = function () {
    var obj = {};
    obj.TenDangNhap = $("#tenDangNhap").val() || "";
    obj.MatKhau = $("#matKhau").val() || "";
    obj.NhoTaiKhoan = document.getElementById("nhoMatKhau").checked;
   return obj;
}

var ResetForm = function () {
    $("#matKhau").val('');
}
