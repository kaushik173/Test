var page = (function () {
    return {
        $form: $('#recordTime-form'),
        $table: $('#searchCase'),
        mainKey: 'recordTime-form' + GetWindowID(),
        resultPageIdKey: GetWindowID() + 'recordTime-form-page#',
        rowSelectedKey: GetWindowID() + 'recordTime-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();


var ResetPageState = function () {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
};

//var SaveFormState = function () {
//    var formData = {};
//    page.$form.serializeArray().map(function (item) {
//        formData[item.name] = item.value;
//    });
//    var pageState = { formData: formData };
//    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
//};

var SaveFormState = function () {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData  };
    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
};
var ClearCaseSearchGuidID = function () {
    simpleStorage.deleteKey(GetWindowID() + '_SearchGuidID');
    simpleStorage.deleteKey(page.resultPageIdKey);
};


var LoadPreviousFormState = function () {
    var flag = false;
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData) {
        /*Load Form inputs*/
        var formData = pageState.formData;
        page.$form.find('*').filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')]) {
                $this.val(formData[$this.attr('id')]);
                flag = true;

            }
        });
    }

    formDataPased = $('#case-search-form').serialize();
    return flag;
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

    //console.log("calc :" + calc_height + " offset: " + _offset);
    calc_height = calc_height - _offset;
    //console.log("total: " + calc_height);
    $('#divSearchResult .table-responsive').css('max-height', calc_height + extra + 'px');

    return calc_height;
}

$('#search').on('click', function (e) {
    e.preventDefault();
    IPadKeyboardFix();
    ClearCaseSearchGuidID();
    if ($("#StartDate").val() == "") {
        notifyDanger('Start Date is required.');
        $("#StartDate").focus();
        return false;
    } else if ($("#EndDate").val() == "") {
        notifyDanger('End Date is required.');
        $("#StartDate").focus();
        return false;
    }
    else if (moment($("#StartDate").val()) > moment($("#EndDate").val())) {
        notifyDanger('End Date can not be before Start Date.');
        $("#WorkEndDate").focus();
        return false;
    }

    var $form = $('#recordTime-form');
    SaveFormState();
    $.ajax({
        type: "POST", url: '/Task/RecordTimeNonCaseList', data: $form.serialize(), success: function (data) {
            $('#recordTimeData').html(data);
            fitCalculatedHeightForSearchDataTable();
        }
    });
});

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

$('#addNew').on('click', function (e) {
    
        window.location.href = "/Task/RecordTimeNonCaseAdd";
   

});

$("#btnPrint").on("click", function () {
    var printWorks = $(".chkPrint:checked");
    if (printWorks.length <= 0) {
        notifyDanger('No reccord time has been selected.');
        return;
    }


    var workids = [];
    for (var indx = 0; indx < printWorks.length; indx++) {
        var chk = printWorks.eq(indx);
        workids.push(chk.attr("id"));
    }


    var data = { id: workids.join(',') };
   $.download($('#hdnCurrentSessionGuidPath').val()+'/Case/RecordTimeListPrint', data, "POST", "target='_blank'");

});

$(document).on("click", ".chkPrint", function () {
    $("#chkAllPrint").prop("checked", $(".chkPrint").length == $(".chkPrint:checked").length);
});

$(document).on("click", "#chkAllPrint", function () {
    var chkPrint = $(".chkPrint");
    for (var indx = 0; indx < chkPrint.length; indx++) {
        var chk = chkPrint.eq(indx);
        chk.prop("checked", $(this).is(":checked"));
    }
});

$(document).ready(function () {
    LoadPreviousFormState();
    $('#search').trigger('click');
});
//$('body').on('click', '.deleteRecord', function () {
//    var id = $(this).attr('data-id');
//    var tr = $(this).parent().parent();
//    confirmBox("Are you sure you want to remove selected records?", function (result) {
//        if (result) {
//            $.ajax({
//                type: "POST", url: '/Case/RecordTimeDelete/' + id,
//                dataType: "json",
//                success: function (data) {
//                    tr.remove();
//                    Notify('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
//                },
//                error: function (XMLHttpRequest, textStatus, errorThrown) {
//                }
//            });
//        }
//    });
//});