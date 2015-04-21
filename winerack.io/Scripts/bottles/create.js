$('#Vineyard').typeahead(null, {
  name: 'vineyards',
  displayKey: 'name',
  source: vineyardBloodhound.ttAdapter()
}).on('typeahead:autocompleted', function (obj, datum) {
  $('#VineyardID').val(datum.id);
}).on('typeahead:selected', function (obj, datum) {
  console.info(datum);
  $('#VineyardID').val(datum.id);
  $('#RegionID').val(datum.regionId);
  $('#Region').val(datum.region);
  $("#Country").select2('val', datum.country);
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
  $("#Country").select2('val', datum.country);
}).on('typeahead:selected', function (obj, datum) {
  $('#RegionID').val(datum.id);
  $("#Country").select2('val', datum.country);
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

$(function () {
  $('#Country').select2();
  $('#Varietals').select2();
});