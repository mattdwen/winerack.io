var Winerack = Winerack || {};
Winerack.Directives = Winerack.Directives || {};

Winerack.Directives.DrinkingWindow = function () {
  'use strict';

  return {
    restrict: 'E',
    replace: true,
    scope: {
      bottle: '=bottle'
    },
    link: function (scope) {
      var now = new Date(),
          currentYear = now.getFullYear(),
          currentDayOfYear = (now - new Date(currentYear, 0, 0)) / (1000 * 60 * 60 * 24),
          vintage = scope.bottle.Vintage,
          cellarMin = scope.bottle.CellarMin,
          cellarMax = scope.bottle.CellarMax;

      if (cellarMin === null && cellarMax === null) {
        scope.marker = 50;
        scope.title = 'No cellar window';
        return;
      } else if (cellarMin === null) {
        cellarMin = 0;
        scope.title = 'Before ' + (vintage + cellarMax);
      } else if (cellarMax === null) {
        cellarMax = currentYear - vintage;
        scope.title = (vintage + cellarMin) + ' onwards';
      }

      var windowStartYear = vintage + cellarMin,
          windowEndYear = vintage + cellarMax,
          rangeStartYear = (currentYear <= windowStartYear) ? (currentYear) : windowStartYear,
          rangeEndYear = (currentYear >= windowEndYear) ? (currentYear) : windowEndYear,
          rangeStartDay = 0,
          rangeEndDay = (rangeEndYear - rangeStartYear + 1) * 365,
          windowStartDay = (windowStartYear - rangeStartYear) * 365,
          windowEndDay = (windowEndYear - rangeStartYear + 1) * 365,
          markerDay = ((currentYear - rangeStartYear) * 365) + currentDayOfYear;

      scope.windowStart = windowStartDay / rangeEndDay * 100;
      scope.windowEnd = 100 - (windowEndDay / rangeEndDay * 100);
      scope.marker = markerDay / rangeEndDay * 100;

      if (currentYear > windowEndYear) {
        scope.windowStyle = 'past';
      } else if (currentYear < windowStartYear) {
        scope.windowStyle = 'future';
      } else {
        scope.windowStyle = 'current';
      }

      if (scope.title === undefined) {
        if (windowStartYear === windowEndYear) {
          scope.title = windowStartYear;
        } else if (windowStartYear > 0 && windowEndYear > 0) {
          scope.title = windowStartYear + ' to ' + windowEndYear;
        }
      }
    },
    template: '<div class="drinking-window" title="{{title}}"><div class="window {{windowStyle}}" style="left: {{windowStart}}%; right: {{windowEnd}}%;"></div><div class="marker" style="left: {{marker}}%;"></div></div>'
  };
};