$('#signIn').on('submit', function (e) {
	e.preventDefault();
	var userName = $('input[name=userName').val();
	var password = md5($('input[name=password').val());
	$.ajax({
		type: 'POST',
		dataType: 'text',
		url: '/Auth/Login?' + 'userName=' + userName,
		data: {
			password: password,
		},
		success: function (res) {
			window.location.href = res;
		},
	});
});
