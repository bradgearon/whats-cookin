'use strict';

angular.module('menuApp')
    .controller('MainCtrl', ['$scope', 'Menu', 'Embed', function ($scope, Menu, Meal, Embed) {
      var menu = new Menu();
      $scope.embedly = { key: 'ade6e90f8259426bb359440c9b8e446e' };
      $scope.menu = { date: new Date() };
      $scope.rate = 7;
      $scope.max = 10;
      $scope.isReadonly = false;

      $scope.ratingOver = function (meal, value) {
        meal.overStar = value;
        meal.percent = value;
      };

      $scope.ratingLeave = function (meal) {
        if (meal.overStar == meal.rating) {
          $scope.updateMeal(meal);
        }
        meal.overStar = null;
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

      $scope.updateMeal = function (meal) {
        Menu.save({ res: 'meal' }, meal);
      }

      $scope.urlChange = function (meal) {
        meal.wait = true;
        $scope.updateMeal(meal);

        $.when($.embedly.oembed(meal.url, $scope.embedly))
          .then(function (results) {
            meal.wait = false;
            if (results && results.length > 0) {
              angular.extend(meal, results[0]);
              $scope.$apply();
              $scope.updateMeal(meal);
            }
          });
        /*
        Embed.get({ url: meal.url, width: '300' },
          function (embed) {
            meal.wait = false;
            angular.extend(meal, embed);
          });
          */
      };

      $scope.$watch('menu.date', function (date) {
        $scope.meals = Menu.query({ res: 'meals' }, date);
      });

    }]);
