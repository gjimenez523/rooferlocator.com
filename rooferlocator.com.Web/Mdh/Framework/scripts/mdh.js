var mdh = mdh || {};
var mdhUrlPrefix = 'http://mydh.azurewebsites.net'; //'http://localhost:6634';
var mdhSignalRUrl = 'http://mydh.azurewebsites.net'; //'http://localhost:6634'; 
var loginUrl = 'http://localhost:6234/';
var loginResponseView = '', loginResponseController = '';
var mdhUser = mdhUser || {};

(function () {
    var $$mdhServiceFactory = ['$q', '$rootScope', function ($q, $rootScope) {
        var results = { data: null };

        function _callMe() {
            var d = $q.defer();
            setTimeout(function () {
                d.resolve();
                //defer.reject();
            }, 100);
            return d.promise;
        }

        function createCORSRequest(method, url) {
            var xhr = new XMLHttpRequest();
            if ("withCredentials" in xhr) {
                // XHR for Chrome/Firefox/Opera/Safari.
                xhr.open(method, url, true);
            } else if (typeof XDomainRequest != "undefined") {
                // XDomainRequest for IE.
                xhr = new XDomainRequest();
                xhr.open(method, url);
            } else {
                // CORS not supported.
                xhr = null;
            }
            return xhr;
        }

        function makeCorsRequest(url, postData) {
            var d = $q.defer();

            var xhr = createCORSRequest('POST', url);
            if (!xhr) {
                alert('CORS not supported');
                return;
            }

            // Response handlers.
            xhr.onload = function () {
                results = xhr.responseText; //instead if responsetext use response which will return your list.
                d.resolve(results);
                //return results;
            };

            xhr.onerror = function () {
                reject(Error("There was an error making the request."));
            };

            if (postData == undefined) {
                xhr.send();
            } else {
                xhr.overrideMimeType('application/json');
                xhr.responseType = "text";
                xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
                xhr.setRequestHeader("Authorization", $rootScope.authBearerToken)
                xhr.send(postData);
            }

            return d.promise;
        };

        return {
            callMe: _callMe,
            getTenantTotals: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/tenantReport/GetTenantTotals', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            getBuildingUnits: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/company/getCompanies', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            getApartmentUnit: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/company/getCompanyPackages', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            getTenantRoles: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/role/getRolesByTenant', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            getPackages: function () {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/package/getPackages')
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            getUser: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/user/getUser', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            updateRole: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/role/updateRole', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            addCompanyPackage: function (postData) {
                var d = $q.defer();
                makeCorsRequest(mdhUrlPrefix + '/api/services/app/package/addCompanyPackage', postData)
                .then(function (data) {
                    results = data;
                    results = JSON.parse(data).result;
                    d.resolve(results);
                })
                return d.promise;
            },
            login: function (postData, $rootScope) {
                var d = $q.defer();
                loginResponseView = JSON.parse(postData).view;
                loginResponseController = JSON.parse(postData).controller;
                makeCorsRequest(mdhUrlPrefix + '/api/account/authenticate', postData)
                .then(function (data) {
                    results = JSON.parse(data);
                    mdhUser = results.result;
                    if ($rootScope == undefined) {
                        if (results.success == true) {
                            //**IMPORTANT**Set the mdhUserId at the rootScope level
                            location.href = '/' + loginResponseController + '/' + loginResponseView + '?auth=' + results.result.ticket + '&uid=' + results.result.user.id;//results.targetUrl;

                            //START:MyDesignHeroHub SignalR
                            var mdhConnection = $.connection(mdhSignalRUrl);

                            $.connection.hub.url = mdhSignalRUrl + '/signalr';

                            var mdhHub = $.connection.myDesignHeroHub; //get a reference to the hub

                            //Register to get notifications
                            mdhHub.client.getNotification = function (notification) {
                                abp.event.trigger('abp.notifications.received', notification);
                            };

                            //Connect to the server
                            //abp.signalr.connect = function () {
                            $.connection.hub.start().done(function () {
                                abp.log.debug('Connected to MyDesignHeroes SignalR server!'); //TODO: Remove log
                                abp.event.trigger('abp.signalr.connected');
                                mdhHub.server.register().done(function () {
                                    abp.log.debug('Registered to the MyDesignHeroes SignalR server!'); //TODO: Remove log
                                });
                            });
                            //};

                            //if (abp.signalr.autoConnect === undefined) {
                            //    abp.signalr.autoConnect = true;
                            //}

                            //if (abp.signalr.autoConnect) {
                            //    abp.signalr.connect();
                            //}

                            mdhHub.client.getMessage = function (message) { //register for incoming messages
                                //$rootScope.MdhMessage = message;
                                console.log('MDH (GLOBAL): ' + message);
                            };
                            //END:MyDesignHeroHub SignalR

                        } else
                            d.resolve(results);
                    }
                    else {
                        $rootScope.authBearerToken = results.result.ticket;
                        if (results.success == true) {
                            //**IMPORTANT**Set the mdhUserId at the rootScope level
                            $rootScope.mdhUserId = results.result.user.id;
                            location.href = '/Home/DashboardIndex?auth=' + $rootScope.authBearerToken + '&uid=' + $rootScope.mdhUserId;//results.targetUrl;
                        } else
                            d.resolve(results);
                    }
                })
                return d.promise;
            },
            getResults: function () {
                //return settings so I can keep updating assessment and the
                //reference to settings will stay in tact
                return results;
            },
            updateResults: function () {
                $http.get('/assessment/api/get/' + scan.assessmentId).success(function (response) {
                    //I don't have to return a thing.  I just set the object.
                    results.data = response;
                });
            }
        };
    }];

    //MyDesignHeroes Service
    angular
        .module('mdh', [function () {
        }])
        .config(function ($locationProvider) {
            //$locationProvider.html5Mode(true).hashPrefix('!');
        })
        .factory('$$mdhServiceFactory', $$mdhServiceFactory)
        .controller('mdh', ['$$mdhServiceFactory', '$q', '$scope', '$rootScope', '$location',
            function ($$mdhServiceFactory, $q, $scope, $rootScope, $location) {
                $scope.tenantSummary;
                $scope.buildingUnits;
                $scope.role;
                $scope.tenantRoles;
                $scope.mdhUserId;
                $scope.companyPackages;

                //Used to redirect user to login
                if (window.location.pathname.toLowerCase().indexOf('login') < 0) {
                    if (mdhUser.ticket == undefined) {
                        if (window.location.pathname != '/' && $rootScope.authBearerToken == undefined) {
                            var params = window.location.href.slice(window.location.href.indexOf('?') + 1);
                            var params2 = params.split('&');
                            qsAuthBearer = undefined;
                            qsUserId = undefined;
                            for (i = 0; i < params2.length; i++) {
                                var keyVal = params2[i].split('=');
                                if (keyVal[0] == 'auth') qsAuthBearer = keyVal[1];
                                if (keyVal[0] == 'uId') qsUserId = keyVal[1];
                            }
                            if (qsAuthBearer == undefined) {
                                if (abp.session.userId == undefined)
                                    location.href = '/';
                            }
                            else if (qsAuthBearer[1] == undefined) location.href = '/';

                            $rootScope.authBearerToken = qsAuthBearer;
                            $rootScope.mdhUserId = qsUserId;
                        }
                    }
                }
            }])
        .directive('mdhTenantTotals', ['$$mdhServiceFactory', '$sce', '$stateParams', function ($$mdhServiceFactory, $sce, $stateParams) {
            //Display Tenant Totals
            return {
                restrict: 'A',
                controller: function ($scope) {
                    $$mdhServiceFactory.getTenantTotals(JSON.stringify({ tenantId: abp.session.tenantId }))
                    .then(function (data) {
                        $scope.tenantSummary = data;
                    });
                },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/mdh_tenantTotals.html')
                }
            };
        }])
        .directive('mdhTenantRoles', ['$$mdhServiceFactory', '$sce', '$stateParams', function ($$mdhServiceFactory, $sce, $stateParams) {
            //Display Tenant Roles Summary
            return {
                restrict: 'A',
                //LINK:  Creates/Updates Role for a Tenant
                link: function (scope, element, attr) {
                    element.on('click', function (event) {
                        if (event.target.id == 'btnSaveRole') {
                            //Disable form button
                            $('#' + event.target.id)[0].innerText = 'Saving role...';
                            $('#' + event.target.id)[0].disabled = true;

                            //Update Role
                            $$mdhServiceFactory.updateRole(JSON.stringify({
                                roleId: 0,
                                name: $('#txtRoleName')[0].value,
                                displayName: $('#txtRoleName')[0].value,
                                tenantId: $stateParams.tenantId,
                                isStatic: $('#isStatic')[0].value,
                                isDefault: $('#isDefault')[0].value,
                                isDeleted: 0
                            })).then(function (data) {
                                $('#' + event.target.id)[0].innerText = 'Role Saved';
                            });
                        }
                    });
                },
                controller: function ($scope) {
                    $$mdhServiceFactory.getTenantRoles(JSON.stringify({ tenantId: $stateParams.tenantId }))
                    .then(function (data) {
                        $scope.tenantRoles = data;
                        $scope.tenantRoles.tenantId = $stateParams.tenantId;
                    });
                },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/mdh_tenantRoles.html')
                }
            };
        }])
        .directive('mdhTenantUsers', ['$$mdhServiceFactory', '$sce', '$stateParams', function ($$mdhServiceFactory, $sce, $stateParams) {
            //Display Tenant Users Summary
            return {
                restrict: 'A',
                //LINK:  Creates/Updates Role for a Tenant
                link: function (scope, element, attr) {
                    element.on('click', function (event) {
                        if (event.target.id == 'btnSaveUser') {
                            //Disable form button
                            $('#' + event.target.id)[0].innerText = 'Saving user...';
                            $('#' + event.target.id)[0].disabled = true;

                            //Update Role
                            $$mdhServiceFactory.updateRole(JSON.stringify({
                                roleId: 0,
                                name: $('#txtUserName')[0].value,
                                displayName: $('#txtUserName')[0].value,
                                tenantId: $stateParams.tenantId,
                                isStatic: $('#isStatic')[0].value,
                                isDefault: $('#isDefault')[0].value,
                                isDeleted: 0
                            })).then(function (data) {
                                $('#' + event.target.id)[0].innerText = 'User Saved';
                            });
                        }
                    });
                },
                controller: function ($scope) {
                    //TODO:  Implement getTenantUsers
                    //$$mdhServiceFactory.getTenantRoles(JSON.stringify({ tenantId: $stateParams.tenantId }))
                    //.then(function (data) {
                    //    $scope.tenantRoles = data;
                    //    $scope.tenantRoles.tenantId = $stateParams.tenantId;
                    //});
                },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/mdh_tenantUsers.html')
                }
            };
        }])
        .directive('mdhBuildingUnits', ['$$mdhServiceFactory', '$sce', '$stateParams', function ($$mdhServiceFactory, $sce, $stateParams) {
            //Display Building Units (aka Companies)
            return {
                restrict: 'A',
                //scope: { tenants: data.result.tenants },
                controller: function ($scope) {
                    $$mdhServiceFactory.getBuildingUnits(JSON.stringify({ companyId: undefined, tenantId: $stateParams.tenantId }))
                    .then(function (data) {
                        $scope.buildingUnits = data;
                    });
                },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/mdh_buildingUnits.html')
                }
                //template: '<ul>' + '<li ng-repeat="item in tenants" >' + '{{item.tenancyName}}' + '</li>' + '</ul>',
            };
        }])
        .directive('mdhApartmentUnits', ['$$mdhServiceFactory', '$sce', '$stateParams', function ($$mdhServiceFactory, $sce, $stateParams) {
            //Display Building Units (aka Companies)
            return {
                restrict: 'A',
                scope: {
                    apikey: '@apiKey',
                    tenant: '@tenant'
                },
                link: function (scope, element, attr) {
                    element.on('click', function (event) {
                        if (event.target.id == 'btnSaveProduct') {
                            //Disable form button
                            $('#' + event.target.id)[0].innerText = 'Adding product...';
                            $('#' + event.target.id)[0].disabled = true;

                            //Update Role
                            $$mdhServiceFactory.addCompanyPackage(JSON.stringify({
                                companyId: $stateParams.companyId,
                                packageId: $('#selSaveProduct')[0].value
                            })).then(function (data) {
                                $('#' + event.target.id)[0].innerText = 'Product Added';

                                //Get the apartment unit
                                $$mdhServiceFactory.getApartmentUnit(JSON.stringify({ companyId: $stateParams.companyId }))
                                .then(function (data) {
                                    scope.companyPackages = data;
                                });
                            });
                        }
                    });
                },
                controller: function ($scope, $element, $attrs) {
                    $scope.apikey = $stateParams.companyId;
                    //Get the apartment unit
                    $$mdhServiceFactory.getApartmentUnit(JSON.stringify({ companyId: $stateParams.companyId }))
                    .then(function (data) {
                        $scope.companyPackages = data;
                    });

                    //Get products list
                    $$mdhServiceFactory.getPackages()
                    .then(function (data) {
                        $scope.packages = data.packages;
                    });
                },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/mdh_apartmentunits.html')
                }
                //template: '<ul>' + '<li ng-repeat="item in tenants" >' + '{{item.tenancyName}}' + '</li>' + '</ul>',
            };
        }])
        .directive('mdhRegisterUser', ['$sce', function ($sce) {
            return {
                restrict: 'A',
                scope: { building: '=' },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/account/register?tenantName=' + attr.building
                        + '&userRole=' + attr.userrole
                        + '&returnUrl=' + attr.returnurl
                        + '&authority=' + attr.authority
                        + '&returnView=' + attr.returnview
                        + '&returnController=' + attr.returncontroller)
                }
            };
        }])
        .directive('mdhLoginUser', ['$$mdhServiceFactory', '$sce', function ($$mdhServiceFactory, $sce) {
            return {
                restrict: 'A',
                //LINK:  Login User
                link: function (scope, element, attr) {
                    element.on('click', function (event) {
                        if (event.target.id == 'LoginButton') {
                            //Disable form button
                            $('#' + event.target.id)[0].innerText = 'Logging in...';
                            $('#' + event.target.id)[0].disabled = true;

                            //Login User
                            $$mdhServiceFactory.login(JSON.stringify({
                                tenancyName: $('#TenancyName').val(),
                                userName: $('#EmailAddressInput').val(),
                                password: $('#PasswordInput').val(),
                                usernameOrEmailAddress: $('#EmailAddressInput').val(),
                                controller: attr.returncontroller,
                                view: attr.returnview,
                                //rememberMe: $('#RememberMeInput').is(':checked'),
                                //returnUrlHash: $('#ReturnUrlHash').val(),
                                //returnView: $('#returnView').val(),
                                //returnController: $('#returnController').val(),
                                //returnUrl: $('#returnUrl').val()
                            })).then(function (data) {
                                if (data.success == true) {
                                    $('#' + event.target.id)[0].innerText = 'Login Succeed!';
                                }
                                else {
                                    $('#returnMessage')[0].style.display = 'block';
                                    $('#returnMessage')[0].innerText = data.error.details;
                                    $('#' + event.target.id)[0].disabled = false;
                                    $('#' + event.target.id)[0].innerText = 'Log in';
                                }
                            });
                        }
                    });
                },
                scope: { building: '=' },
                templateUrl: function (elem, attr) {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/account/login?tenantName=' + attr.building
                        + '&userRole=' + attr.userrole
                        + '&returnUrl=' + attr.returnurl
                        + '&authority=' + attr.authority
                        + '&returnView=' + attr.returnview
                        + '&returnController=' + attr.returncontroller
                        + '&showRegisterButton=' + attr.showregisterbutton)
                }
            };
        }])
    .directive('mdhUserDefaultView', ['$$mdhServiceFactory', '$sce', '$stateParams', function ($$mdhServiceFactory, $sce, $stateParams) {
        //Display User 
        return {
            restrict: 'A',
            //LINK:  Creates/Updates Role for a Tenant
            link: function (scope, element, attr) {
                element.on('click', function (event) {
                    if (event.target.id == 'btnSaveUser') {
                        //Disable form button
                        $('#' + event.target.id)[0].innerText = 'Saving user...';
                        $('#' + event.target.id)[0].disabled = true;

                        //Update Role
                        $$mdhServiceFactory.updateRole(JSON.stringify({
                            roleId: 0,
                            name: $('#txtUserName')[0].value,
                            displayName: $('#txtUserName')[0].value,
                            tenantId: $stateParams.tenantId,
                            isStatic: $('#isStatic')[0].value,
                            isDefault: $('#isDefault')[0].value,
                            isDeleted: 0
                        })).then(function (data) {
                            $('#' + event.target.id)[0].innerText = 'User Saved';
                        });
                    }
                });
            },
            controller: function ($scope, $rootScope) {
                //TODO:  Implement getUser
                $$mdhServiceFactory.getUser(JSON.stringify({ userId: $rootScope.mdhUserId }))
                .then(function (data) {
                    $scope.user = data;
                });
            },
            templateUrl: function (elem, attr) {
                if (attr.controlstyle != '') {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/' + attr.controlstyle + '/mdh_userDefaultView.html')
                } else {
                    return $sce.trustAsResourceUrl('http://' + attr.authority + '/Mdh/Framework/pages/mdh_userDefaultView.html')
                }
            }
        };
    }]);
})();