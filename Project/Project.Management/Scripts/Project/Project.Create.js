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
                TenDuAn: {
                    required: true,
                    minlength: 10,
                },
                TruongDuAnID: {
                    required: true,

                }
            },
            messages: {
                TenDuAn: {
                    required: "Vui lòng nhập tên dự án!",
                    minlength: "Tên dự án tối thiểu 10 ký tự!"
                },
                TruongDuAnID: {
                    required: "Vui lòng chọn trưởng dự án!",

                },
            },
            submitHandler: function (form) {
                if ($(form).attr('id') == "frmAddProject") {
                    submitAddProject();
                }
                if ($(form).attr('id') == "frmUpdateProject") {
                    submitUpdateProject();
                }
                return false;
            }
        });
    });

}

var submitAddProject = function () {
    $('#frmAddProject button[type="submit"]').hide();
    const data = $('#frmAddProject').serialize();//serializeJSON serialize
    var url = $('#frmAddProject').attr('action');
    callAjax({ url: url, type: "post", data: data })
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

var submitUpdateProject = function () {
    const data = $('#frmUpdateProject').serialize();//serializeJSON serialize
    var url = $('#frmUpdateProject').attr('action');
    callAjax({ url: url, type: "post", data: data })
		.done(function (data) {
		    //$('#modalAddProject').modal('hide');
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
            btnProjectDetail_Click($(this).data('id'));
        })
    }
}

$(document).ready(function () {
    ruleForm();
    eventElemenet.init();
    getProjectList();

});

var btnAddProject_Click = function () {
    callAjax({ url: "/Project/Management/GetUserList", showLoading: false })
	.then(function (data) {
	    var lstUser = JSON.parse(data.result);
	    var html = "<option value=''>Chọn trưởng dự án</option>";
	    $.each(lstUser, function (i, item) {
	        html += "<option value='" + item.TaiKhoanID + "'>" + item.HoTen + "</option>";
	    });
	    $('#frmAddProject select[name="TruongDuAnID"').html(html);

	}).catch(function (x, t, e) {
	    console.log(e);
	});


    clearForm($('#frmAddProject'));
}
var btnProjectDetail_Click = function (id) {
    
    callAjax({ url: "/Project/Management/GetUserList", showLoading: false })
	.then(function (data) {
	    var lstUser = JSON.parse(data.result);
	    var html = "<option value=''>Chọn trưởng dự án</option>";
	    $.each(lstUser, function (i, item) {
	        html += "<option value='" + item.TaiKhoanID + "'>" + item.HoTen + "</option>";
	    });
	    //clearForm($('#frmUpdateProject'));
	    $('#frmUpdateProject select[name="TruongDuAnID"').html(html);

	})
    .then(function (data) {
        callAjax({
            url: "/Project/Management/GetProjectDetail",
            data: { duAnId: id }
        })
        .then(function (data) {
            var project = JSON.parse(data.result);
            if (project.length == 0) {
                swal(
			    'Không tìm thấy dự án!',
			    '',
			    'error'
		        );
                return;
            }
            $('#frmUpdateProject input[name="TenDuAn"]').val(project[0].TenDuAn);
            $('#frmUpdateProject input[name="DuAnID"]').val(project[0].DuAnID);
            var date = new Date(project[0].NgayTao);
            var strDate = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
            $('#frmUpdateProject input[name="NgayTao"]').val(strDate);
            $('#frmUpdateProject textarea[name="MoTa"]').val(project[0].MoTa);
            $('#frmUpdateProject select[name="TruongDuAnID"]').val(project[0].TruongDuAnID);
            $('.nav-tabs a[href="#projectDetail"]').tab('show');
        }).catch(function (x, t, e) {
            console.log(e);
        });

    })
    .catch(function (x, t, e) {
	    console.log(e);
	});

    




    
}
var clearForm = function (frm) {
    frm.find('input[type=text], textarea, select').removeClass('text-danger').val('');
    $('span.text-danger').remove();
}
var getProjectList = function () {
    $('#tableProject').DataTable({
        //data: lst,
        ajax: {
            url: "/Project/Management/GetProjectList",
            "dataSrc": function (data) {
                // manipulate your data (json)
                //console.log(JSON.parse(data.result));

                // return the data that DataTables is to use to draw the table
                return JSON.parse(data.result);
            }
        },
        columns: [
			{ "data": "DuAnID" },
			{ "data": "TenDuAn" },
			{
			    "data": "NgayTao",
			    render: function (data, type, row) {
			        var date = new Date(data);
			        return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
			    }
			},
			{ "data": "HoTen" },
			{
			    "data": "DuAnID",
			    render: function (data, type, row) {
			        return '<button class="btn btn-outline-info waves-effect waves-light btn-sm btnProjectDetail" type="button" data-id=' + data + '><i class="fa fa-pencil "></i> Chi tiết</button>';
			    }
			}
        ]
    });

}