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
    $('#your-modal-id').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}
