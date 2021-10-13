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

function toggleEdit(enableEditId, saveEditId, toBeDisable, hasExtraClass = false) {
    document.getElementById(toBeDisable).toggleAttribute("disabled");
    $("#" + saveEditId).toggleClass("d-flex");
    $("#" + enableEditId).toggleClass("d-none");

    if (hasExtraClass)
        $("#" + enableEditId).removeClass("d-flex");

    if (!$("#" + enableEditId).hasClass("d-none") && hasExtraClass)
        $("#" + enableEditId).addClass("d-flex");
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

function createNoteContainer(taskId) {
    var notesContainer = $("#notes-container");
    var antiForgeryKey = $("input[name=__RequestVerificationToken]").val();

    var note = `
        <div class="d-flex flex-column border rounded p-2 mb-2" id="create-note">

            <form action="/Tasks/CreateNote" method="post" data-ajax="true" data-ajax-method="post" data-ajax-mode="replace-with"
                data-ajax-update="#notes-container" data-ajax-complete="autoTextarea()">

                <input type="hidden" name="__RequestVerificationToken" value="${antiForgeryKey}" />
                <input type="hidden" name="taskId" value="${taskId}" />
                <textarea class="border rounded w-100 text-white" name="noteContent" id="note-content"></textarea>

                <div>
                    <div class="d-flex float-right">

                        <div class="align-items-center" id="save-note">
                            <i onclick="closeNoteCreation()" class="fas fa-times mr-2"></i>
                            <button type="submit" class="border-0"><i class="fas fa-save"></i></button>
                        </div>

                        <i onclick="toggleEdit('enable-desc-edit', 'save-desc-edit', 'note-content')" 
                            id="edit-note}" class="fas fa-edit d-none"></i>

                    </div>
                </div>

            </form>

        </div>
    `;

    notesContainer.prepend(note);

    autosize(document.querySelectorAll('textarea'));
}

function closeNoteCreation() {
    var creationContainer = $("#create-note");
    creationContainer.remove();
}

function removeNoteFromList(containerId){
    var containerToBeDeleted = document.getElementById(containerId);
    containerToBeDeleted.remove();
}