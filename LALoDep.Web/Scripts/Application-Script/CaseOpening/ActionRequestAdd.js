
var validateTabClose = true;
$(function () {


    setInitialFormValues('ARAdd');
});

$('#btnSave').on('click', function () {

    Save(1, false);

});
$('#btnSaveAndPrint').on('click', function () {

    Save(2, false);

});
$('#btnCancel').on('click', function () {
	validateTabClose = false;
    document.location.href = '/CaseOpening/ActionRequest?' + dataEntryQueryString;


});


var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(6, false);

});
var associations = [];

function Validation(buttonId) {
    var isValid = true;
    associations = [];

    if (!hasFormChanged('ARAdd')) {
        if (buttonId == 6) {
            document.location.href = wizardUrl;
            return false;
        }
        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }


    if ($('#RequestDate').val() == '') {
        isValid = false;
        $('#RequestDate').focus();
        notifyDanger('Request Date is required.');
        return false;
    }
    if ($('#RequestTypeID').val() == '') {
        isValid = false;
        $('#RequestTypeID').focus();
        notifyDanger('Request Type is required.');
        return false;
    }
    if ($('#RequestByID').val() == '') {
        isValid = false;
        $('#RequestByID').focus();
        notifyDanger('Request By is required.');
        return false;
    }
    if ($('#RequestForID').val() == '') {
        isValid = false;
        $('#RequestForID').focus();
        notifyDanger('Request For is required.');
        return false;
    }
    if ($('#DueDate').val() == '') {
        isValid = false;
        $('#DueDate').focus();
        notifyDanger('Due Date is required.');
        return false;
    }
    if (moment($('#DueDate').val()) < moment($('#RequestDate').val())) {
        isValid = false;
        $('#DueDate').focus();
        notifyDanger('Due Date can not be before request date.');
        return false;
    }

    if ($('.chkRequest:checked').length == 0) {
        isValid = false;
        $('.chkRequest:eq(0)').focus();
        notifyDanger('At least one Request task must be checked.');
        return false;
    }
    if ($('.chkAddress').length > 0 && $('.chkAddress:checked').length == 0) {
        isValid = false;
        $('.chkRequest:eq(0)').focus();
        notifyDanger('At least one Client must be checked.');
        return false;
    }





    return isValid;
}
function Save(buttonId, forceCreateARAnyway) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }

    var isvalid = Validation(buttonId);

    if (isvalid) {
        var ClientAddressIds = '';
        $('.chkAddress:checked').each(function () {
            if (ClientAddressIds != '')
                ClientAddressIds += ',';
            ClientAddressIds += $(this).attr('data-id');

        });
        var RequestItemIds = '';
        $('.chkRequest:checked').each(function () {
            if (RequestItemIds != '')
                RequestItemIds += ',';
            RequestItemIds += $(this).attr('data-pageid');

        });
        var model = {
            'RequestDate': $('#RequestDate').val(),
            'RequestTypeID': $('#RequestTypeID').val(),
            'RequestByID': $('#RequestByID').val(),
            'RequestForID': $('#RequestForID').val(),
            'DueDate': $('#DueDate').val(),
            'HearingID': $('#HearingID').val() == '' ? 0 : $('#HearingID').val(),
            'LegalResearchTypeID': $('#LegalResearchTypeID').val() == '' ? 0 : $('#LegalResearchTypeID').val(),
            'ClientAddressIds': ClientAddressIds
            , 'RequestItemIds': RequestItemIds,
            'RequestNote': $('#RequestNote').GetHtml()
            , 'ForceCreateARAnyway': forceCreateARAnyway
        }
        var params = model;
        $.ajax({
            type: "POST", url: '/CaseOpening/ActionRequestAdd', data: { model: params },
            success: function (result) {

				if (result.Status == "Done") {
					validateTabClose = false;
                    RequestSubmitted();

                    notifySuccess('Data Saved Successfully!.');


                    if (buttonId == 2) {
                        document.location.href = '/CaseOpening/ActionRequest?print=true&insertedId=' + result.insertedId + '&' + dataEntryQueryString;
                    } else if (buttonId == 6) {
                        document.location.href = wizardUrl;
                        return false;
                    }  else {
                        document.location.href = '/CaseOpening/ActionRequest?' + dataEntryQueryString;

                    }
                } else if (result.Status == "AssignmentFail") {
                    if (result.DialogType == 1) {
                        AlertBoxWithCallback(result.ErrorMessage, function () {
                            $('#RequestForID').focus();

                        })
                    } else if (result.DialogType == 2) {
                        confirmBox(result.ErrorMessage, function (result) {
                            if (result) {
                                Save(buttonId, true);
                            } else {
                                $('#RequestForID').focus();
							}
                            

                        })
					}
                 
                }
				else {
					validateTabClose = false;
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }

}


window.onbeforeunload = function (e) {

	if (hasFormChanged('ARAdd') && validateTabClose) {
		return 'There is unsaved data.';
	}
	return undefined;
}