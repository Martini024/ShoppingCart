$('.addToCart').click(function (e) {
	// get current clicked product
	var productId = $(this).attr('value');
	var price = $(this).text().substring(1, $(this).text().indexOf(' '));
	$.ajax({
		type: 'POST',
		contentType: 'application/json',
		url: '/Cart/AddToCart?' + 'ProductId=' + productId + '&Price=' + price,
		success: function (res) {
			console.log(res);

			$('#totalQty').html(res);
		},
	});
});
