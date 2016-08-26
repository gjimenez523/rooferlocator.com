(function () {
    $(function () {

        $('#NewMember').click(function (e) {
            $('#Details')[0].style.display = 'block';
            if ($('#List').length > 0) $('#List')[0].style.display = 'none';
        });

        $('#btnSave').click(function (e) {
            //$('#Details')[0].style.display = 'block';
            //$('#List')[0].style.displayl = 'none';
            e.preventDefault();
            var inquiry = {
                companyId: chCompanyId,
                queryRequest: [{ key: 'Type Of Roof', value: 'Shingle' }]
            };

            abp.ajax({
                type: 'POST',
                url: chWebApiMakeInquiryUrl,
                data: JSON.stringify(inquiry)
            })
            .done(function (data, status, xhr) {
                alert(bar);
            })
            .fail(function (data, status, xhr) {
                alert("Unable to retrieve subscribers");
            });
        });

        $('#btnCancel').click(function (e) {
            showListPanel();
        });

        function showListPanel() {
            $('#Details')[0].style.display = 'none';
            $('#List')[0].style.displayl = 'block';
        }
    });

    //angular.module('app').controller(controllerId, [
    //    '$rootScope', '$scope', '$state', 'appSession', 'abp.services.app.member',
    //    function ($rootScope, $scope, $state, appSession, memberService) {
    //        var vm = this;
    //        //Layout logic...

    //        vm.members = [{}];
    //        vm.membersInput = {};
    //        vm.selectedMember = {};
    //        vm.subscriberSkills = [{}];

    //        //Get members list
    //        vm.getMembers = function () {
    //            document.getElementById('MemberArea').style.display = 'none';
    //            document.getElementById('LoadingArea').style.display = 'block';
    //            abp.ui.setBusy(
    //                $('#MemberArea'),
    //                memberService.getMembers(
    //                    vm.membersInput
    //                    ).success(function (data) {
    //                        vm.members = data.members

    //                        //If not admin get the logged in user details
    //                        if (appSession.user.userName != 'admin') {
    //                            document.getElementById('Details').style.display = 'block';
    //                            document.getElementById('List').style.display = 'none';

    //                            //TODO:  Need to take this out and call the Application Service layer for a Member
    //                            for (var count = 0; count < vm.members.length ; count++) {
    //                                if (vm.members[count].email === appSession.user.emailAddress) {
    //                                    $scope.showDetails(vm.members[count]);
    //                                    document.getElementById('btnCancel').style.display = 'none';
    //                                    document.getElementById('btnSave').style.display = 'block';
    //                                }
    //                            }
    //                        }
    //                        document.getElementById('MemberArea').style.display = 'block';
    //                        document.getElementById('LoadingArea').style.display = 'none';
    //                    }).error(function (msg) {
    //                        abp.notify.info("Members Not Loaded.");
    //                    })
    //                );
    //        }

    //        //Save Member
    //        vm.saveMember = function () {
    //            if (vm.selectedMember.id == undefined) {
    //                memberService.createMember(
    //                vm.selectedMember
    //                ).success(function () {
    //                    vm.getMembers();
    //                    $scope.showList();
    //                    abp.notify.info("Member added.");
    //                }).error(function (msg) {
    //                    abp.notify.info("Member was not added.");
    //                });
    //            }
    //            else {
    //                memberService.updateMember(
    //                    vm.selectedMember
    //                    ).success(function () {
    //                        vm.getMembers();
    //                        $scope.showList();
    //                        abp.notify.info("Member updated.");
    //                    }).error(function (msg) {
    //                        abp.notify.info("Member was not updated.");
    //                    });
    //            }
    //        }

    //        //Delete Members
    //        vm.deleteItem = function (type) {
    //            vm.selectedMember = type;
    //            vm.selectedMember.isDeleted = 1;
    //            vm.saveMember();
    //        }

    //        //Show Details panel
    //        $scope.showDetails = function (member) {
    //            vm.selectedMember = member;
    //            vm.subscriberInput = { companyId: chCompanyId, subscribersId: vm.selectedMember.id, subscribersName: vm.selectedMember.fullName };
    //            //Get selectedMembers subscriptions
    //            memberService.getMemberSubscriptions(
    //               vm.subscriberInput
    //               ).success(function (data) {
    //                   vm.subscriberSkills = data.subscriberSkills[0].value;
    //               }).error(function (msg) {
    //                   abp.notify.info("Member was not found.");
    //               });

    //            document.getElementById('Details').style.display = 'block';
    //            document.getElementById('List').style.display = 'none';

    //            if (appSession.user.userName != 'admin') {
    //                document.getElementById('btnSave').style.display = 'block';
    //            }
    //            else {
    //                document.getElementById('btnSave').style.display = 'none';
    //                document.getElementById('btnCancel').style.display = 'block';
    //            }
    //        }

    //        //Show List panel
    //        $scope.showList = function () {
    //            document.getElementById('Details').style.display = 'none';
    //            document.getElementById('List').style.display = 'block';
    //        }

    //        //Get list of Members
    //        vm.getMembers();
    //    }]);
})();