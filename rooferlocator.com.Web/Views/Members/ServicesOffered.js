(function () {
    $(function () {
    })
});

//(function () {
//    var controllerId = 'app.views.common.types.servicesoffered';
    
//    angular.module('app').controller(controllerId, [
//        '$rootScope', '$scope', '$state', 'abp.services.app.roofType', 'abp.services.app.serviceType',
//        function ($rootScope, $scope, $state, roofTypeService, serviceTypeService) {
//            var vm = this;
//            //Layout logic...

//            vm.rooftypes = [{}];
//            vm.roofTypeInput = { criteriaRefId: 0, companyId: chCompanyId };
//            vm.selectedRoofType = {};
//            vm.servicetypes = [{}];
//            vm.serviceTypeInput = { criteriaRefId: 0, companyId: chCompanyId };
//            vm.selectedServiceType = {};


//            //Get roofType list
//            vm.getRoofTypes = function () {
//                roofTypeService.getRoofTypes(
//                        vm.roofTypeInput
//                        ).success(function (data) {
//                            vm.rooftypes = data.roofTypes
//                        }).error(function (msg) {
//                            abp.notify.info("Roof Types Not Loaded.");
//                        });
//            }

//            //Get ServiceType list
//            vm.getServiceTypes = function () {
//                serviceTypeService.getServiceTypes(
//                        vm.serviceTypeInput
//                        ).success(function (data) {
//                            vm.servicetypes = data.serviceTypes
//                        }).error(function (msg) {
//                            abp.notify.info("Service Types Not Loaded.");
//                        });
//            }

//            //Get list of RoofTypes
//            vm.getRoofTypes();

//            //Get list of ServiceTypes
//            vm.getServiceTypes();
//        }
//    ]);
//})();