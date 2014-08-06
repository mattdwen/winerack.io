$('#photoSelect').click(function (e) {
    e.preventDefault();
    $("#Photo").click();
});

$('#Photo').change(function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#photoPreview').attr('src', e.target.result);
        }

        reader.readAsDataURL(this.files[0]);
    }
});

$('#Friends').select2();