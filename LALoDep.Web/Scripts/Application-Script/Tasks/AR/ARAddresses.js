
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    SaveData(6);

});

function isValidPhone(str) {
    var plainNumber = str.replace(/[^0-9]/g, "");
    return plainNumber.length == 10;

}

$(function () {
    $('#CountryCodeID option[value="2246"]').attr("selected", true);
    $('#StateCodeID option[value="841"]').attr("selected", true);
    //$('#StateCodeID').find('option:eq(0)').prop('selected', true);

    $('#btnSaveAndContinue').on('click', function () {

        SaveData(1);

    });

    $('#btnSaveAndReturn').on('click', function () {

        SaveData(2);
    });
    $('#btnSaveAndExitAR').on('click', function () {

        SaveData(3);
    });
    $('#btnSaveAndPrintAR').on('click', function (e) {
        e.preventDefault();
        SaveData(4);


    });
    $('#btnSaveAndPrint').on('click', function (e) {
        e.preventDefault();
        SaveData(5);


    });



    $('#StateCodeID').on('change', function () {
        $('#StateCodeID').val($(this).val());
    });
    $('#CountryCodeID').on('change', function () {
        $('#CountryCodeID').val($(this).val());

    });

    $('#Street').blur(function () {
        $("#lblStreet").hide();
        $("#lblStreetPlacement").hide();
        $('#Street').val($.trim($('#Street').val()));
        if ($('#Street').val().length > 0) {
            var list = $('#PlacementAgencyAddress').val().split(',');
            $.each(list, function (ind, obj) {

                var add = obj.split(' ');
                $.each(add, function (indd, objj) {
                    if ($.trim($('#Street').val()).split(' ')[0] == add[indd]) {
                        $("#lblStreetPlacement").show();
                        $("#lblStreet").hide();
                        return false;
                    }

                })


            });
            list = $('#AddressStreet').val().split(',');
            $.each(list, function (ind, obj) {

                var add = obj.split(' ');

                if ($.trim($('#Street').val()).split(' ')[0] == add[0]) {
                    $("#lblStreetPlacement").hide();
                    $("#lblStreet").show();
                    return false;
                }

            });



        }


    });

    $('#HomePhone').blur(function () {
        $("#lblHomePhone").hide();
        var list = $('#AddressHomePhone').val().split(',');
        var phone = $('#HomePhone').val().replace(')', '').replace('(', '').replace('-', '').replace(' ', '');
        var phoneTxt = $.trim(phone);
        if ($.trim(phone).length > 7) {
            phoneTxt = $.trim(phone).substring(phone.length - 7, phone.length);
        }

        if ($.trim(phone) != '') {


            $.each(list, function (ind, obj) {

                var phoneExistText = obj.replace(')', '').replace('(', '').replace('-', '').replace(' ', '');


                if (phoneExistText != "") {



                    if (strEndsWith($.trim(phoneExistText), $.trim(phoneTxt))) {
                        $("#lblHomePhone").show();
                        return false;
                    }
                }

            });
        }

    });


    $('.btn-delete').on('click', function (e) {
        e.preventDefault();
        var $this = $(this);
        confirmBox("Are you sure you want to delete?", function (result) {

            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/CaseOpening/CaseAddressDelete/' + $this.attr('data-id'),

                    dataType: 'json',
                    success: function (data) {
                        if (data.Status == 'Done') {
                            Notify('Address Removed!.', 'bottom-right', '3000', 'success', 'fa-smile-o', true);

                            $this.parents('tr').remove();
                            document.location.href = document.location.href;
                        } else {
                            document.location.href = data.URL;
                        }
                    }
                });
            }

        });



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

function PrintAR() {
    var data = {
        'id': $('#simplewizard').attr('data-id'),
    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/ActionRequestPrint/' + $('#simplewizard').attr('data-id'), data, "POST", 'target="_blank"');

}
function PrintARAddress() {

   $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/CaseAddressesPrint', "", "POST", 'target="_blank"');

    
}
function SaveData(buttonId) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged('case-addresses-form')) {
        if (fnRedirect(buttonId))
            notifyDanger('Nothing was changed.');

        return;
    }


    /* Validations */
    if (($('#PlacementAgencyAddressID').val() > 0 && $('#ExistingAddressID').val() > 0) || ($('#PlacementAgencyAddressID').val() > 0 && ($('#Street').val().length > 0 || $('#City').val().length > 0 || $('#ZipCode').val().length > 0)) || ($('#ExistingAddressID').val() > 0 && ($('#Street').val().length > 0 || $('#City').val().length > 0 || $('#ZipCode').val().length > 0))) {

        notifyDanger('Select an placement agency or select an existing address or enter new address information only');
        return;
    }
    if ($('#Street').val().length > 0 && ($('#City').val().length == 0 || $('#ZipCode').val().length == 0)) {
        notifyDanger('Street,City,ZipCode and State is required!');
        return;

    } else if ($('#City').val().length > 0 && ($('#Street').val().length == 0 || $('#ZipCode').val().length == 0)) {
        notifyDanger('Street,City,ZipCode and State is required!');
        return;
    } else if ($('#ZipCode').val().length > 0 && ($('#Street').val().length == 0 || $('#City').val().length == 0)) {

        notifyDanger('Street,City,ZipCode and State are required!');
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

    if ($('#PlacementAgencyAddressID').val() > 0 || $('#ExistingAddressID').val() > 0 || ($('#Street').val().length > 0 || $('#City').val().length > 0 || $('#ZipCode').val().length > 0 || $('#HomePhone').val().length > 0)) {
        var typeSelected = false;
        var errorReturn = false;

        $('.personRow').each(function () {
            if ($(this).find('select[id^="PersonAddressTypeCodeID"]').val() > 0) {
                typeSelected = true;

                if ($(this).find('input[id^="StartDate"]').val() == '') {
                    $(this).find('input[id^="StartDate"]').focus();
                    notifyDanger('Start Date is required to add an address to a person');
                    errorReturn = true;
                    return;

                } else if ($(this).find('input[id^="EndDate"]').val() != '' && moment($(this).find('input[id^="StartDate"]').val()) > moment($(this).find('input[id^="EndDate"]').val())) {
                    $(this).find('input[id^="EndDate"]').focus();
                    notifyDanger('End Date can not be prior to Start Date');
                    errorReturn = true;
                    return;
                }


            }

        });
        if (!typeSelected) {
            notifyDanger('Select people for this new address');
            $(this).find('select[id^="PersonAddressTypeCodeID"]:eq(0)').focus();
            return;
        }
        if (errorReturn)
            return;


    } else {
        var errorReturn = false;

        $('.addressRow').each(function () {
            if ($(this).find('input[id^="StartDate"]').val() == '') {
                $(this).find('input[id^="StartDate"]').focus();
                notifyDanger('Start Date is required');
                errorReturn = true;
                return;

            } else if ($(this).find('input[id^="EndDate"]').val() != '' && moment($(this).find('input[id^="StartDate"]').val()) > moment($(this).find('input[id^="EndDate"]').val())) {
                $(this).find('input[id^="EndDate"]').focus();
                notifyDanger('End Date can not be prior to Start Date');
                errorReturn = true;
                return;
            }
        });
        if (errorReturn)
            return;
    }
    if ($('#PlacementAgencyAddressID').val() > 0 || $('#ExistingAddressID').val() > 0 || ($('#Street').val().length > 0 || $('#City').val().length > 0 || $('#ZipCode').val().length > 0 || $('#HomePhone').val().length > 0)) {
        var typeSelected = false;
        var errorReturn = false;

        $('.personRow').each(function () {
            if ($(this).find('select[id^="PersonAddressTypeCodeID"]').val() > 0) {
                typeSelected = true;

                if ($(this).find('input[id^="StartDate"]').val() == '') {
                    $(this).find('input[id^="StartDate"]').focus();
                    Notify('Start Date is required to add an address to a person', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
                    errorReturn = true;
                    return;

                } else if ($(this).find('input[id^="EndDate"]').val() != '' && moment($(this).find('input[id^="StartDate"]').val()) > moment($(this).find('input[id^="EndDate"]').val())) {
                    $(this).find('input[id^="EndDate"]').focus();
                    Notify('End Date can not be prior to Start Date', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
                    errorReturn = true;
                    return;
                }
            }
        });
        if (!typeSelected) {
            Notify('Select people for this new address', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
            $(this).find('select[id^="PersonAddressTypeCodeID"]:eq(0)').focus();
            return;
        }
        if (errorReturn)
            return;


    } else {
        var errorReturn = false;

        $('.addressRow').each(function () {
            if ($(this).find('input[id^="StartDate"]').val() == '') {
                $(this).find('input[id^="StartDate"]').focus();
                Notify('Start Date is required', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
                errorReturn = true;
                return;

            } else if ($(this).find('input[id^="EndDate"]').val() != '' && moment($(this).find('input[id^="StartDate"]').val()) > moment($(this).find('input[id^="EndDate"]').val())) {
                $(this).find('input[id^="EndDate"]').focus();
                Notify('End Date can not be prior to Start Date', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
                errorReturn = true;
                return;
            }
        });
        if (errorReturn)
            return;
    }

    var isStreetExist = false;
    if ($('#AddressStreet').val() != "") {
        var list = $('#AddressStreet').val().split(',');
        $.each(list, function (ind, obj) {
            if ($('#Street').val().trim().length > 0) {
                if ($('#Street').val().trim() == obj.trim()) {
                    isStreetExist = true;
                    return false;
                }
            }

        });
    }

    if (isStreetExist) {
        Notify('Exact Street found in Existing Addresses. Please verify this new address or select an existing address.', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        $('#Street').focus();
        return;
    }

    isStreetExist = false;
    if ($('#PlacementAgencyAddress').val() != "") {
        var list = $('#PlacementAgencyAddress').val().split(',');
        $.each(list, function (ind, obj) {
            if ($('#Street').val().trim().length > 0) {
                if ($('#Street').val().trim() == obj.trim() || obj.trim().startsWith($('#Street').val().trim())) {
                    isStreetExist = true;
                    return false;
                }
            }

        });
    }

    if (isStreetExist) {
        Notify('Exact Street found in Existing Placement Agency. Please verify this new address or select an existing Placement Agency.', 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
        $('#Street').focus();
        return;
    }
    /* End Validations */

    var data = GetData();

    $.ajax({
        type: "POST",
        url: '/CaseOpening/CaseAddresses',
        data: data,
        dataType: 'json',
        success: function (data) {
            if (data.Status == 'Done') {


                notifySuccess('Data Saved!.');
                RequestSubmitted();
                fnRedirect(buttonId);

            } else {
                document.location.href = data.URL;
            }
        }
    });

}
function fnRedirect(buttonId) {

    if (buttonId == 1) {
        document.location.href = '/Task/EditRfdProfile/' + $('#EncryptHearingReportFilingDueID').val();
        return false;
    }

    else if (buttonId == 2) {
        document.location.href = document.location.href;
        return false;
    }
    else if (buttonId == 3) {
        if ($exitUrl.length > 0) {
            document.location.href = $exitUrl;
            return false;
        }

    } else if (buttonId == 4) {
        PrintAR();
        setTimeout(function() {
            document.location.href = document.location.href;
        }, 1000);
        return false;


    } else if (buttonId == 5) {
        PrintARAddress();
        setTimeout(function () {
            document.location.href = document.location.href;
        }, 1000);
        return false;

    } else if (buttonId == 6) {
        document.location.href = wizardUrl;
        return false;

    }
    return true;

}
