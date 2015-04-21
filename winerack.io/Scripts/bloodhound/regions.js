var regionBloodhound = new Bloodhound({
  datumTokenizer: Bloodhound.tokenizers.obj.whitespace('label'),
  queryTokenizer: Bloodhound.tokenizers.whitespace,
  limit: 10,
  remote: {
    url: '/api/regions?q=%QUERY',
    filter: function (list) {
      return $.map(list, function (region) {
        return {
          id: region.ID,
          name: region.Name,
          country: region.Country,
          label: region.Label
        };
      });
    }
  }
});

regionBloodhound.initialize();