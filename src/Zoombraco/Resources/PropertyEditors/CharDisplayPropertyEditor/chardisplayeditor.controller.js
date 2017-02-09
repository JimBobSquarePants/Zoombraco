angular.module("umbraco")
       .controller("Zoombraco.CharDisplayEditorController",
       function ($scope) {
           $scope.load = function () {
               if ($scope.model.value) {
                   $scope.info = "You have used " + ($scope.model.value.length) + " characters.";
               } else {
                   $scope.info = "You have used 0 characters.";
               }
           };

           $scope.limitChars = function () {
               $scope.info = "You have used " + ($scope.model.value.length) + " characters.";
           };

           $scope.load();
       });