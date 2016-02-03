(function () {

    $(function () {
        $('#LoginButton').click(function (e) {
            e.preventDefault();
            abp.ui.setBusy(
                $('#LoginArea'),
                abp.ajax({
                    url: abp.appPath + 'Account/Login',
                    type: 'POST',
                    data: JSON.stringify({
                        tenancyName: $('#TenancyName').val(),
                        usernameOrEmailAddress: $('#EmailAddressInput').val(),
                        password: $('#PasswordInput').val(),
                        rememberMe: $('#RememberMeInput').is(':checked'),
                        returnUrlHash: $('#ReturnUrlHash').val()
                    })
                })
            );
        });

        $('a.social-login-link').click(function () {
            var $a = $(this);
            var $form = $a.closest('form');
            $form.find('input[name=provider]').val($a.attr('data-provider'));
            $form.submit();
        });

        $('#ReturnUrlHash').val(location.hash);

        $('#LoginForm input:first-child').focus();

        $('#StateValues').change(function (e) {
            e.preventDefault();
            var selectedState = "";
            selectedState = $('#StateValues').val();
            //$("#states-loading-progress").show();
            $("#City").html('');
            $("#City").append($('<option></option>').val("Loading cities...").html("Loading cities..."));
            abp.ajax({
                url: abp.appPath + 'Account/GetCities',
                type: 'GET',
                data: { "state": selectedState }
            })
            .done(function (data) {
                $("#City").html('');
                $.each(data.city, function (id, name) {
                    $("#City").append($('<option></option>').val(name.name).html(name.name));
                });
            })
            .fail(function (data, status, xhr) {
                alert('Error loading cities.');
            })
        });

        $('#SubmitRequest').click(function (e) {
            e.preventDefault();
            var inquiry = {
                companyId: chCompanyId,
                queryRequest: [{ key: 'Type Of Roof', value: 'Shingle' }]
            };

            abp.ajax({
                type: 'POST',
                url: "http://CreditsHero.azurewebsites.net/api/services/cd/Inquiry/MakeInquiry",
                data: JSON.stringify(inquiry)
            })
            .done(function (data, status, xhr) {
                alert(bar);
            })
            .fail(function (data, status, xhr) {
                alert("Unable to retrieve subscribers");
            });
        });

        function callbackFunc(resultData) {
            alert(resultData);
        }
    })
})();