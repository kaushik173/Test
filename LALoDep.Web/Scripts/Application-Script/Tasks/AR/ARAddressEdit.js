function isValidPhone(str) {
    var plainNumber = str.replace(/[^0-9]/g, "");
    return plainNumber.length == 10;

}

$(function () {

    var stateCode = $('#hdn_StateCodeID').val();
    var countryCode = $('#hdn_CountryCodeID').val();

    $('#CountryCodeID option[value="' + countryCode + '"]').attr("selected", true);
    $('#StateCodeID option[value="' + stateCode + '"]').attr("selected", true);

    $('#StateCodeID').on('change', function () {
        //     AddressAutoComplete();
        $('#StateCodeID').val($(this).val());
    });
    $('#CountryCodeID').on('change', function () {
        //     AddressAutoComplete();
        $('#CountryCodeID').val($(this).val());
    });



    $('#btnSaveAndContinue').on('click', function () {

        SaveData(1);
    });
    setInitialFormValues('case-addresses-form', true);

});

function GetData() {

    var countryID = parseInt($("#CountryCodeID option:selected").val());
    var stateID = parseInt($("#StateCodeID option:selected").val());
    $('#CountryID').val(countryID);
    $('#StateID').val(stateID);
    var fData = $('#case-addresses-form').serialize();

    return fData;

}

function SaveData(buttonId) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged('case-addresses-form')) {
        document.location.href = $('#simplewizard ul li.active a').attr('href');

        return;
    }

    if ($('#Street').val().length == 0) {

        Notify('Street is required!', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        return;
    }
    if ($('#City').val().length == 0) {

        Notify('City is required!', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        return;
    }
    if ($('#ZipCode').val().length == 0) {

        Notify('ZipCode is required!', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        return;
    }

    if ($("#CanText").is(":checked") && $("#HomePhone").val().length == 0) {
        Notify('Home phone is required when Can Text is checked!', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        return;
    }
    if ($("#CanText").is(":checked") && !isValidPhone($("#HomePhone").val())) {
        Notify('Invalid Home Phone!, it must contain 10 digits.', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        return;
    }


    /* End Validations */
    var data = GetData();
    $.ajax({
        type: "POST",
        url: '/CaseOpening/CaseAddressEdit',
        data: data,
        dataType: 'json',
        success: function (data) {
            RequestSubmitted();
            debugger;
            if (data.Status == 'Done') {
                document.location.href = $('#slidecontent .wizardstep a.list-group-item.active').attr("href");
            } else {
                document.location.href = data.URL;
            }
        }
    });

}


function AddressAutoComplete() {
    var states = "";
    $('#StateCodeID  option').each(function () {
        if (states != "")
            states += ',';
        states += $(this).text();
    });

    var options = {
        paramName: 'prefix',
        params: { "state_filter": states, "auth-id": $authid, "auth-token": $authtoken },
        serviceUrl: 'https://autocomplete-api.smartystreets.com/suggest',
        minChars: 2,
        dataType: "json",
        transformResult: function (response) {
            if (response.suggestions === null) {
                response.suggestions = [];
            }
            return {
                suggestions: $.map(response.suggestions, function (dataItem) {
                    return { value: dataItem.text, data: { "Street": dataItem.street_line, "City": dataItem.city, "State": dataItem.state } };
                })
            };
        },
        onSelect: function (suggestion) {
            $('#loading').hide();
            $('#City').val(suggestion.data.City);
            $('#Street').val(suggestion.data.Street);
            $("#StateCodeID option").filter(function (index) { return $(this).text() === suggestion.data.State; }).attr('selected', 'selected');
            var url = $httpOrHttps + "maps.googleapis.com/maps/api/geocode/json?address=" + suggestion.data.Street + ', ' + suggestion.data.City + ', ' + suggestion.data.State;


            $.getJSON(url, function (data) {


                for (var i in data.results) {
                    for (var x in data.results[i].address_components) {
                        if (data.results[i].address_components[x].types == 'postal_code') {
                            $('#ZipCode').val(data.results[i].address_components[x].short_name);

                        }
                    }
                }



            });
        }
    };
    $('#Street').autocomplete(options);

}