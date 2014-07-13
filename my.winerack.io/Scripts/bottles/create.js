$('#Vineyard').typeahead(null, {
    name: 'vineyards',
    displayKey: 'name',
    source: vineyardBloodhound.ttAdapter()
}).on('typeahead:autocompleted', function (obj, datum) {
    $('#VineyardID').val(datum.id);
}).on('typeahead:selected', function (obj, datum) {
    $('#VineyardID').val(datum.id);
});

$('#Region').typeahead(null, {
    name: 'regions',
    displayKey: 'name',
    source: regionBloodhound.ttAdapter(),
    templates: {
        suggestion: function (data) {
            return "<p>" + data.label + "</p>"
        }
    }
}).on('typeahead:autocompleted', function (obj, datum) {
    $('#RegionID').val(datum.id);
    $("#Country").val(datum.country);
}).on('typeahead:selected', function (obj, datum) {
    $('#RegionID').val(datum.id);
    $("#Country").val(datum.country);
});

$('#Varietal').typeahead(null, {
    name: 'varietals',
    displayKey: 'name',
    source: varietalBloodhound.ttAdapter()
});

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

$('#cellarSlider').editRangeSlider({
    arrows: false,
    bounds: { min: 0, max: 20 },
    defaultValues: { min: $('#CellarMin').val(), max: $('#CellarMax').val() },
    step: 1
}).bind("valuesChanged", function (e, data) {
    $('#CellarMin').val(data.values.min);
    $('#CellarMax').val(data.values.max);
});