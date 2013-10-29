'use strict';

angular.module('menuApp.services.embed', ['ngResource'])
    .factory('Embed', ['$resource',
        function Embed($resource) {
          var defaults = {
            format: 'json'
          };
          var actions = {};


          var embed = $resource('', defaults, actions);

          //var embed = $resource('http://api.' + window.location.hostname + '/embed', defaults, actions);
          return embed;
        }]);
