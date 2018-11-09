var dateFormat = 'dd/mm/yyyy';
var existsUser = false;
var existsEmail = false;
$(document).ready(function () {
    $('.dateOfbirth').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    });
    $('#loginform').validate({
        rules: {
            HoTen: {
                required: true,
            },
            NgaySinh: {
                required: true,
            },
            TenDangNhap:{
                required: true,
                minlength: 5,
            },
            MatKhau: {
                required: true,
                minlength:6,
            },
            NhapLaiMatKhau:{
                equalTo: "#matKhau"
            },
            Email: {
                required: true,
                email:true,
            }
        },
        messages: {
            HoTen: {
                required: "Điền thông tin vào trường này",
            },
            NgaySinh: {
                required: "Chọn ngày sinh",
            },
            TenDangNhap: {
                required: "Điền thông tin vào trường này",
                minlength: "Tên đăng nhập ít nhất 5 ký tự",
            },
            MatKhau: {
                required: "Điền thông tin vào trường này",
                minlength: "Mật khẩu ít nhất 6 ký tự",
            },
            NhapLaiMatKhau: {
                equalTo: "Mật khẩu nhập lại không khớp"
            },
            Email: {
                required: "Điền thông tin vào trường này",
                email: "Định dạng email không đúng",
            }
        }
    });
});

$('#btn-register').on('click', function () {
    var idForm = '#loginform';
    var valid = true;
    if(!$(idForm).valid()){
        valid = false;
    }
    if(CheckExistsUserName()){
        valid = false
    }
    if(CheckExistsEmail()){
        valid = false;
    }
    if (valid) {
        var objmodel = {};
        objmodel = GetValueToObject();
        var formdata = new FormData();
        formdata.append("usermodel", JSON.stringify(objmodel));
        $.ajax({
            url: '/User/Register/RegisterUser',//User/Register/RegisterUser @@
            type: 'POST',
            data: formdata,
            contentType: false,
            processData: false,
            success: function (response) {
                if (parseInt(response) > 0) {
                    swal("Thông báo", "Tạo tài khoản thành công", "success");
                    ResetForm();
                    setTimeout(function () {
                        window.location.href = window.location.origin + "/User/Login";
                    },800);
                }
                else {
                    swal("Thông báo", "Tạo tài khoản thất bại", "error");
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('JAVASCRIPT ERROR !!!');
            }
        });
    }
});

$('#btn-callback').on('click', function () {
    window.location.href = window.location.origin;
});

$('#tenDangNhap').focusout(function () {
    CheckExistsUserName();
});

var CheckExistsUserName = function () {
    var $element = $('#tenDangNhap');
    var username = $element.val();
    if (username.length > 4) {
        var $parent = $element.parent();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: '/User/Register/CheckUserName',
            type: 'POST',
            data: JSON.stringify({ username: username }),
            dataType: 'json',
            success: function (response) {
                if ($('#tenDangNhap-exists')) {
                    $('#tenDangNhap-exists').remove();
                }
                if (response == 1) {
                    var htmlError = '<label id="tenDangNhap-exists" class="error" for="tenDangNhap">Tên đăng nhập tồn tại</label>';
                    $element.addClass('error');
                    $parent.append(htmlError);
                    existsUser = true;
                }
                else {
                    $element.removeClass('error');
                    existsUser = false;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if ($('#tenDangNhap-exists')) {
                    $('#tenDangNhap-exists').remove();
                }
                console.log('JAVASCRIPT ERROR !!!');
            }
        });
    }
    return existsUser;
}

var CheckExistsEmail = function () {
    var $element = $('#email');
    var email = $element.val();
    var $parent = $element.parent();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/User/Register/CheckEmail',
        type: 'POST',
        data: JSON.stringify({ email: email }),
        dataType: 'json',
        success: function (response) {
            if ($('#email-exists')) {
                $('#email-exists').remove();
            }
            if (response == 1) {
                var htmlError = '<label id="email-exists" class="error" for="email">email đã tồn tại</label>';
                $element.addClass('error');
                $parent.append(htmlError);
                existsEmail = true;
            }
            else {
                $element.removeClass('error');
                existsEmail = false;
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if ($('#email-exists')) {
                $('#email-exists').remove();
            }
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
    return existsEmail;
}

var GetValueToObject = function () {
    var obj = {};
    obj.HoTen = $('#hoTen').val() || "";
    obj.NgaySinh = new moment($('#ngaySinh').val(),dateFormat.toLocaleUpperCase()).format("MM/DD/YYYY");
    obj.TenDangNhap = $("#tenDangNhap").val() || "";
    obj.MatKhau = $("#matKhau").val() || "";
    obj.GioiTinh = $('input[name=GioiTinh]:checked').val();
    obj.Email = $('#email').val() || "";
   return obj;
}

var ResetForm = function () {
    $('#hoTen').val('');
    $("#tenDangNhap").val('');
    $("#matKhau").val('');
    $("#nhapLaiMatKhau").val('');
    $('#ngaySinh').datepicker('setDate', new Date());
    $('#email').val('');
    $('#CaptchaText').val('');
}

var CheckCaptchaValidate = function () {
    var $element = $('#CaptchaText');
    var captchaCode = $element.val();
    var value = $element.val();
    var check = false;
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: '/User/Register/CheckCaptchaValidate',
        type: 'POST',
        data: JSON.stringify({ captchaCode: captchaCode }),
        dataType: 'json',
        async:false,
        success: function (response) {
            if ($('#captcha-error')) {
                $('#captcha-error').remove();
            }
            if (response == true) {
                check = true;
                //return check;
            }
            else {
                check = false;
                var htmlError = '<label id="captcha-error" class="error" for="CaptchaText">Sai mã captcha</label>';
                var $parent = $element.parent();
                $parent.append(htmlError);
                //return check;
            }
        },
        error: function (xhr, textStatus, errorThrown) {
                
            console.log('JAVASCRIPT ERROR !!!');
        }
    });
    return check;
}

$('#email').focusout(function () {
    CheckExistsEmail();
});
