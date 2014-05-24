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