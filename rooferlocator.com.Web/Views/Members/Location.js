(function () {
    $(function () {
    })
});

//(function () {
//    var controllerId = 'app.views.common.types.location.list';

//    angular.module('app').controller(controllerId, [
//        '$rootScope', '$scope', '$state', 'abp.services.app.location',
//        function ($rootScope, $scope, $state, locationService) {
//            var vm = this;
//            //Layout logic...

//            vm.locations = [{}];
//            vm.states = [{}];
//            vm.locationInput = {};
//            vm.selectedLocation = {};

//            //Get All Locations list
//            vm.getLocations = function () {
//                locationService.getLocations(
//                        vm.locationInput
//                        ).success(function (data) {
//                            vm.locations = data.locations
//                        }).error(function (msg) {
//                            abp.notify.info("Locations Not Loaded.");
//                        });
//            }

//            //Get States list
//            vm.getStates = function () {
//                locationService.getStates(
//                        ).success(function (data) {
//                            vm.states = data.locations
//                        }).error(function (msg) {
//                            abp.notify.info("States Not Loaded.");
//                        });
//            }

//            //Get Cities list
//            vm.getCities = function (state) {
//                locationService.getCities(
//                    state.state
//                        ).success(function (data) {
//                            vm.locations = data.locations
//                        }).error(function (msg) {
//                            abp.notify.info("Cities Not Loaded.");
//                        });
//            }

//            //Save Location
//            vm.saveType = function () {
//                if (vm.selectedLocation.id == undefined) {
//                    vm.selectedLocation.state = vm.selectedLocation.state.state;
//                    locationService.createLocation(
//                    vm.selectedLocation
//                    ).success(function () {
//                        vm.getCities($scope.item);
//                        $scope.showList();
//                        abp.notify.info("Location added.");
//                    }).error(function (msg) {
//                        abp.notify.info("Location was not added.");
//                    });
//                }
//                else {
//                    locationService.updateLocation(
//                        vm.selectedLocation
//                        ).success(function () {
//                            vm.getCities($scope.item);
//                            $scope.showList();
//                            abp.notify.info("Location updated.");
//                        }).error(function (msg) {
//                            abp.notify.info("Location was not updated.");
//                        });
//                }
//            }

//            //Delete Location
//            vm.deleteItem = function (type) {
//                vm.selectedLocation = type;
//                vm.selectedLocation.isDeleted = 1;
//                vm.saveType();
//            }

//            //Show Details panel
//            $scope.showDetails = function (type) {
//                vm.selectedLocation = type;
//                document.getElementById('Details').style.display = 'block';
//                document.getElementById('List').style.display = 'none';
//            }

//            //Show List panel
//            $scope.showList = function () {
//                document.getElementById('Details').style.display = 'none';
//                document.getElementById('List').style.display = 'block';
//            }

//            //Get list of RoofTypes
//            vm.getStates();
//        }
//    ]);
//})();