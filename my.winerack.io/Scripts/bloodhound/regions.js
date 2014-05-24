var regionBloodhound = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('label'),
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    limit: 10,
    prefetch: {
        url: '/api/regions',
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