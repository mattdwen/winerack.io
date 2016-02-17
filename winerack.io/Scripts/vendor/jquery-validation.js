jQuery.validator.setDefaults({
  highlight: function (element) {
    $(element).closest('.form-group').addClass('has-danger');
  },
  unhighlight: function (element) {
    $(element).closest('.form-group').removeClass('has-danger');
  }
});