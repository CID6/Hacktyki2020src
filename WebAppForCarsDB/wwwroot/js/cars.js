"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/carhub").build();

connection.on("DropTable", function () {
    dropTable();
});

connection.on("UpdateCars", function (year, vin, modelname, factoryname) {
    var encodedMsg = "new row: " + year + " " + vin + " " + modelname + " " + factoryname;
    console.log(encodedMsg);
    createRow(year, vin, modelname, factoryname);
});

connection.start().then(function () {
    console.log("connected! signalr");
});

function dropTable() {
    var tablebody = $("#tablebody");
    tablebody.empty(); 
}

function createRow(year, vin, modelname, factoryname) {
    var table = document.getElementById("tablebody");
    var row = table.insertRow(0);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = year;
    cell2.innerHTML = vin;
    cell3.innerHTML = modelname;
    cell4.innerHTML = factoryname;
}