'use strict';

angular.module('menuApp.services.menu', ['ngResource'])
  .factory('Menu', ['$resource', function Menu($resource) {
    var defaults = {
      format: 'json'
    };
    var actions = {};
    var intervalDuration = 15;
    var host = window.location.hostname;
    if (window.location.hostname === 'localhost') {
      host = 'menu.locl';
    }
    var menu = $resource('http://api.' + host + '/:res', defaults, actions);

    menu.prototype.getMeals = function (date) {
      var meals = [];
      var incrementStart = moment(date).startOf('week');
      var incrementEnd = moment(incrementStart).add('day', intervalDuration);
      for (var i = 0; i <= intervalDuration; i++) {
        meals.push({
          date: moment(incrementStart).add('day', i).format('MM/DD/YY'),
          name: 'meal' + i,
          url: 'http://example.org/meals/' + i
        });
      }

      return meals;
    };

    return menu;
  }])
  /*
  .factory('Meal', ['$resource', function Meal($resource) {
    var defaults = {
      format: 'json'
    };

    var actions = {};
    var host = window.location.hostname;
    if (window.location.hostname === 'localhost') {
      host = 'menu.locl';
    }
    var menu = $resource('http://api.' + host + '/menu', defaults, actions);
  }])
*/
;