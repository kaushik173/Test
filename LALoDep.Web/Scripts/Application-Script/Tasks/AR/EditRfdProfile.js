
$('#btnContinue').on('click', function () {


    document.location.href = '/Case/RecordTime?arId=' + $('#EncryptHearingReportFilingDueID').val();

});
$('#btnPrintAR').on('click', function () {

    Print();

});
$('#btnExitAR').on('click', function () {
    if ($exitUrl.length > 0) {
        document.location.href = $exitUrl;
        return false;
    }
});


$('#btnAddNewProfile').on('click', function () {

    if ($('#PersonID').val() == '') {
         $('#PersonID').focus();
        notifyDanger('Person is required.');
        return;
    }

    if ($('#ProfileTypeID').val() == '') {
         $('#ProfileTypeID').focus();
        notifyDanger('Profile Type is required.');
        return;
    }
    document.location.href = '/Task/EditRFDProfileQuestion/' + $('#EncryptHearingReportFilingDueID').val() + '?roleId=' + $('#PersonID').val() + '&profileTypeId=' + $('#ProfileTypeID').val();

});

$('.printProfile').on('click', function () {
    PrintARProfile($(this));
});
$('.btnNote').on('click', function () {
    OpenPopup('/Task/ARProfileQuestionNote?profileId=' + $(this).attr('data-profileId'), 'Note');
});


function Print() {
    var data = {
        'id': $('#EncryptHearingReportFilingDueID').val(),
    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/ActionRequestPrint/' + $('#EncryptHearingReportFilingDueID').val(), data, "POST", 'target="_blank"');

}
function PrintARProfile(el) {
    var data = {
        rfdId: $(el).attr('data-rfdId'),
        roleId: $(el).attr('data-roleid'),
        profileTypeCodeId: $(el).attr('data-profileTypeCodeId'),
    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/Task/PrintARProfile/', data, "POST", 'target="_blank"');
}
