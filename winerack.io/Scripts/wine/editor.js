//
// Wine Editor
// ============================================================================



// Vineyard selector
// ----------------------------------------------------------------------------

$('#Vineyard').typeahead(null, {
  name: 'vineyards',
  displayKey: 'name',
  source: vineyardBloodhound.ttAdapter()
}).on('typeahead:autocompleted', function (obj, datum) {
  $('#VineyardID').val(datum.id);
  $('#Region').typeahead('val', datum.region);
  $("#Country").select2('val', datum.country);
}).on('typeahead:selected', function (obj, datum) {
  $('#VineyardID').val(datum.id);
  $('#Region').typeahead('val', datum.region);
  $("#Country").select2('val', datum.country);
});



// Region selector
// ----------------------------------------------------------------------------

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
  $("#Country").select2('val', datum.country);
}).on('typeahead:selected', function (obj, datum) {
  $('#RegionID').val(datum.id);
  $("#Country").select2('val', datum.country);
});



// Page loaded
// ----------------------------------------------------------------------------

$(function () {
  $('#Country').select2();
  $('#Varietals').select2();
});