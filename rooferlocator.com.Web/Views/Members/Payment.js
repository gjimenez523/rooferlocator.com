(function () {
    $(function () {
        $('#btnStripeSubmit').click(function (e) {
            var $form = $('#frmPayment');

            // Disable the submit button to prevent repeated clicks
            $form.find('btnStripeSubmit').prop('disabled', true);

            Stripe.card.createToken($form, stripeResponseHandler);

            // Prevent the form from submitting with the default action
            return false;
        });

        function stripeResponseHandler(status, response) {
            var $form = $('#frmPayment');

            if (response.error) {
                // Show the errors on the form
                $form.find('.payment-errors').text(response.error.message);
                $form.find('btnStripeSubmit').prop('disabled', false);
            } else {
                // response contains id and card, which contains additional card details
                var token = response.id;
                // Insert the token into the form so it gets submitted to the server
                $form.append($('<input type="hidden" name="stripeToken" />').val(token));
                // and submit
                $form.get(0).submit();
            }
        };
    })
});

//(function () {
//    var controllerId = 'app.views.sales.payment';
//    angular.module('app').controller(controllerId, [
//        '$scope', '$rootScope', 'abp.services.app.member',
//        function ($scope, $rootScope, subscriberService) {
//            var vm = this;

//            $scope.company = {};
//            vm.selectedCompany = {};
//            vm.paymentInfo = {};
//            vm.paymentResponse = {};
            
//            vm.paymentGatewayOptions = [
//                { name: 'Authorize.NET', controlId: 'authorizeNet', imageUri: '/App/Main/images/logo_CreditCard.png' },
//                { name: 'PayPal', controlId: 'paypal', imageUri: '/App/Main/images/logo_Paypal.png' }];
//            var dialogOptions = { controller: 'EditCtrl', templateUrl: 'itemEdit.html' }

//            $scope.save = function (gatewayType) {
//                vm.paymentInfo.paymentGatewayType = gatewayType;
//                if (vm.paymentInfo.paymentGatewayType == 'AuthorizeNET') {
//                    vm.paymentInfo = {
//                        paymentGatewayType: 'AuthorizeNET',
//                        paymentMethod: 'ChargeCreditCard',
//                        transactionType: 'AuthorizeAndCapture',
//                        amount: vm.paymentGateway.amount,
//                        taxAmount: '0.00',
//                        cardNumber: vm.paymentGateway.cardNumber,
//                        expirationDate: vm.paymentGateway.expirationDate,
//                        cardCode: vm.paymentGateway.cardCode,
//                        purchaseDescription: 'Test CreditsHero',
//                        marketType: ''
//                    };

//                    subscriberService.makeAuthorizationNetPurchase(
//                        vm.paymentInfo
//                    ).success(function (data) {
//                        vm.paymentResponse = data;
//                        if (vm.paymentResponse.resultCode == 'Ok') {
//                            vm.paymentResponse.message = "Your transaction has been successfully processed.  You will receive an email receipt."
//                        }
//                        document.getElementById('authorizeNetBody').style.display = 'none';
//                        document.getElementById('authorizeNetFooter').style.display = 'none';
//                        document.getElementById('authorizeNetResponse').style.display = 'block';

//                        abp.notify.info("Subscriber updated.");
//                    }).error(function (msg) {
//                        abp.notify.info("Subscriber was not updated.");
//                    });
//                }
//                else if (vm.paymentInfo.paymentGatewayType == 'Paypal') {
//                    vm.paymentInfo = {
//                        paymentGatewayType: 'Paypal',
//                        paymentMethod: 'ChargeCreditCard',
//                        transactionType: 'AuthorizeAndCapture',
//                        amount: vm.paymentGateway.amount,
//                        taxAmount: '0.00',
//                        payerId: 'test Paypal'
//                    };

//                    subscriberService.makePaypalPurchase(
//                        vm.paymentInfo
//                    ).success(function (data) {
//                        abp.notify.info("Subscriber updated.");
//                    }).error(function (msg) {
//                        abp.notify.info("Subscriber was not updated.");
//                    });
//                }


//            };
//        }
//    ]);
//})();
