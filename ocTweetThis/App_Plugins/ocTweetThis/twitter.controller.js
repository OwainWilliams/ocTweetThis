angular.module("umbraco")
    .controller("ocTweetThis", function ($scope, $timeout, $http, editorState, userService, contentResource, mediaHelper, editorService, tweetpublishedResource) {

        var vm = this;
        vm.CurrentNodeId = editorState.current.id;
        vm.CurrentNodeAlias = editorState.current.contentTypeAlias;
        vm.CurrentNodeUrl = GetUrl();

         
        tweetpublishedResource.getAll(vm.CurrentNodeId)
           .then(function (data) {
               if (Utilities.isArray(data))
                   vm.TweetAudit = data
           });

     

        var counter = contentResource.getById(vm.CurrentNodeId).then(function (node) {
            var properties = node.variants[0].tabs[0].properties;

            vm.propertyWordCount = {};

            var index;
            for (index = 0; index < properties.length; ++index) {
                var words = properties[index].value;
                var wordCount = words.trim().split(/\s+/).length;

                vm.propertyWordCount[properties[index].label] = wordCount;
            }
        });

        $scope.submit = function ($scope) {
            var twitterMessage =
            {
                params: {
                    message: $scope.message,
                    nodeId: $scope.CurrentNodeId
                }
            };
            $http.get('/Twitter', twitterMessage);

        };


      
        $scope.add = function () {
            var startNodeId = $scope.model.config && $scope.model.config.startNodeId ? $scope.model.config.startNodeId : undefined;
          
            var mediaPicker = {
                startNodeId: startNodeId,
               
                disableFolderSelect: true,
                onlyImages: true,
                submit: function (model) {
                    var selectedImage = model.selection[0];

                    $scope.control.value = {
                      
                        id: selectedImage.id,
                        udi: selectedImage.udi,
                        image: selectedImage.image,
                      
                    };

                    editorService.close();
                },
                close: function () {
                    editorService.close();
                }
            }

            editorService.mediaPicker(mediaPicker);
        };

        $scope.$watch('control.value', function (newValue, oldValue) {
            if (angular.equals(newValue, oldValue)) {
                return; // simply skip that
            }

            $scope.thumbnailUrl = getThumbnailUrl();
        }, true);

        function getThumbnailUrl() {

            if ($scope.control.value && $scope.control.value.image) {
                var url = $scope.control.value.image;

                if ($scope.control.editor.config && $scope.control.editor.config.size) {
                    url += "?width=" + $scope.control.editor.config.size.width;
                    url += "&height=" + $scope.control.editor.config.size.height;
                    url += "&animationprocessmode=first";

                    if ($scope.control.value.focalPoint) {
                        url += "&center=" + $scope.control.value.focalPoint.top + "," + $scope.control.value.focalPoint.left;
                        url += "&mode=crop";
                    }
                }

                // set default size if no crop present (moved from the view)
                if (url.indexOf('?') == -1) {
                    url += "?width=800&upscale=false&animationprocessmode=false"
                }
                return url;
            }

            return null;
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