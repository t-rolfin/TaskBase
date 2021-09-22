function uploadAvatar() {
    var inputElement = document.getElementById("upload_avatar");
    inputElement.click();
}

function callMethod() {
    var form = document.getElementById("upload_avatar_form");
    form.submit();
    console.log("submited!");
}