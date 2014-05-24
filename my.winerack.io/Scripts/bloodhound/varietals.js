var varietalBloodhound = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    limit: 10,
    prefetch: {
        url: '/api/varietals',
        filter: function (list) {
            return $.map(list, function (varietal) {
                return {
                    name: varietal
                };
            });
        }
    }
});

varietalBloodhound.initialize();