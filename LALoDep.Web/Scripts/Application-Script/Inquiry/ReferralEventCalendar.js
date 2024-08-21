var page = (function () {
    return {
        $form: $('#ref-search-form'),
        $table: $('#tblReferralList'),
        mainKey: 'ref-search-form' + GetWindowID(),
        resultPageIdKey: GetWindowID() + 'refsearch-form-page#',
        rowSelectedKey: GetWindowID() + 'refsearch-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();
function SavePageState() {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData };
    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
    enableLoadDataFromCache();
};
function ResetPageState() {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
};
function LoadPreviousPageState() {
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData) {
    /*Load Form inputs*/
        var formData = pageState.formData;
        $('*', page.$form).filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')])
                $this.val(formData[$this.attr('id')]);
            if ($this.attr('id') == "ReferralTypeID" && $this.val().length !== 0) {
                var codeID = $this.val();
                appearingStaffAttyDrpLst(codeID);
                eventTypeAndLocationDrpLst(codeID);
            }
        });
        if ($("#StartDate").val().length !== 0 && $("#EndDate").val().length !== 0) {
            $("#daterangepicker").val($("#StartDate").val() + " - " + $("#EndDate").val())
        }
        $('#btnSearch').trigger("click");
    }
};
function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    calc_height = $(window).height();
    var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divSearchResult .table-responsive').height();
    var extra = 0;
    if ($('#slideout').length > 0) {
        extra = 100;
    }
    $("#divSearchResult .table-responsive").children().first().parentsUntil("body").each(function () {

        $(this).siblings().each(function () {
            if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                //console.log(calc_height + " - " + $(this).outerHeight(true));
                if ($(this).attr("id") == 'loading')
                    return;
                calc_height = calc_height - $(this).outerHeight(true);
            }
        });
        _offset = _offset + $(this).outerHeight(true) - $(this).height();
    });
    calc_height = calc_height - _offset;
    calc_height = calc_height < 250 ? 250 : calc_height;
    $('#divSearchResult .table-responsive').css('max-height', calc_height + extra + 'px');

    return calc_height;
}
function appearingStaffAttyDrpLst(codeID) {
    var pageState = simpleStorage.get(page.mainKey);
    $('#AppearingPersonID').empty();
    if (codeID > 0) {
        $('#AppearingPersonID').append($("<option></option>").attr("value", "").text(""));
        $.each(appearingStaffAttyList.filter(m => m.ReferralTypeCodeID == codeID), function (key, element) {
            if (element.PersonID == $("#PersonID").val() && !(pageState && pageState.formData)) {
                $('#AppearingPersonID').append($("<option selected></option>").attr("value", element.PersonID).text(element.DisplayName));
            }
            else {
                $('#AppearingPersonID').append($("<option></option>").attr("value", element.PersonID).text(element.DisplayName));
            }
        });
    }
}
function eventTypeAndLocationDrpLst(codeID) {
    $('#EventTypeID').empty();
    $('#EventLocationID').empty();
    if (codeID > 0) {
        $('#EventTypeID').append($("<option></option>").attr("value", "").text(""));
        $('#EventLocationID').append($("<option></option>").attr("value", "").text(""));
        $.each(eventTypeList.filter(m => m.ReferralTypeCodeID == codeID), function (key, element) {
            $('#EventTypeID').append($("<option></option>").attr("value", element.CodeID).text(element.CodeDisplay));
        });
        $.each(eventLocationList.filter(m => m.ReferralTypeCodeID == codeID), function (key, element) {
            $('#EventLocationID').append($("<option></option>").attr("value", element.CodeID).text(element.CodeDisplay));
        });
    }
}
$("#ReferralTypeID").on("change", function () {
    var codeID = $(this).val();
    appearingStaffAttyDrpLst(codeID);
    eventTypeAndLocationDrpLst(codeID);
});
$('.daterange').daterangepicker({ autoClose: true }, function (start, end) {
    $('#StartDate').val(start.format('MM/DD/YYYY'));
    $('#EndDate').val(end.format('MM/DD/YYYY'));
});
$('#btnSearch').on('click', function (e) {
    e.preventDefault();
    IPadKeyboardFix();
    SavePageState();

    var $form = $('#ref-search-form');
    $.ajax({
        type: "POST", url: '/Inquiry/ReferralEventCalendar', data: $form.serialize(), success: function (data) {
            $('#referralEventData').html(data);
            $("#totalResults").html("Search Results (" + $("#tblReferralList tbody .ref-row-item").length + ")");
            fitCalculatedHeightForSearchDataTable();
        }
    });
});
$(document).on("click", ".link-ref-edit", function () {
    var data = $(this).data();
    $.ajax({
        type: "POST", url: '/Inquiry/UpdateReferralCase', data: { refID: data.refid, caseID: data.id, evtID : data.evtid }, success: function (data) {
            window.location.href = data.URL;
        }
    });
})
$(document).ready(function () {
    enableLoadDataFromCache();
    if ($loadFromCache && isLoadDataFromCache() && simpleStorage.get(page.mainKey)) {
        LoadPreviousPageState();
    }
    else {
        ResetPageState();
        $('#btnSearch').trigger("click");
    }
})