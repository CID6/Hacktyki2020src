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
    SetGroups();
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

function SetGroups() {
    var astra = false;
    var vectra = false;
    var cruze = false;
    var almera = false;
    var epica = false;

    var cars = [];

    if (document.getElementById("astra").checked) cars.push("Astra");
    if (document.getElementById("vectra").checked) cars.push("Vectra");
    if (document.getElementById("cruze").checked) cars.push("Cruze");
    if (document.getElementById("almera").checked) cars.push("Almera");
    if (document.getElementById("epica").checked) cars.push("Epica");

    for (var i = 0; i < cars.length; i++) {
        try {
            connection.invoke("JoinGroup", cars[i]);
            console.log("user joined the group: " + cars[i]);
        }
        catch (e) {
            console.error(e.toString());
        }
    }
}

function ResetGroups() {
    var carNameArray = ["Astra", "Vectra", "Cruze", "Almera", "Epica"];

    for (var i = 0; i < carNameArray.length; i++) {
        try {
            connection.invoke("LeaveGroup", carNameArray[i]);
        }
        catch (e) {
            console.error(e.toString());
        }
    }
}