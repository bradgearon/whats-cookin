'use strict';

angular.module('menuApp.services', ['ngResource'])
    .factory('Menu', ['$resource',
        function Menu($resource) {
            var defaults = {};
            var actions = {};
            var intervalDuration = 15;
            var menu = $resource('menu', defaults, actions);

            menu.prototype.getMeals = function (date) {
                var meals = [];
                var incrementStart = moment(date).startOf('week');
                var incrementEnd = moment(incrementStart).add('day', intervalDuration);
                for(var i = 0; i <= intervalDuration; i++) {
                    meals.push({
                        date: moment(incrementStart).add('day', i).format('MM/DD/YY'),
                        name: 'meal' + i,
                        url: 'http://example.org/meals/' + i
                    });
                }

                return meals;
            };

            return menu;
        }]);
