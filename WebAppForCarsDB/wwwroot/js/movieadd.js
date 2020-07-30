"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/moviehub").build();

document.getElementById("createMovieButton").disabled = true;

connection.start().then(function () {
    document.getElementById("createMovieButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
}
);

function sendData() {
    var message = "something to the database";
    connection.invoke("SendMessage", "Ktoś ", message).catch(function (err) {
        return console.error(err.toString());
    });
    return true;
}



//document.getElementById("createMovieButton").addEventListener("click", function (event) {
//    var message = "something to the database";
//    connection.invoke("SendMessage", "Ktoś ", message).catch(function (err) {
//        return console.error(err.toString());
//    }); 
//    event.preventDefault();
//});