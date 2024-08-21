$.fn.initAddressAutoComplete = function (onSelect) {
    if (!$(this).data('uiAutocomplete')) {
        initAddressAutoComplete(this, onSelect);
    }
};

$.fn.removeAddressAutoComplete = function () {
    if ($(this).data('uiAutocomplete')) {
        $(this).autocomplete("destroy");
    }
};

function initAddressAutoComplete(element, onSelect) {
    $(element).autocomplete({
        source: function (request, response) {
            $.ajax({                
                url: 'https://autocomplete.geocoder.ls.hereapi.com/6.2/suggest.json',
                dataType: "json",
                data: {
                    query: request.term,
                    app_id: addressLookupAppId,                    
                    apiKey: addressLookupApiKey,
                    country: 'USA',
                    maxresults: 10
                },
                success: function (data) {
                    var result = $.map(data.suggestions, function (obj) {
                        return {
                            value: ((obj.address.houseNumber ? obj.address.houseNumber : "") + " " + obj.address.street).trim(),
                            label: obj.label.split(', ').reverse().join(', '), data: obj
                        };
                    });
                    response(result);
                }
            });
        },
        minLength: 3,
        select: function (event, ui) {
            var address = ui.item.data.address;
            onSelect(address);
        }
    });
}