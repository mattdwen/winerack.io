$('#photoSelect').click(function (e) {
    e.preventDefault();
    $("#Photo").click();
});

$('#Photo').change(function () {
    if (this.files && this.files[0]) {
        $('#formProfilePicture').submit();
    }
});