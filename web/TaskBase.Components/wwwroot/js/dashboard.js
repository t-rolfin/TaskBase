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

    var notificationContainer = $("#notifications-wrapper");
    var notificationCounter = $("#notification-counter").val();

    var notification = `
        <div class="card bg-dark mb-2" id="${notification.id}">

            <div class="card-header d-flex text-white p-0 m-0 align-items-center justify-content-between">
                <h5 class="p-0 m-1">${notification.title}</h5>
                <i onclick="removeNotification('${notification.id}')" class="fas fa-times mx-2 my-1"></i>
            </div>

            <div class="card-body px-1 py-0">
                <p>${notification.description}</p>
            </div>

        </div>
    `
    $("#notification-counter").val(Number(notificationCounter) + 1);

    notificationContainer.prepend(notification);
});

connection.on("RecievePageNotification", function (notification) {
    PageNotification(notification.message, notification.isSuccess);
});

$(".notifications-container").on("click", function (e) {
    e.stopPropagation();
});

function removeNotification(notificationId) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    console.log("here!");

    $.post("/Tasks/RemoveNotification", { notificationId, "__RequestVerificationToken": token }, (isSuccess) => {
        if (isSuccess) {
            var notificationCounter = $("#notification-counter").val();
            $("#notification-counter").val(Number(notificationCounter) - 1);

            $("#" + notificationId).fadeOut(100, function () {
                $(this).remove();
            });

            event.stopPropagation();
            PageNotification("The notification was successfully deleted.", isSuccess)
        } else {
            PageNotification("The notification couldn't be deleted.", isSuccess)
        }
    })
}

function PageNotification(message, isSuccess) {
    var pageNotifications = $("#page-notifications");

    var notification = `
                <div class="alert ${ isSuccess ? 'alert-success' : 'alert-warning' } alert-dismissible fade show" role="alert">
                  ${message}
                </div>
             `;

    pageNotifications.prepend(notification);

    $(".alert").delay(2000).slideUp(200, function () {
        $(this).alert('close');
    });
}