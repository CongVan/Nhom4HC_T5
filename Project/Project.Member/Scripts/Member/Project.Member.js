var callAjax = function (params) {
    params.showLoading = params.showLoading == undefined ? true : params.showLoading;
    return $.ajax({
        url: params.url,
        type: params.type === undefined ? "get" : params.type,
        data: params.data,
        beforeSend: params.showLoading == true ? function () {
            $('#loadingMain').show();
        } : false,
        success: params.showLoading == true ? function () {
            $('#loadingMain').hide();
        } : false,
        error: params.showLoading == true ? function () {
            $('#loadingMain').hide();
        } : false,
    });
}

var ruleForm = function () {
    $('form').each(function () {
        $(this).validate({
            errorClass: "text-danger font-weight-bold",
            errorElement: "span",
            rules: {
                ThanhVienID: {
                    required: true,
                },
                RollID: {
                    required: true,
                }
            },
            messages: {
                ThanhVienID: {
                    required: "Vui lòng chọn thành viên!",
                },
                RollID: {
                    required: "Vui lòng chọn chức vụ!",
                }
            },
            submitHandler: function (form) {
                if ($(form).attr('id') == "frmAddProject") {
                    submitAddProject();
                }
                if ($(form).attr('id') == "frmAddProject") {
                    submitDeleteProject();
                }
                return false;
            }
        });
    });

}

var submitAddProject = function () {
    $('#frmAddProject button[type="submit"]').hide();
    const data = $('#frmAddProject').serialize();//serializeJSON serialize
    callAjax({ url: "/Project/Member/AddProject/"+id, type: "post", data: data })
		.done(function (data) {
		    $('#modalAddProject').modal('hide');
		    if (data.result === 1) {
		        swal(
					'Thành công!',
					'',
					'success'
				);
		        //getProjectList();
		        $('#tableProject').DataTable().ajax.reload().draw();
		    } else {
		        swal(
			    'Thất bại!',
			    data.msg,
			    'error'
		        );
		    }
		})
        .then(function () {
            $('#frmAddProject button[type="submit"]').show();
        })
		.fail(function (j, t, e) {
		    swal(
			'Thất bại!',
			e,
			'error'
		);
		});
}
var eventElemenet = {
    init: function () {
        this.btnAddProject();
        this.btnProjectDetail();
    },
    btnAddProject: function () { $('#btnAddProject').on('click', function () { btnAddProject_Click(); }) },
    btnProjectDetail: function () {
        $('#tableProject').on('click', '.btnProjectDetail', function () {
            var idtv=$(this).data('id');
            callAjax({ url: "/Project/Member/DeleteProject/" + idtv, type: "post" })
		.done(function (data) {
		    $('#modalAddProject').modal('hide');
		    if (data.result === 1) {
		        swal(
					'Thành công!',
					'',
					'success'
				);
		        //getProjectList();
		        $('#tableProject').DataTable().ajax.reload().draw();
		    } else {
		        swal(
			    'Thất bại!',
			    data.msg,
			    'error'
		        );
		    }
		})
        .then(function () {
            $('#frmAddProject button[type="submit"]').show();
        })
		.fail(function (j, t, e) {
		    swal(
			'Thất bại!',
			e,
			'error'
		);
		});
        })
    }
}
var name = '';
var id;
$(document).ready(function () {
    var url = new URL(window.location.href);
    id = url.searchParams.get("id");
    callAjax({ url: "/Project/Member/LeaderOfProject/" + id, showLoading: false })
    .then(function (data) {
        var check = JSON.parse(data.result);
        if (check.length == 0)
        {
            document.getElementById('btnAddProject').style.visibility = 'hidden';
        }
    }).catch(function (x, t, e) {
        console.log(e);
    });
    ruleForm();
    eventElemenet.init();
    getMemberOfProject(id);
    var getData;
    $.ajax({
        type: 'GET',
        url: "/Project/Member/GetNameOfProject/" + id,
        dataType: 'json',
        success: function (response) {          
            getData = response.result
              for (var i = 13; i < getData.length - 3; i++) {
                name = name + getData[i];
            }
        }
    });

});

var btnAddProject_Click = function () {
    callAjax({ url: "/Project/Member/GetUserList/"+id, showLoading: false })
	.then(function (data) {
	    var lstUser = JSON.parse(data.result);
	    var html = "<option value=''>Chọn thành viên vào dự án</option>";
	    $.each(lstUser, function (i, item) {
	        html += "<option value='" + item.TaiKhoanID + "'>" + item.TenDangNhap + "</option>";
	    });
	    $('#frmAddProject select[name="ThanhVienID"').html(html);
	}).catch(function (x, t, e) {
	    console.log(e);
	});
    callAjax({ url: "/Project/Member/RollOfProject", showLoading: false })
    .then(function (data) {
        var lstRoll = JSON.parse(data.result);
        var html = "<option value=''>Chọn chọn chức vụ</option>";
        $.each(lstRoll, function (i, item) {
            html += "<option value='" + item.IDRoll + "'>" + item.NameRoll + "</option>";
        });
        $('#frmAddProject select[name="RollID"').html(html);
    }).catch(function (x, t, e) {
        console.log(e);
    });
    clearForm($('#frmAddProject'));
}

var clearForm = function (frm) {
    frm.find('input[type=text], textarea, select').removeClass('text-danger').val('');
    $('span.text-danger').remove();
}
var getMemberOfProject = function () {
    $('#tableProject').DataTable({
        //data: lst,   
        ajax: {
            url: "/Project/Member/GetMemberOfProject/"+id,
            "dataSrc": function (data) {
            return JSON.parse(data.result);
            }
        },
        columns: [
			{ "data": "TenDangNhap" },
            { "data": "TenThanhVien" },
            { "data": "ChucVu" },
            { "data": "NguoiThem" },
			{
			    "data": "NgayTao",
			    render: function (data, type, row) {
			        var date = new Date(data);
			        return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
			    }
			},
            {
                "data": "MaThanhVien",
            	render: function (data, type, row) {
            	    return '<button class="btn btn-outline-info waves-effect waves-light btn-sm btnProjectDetail" type="button"  data-id=' + data + '><i class="fa fa-pencil "></i> Xoá</button>';
                }
            }
        ]
    });

}
    
