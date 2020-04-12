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
				for (let i = 0; i < cart.cartDetails.length; i++) {
					const cartDetail = cart.cartDetails[i];
					var product = $(
						'<div class="row mt-4 product"></div>'
					).data('value', cartDetail.productId);

					var card = $(
						'<div class="card col-6 h-100 w-75 border-secondary p-0"></div>'
					);

					var cardHeader = $(
						'<div class="card-header bg-transparent border-secondary h-75"></div>'
					);
					var img = $(
						'<img src="" class="card-img-top h-100"/>'
					).attr('src', cartDetail.product.image);
					cardHeader.append(img);
					card.append(cardHeader);

					var cardBody = $('<div class="card-body h-30"></div>');
					var cardTitle = $('<h5 class="card-title"></h5>').text(
						cartDetail.product.name
					);
					var cardText = $(
						'<p class="card-text mb-0 overflow-hidden w-75 text-left pr-2"></p>'
					).text(cartDetail.product.description);
					cardBody.append(cardTitle);
					cardBody.append(cardText);

					card.append(cardBody);

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
						'<input type="number" min=0 class="form-control border-secondary updateQty" />'
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
		} else {
			$('#cart').append(
				'<h1 class="mt-4">Sorry, nothing is in your cart.</h1>'
			);
		}
	}
});

var updateQty = $('.updateQty').change(function () {
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

var removeCartDetail = $('.removeCartDetail').click(function () {
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
