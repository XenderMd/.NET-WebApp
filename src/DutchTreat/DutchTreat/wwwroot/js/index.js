
let form = document.getElementById('form');
form.hidden = true;


let button = document.getElementById('buyButton');
if (button) {
	button.addEventListener("click", (event) => { console.log(event);});
};

let productInfo = document.getElementsByClassName('product-props');
let listItems = productInfo.item[0].children;
	