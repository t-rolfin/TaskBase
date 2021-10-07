const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5004/notificationhub",
        {
            accessTokenFactory: () => $("input[name='__NameIdentifier']").val(),
            transport: signalR.HttpTransportType.LongPolling
        })
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    await connection.start()
        .then(() => console.log("Connection started"))
        .catch(e => console.log(e));
};

$(document).ready(() => {
    start();
});

connection.on("ReceiveNotification", function (notification) {

    var notificationContainer = $("#notifications-container");
    var notificationCounter = $("#notification-counter").val();

    var notification = `
        <div class="card bg-dark mb-2">

            <div class="card-header d-flex text-white p-0 m-0 align-items-center justify-content-between">
                <h5 class="p-0 m-1">${notification.title}</h5>
                <i class="fas fa-times mx-2 my-1"></i>
            </div>

            <div class="card-body px-1 py-0">
                <p>${notification.description}</p>
            </div>

        </div>
    `
    $("#notification-counter").val(Number(notificationCounter) + 1);

    notificationContainer.prepend(notification);
});

$(".removenotification").on("click", function (e) {

    var fadeDelete = $(this).parents('.card');

    $(fadeDelete).fadeOut(function () {
        $(this).remove();
    });

    e.stopPropagation();
});