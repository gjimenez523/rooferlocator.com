(function () {
    $(function () {
    })
});

//(function () {
//    var controllerId = 'app.views.home';
    
//    angular.module('app').controller(controllerId, [
//        '$rootScope', '$scope',
//        function ($rootScope, $scope) {
//            var vm = this;
//            //Layout logic...

//            $scope.handleClick = function (controllerId) {
//                $rootScope.sharedService.prepForBroadcast($rootScope.selectedCustomer, controllerId);
//            };

//            $scope.handleLocationClick = function (controllerId) {
//                $rootScope.sharedService.prepForBroadcast($rootScope.selectedCustomer, controllerId);
//            };
//        }]);

//    angular.module('app').factory(
//        'customerSharedService',
//        function ($rootScope) {
//            $rootScope.sharedService = {};
//            $rootScope.selectedCustomer = null;
//            $rootScope.selectedLocation = null
//            $rootScope.sharedService.message = '';

//            $rootScope.sharedService.prepForBroadcast = function (msg, controlId) {
//                this.message = msg;
//                $rootScope.sharedService.message = msg;
//                this.broadcastItem(msg, controlId);

//                //document.getElementById('customerDetail').style.display = 'none';
//                document.getElementById('customerPayment').style.display = 'none';
//                document.getElementById('customerAgreement').style.display = 'none';
//                document.getElementById('customerAgreementList').style.display = 'none';
//                document.getElementById('customerOrder').style.display = 'none';
//                document.getElementById('customerOrderList').style.display = 'none';
//                document.getElementById('customerLocation').style.display = 'none';
//                document.getElementById('customerCompany').style.display = 'none';

//                if (document.getElementById(controlId) != undefined)
//                    document.getElementById(controlId).style.display = 'block';

//                if (controlId == 'customerPayment'
//                    || controlId == 'customerSales'
//                    || controlId == 'customerAgreement'
//                    || controlId == 'customerOrder') {
//                    //document.getElementById('customerAgreement').style.display = 'block';
//                    document.getElementById('customerAgreementList').style.display = 'block';
//                    document.getElementById('customerOrderList').style.display = 'block';
//                }
//            };

//            $rootScope.sharedService.broadcastItem = function (msg, controlId) {
//                $rootScope.$broadcast(controlId);
//            };

//            return $rootScope.sharedService;
//        });

//    angular.module('app').directive(
//        'editCustomer',
//        function (customerSharedService) {
//            return {
//                restrict: 'A',
//                link: function ($scope, $attrs, customerSharedService) {
//                    $scope.$on('customerSelected', function () {
//                        $scope.message = 'Directive: ' + $scope.sharedService.message;
//                    });
//                }
//            };
//        });

//})();