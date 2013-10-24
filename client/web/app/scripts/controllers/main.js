'use strict';

angular.module('menuApp')
    .controller('MainCtrl', ['$scope','Menu', function ($scope, Menu) {
        var menu = new Menu();

        $scope.menu = { date: new Date() };
        $scope.rate = 7;
        $scope.max = 10;
        $scope.isReadonly = false;

        $scope.hoveringOver = function (value) {
            $scope.overStar = value;
            $scope.percent = 100 * (value / $scope.max);
        };

        $scope.ratingStates = [
            {stateOn: 'icon-ok-sign', stateOff: 'icon-ok-circle'},
            {stateOn: 'icon-star', stateOff: 'icon-star-empty'},
            {stateOn: 'icon-heart', stateOff: 'icon-ban-circle'},
            {stateOn: 'icon-heart'},
            {stateOff: 'icon-off'}
        ];

        $scope.incrementDate = function (date, interval) {
            $scope.menu.date = moment(date).add('days', interval)._d;
        };

        $scope.$watch('menu.date', function(date) {
            $scope.meals = menu.getMeals(date);
        });

    }]);
