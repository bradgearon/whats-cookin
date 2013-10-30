'use strict';

angular.module('menuApp')
    .controller('MainCtrl', ['$scope', '$routeParams', '$location', 'Menu', 'Embed',
        function ($scope, $routeParams, $location, Menu, Meal, Embed) {
            var menu = new Menu();

            var dt = moment($routeParams.date) || moment(new Date());
            $scope.embedly = { key: 'ade6e90f8259426bb359440c9b8e446e' };
            $scope.menu = { date: dt.toISOString() };
            $scope.rate = 7;
            $scope.max = 10;
            $scope.isReadonly = false;

            $scope.$watch(function () {
                    return $routeParams.date
                },
                function () {
                    $scope.meals = Menu.query({ res: 'meals',
                        date: moment($scope.menu.date).toISOString()

                    });
                });

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
                $scope.menu.date = moment(date)
                    .add('days', interval).format('YYYY-MM-DD');
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
                meal.wait.push(1);
                Menu.save({ res: 'meal' }, meal, function (result) {
                    meal.wait.pop();
                    angular.extend(meal, result);
                });
            };

            $scope.urlChange = function (meal) {
                $scope.updateMeal(meal);

                meal.wait.push(1);
                $.when($.embedly.oembed(meal.url, $scope.embedly))
                    .then(function (results) {
                        if (results && results.length > 0) {
                            delete results[0].id;
                            meal = angular.extend(meal, results[0]);
                            $scope.$apply();
                            $scope.updateMeal(meal);
                            meal.wait.pop();
                        }
                    });
            };

            $scope.$watch('menu.date', function (date) {
                var formDate = moment(date)
                    .format('YYYY-MM-DD');
                $location.search({date: formDate});
            });

        }]);
