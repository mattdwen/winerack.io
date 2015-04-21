var vineyardBloodhound = new Bloodhound({
  datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
  queryTokenizer: Bloodhound.tokenizers.whitespace,
  limit: 10,
  remote: {
    url: '/api/vineyards?q=%QUERY',
    filter: function (list) {
      return $.map(list, function (vineyard) {
        return {
          id: vineyard.ID,
          name: vineyard.Name,
          regionId: (vineyard.Region !== null) ? vineyard.Region.ID : null,
          region: (vineyard.Region !== null) ? vineyard.Region.Name : null,
          country: (vineyard.Region !== null) ? vineyard.Region.Country : null,
        };
      });
    }
  }
});

vineyardBloodhound.initialize();