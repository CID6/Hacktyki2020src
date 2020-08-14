
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

//create reference
//ref - root
const dfRefObject = firebase.database().ref().child('movies/');

//synchronize obj change
//snap - data snapshot, returns keyname and ways to iterate children
//dfRefObject.once('value', snap => parseSnap(snap.val()));
dfRefObject.on("child_added", snap => parseNewRow(snap.val(), snap.key));
dfRefObject.on("child_changed", snap => parseUpdatedRow(snap.val(), snap.key));
dfRefObject.on("child_removed", snap => deleteRow(snap.key));



function parseNewRow(snapValue, snapKey) {
    var child = snapValue;
    //console.log(snapValue);
    //console.log(snapKey);
    createRow(child["title"], child["release_date"], child["genre"], child["price"], snapKey);
}

function parseUpdatedRow(snapValue, snapKey) {
    var child = snapValue;
    console.log(snapValue);
    console.log(snapKey);
    updateRow(child["title"], child["release_date"], child["genre"], child["price"], snapKey);
}

function createRow(title, releasedate, genre, price, movieId) {
    var table = document.getElementById("tablebody");
    var row = table.insertRow(0);
    row.id = movieId;
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = title;
    cell2.innerHTML = releasedate;
    cell3.innerHTML = genre;
    cell4.innerHTML = price;
}

function updateRow(title, releasedate, genre, price, movieId) {
    const rowChanged = document.getElementById(movieId);
    rowChanged.deleteCell(3);
    rowChanged.deleteCell(2);
    rowChanged.deleteCell(1);
    rowChanged.deleteCell(0);
    var cell1 = rowChanged.insertCell(0);
    var cell2 = rowChanged.insertCell(1);
    var cell3 = rowChanged.insertCell(2);
    var cell4 = rowChanged.insertCell(3);
    cell1.innerHTML = title;
    cell2.innerHTML = releasedate;
    cell3.innerHTML = genre;
    cell4.innerHTML = price;
}

function deleteRow(movieId) {
    var rowToDelete = document.getElementById(movieId);
    var rowToDeleteIndex = rowToDelete.rowIndex;
    var parentTable = document.getElementById("mainTable");
    parentTable.deleteRow(rowToDeleteIndex);
}