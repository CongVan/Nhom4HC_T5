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

                }
            },
            messages: {
                ThanhVienID: {
                    required: "Vui lòng chọn trưởng dự án!",

                },
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
    ruleForm();
    eventElemenet.init();
    var url = new URL(window.location.href);
    id = url.searchParams.get("id");
    getMemberOfProject(id);
    var getData;
    $.ajax({
        type: 'GET',
        url: "/Project/Member/GetNameOfProject/" + id,
        dataType: 'json',
        success: function (response) {
            
            getData = response.result
            for (var i = 13; i < getData.length-3; i++) {
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
	        html += "<option value='" + item.TaiKhoanID + "'>" + item.HoTen + "</option>";
	    });
	    $('#frmAddProject select[name="ThanhVienID"').html(html);

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
                // manipulate your data (json)
                //console.log(JSON.parse(data.result));

                // return the data that DataTables is to use to draw the table
                return JSON.parse(data.result);
            }
        },
        columns: [
			{ "data": "TenThanhVien" },
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
    
