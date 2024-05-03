"use strict";

function AddMesage(fromId, mess) {

    if (fromId === FromId) {
        $("#content_chat").append(`<li class="list-group-item text-end">${mess}</li>`)
    } else {
        $("#content_chat").prepend(`<li class="list-group-item">${mess}</li>`)
    }
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    console.log("connection success")
}).catch(function (err) {
    console.log("connection err:::", err)
    return;
});

connection.on("HistoryChatRecord", function (message) {
    console.log("HistoryChatRecord", message)

    message.forEach(item => {
        AddMesage(item.fromUserId, item.content)
    })


});

connection.on("History", function (fromId, toId, roomId, message) {
    RoomId = roomId;
    this.toId = toId
    message.reverse();
    console.log("History", message)
    $("#content_chat").empty();
    message.forEach(item => {
        console.log(item)
        AddMesage(item.fromUserId, item.content)
    })

});

connection.on("ReceiveMessage", function (fromId, toId, message) {

    console.log("ReceiveMessage::", message)

    if (fromId === '@ViewBag.userId') {
        $("#content_chat").append(`<li class="list-group-item text-end">${message}</li>`)
    } else {
        $("#content_chat").append(`<li class="list-group-item">${message}</li>`)
    }
});

$("#input_chat").keyup(function (event) {
    if (event.keyCode === 13) {
        let value = $(this).val();
        console.log(value);
        $(this).val("")
        connection.invoke("SendMessage", FromId, toId, RoomId, value)
            .then(res => {

            })
            .catch(err => {
                console.log(err)
            })
    }
});
