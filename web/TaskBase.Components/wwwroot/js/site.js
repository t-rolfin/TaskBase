$("#avatar-img").hover(() => {
    $("#avatar-placeholder").toggleClass("d-none");
});

$("#culture-switcher").change(() => {
    $.get("/Culture/HandleCulture?culture=" + $("#culture-options").val() + "&redirectUri=" + location.href,
        (response) => {
            window.location.href = response;
        }
    )
});

function triggerInput() {
    var inputElement = document.getElementById("upload-avatar");
    inputElement.click();
    return false;
}

function submiteForm() {
    var btn = document.getElementById("submit-form");
    btn.click();
}

function closeModal() {
    $('#createTaskModal').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();

    enableDragula();
}

const rowType = [
    "New",
    "InProgress",
    "Completed"
]

$(document).ready(() => {
    enableDragula();
});

function enableDragula() {
    dragula([
        document.querySelector("#ToDo"),
        document.querySelector("#Doing"),
        document.querySelector("#Done")
    ]).on("drop", function (el, target, source) {

        var token = $('input[name="__RequestVerificationToken"]').val();
        var taskid = el.dataset.id;
        var newstate = target.id;

        $.post("/Tasks/ChangeTaskState", { taskid, newstate, "__RequestVerificationToken": token });
    });
}

function toggleEdit(enableEditId, saveEditId, toBeDisable) {
    document.getElementById(toBeDisable).toggleAttribute("disabled");
    $("#" + saveEditId).toggleClass("d-flex");
    $("#" + enableEditId).toggleClass("d-none");
}

$(document).ready(() => autosize(document.querySelectorAll('textarea')));

function autoTextarea() {
    autosize(document.querySelectorAll('textarea'));
}

function openDetailsModal() {
    $("#details-modal").addClass("show-details");
}

function closeDetailsModal() {
    $("#details-modal").removeClass("show-details");
}