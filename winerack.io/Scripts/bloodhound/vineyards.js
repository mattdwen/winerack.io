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
          name: vineyard.Name
        };
      });
    }
  }
});

vineyardBloodhound.initialize();