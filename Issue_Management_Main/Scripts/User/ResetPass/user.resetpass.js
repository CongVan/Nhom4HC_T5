

$('#btn-resetPass').on('click', function () {
    var Email = $("#txtEmail").val();
    if (Email != '') {       
        $.ajax({
            url: '/User/ResetPass/LayLaiMatKhau',
            type: 'POST',
            data: { "Email": Email},           
            success: function (response) {   
				var desc;
				var type;
				$.each(response, function (key, value) {					
					if(key == "ResultID")
					{
						if(value == 1)
							type = "success";
						else
							type = "error";
					}
						
					if(key == "ResultDesc")
					{
						desc = value;
					}										
				});
				
				swal("Thông báo", desc, type);	
				
				if (type == "success")
				{
					setTimeout(function () {
						window.location.href = "/User/Login";
					}, 3000);
				}												
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('JAVASCRIPT ERROR !!!');
            }
        });
    }
	else
	{
		swal("Thông báo", "Vui lòng nhập địa chỉ email", "error");	
	}
});

