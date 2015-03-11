var bottleService = angular.module('bottleService', ['ngResource']);

bottleService.factory('Bottle', ['$resource', function ($resource) {
    return $resource('/api/bottles/:id');
}]);