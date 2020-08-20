
var firebaseConfig = {
	apiKey: "AIzaSyA1z_4pbREdKX8c3c5sHhYBm0SOUgaT3_E",
	authDomain: "fir-hacktyki.firebaseapp.com",
	databaseURL: "https://fir-hacktyki.firebaseio.com",
	projectId: "fir-hacktyki",
	storageBucket: "fir-hacktyki.appspot.com",
	messagingSenderId: "768832612712",
	appId: "1:768832612712:web:2bc638f0454dc7fbe20edd",
	measurementId: "G-P66RDJKSXD"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);

var database = firebase.database();

function parseForm() {
	var data = $('#createForm').serializeArray().reduce(function (obj, item) {
		obj[item.name] = item.value;
		return obj;
	}, {});

	writeMovie(data["Movie.Title"], data["Movie.Genre"], data["Movie.ReleaseDate"], data["Movie.Price"]);

	return true;
}

function writeMovie(title, genre, releaseDate, price) {
	database.ref("movies/"+genre).push({
		title: title,
		genre: genre,
		release_date: releaseDate,
		price: price
	});
}


