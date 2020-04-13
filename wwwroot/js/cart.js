$(document).ready(function () {
	var user = $.cookie('User');
	if (user == null) {
		if (localStorage.getItem('cart') !== null) {
			var cart = JSON.parse(localStorage.getItem('cart'));
			if (cart.cartDetails.length === 0)
				$('#cart').append(
					'<h1 class="mt-4">Sorry, nothing is in your cart.</h1>'
				);
			else {
				$('#cart').before(
					'<div class="row my-4 justify-content-between mx-0"><button id="clearAll" class="btn btn-outline-danger my-2 my-sm-0 col-12 px-0" type="button">Clear All</button></div>'
				);
				$('#clearAll').click(function () {
					bootbox.confirm('Are you sure clear cart?', function (res) {
						/* your callback code */
						if (res) {
							var user = $.cookie('User');
							if (user == null) {
								localStorage.removeItem('cart');
								location.reload();
							} else {
								$.ajax({
									type: 'POST',
									contentType: 'application/json',
									url: '/Cart/ClearAll',
									success: function () {
										location.reload();
									},
								});
							}
						}
					});
				});
				for (let i = 0; i < cart.cartDetails.length; i++) {
					const cartDetail = cart.cartDetails[i];
					var product = $(
						'<div class="row mt-4 product border border-secondary rounded bg-light pl-4"></div>'
					).data('value', cartDetail.productId);

					var card = $('<div class="col-5 h-100 my-auto"></div>');

					var row = $('<div class="row"></div>');

					var cardHeader = $(
						'<div class="bg-transparent h-100 col-4 py-3"></div>'
					);
					var img = $(
						'<img src="" class="card-img-top h-100"/>'
					).attr('src', cartDetail.product.image);
					cardHeader.append(img);
					row.append(cardHeader);

					var cardBody = $(
						'<div class="card-body h-30 col-8"></div>'
					);
					var cardTitle = $('<h5 class="card-title"></h5>').text(
						cartDetail.product.name
					);
					var cardText = $(
						'<p class="card-text mb-0 overflow-hidden text-left pr-2"></p>'
					).text(cartDetail.product.description);
					cardBody.append(cardTitle);
					cardBody.append(cardText);
					row.append(cardBody);
					card.append(row);

					var details = $('<div class="col-6"></div>');
					var container = $(
						'<div class="container h-100 pl-5"></div>'
					);

					var priceRow = $(
						'<div class="row h-50 align-content-end"></div>'
					);
					var priceTag = $(
						'<div class="col-4"><h5 class="card-title">Price: </h5></div>'
					);
					var priceCol = $('<div class="col-8"></div>');
					var price = $('<h5 class="card-title price"></h5>').text(
						'$' + cartDetail.product.price * cartDetail.qty
					);
					priceCol.append(price);
					priceRow.append(priceTag);
					priceRow.append(priceCol);

					var quantityRow = $(
						'<div class="row h-50 align-content-start"></div>'
					);
					var quantityTag = $(
						'<div class="col-4 my-auto"><h5 class="card-title my-0">Quantity: </h5></div>'
					);
					var quantityCol = $('<div class="col-8"></div>');
					var quantityInputGroup = $(
						'<div class="input-group"></div>'
					);
					var qtyControl = $(
						'<input type="number" min=1 class="form-control border-secondary updateQty" onClick="this.select();" />'
					).val(cartDetail.qty);
					var removeBtn = $(
						'<div class="input-group-append removeCartDetail"><button class="btn btn-outline-danger" type="button">Remove</button></div>'
					);
					quantityInputGroup.append(qtyControl);
					quantityInputGroup.append(removeBtn);
					quantityCol.append(quantityInputGroup);
					quantityRow.append(quantityTag);
					quantityRow.append(quantityCol);

					container.append(priceRow);
					container.append(quantityRow);
					details.append(container);
					// qtyControl.html(quantity);
					// price.after(quantity);
					// price.html(details);
					product.append(card);
					product.append(details);
					$('#cart').append(product);
					let total = cart.cartDetails.reduce(function (prev, cur) {
						return prev + cur.qty * cur.product.price;
					}, 0);
					$('#totalPrice').text('Total: $' + total);
				}
			}
			// TODO: changeto cannot <= 0
			$('.updateQty').change(function () {
				var productId = $(this).closest('.product').data('value');
				var changeTo = $(this).val();
				// if (changeTo <= 0) {
				// 	$(this).val();
				// }
				var user = $.cookie('User');
				if (user == null) {
					let cart = JSON.parse(localStorage.getItem('cart'));
					for (let i = 0; i < cart.cartDetails.length; i++) {
						const cartDetail = cart.cartDetails[i];
						if (cartDetail.productId === productId) {
							cartDetail.qty = changeTo;
							break;
						}
					}
					localStorage.setItem('cart', JSON.stringify(cart));
					location.reload();
				} else {
					$.ajax({
						type: 'POST',
						contentType: 'application/json',
						url:
							'/Cart/UpdateQty?' +
							'productId=' +
							productId +
							'&changeTo=' +
							changeTo,
						success: function () {
							location.reload();
						},
					});
				}
			});
			$('.removeCartDetail').click(function () {
				var productId = $(this).closest('.product').data('value');
				var changeTo = 0;
				var user = $.cookie('User');
				if (user == null) {
					let cart = JSON.parse(localStorage.getItem('cart'));
					for (let i = 0; i < cart.cartDetails.length; i++) {
						const cartDetail = cart.cartDetails[i];
						if (cartDetail.productId === productId) {
							cart.cartDetails.splice(i, 1);
							break;
						}
					}
					localStorage.setItem('cart', JSON.stringify(cart));
					location.reload();
				} else {
					$.ajax({
						type: 'POST',
						contentType: 'application/json',
						url:
							'/Cart/UpdateQty?' +
							'productId=' +
							productId +
							'&changeTo=' +
							changeTo,
						success: function () {
							location.reload();
						},
					});
				}
			});
		} else {
			$('#cart').append(
				'<h1 class="mt-4">Sorry, nothing is in your cart.</h1>'
			);
		}
	}
});

$('.updateQty').change(function () {
	var productId = $(this).closest('.product').data('value');
	var changeTo = $(this).val();
	var user = $.cookie('User');
	if (user == null) {
		let cart = JSON.parse(localStorage.getItem('cart'));
		for (let i = 0; i < cart.cartDetails.length; i++) {
			const cartDetail = cart.cartDetails[i];
			if (cartDetail.productId === productId) {
				cartDetail.qty = changeTo;
				break;
			}
		}
		localStorage.setItem('cart', JSON.stringify(cart));
		location.reload();
	} else {
		$.ajax({
			type: 'POST',
			contentType: 'application/json',
			url:
				'/Cart/UpdateQty?' +
				'productId=' +
				productId +
				'&changeTo=' +
				changeTo,
			success: function () {
				location.reload();
			},
		});
	}
});

$('.removeCartDetail').click(function () {
	var productId = $(this).closest('.product').data('value');
	var changeTo = 0;
	var user = $.cookie('User');
	if (user == null) {
		let cart = JSON.parse(localStorage.getItem('cart'));
		for (let i = 0; i < cart.cartDetails.length; i++) {
			const cartDetail = cart.cartDetails[i];
			if (cartDetail.productId === productId) {
				cart.cartDetails.splice(i, 1);
				break;
			}
		}
		localStorage.setItem('cart', JSON.stringify(cart));
		location.reload();
	} else {
		$.ajax({
			type: 'POST',
			contentType: 'application/json',
			url:
				'/Cart/UpdateQty?' +
				'productId=' +
				productId +
				'&changeTo=' +
				changeTo,
			success: function () {
				location.reload();
			},
		});
	}
});

$('#clearAll').click(function () {
	bootbox.confirm('Are you sure clear cart?', function (res) {
		/* your callback code */
		if (res) {
			var user = $.cookie('User');
			if (user == null) {
				localStorage.removeItem('cart');
				location.reload();
			} else {
				$.ajax({
					type: 'POST',
					contentType: 'application/json',
					url: '/Cart/ClearAll',
					success: function () {
						location.reload();
					},
				});
			}
		}
	});
});
