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

function createNoteContainer(taskId) {
    var notesContainer = $("#notes-container");
    var generatedId = makeid(10);
    var antiForgeryKey = $("input[name=__RequestVerificationToken]").val();

    var note = `
        <div class="d-flex flex-column border rounded p-2 mb-2" id="${generatedId}">

            <form action="/Tasks/CreateNote" method="post" data-ajax="true" data-ajax-method="post" data-ajax-mode="replace-with"
                data-ajax-update="#notes-container">

                <input type="hidden" name="__RequestVerificationToken" value="${antiForgeryKey}" />
                <input type="hidden" name="taskId" value="${taskId}" />
                <textarea class="border rounded w-100 text-white" name="noteContent" id="note-content-${generatedId}"></textarea>

                <div>
                    <div class="d-flex float-right">
                        <div class="align-items-center" id="save-note-${generatedId}">
                            <i onclick="toggleEdit('enable-desc-edit', 'save-desc-edit', 'note-content-${generatedId}')" class="fas fa-times mr-2"></i>
                            <button type="submit" class="border-0"><i class="far fa-save"></i></button>
                        </div>

                        <i onclick="toggleEdit('enable-desc-edit', 'save-desc-edit', 'note-content-${generatedId}')" 
                            id="edit-note-${generatedId}" class="fas fa-edit d-none"></i>
                    </div>
                </div>

            </form>

        </div>
    `;

    notesContainer.prepend(note);

    autosize(document.querySelectorAll('textarea'));
}


function makeid(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() *
            charactersLength));
    }
    return result;
}