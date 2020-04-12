$('#signIn').on('submit', function (e) {
	e.preventDefault();
	var userName = $('input[name=userName').val();
	var password = md5($('input[name=password').val());
	if (localStorage.getItem('cart') !== null) {
		var cart = JSON.parse(localStorage.getItem('cart'));
		var cartDetails = cart.cartDetails;
		cartDetails = JSON.stringify(cartDetails);
		console.log(cartDetails);
	}

	$.ajax({
		type: 'POST',
		dataType: 'text',
		contentType: 'application/json; charset=utf-8',
		url: '/Auth/Login?' + 'userName=' + userName + '&password=' + password,
		data: cartDetails,
		success: function (res) {
			localStorage.removeItem('cart');
			window.location.href = res;
		},
	});
});
