(function () {

    $(function() {
        $('#FinishedButton').click(function (e) {
            e.preventDefault();
            var criteriaValues = null;
            criteriaValues = [];

            $('input:checkbox:checked').each(function () {
                var criteriaId = $(this).attr('value')
                var subscriberId = $('#subscriberId').attr('value');
                var createSubscribersValuesInput = { subscribersId: subscriberId, criteriaValuesRefId: criteriaId };
                criteriaValues.push(createSubscribersValuesInput);
            });

            abp.ajax({
                type: 'POST',
                //url: "http://localhost:6234/api/services/cd/Subscriber/AddSubscribersValue",
                //url: "http://CreditsHero.azurewebsites.net/api/services/cd/Subscriber/AddSubscribersValue",
                url: abp.appPath + 'Register/SubscribeCreditsHeroValues',
                data: JSON.stringify(criteriaValues)
            })
                .done(function (data, status, xhr) {

                })
                .fail(function (data, status, xhr) {
                    alert('Unable to set preference.');
                });
        });
    });
})();