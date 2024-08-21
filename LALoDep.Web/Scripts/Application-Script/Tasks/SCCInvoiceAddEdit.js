

$("#btnCancel").on("click", function () {
    window.location.href = "/SCCInvoiceQueue/Search?load=true";
});

$('#btnPrint').on('click', function () {
    //$.ajax({
    //    type: "POST", url: '/SCCInvoiceQueue/PrintSSCInvoiceQueue',
    //    success: function (returnValue) {
    //        if (returnValue.FileName != '') {
    //            window.location = '/Inquiry/Download?file=' + returnValue.FileName;
    //        }
    //    },
    //    dataType: 'json'
    //});
    var data = {
        id: $('#SCCInvoiceID').val(),
    };
    var _target = 'target="_blank"';//$("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';
    $.download($('#hdnCurrentSessionGuidPath').val() + '/SCCInvoiceQueue/PrintSSCInvoiceQueueAddEdit', data, "POST", _target);
});

$(document).ready(function () {
    setInitialFormValues('SCCInvoiceAddEdit-form', true);
});


function Save(statusCodeId) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }

    var isvalid = Validation(statusCodeId);

    if (isvalid) {
        var params = $('#SCCInvoiceAddEdit-form').serialize() + '&SCCInvoiceStatusCodeID=' + statusCodeId;
        if (statusCodeId == 22591) { // if user submitting the invoice requiest then ask for confirmation.
            bootbox.dialog({
                message: "Pursuant to code of civil procedure section 2015.5.  I declare under penalty of perjury " +
                            "and the laws of the state of California, that I am attorney-of-record for the dependency client(s) named " +
                            "above, and that my client is active in the case.  As defined and authorized by the Juvenile Court Flat-Fee " +
                            "Policy.  All information contained in this declaration is true and correct and this claim is being submitted " +
                            "for payment within thirty (30) days of the date the fee became due.  I have not previously claimed, nor have " +
                            "I been reimbursed for, any amounts in excess of those authorized by the Flat-Fee Policy effective May 1, 2002. " +
                            "I am claiming Flat-Fees only on a single payment sequence, either A, B, C, D, E, F, G, or H.  As defined and " +
                            "authorized by the Flat-Fee Policy, irrespective of the number of clients that I represent in this case.  On " +
                            "the date the fee became due, I had not been relieved as attorney-of-record by the Court, and was actively " +
                            "representing my client(s) in this case, and said case was also active, and had not been transferred to another " +
                            "county, consolidated into another case, dismissed or terminated on or before the date the fee became due. " +
                            "I understand that no notice of any fees due will be made by the Court and that the timely filing of all initial " +
                            "and subsequent fee declarations is my responsibility.  I understand that all references to the Juvenile Court " +
                            "Flat-Fee Policy refer to the policy statement dated May 1, 2002.",
                title: "Warning",
                header: "<i class='fa fa-warning'></i>",
                buttons: {
                    yes: {
                        label: "OK",
                        className: "btn-secondary",
                        callback: function () {
                            $.ajax({
                                type: "POST", url: '/SCCInvoiceQueue/SCCInvoiceAddEdit', data: params,
                                success: function (result) {
                                    if (result.Status == "Done") {
                                        notifySuccess('Data Saved Successfully!.');
                                        RequestSubmitted();
                                        //document.location.href = document.location.href;
                                        self.parent.RefreshAndClosePopup();
                                    }
                                },
                                dataType: 'json'
                            });
                        }
                    }
                    , no: {
                        label: "Cancel",
                        className: "btn-secondary",
                        callback: function () { }
                    }
                }
            });
        }
        else {
            $.ajax({
                type: "POST", url: '/SCCInvoiceQueue/SCCInvoiceAddEdit', data: params,
                success: function (result) {
                    if (result.Status == "Done") {
                        notifySuccess('Data Saved Successfully!.');
                        RequestSubmitted();
                       // document.location.href = document.location.href;
                        self.parent.RefreshAndClosePopup();

                    }
                },
                dataType: 'json'
            });
        }
    }
}

function Validation(statusCodeId) {
    var isValid = true;

    if (statusCodeId != 22596 && statusCodeId != 22592) {
        if (!hasFormChanged('SCCInvoiceAddEdit-form')) {

            notifyDanger('Nothing was changed.');
            isValid = false;
            return false;
        }
    }

    if ($('input[name*="chkClient"]:checked').length == 0) {
        notifyDanger('At least one client is required');
        return false;
    }
    if ($('#SCCInvoiceRateID').val() == '') {
        isValid = false;
        $('#SCCInvoiceRateID').focus();
        notifyDanger('Invoice type is a required field.');
        return false;
    }
    if ($('#SCCInvoiceSubmittedByPersonID').val() == '') {
        isValid = false;
        $('#SCCInvoiceSubmittedByPersonID').focus();
        notifyDanger('Attorney is a required field.');
        return false;
    }
    if ($('#SCCInvoiceDepartmentCodeID').val() == '') {
        isValid = false;
        $('#SCCInvoiceSubmittedByPersonID').focus();
        notifyDanger('Department is a required field.');
        return false;
    }

    var selectedClients = $("#clientList .chk-client:checked");
    for (var indx = 0; indx < selectedClients.length; indx++) {
        var chk = selectedClients.eq(indx);
        if ($('#PetitionCloseDate' + chk.data("indx")).val() == '' && $("#SCCInvoiceNextHearingDate").val() == '') {
            isValid = false;
            $("#SCCInvoiceNextHearingDate").focus();
            notifyDanger('Next Hearing Date is required when invoice includes an active client.');
            return false;
        }
    }

    if (statusCodeId == 22594 && $("#AdminNote").val() == '') {
        isValid = false;
        $("#AdminNote").focus();
        notifyDanger("Admin Note is required when denying an invoice.");
        return false;
    }

    if (statusCodeId == 22595 && $("#SCCInvoicePaidDate").val() == '') {
        isValid = false;
        $("#SCCInvoicePaidDate").focus();
        notifyDanger("Date To Be Paid is required.")
        return false;
    }


    return isValid;
}


