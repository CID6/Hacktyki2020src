
// Your web app's Firebase configuration
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

const preObject = document.getElementById('object');

//create reference
//ref - root
const dfRefObject = firebase.database().ref().child('Movies/');

//synchronize obj change
//snap - data snapshot, returns keyname and ways to iterate children
dfRefObject.on('value', snap => console.log(snap.val()));
