var $BaseURL = '/';
var $emptyName = null;
var origin_wrapper_height = 0, origin_content_height = 0;

function GetData() {
    var data = {
        ContactInfoAddList: [],
        ContactInfoList: []
    };

    var $contactInfoTr = $("#contactInfoList tbody tr");
    for (var indx = 0; indx < $contactInfoTr.length; indx++) {
        $tr = $contactInfoTr.eq(indx);
        $PersonContactType = $tr.find(".ContactTypeListEdit");
        $PersonAddress = $tr.find(".PersonAddressEdit");
        if ($tr.data("personcontacttypecodeid") != parseInt($PersonContactType.val()) || $tr.data("personcontactinfo") != $PersonAddress.val()) {
            var contactInfoEditList = {
                PersonContactID: $tr.data("personcontactid"),
                AgencyID: $tr.data("agencyid"),
                PersonID: $tr.data("personid"),
                PersonContactTypeCodeID: $PersonContactType.val(),
                PersonContactInfo: $PersonAddress.val(),
                RecordStateID: $tr.data("recordstateid"),
            };
            data.ContactInfoList.push(contactInfoEditList);
        }
    }

    for (var indx = 0; indx < $('#all-names .name').length ; indx++) {
        var $nameRow = $('#all-names .name').eq(indx);
        if ($nameRow.find(".PersonID").val() != '' && parseInt($nameRow.find(".PersonContactTypeCodeID").val()) != '' && $nameRow.find(".PersonContactInfo").val() != '') {
            var contactInfoAddList = {
                PersonID: $nameRow.find(".PersonID").val(),
                PersonContactTypeCodeID: $nameRow.find(".PersonContactTypeCodeID").val(),
                PersonContactInfo: $nameRow.find(".PersonContactInfo").val(),
            }
            data.ContactInfoAddList.push(contactInfoAddList);
        }

    }

    return data;
}

function Validate() {
    var validate = true;

    var $contactInfoTr = $("#contactInfoList tbody tr");
    for (var indx = 0; indx < $contactInfoTr.length; indx++) {
        $tr = $contactInfoTr.eq(indx);
        $PersonContactType = $tr.find(".ContactTypeListEdit");
        $PersonAddress = $tr.find(".PersonAddressEdit");
        if ($PersonContactType.val() != '' && $PersonAddress.val() == '') {
            $PersonAddress.focus();
            notifyDanger('Address/Number is required.');
            validate = false;
            break;
        }

        if ($PersonAddress.val() != '' && $PersonContactType.val() == '') {
            $PersonContactType.focus();
            notifyDanger('Type is required.');
            validate = false;
            break;
        }

        if ($PersonContactType.val() != '' && $PersonAddress.val() != '') {
            if ($PersonContactType.val() == "22794" || $PersonContactType.val() == "2007" || $PersonContactType.val() == "2801")
                if (!validateEmail($PersonAddress.val())) {
                    $PersonAddress.focus();
                    notifyDanger('Invalid Email.', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
                    validate = false;
                    break;
                }
        }

    }

    for (var indx = 0; indx < $('#all-names .name').length ; indx++) {
        var $nameRow = $('#all-names .name').eq(indx);

        if ($nameRow.find(".PersonID").val() != '' && $nameRow.find(".PersonContactTypeCodeID").val() == '') {
            $nameRow.find(".PersonContactTypeCodeID").focus();
            notifyDanger('Type is required.');
            validate = false;
            break;
        }
        else if ($nameRow.find(".PersonID").val() != '' && $nameRow.find(".PersonContactInfo").val() == '') {
            $nameRow.find(".PersonContactInfo").focus();
            notifyDanger('Address/Number is required.');
            validate = false;
            break;
        }
        else if ($nameRow.find(".PersonContactTypeCodeID").val() != '' && $nameRow.find(".PersonID").val() == '') {
            $nameRow.find(".PersonContactInfo").focus();
            notifyDanger('Person is required.');
            validate = false;
            break;
        }
        else if ($nameRow.find(".PersonContactTypeCodeID").val() != '' && $nameRow.find(".PersonContactInfo").val() == '') {
            $nameRow.find(".PersonContactInfo").focus();
            notifyDanger('Address/Number is required.');
            validate = false;
            break;
        }
        else if ($nameRow.find(".PersonContactInfo").val() != '' && $nameRow.find(".PersonID").val() == '') {
            $nameRow.find(".PersonID").focus();
            notifyDanger('Person is required.');
            validate = false;
            break;
        }
        else if ($nameRow.find(".PersonContactInfo").val() != '' && $nameRow.find(".PersonContactTypeCodeID").val() == '') {
            $nameRow.find(".PersonContactTypeCodeID").focus();
            notifyDanger('Type is required.');
            validate = false;
            break;
        }
        else if ($nameRow.find(".PersonContactInfo").val() != '' && $nameRow.find(".PersonContactTypeCodeID").val() != '' && $nameRow.find(".PersonID").val() != '') {
            if ($nameRow.find(".PersonContactTypeCodeID").val() == "22794" || $nameRow.find(".PersonContactTypeCodeID").val() == "2007" || $nameRow.find(".PersonContactTypeCodeID").val() == "2801")
                if (!validateEmail($nameRow.find(".PersonContactInfo").val())) {
                    $nameRow.find(".PersonContactInfo").focus();
                    notifyDanger('Invalid Email.', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
                    validate = false;
                    break;
                }
        }
    }
    return validate;
}

function SaveData() {
    var data = JSON.stringify(GetData());
    $.ajax({
        type: "POST", dataType: 'json', url: $BaseURL + 'Case/ContactInfoSave', data: data, contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                RequestSubmitted();
                //notifyDanger('Contact Information has been saved successfully.', 'bottom-right', '4000', 'success', 'fa-check', true);
                window.location.href = window.location.href;
            }
        }
    });
}

$("#btnAddNewContactInfo").on("click", function () {
    $("#all-names").append('<div class="row name newAdded margin-bottom-10" id="name-' + $("#all-names .name").length + '">' + $emptyName + '</div>');

    $(".newAdded .form-group .control-label").remove();

    $("#all-names .name:last input").each(function (i) {
        $(this).val('');
    });

    $("#all-names .name:last  .dldPerson").focus();
    if ($("#all-names .newAdded").length > 0) {
        $("#btnRemoveContactInfo").prop("disabled", false);
    }
});

$("#btnRemoveContactInfo").on("click", function () {
    var latestname = $("#all-names .name").length - 1;
    $("#name-" + latestname).remove();
    $("#all-names .name:last .dldPerson").focus();
    if ($("#all-names .newAdded").length <= 0) {
        $(this).prop("disabled", true);
    }
});

$('#btnSave').on("click", function (e) {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }
    if (!hasFormChanged('contactInfo-form')) {

        notifyDanger('Nothing was changed.');

        return false;
    }


    if (Validate())
        SaveData();
    else
        return false;
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to delete?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Case/ContactInfoDelete/' + id,
                dataType: "json",
                success: function (data) {
                    tr.remove();
                    notifyDanger('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
});

$('body').on('change', '.ddlType', function () {
 

    if ($(this).val() == '1973' || $(this).val() == '1975' || $(this).val() == '1974') {
        $(this).parent().parent().find('.phone_global').addClass('phone_format');
    } else {
        $(this).parent().parent().find('.phone_global').removeClass('phone_format');
    }
});
$('.ddlType').change();
$(document).ready(function () {
    $emptyName = $("#name-0").html();

    var oTableL = $('#contactInfoList').dataTable({
        "searching": false,
        "bSort": false,
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
    });
    setTimeout(function () {

        $('#contactInfoList tbody tr').each(function () {
            if ($(this).data('roleclient') == "1") {

                $(this).addClass('highLightBlue')
            }
        });
    }, 500);

    fitCalculatedHeightForSearchDataTable();

    $(window).bind('resize', function () {
        fitCalculatedHeightForSearchDataTable();
    });

    function fitCalculatedHeightForSearchDataTable() {
        var calc_height = 0;
        calc_height = $(window).height();
        var _offset = 25;
        origin_wrapper_height = $('body>div.container-fluid').height();
        origin_content_height = $('#divSearchResult .dataTables_scrollBody').height();

        $("#divSearchResult .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {

            $(this).siblings().each(function () {
                if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                    //console.log(calc_height + " - " + $(this).outerHeight(true));

                    console.log($(this).siblings());
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        //console.log("calc :" + calc_height + " offset: " + _offset);
        calc_height = calc_height - _offset;
        //console.log("total: " + calc_height);
        if (calc_height < 100) {

            calc_height = 100;
        }
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTableL.fnAdjustColumnSizing();
        return calc_height;
    }

    setInitialFormValues('contactInfo-form', true);


});