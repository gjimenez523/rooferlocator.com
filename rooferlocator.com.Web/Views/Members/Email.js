(function () {
    $(function () {
    })
});


//(function () {
//    var controllerId = 'app.views.common.member.email';
    
//    angular.module('app').controller(controllerId, [
//        '$rootScope', '$scope', '$state', 'abp.services.app.member', 
//        function ($rootScope, $scope, $state, memberService) {
//            var vm = this;
//            //Layout logic...

//            vm.email = {}; //emailMessage: undefined, emailTo: chEmailTo};
//            vm.email.emailTo = chEmailTo;

//            //Save Member
//            vm.sendEmail = function () {
//                memberService.sendEmail(
//                vm.email
//                ).success(function () {
//                    abp.notify.info("Email submitted to Administrator.");
//                }).error(function (msg) {
//                    abp.notify.info("Email failed to submit to Administrator.");
//                });
//            }
//        }]);
//})();