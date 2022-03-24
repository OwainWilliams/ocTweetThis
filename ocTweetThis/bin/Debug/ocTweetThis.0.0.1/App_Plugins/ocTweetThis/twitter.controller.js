angular.module("umbraco")
    .controller("ocTweetThis", function ($scope, $timeout, notificationsService, $http, editorState, userService, contentResource, mediaHelper, editorService, tweetpublishedResource) {

        var vm = this;
        vm.CurrentNodeId = editorState.current.id;
        vm.CurrentNodeAlias = editorState.current.contentTypeAlias;
        vm.CurrentNodeUrl = GetUrl();
        vm.ImageUrl = "";
        var numOfTweets = 0;
       
        
        // Get all the tweets from DB table 
        function getTweets() {
            tweetpublishedResource.getAll(vm.CurrentNodeId).then(function (data) {
                if (Utilities.isArray(data)) {
                    vm.TweetAudit = data;
                    numOfTweets = data.length;
                    $scope.model.badge.count = data.length;
                }

            });
        }

        getTweets();

        $scope.model.badge = {
            count: numOfTweets, // the number for the badge - anything non-zero triggers the badge
            type: "warning" // optional: determines the badge color - "warning" = dark yellow, "alert" = red, anything else = blue (matching the top-menu background color)
        };
      


        //Submit Tweet
        $scope.submit = function ($scope) {
            var twitterMessage =
            {
                params: {
                    message: $scope.message,
                    nodeId: $scope.CurrentNodeId,
                    url: $scope.CurrentNodeUrl
                }
            };
            $http.get('/Twitter', twitterMessage)
                .then(function successCallBack(response) {
                    notificationsService.success("Tweet sent!");
                    getTweets;
                })
                .catch(function (err) { notificationsService.error("Sorry! Tweet failed to send"); });
              
        };

      
        function GetUrl() {


            // find out the url based on the current culture
            var allUrls = editorState.getCurrent().urls;

            var url = '';

            if (allUrls) {

                // If just one culture, lets use that
                if (allUrls.length === 1) {

                    url = allUrls[0].text;

                } else {
                    for (var i = 0; i < allUrls.length; i++) {
                        if (allUrls[i].culture === $scope.model.culture) {
                            url = allUrls[i].text;
                        }
                    }
                }

                if (url.indexOf('http://') === 0 || url.indexOf('https://') === 0) {
                    // if umbraco returns absolute urls we don't need to append the protocol and host
                    return url;
                }
            }
            return ProtocolAndHost() + url;
        };

        // Returns the protocal and host for the current domain (ie. http://www.mainwebsite.com).
         function ProtocolAndHost() {
            var http = location.protocol;
            var slashes = http.concat("//");
            return slashes.concat(window.location.hostname);
        };


    });