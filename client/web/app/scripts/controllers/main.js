'use strict';

angular.module('menuApp')
    .controller('MainCtrl', ['$scope', 'Menu', 'Embed', function ($scope, Menu, Embed) {
      var menu = new Menu();

      $scope.menu = { date: new Date() };
      $scope.rate = 7;
      $scope.max = 10;
      $scope.isReadonly = false;

      $scope.hoveringOver = function (value) {
        this.meal.overStar = value;
        this.meal.percent = value;
      };

      $scope.ratingStates = [
          { stateOn: 'icon-ok-sign', stateOff: 'icon-ok-circle' },
          { stateOn: 'icon-star', stateOff: 'icon-star-empty' },
          { stateOn: 'icon-heart', stateOff: 'icon-ban-circle' },
          { stateOn: 'icon-heart' },
          { stateOff: 'icon-off' }
      ];

      $scope.incrementDate = function (date, interval) {
        $scope.menu.date = moment(date).add('days', interval)._d;
      };

      $scope.spin = {
        radius: 15,
        hwaccel: true,
        length: 20,
        width: 10,
        color: '#3388DD',
        shadow: true,
        className: 'spinner',
        top: 'auto',
        left: 'auto'
      };

      $scope.urlChange = function (meal) {
        meal.wait = true;
        Embed.get({
          url: meal.url,
          width: '300'
        }, function (embed) {
          meal.wait = false;
          angular.extend(meal, embed);
        });
      };

      $scope.$watch('menu.date', function (date) {
        $scope.meals = Menu.query(date);
      });

    }]);
