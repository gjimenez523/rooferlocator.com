(function () {
    $(function () {
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

    });
})();