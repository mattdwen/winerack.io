var openingsApp = angular.module('openingsApp', ['bottleService', 'ui.utils', 'ui.select2']);

openingsApp.controller('StartCtrl', function ($scope, Bottle) {
    $scope.select2Options = {
        allowClear: true
    };

    $scope.bottles = Bottle.query();

    $scope.filter = {
        drinking: '',
        keywords: '',
        style: '',
        varietal: ''
    };

    $scope.search = function (bottle) {
        var style = false,
            varietal = false,
            keywords = false,
            drinking = false;

        // Drinking
        if ($scope.filter.drinking == '') {
            drinking = true;
        } else {
            var yearNow = new Date().getFullYear(),
                yearStart = bottle.Vintage + bottle.CellarMin,
                yearEnd = bottle.Vintage + bottle.CellarMax;

            if ($scope.filter.drinking == 'now' && yearStart <= yearNow && yearEnd >= yearNow) {
                drinking = true;
            } else if ($scope.filter.drinking == 'over' && bottle.Vintage > 0 && yearEnd < yearNow) {
                drinking = true;
            } else if ($scope.filter.drinking == 'now' && bottle.Vintage == undefined) {
                drinking = true;
            }
        }

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
            style = true;
        }

        // Varietal
        if ($scope.filter.varietal == '') {
            varietal = true;
        } else if ($scope.filter.varietal == bottle.Varietal) {
            varietal = true;
        }

        return (varietal && style && drinking && keywords);
    };
});