var tastingsApp = angular.module('tastingsApp', ['bottleService', 'ui.utils', 'ui.select2']);

tastingsApp.controller('StartCtrl', function ($scope, Bottle) {
    $scope.select2Options = {
        allowClear: true
    };

    $scope.bottles = Bottle.query();

    $scope.filter = {
        keywords: '',
        style: '',
        varietal: '',
    };

    $scope.search = function (bottle) {
        var keywords = false,
            style = false,
            varietal = false;

        // Keywords
        if ($scope.filter.keywords == '') {
            keywords = true;
        } else {
            var q = $scope.filter.keywords,
                desc = bottle.Description.toLowerCase(),
                vineyard = bottle.Vineyard.toLowerCase();

            if (desc.indexOf(q) > -1 || vineyard.indexOf(q) > -1) {
                keywords = true;
            }
        }
        
        // Style
        if ($scope.filter.style == '') {
            style = true;
        } else if ($scope.filter.style == bottle.VarietalStyle) {
            style == true;
        }

        // Varietal
        if ($scope.filter.varietal == '') {
            varietal = true;
        } else if ($scope.filter.varietal == bottle.Varietal) {
            varietal = true;
        }

        return (varietal && style && keywords);
    };
});