function compareDates(start, end) {
    var startDate = new Date(start);
    var endDate = new Date(end);
    if (endDate < startDate) {
        return true;
    } else {
        return false;
    }
}

function SaveData() {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }

    if ($('#MotionFileDate').val() == '') {
        notifyDanger('File Date is a required field.');
        $("#MotionFileDate").focus();
        return false;
    }
    else if ($("#MotionTypeCodeID").val() == '') {
        notifyDanger('Motion type is required.');
        $("#MotionTypeCodeID").focus();
        return false;
    }
    else if ($("#MotionDecisionCodeID").val() == '' && $('#MotionDecisionDate').val() != '') {
        notifyDanger('Decision is a required when decision date is entered.');
        $("#MotionDecisionCodeID").focus();
        return false;
    }
    else if ($("#MotionDecisionCodeID").val() != '' && $('#MotionDecisionDate').val() == '') {
        notifyDanger('Decision date is a required when decision is entered.');
        $("#MotionDecisionDate").focus();
        return false;
    }
    else if ($('#MotionDecisionDate').val().trim() != '' && $('#MotionFileDate').val().trim() != '') {
        dateComparisonResult = compareDates($('#MotionFileDate').val(), $('#MotionDecisionDate').val())
        if (dateComparisonResult) {
            notifyDanger('Decision date cannot be before File date.');
            $('#DecisionDate').focus();
            return false;
        }
    }


    if (!hasFormChanged('motionAddEdit-form')) {
        window.location.href = "/Motions/List";
        return false;
    }

    var data = $('#motionAddEdit-form').serialize();
    if ($('#NoteEntry.summernote').length > 0) {
        data = data.replace('NoteEntry=', 'NoteEntryold=') + '&NoteEntry=' + $('#NoteEntry').GetHtmlWithEscape()
    }
    var unindexed_array = $('#motionAddEdit-form').serializeArray();
    var formData = {};

    $.map(unindexed_array, function (n, i) {

        formData[n['name']] = $('#' + n['name']).GetHtml();
    });



    if ($('#MotionID').val() > 0 && $('#NoteEntry').GetText() == '' && $('#NoteID').val() > 0) {
        confirmBox("Are you sure you want to remove the note from this record?", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/Motions/MotionAddEditSave',
                    data: formData,

                    success: function (result) {
                        if (result.isSuccess) {
                            RequestSubmitted();

                            window.location.href = "/Motions/List";
                        }
                    },
                    dataType: 'json'
                });
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Motions/MotionAddEditSave',
            data: formData,
            success: function (result) {
                if (result.isSuccess) {
                    RequestSubmitted();

                    window.location.href = "/Motions/List";
                }
            },
            dataType: 'json'
        });
    }
}

$('#btnSave').on('click', function () {
    SaveData();
});
$('#btnCancel').on('click', function () {
    window.location.href = "/Motions/List";
});
setInitialFormValues('motionAddEdit-form', true);

