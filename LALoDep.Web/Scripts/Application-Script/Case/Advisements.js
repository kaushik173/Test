var $BaseURL = '/';
var IsAdvisementCheckboxUnchecked = false;
$('.AttorneyPersonID').each(function () {
    if ($(this).data('currentid') > 0) {
        $(this).val($(this).data('currentid'))
    }
})

$('.StatusCodeID').each(function () {
    if ($(this).data('currentid') > 0) {
        $(this).val($(this).data('currentid'))
    }
})


setInitialFormValues('advisements-form', true);

function SaveData() {
    if (!hasFormChanged('advisements-form')) {
        notifyDanger('Nothing was changed.');
        return false;
    }
    var data = [];
    $('.chkAdvisement').each(function () {



        var caseId = $('#btnSave').data('caseid');
        var advisementID = $(this).data('id');
        var clientRoleID = $(this).data('clientroleid');
        var advisementCodeID = $(this).data('advisementcodeid');
        var attorneyPersonID = $(this).parent().parent().find('.AttorneyPersonID').val();
        var advisementDateTime = $(this).parent().parent().find('.AdvisementDateTime').val();
        var fileDueDate = $(this).parent().parent().find('.FileDueDate').val();
        var statusCodeID = $(this).parent().parent().find('.StatusCodeID').val();
        var statusDate = $(this).parent().parent().find('.StatusDate').val();
        var hearingID = $(this).parent().parent().find('.HearingID').val();

        if ($(this).is(':checked')) {
            if (advisementID == 0) {
                data.push({
                    'IUD': 'INSERT',
                    'AdvisementID': advisementID,
                    'CaseID': caseId,
                    'ClientRoleID': clientRoleID,
                    'AttorneyPersonID': attorneyPersonID,
                    'AdvisementCodeID': advisementCodeID,
                    'AdvisementDateTime': advisementDateTime,
                    'RecordStateID': $('#RecordStateID').val(),
                    'HearingID': hearingID,
                    'FileDueDate': fileDueDate,
                    'StatusCodeID': statusCodeID,
                    'StatusDate': statusDate,
                });
            } else if ($(this).parent().parent().find('.AttorneyPersonID').IsValueChanged() || $(this).parent().parent().find('.AdvisementDateTime').IsValueChanged()

                || $(this).parent().parent().find('.StatusCodeID').IsValueChanged()
                || $(this).parent().parent().find('.FileDueDate').IsValueChanged()
                || $(this).parent().parent().find('.StatusDate').IsValueChanged()
            ) {
                data.push({
                    'IUD': 'UPDATE',
                    'AdvisementID': advisementID,
                    'CaseID': caseId,
                    'ClientRoleID': clientRoleID,
                    'AttorneyPersonID': attorneyPersonID,
                    'AdvisementCodeID': advisementCodeID,
                    'AdvisementDateTime': advisementDateTime,
                    'HearingID': hearingID,
                    'FileDueDate': fileDueDate,
                    'StatusCodeID': statusCodeID,
                    'StatusDate': statusDate,
                    'RecordStateID': $('#RecordStateID').val(),
                });
            }



        } else if (advisementID > 0) {
            data.push({
                'IUD': 'DELETE',
                'AdvisementID': advisementID,
                'CaseID': caseId,
                'ClientRoleID': clientRoleID,
                'AttorneyPersonID': attorneyPersonID,
                'AdvisementCodeID': advisementCodeID,
                'AdvisementDateTime': advisementDateTime,
                'RecordStateID': $('#RecordStateID').val(),
                'HearingID': hearingID,
                'FileDueDate': fileDueDate,
                'StatusCodeID': statusCodeID,
                'StatusDate': statusDate,
            });
        }

    });
    console.log(data)
    $.ajax({
        type: "POST",
        url: '/Case/AdvisementsSave',
        data: { spParams: data },
        success: function (result) {
            if (result.isSuccess) {
                RequestSubmitted();
                //window.location.href = $BaseURL + 'Case/Advisements';
                window.location.href = window.location.href;
            }
        },
        dataType: 'json'
    });
}

function Validation() {
    IsAdvisementCheckboxUnchecked = false;
    var flag = true;
    var message;
    $('.chkAdvisement').each(function () {
        if ($(this).is(':checked')) {
            if ($(this).parent().parent().find('.AdvisementDateTime').val() == '') {
                if ($(this).parent().parent().find('.StatusCodeID').val() != 25994 && !($(this).parent().parent().find('.HearingID').length > 0 && $(this).parent().parent().find('.HearingID').val() > 0)) {
                    notifyDanger('Date is required');
                    $(this).parent().parent().find('.AdvisementDateTime').focus();
                    flag = false;
                    return false;
                }
            }
            if ($(this).parent().parent().find('.AttorneyPersonID').val() == '') {
                notifyDanger('Attorney is required');
                $(this).parent().parent().find('.AttorneyPersonID').focus();
                flag = false;
                return false;
            }
            if ($(this).parent().parent().find('.FileDueDate').length > 0 && $(this).parent().parent().find('.FileDueDate').val() == '') {
                notifyDanger('File Due Date is required');
                $(this).parent().parent().find('.FileDueDate').focus();
                flag = false;
                return false;
            }
            if ($(this).parent().parent().find('.StatusCodeID').length > 0 && ($(this).parent().parent().find('.StatusCodeID').val() != '' || $(this).parent().parent().find('.StatusDate').val() != '')) {
                if ($(this).parent().parent().find('.StatusCodeID').val() == '') {
                    notifyDanger('Status is required');
                    $(this).parent().parent().find('.StatusCodeID').focus();
                    flag = false;
                    return false;
                }
                if ($(this).parent().parent().find('.StatusDate').val() == '') {
                    notifyDanger('Status Date is required');
                    $(this).parent().parent().find('.StatusDate').focus();
                    flag = false;
                    return false;
                }
            }
            if ($(this).parent().parent().find('.StatusDate').val() != '' && $(this).parent().parent().find('.GroupDisplayHearingInfo').val() == 1) {
                if ($(this).parent().parent().find('.AdvisementDateTime').val() != '' && moment($(this).parent().parent().find('.StatusDate').val()) < moment($(this).parent().parent().find('.AdvisementDateTime').val())) {
                    notifyDanger('Status Date cannot be before the Advisement Date.');
                    $(this).parent().parent().find('.StatusDate').focus();
                    flag = false;
                    return false;
                }
                if ($(this).parent().parent().find('.StatusCodeID').val() == 25991 && moment($(this).parent().parent().find('.StatusDate').val()) < moment($(this).parent().parent().find('.FileDueDate').val())) {
                    notifyDanger('Status Date cannot be before the Due Date when status is Time Lapsed without Client Direction.');
                    $(this).parent().parent().find('.StatusDate').focus();
                    flag = false;
                    return false;
                }
            }
        }
        else if ($(this).IsCheckboxChanged()) {
            IsAdvisementCheckboxUnchecked = true;
        }
    });
    return flag;
}

$('#btnSave').on("click", function (e) {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }
    if (Validation()) {
        if (IsAdvisementCheckboxUnchecked) {
            confirmBox("You have unchecked a row.This will cause that advisement row to be totally deleted.  Are you sure you want to do this?", function (isConfirm) {
                if (!isConfirm) {

                }
                else {
                    SaveData();
                }
            });
            return;
        }
        else {
            SaveData();
        }
    } else {
        return false;
    }
});

$('body').on("click", '.addAnotherRow', function (e) {

    tr = $(this).closest('tr').clone();
    $(tr).find('.chkAdvisement').data('id', '0');
    var $str = $(tr).insertAfter($(this).closest('tr'));
    $str.find('.chkAdvisement').attr('data-id', '0');
    $str.find('.chkAdvisement').prop('disabled', false);
    $str.find('.addAnotherCell').html('Click Save to Add Record');
    $str.find('.AdvisementDateTime').val(moment().format("L"));

    
});
$('body').on("change", '.chkAdvisement', function (e) {

    if ($(this).is(':checked')) {
        if ($(this).parent().parent().find('.AdvisementDateTime').val() == '')
            $(this).parent().parent().find('.AdvisementDateTime').val(moment().format("MM/DD/YYYY"))

        if ($('#PersonID').val() > 0 && $(this).parent().parent().find('.AttorneyPersonID').val() == '')
            $(this).parent().parent().find('.AttorneyPersonID').val($('#PersonID').val())
        else if ($(this).parent().parent().find('.AttorneyPersonID').data('defaultid') > 0 && $(this).parent().parent().find('.AttorneyPersonID').val() == '')
            $(this).parent().parent().find('.AttorneyPersonID').val($(this).parent().parent().find('.AttorneyPersonID').data('defaultid'))

    } else {
        if ($(this).data('id') > 0) {
            $this =  $(this);
            confirmBox("You have unchecked this checkbox, Therefore, upon saving this page this Advisement will be deleted.  Do you want this to happen?", function (isConfirm) {
                if (!isConfirm) {
                    $this.prop('checked', true);
                }
                else {
                   
                }
            });

        }


    }
});

$('body').on("blur", '.AdvisementDateTime', function (e) {


    if ($(this).val() != '') {
        $(this).parent().parent().parent().find('.chkAdvisement').prop('checked', true);
    }
});
$('body').on("change", '.AttorneyPersonID', function (e) {



    if ($(this).val() != '') {
        $(this).parent().parent().find('.chkAdvisement').prop('checked', true);
    }
});


$('#btnCancel').on("click", function (e) {
    window.location.href = $BaseURL + 'Case/Advisements';
});