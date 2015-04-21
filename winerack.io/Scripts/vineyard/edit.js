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
  //$('#RegionID').val(datum.id);
  $("#Country").select2('val', datum.country);
}).on('typeahead:selected', function (obj, datum) {
  //$('#RegionID').val(datum.id);
  $("#Country").select2('val', datum.country);
});

$(function () {
  $('#Country').select2();
});