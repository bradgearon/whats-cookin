'use strict';

angular.module('menuApp')
  .directive('spin', function () {
    return function (scope, elm, attrs) {
      var spinner = new Spinner({});
      scope.$watch(attrs.spinOpts, function (value) {
        spinner = new Spinner(value);
      });
      scope.$watch(attrs.spinWhen, function (value) {
        if (value) {
          spinner.spin();
          elm.append(spinner.el);
        }
        else {
          spinner.stop(elm);
        }
      });
    };
  });

