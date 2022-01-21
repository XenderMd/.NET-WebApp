
let form = $("#form");
form.hide();


let button = $("buyButton");
if (button) {
	button.on("click", (event) => { console.log(event);});
};

let listItems = $(".product-props li");
listItems.on("click", (event) => {
	console.log("You clicked on " + event.target.innerText);
});

	