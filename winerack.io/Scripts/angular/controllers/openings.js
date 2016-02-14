var openingsApp = angular.module('openingsApp', ['bottleService', 'ui.utils', 'ui.select2']);

openingsApp.directive('drinkingWindow', Winerack.Directives.DrinkingWindow);

openingsApp.controller('StartCtrl', function ($scope, Bottle) {
  $scope.select2Options = {
    allowClear: true
  };

  $scope.styles = [];
  $scope.varietals = [];

  $scope.bottles = Bottle.query(function (bottles) {
    var styles = [];
    var varietals = [];
    angular.forEach(bottles, function (bottle) {
      styles = _.union(styles, bottle.Styles);
      varietals = _.union(varietals, bottle.Varietals);
    });

    console.info(styles);

    $scope.styles = styles.sort();
    $scope.varietals = varietals.sort();
  });

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
    } else if (bottle.Styles.indexOf($scope.filter.style) >= 0) {
      style = true;
    }

    // Varietal
    if ($scope.filter.varietal == '') {
      varietal = true;
    } else if (bottle.Varietals.indexOf($scope.filter.varietal) >= 0) {
      varietal = true;
    }

    return (varietal && style && drinking && keywords);
  };
});