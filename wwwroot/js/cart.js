console.log('cart.js called');

$('.updateQty').change(function () {
	var cartId = $(this).closest('.cart').data('value');
	console.log('cartId', cartId);
	var productId = $(this).closest('.cartDetail').data('value');
	console.log('productId', productId);
	var changeTo = $(this).val();
	console.log('changeTo', changeTo);
	$.ajax({
		type: 'POST',
		contentType: 'application/json',
		url:
			'/Cart/UpdateQty?' +
			'cartId=' +
			cartId +
			'&productId=' +
			productId +
			'&changeTo=' +
			changeTo,
		success: function () {
			location.reload();
		},
	});
});

$('.removeCartDetail').click(function () {
	var cartId = $(this).closest('.cart').data('value');
	console.log('cartId', cartId);
	var productId = $(this).closest('.cartDetail').data('value');
	console.log('productId', productId);
	var changeTo = 0;
	console.log('changeTo', changeTo);
	$.ajax({
		type: 'POST',
		contentType: 'application/json',
		url:
			'/Cart/UpdateQty?' +
			'cartId=' +
			cartId +
			'&productId=' +
			productId +
			'&changeTo=' +
			changeTo,
		success: function () {
			location.reload();
		},
	});
});
