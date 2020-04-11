$('.addToCart').click(function (e) {
	var productId = $(this).attr('value');
	var price = $(this).text().substring(1, $(this).text().indexOf(' '));
	e.preventDefault();
	$.ajax({
		type: 'POST',
		contentType: 'application/json',
		url: '/Cart/AddToCart?' + 'ProductId=' + productId + '&Price=' + price,
		success: function (res) {
			$('#totalQty')
				.empty()
				.append('<i class="fas fa-cart-plus"></i>', ' ' + res);
		},
	});
});
