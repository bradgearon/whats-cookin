'use strict';

angular.module('menuApp')
    .filter('moment', [function () {
        return function (dateString, format) {
            return moment(dateString).format(format);
        };
    }]);
