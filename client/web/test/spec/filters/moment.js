'use strict';

describe('Filter: moment', function () {

  // load the filter's module
  beforeEach(module('menuApp'));

  // initialize a new instance of the filter before each test
  var moment;
  beforeEach(inject(function ($filter) {
    moment = $filter('moment');
  }));

  it('should return the input prefixed with "moment filter:"', function () {
    var text = 'angularjs';
    expect(moment(text)).toBe('moment filter: ' + text);
  });

});
