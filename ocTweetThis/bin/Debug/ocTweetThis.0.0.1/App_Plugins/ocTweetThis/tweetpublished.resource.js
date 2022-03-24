// adds the resource to umbraco.resources module:
angular.module('umbraco').factory('tweetpublishedResource',
    function ($http, umbRequestHelper) {
        // the factory object returned
        return {
            // this calls the ApiController we setup earlier
            getAll: function ($q) {
                return umbRequestHelper.resourcePromise(
                    $http.get("/Umbraco/backoffice/ocTweetThis/TwitterPublishedApi/GetAll"+"?blognodeId="+ $q),
                    "Failed to retrieve all Tweet data");
            }
        };
    }
);