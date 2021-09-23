$("#avatar-img").click(() => {
    var inputElement = document.getElementById("upload_avatar");
    inputElement.click();
})

function submiteForm() {
    var form = document.getElementById("upload_avatar_form");
    var redirect = document.getElementById("redirectUrl");
    redirect.value = location.href;
    form.submit();
}