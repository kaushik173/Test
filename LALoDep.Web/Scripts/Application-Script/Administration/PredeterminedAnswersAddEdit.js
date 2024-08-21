$(function () {

    $("#btnSearch").on("click", function () {

    });

    $("#btnCancel").on("click", function () {
        document.location.href = '/Administration/PredeterminedAnswersList';
    });

    $('#chkAllAgency').click(function () {
        $('.chkAgency').prop('checked', $(this).is(':checked'));
    });

    $('#chkAllNoteType').click(function () {
        $('.chkNoteType').prop('checked', $(this).is(':checked'));
    });

    $('#chkAllHearingType').click(function () {
        $('.chkHearingType').prop('checked', $(this).is(':checked'));
    });

    $(".group-chk").on("click", function () {
        var groupId = $(this).data("groupid");
        $('.group-' + groupId).prop('checked', $(this).is(":checked"));
    });

    $(".chkHearingType").on("click", function () {
        var groupId = $(this).data("groupid");

        $('#chkAll-' + groupId).prop("checked", $('.group-' + groupId).length == $('.group-' + groupId + ':checked').length);

        $('#chkAllHearingType').prop("checked", $(".chkHearingType").length == $(".chkHearingType:checked").length);
    });

    $('#btnSave').click(function (e) {
        e.preventDefault();
        SaveData(1);
    });

    setInitialFormValues('PredeterminedAnswerAddEditform');
});


function Validation() {
    if ($('#ShortValue').val().length == 0) {
        Notify('Short Value is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $('#ShortValue').focus();
        return false;
    }
    if ($('#PredeterminedAnswer').val() == '') {
        Notify('Predetermined Answer is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $('#PredeterminedAnswer').focus();
        return false;
    }
    if (!$('#ChildClients').is(':checked') && !$('#AdultClients').is(':checked')) {
        Notify('At least one client type is required. (Child Clients checked, or Adult Clients checked, or both.  Cannot have both unchecked).', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $('#ChildClients').focus();
        return false;
    }

    return true;
}
function SaveData(buttonID) {
    IPadKeyboardFix();
    if (!hasFormChanged('PredeterminedAnswerAddEditform')) {
        //if (buttonID == 1) {
        //    document.location.href = '/Administration/PredeterminedAnswersList';
        //    return false;
        //}  
        notifyDanger('Nothing was changed.');
        return false;
    }
    if (!IsValidFormRequest()) {
        return;
    }


    var isValid = Validation();
    if (isValid) {

        var data = GetData();

        console.log(data)
        $.ajax({
            type: "POST",
            url: "/Administration/PredeterminedAnswersAddEditSave",
            data: data,
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Done") {
                    notifySuccess('Predetermined Answers Update Successfully!');

                    RequestSubmitted();


                    document.location.href = '/Administration/PredeterminedAnswersList';

                } else {
                }
            }
        });
    }
    else {
        return false;
    }

}

function GetData() {
    var data = {
        'QuickNoteID': $('#QuickNoteID').val(),
        'ShortValue': $('#ShortValue').val(),
        'PredeterminedAnswer': $('#PredeterminedAnswer').val(),
        'ChildClients': $('#ChildClients').is(':checked'),
        'AdultClients': $('#AdultClients').is(':checked'),
        'Seq': $('#Seq').val(),
        'InactiveFlag': $('#InactiveFlag').is(':checked'),


        'AddNodeTypes': [],
        'AddAgencies': [],
        'AddHearingTypes': []
    };


    $('.chkNoteType').each(function () {
        var iud = '';
        if ($(this).is(':checked') && $(this).attr('data-noterecordid') == 0) {
            iud = 'INSERT';
        } else if (!$(this).is(':checked') && $(this).attr('data-noterecordid') > 0) {
            iud = 'DELETE';
        }
        if (iud != '') {

            data.AddNodeTypes.push({
                'CodeID': $(this).attr('data-id'),
                'IUD': iud,
                'QuickNoteDependentID': $(this).attr('data-noterecordid'),

            });
        }

    });
    $('.chkAgency').each(function () {
        var iud = '';
        if ($(this).is(':checked') && $(this).attr('data-noterecordid') == 0) {
            iud = 'INSERT';
        } else if (!$(this).is(':checked') && $(this).attr('data-noterecordid') > 0) {
            iud = 'DELETE';
        }
        if (iud != '') {

            data.AddAgencies.push({
                'CodeID': $(this).attr('data-id'),
                'IUD': iud,
                'QuickNoteDependentID': $(this).attr('data-noterecordid'),

            });
        }

    });
    $('.chkHearingType').each(function () {
        if (!$(this).hasClass('group-chk')) {

            var iud = '';
            if ($(this).is(':checked') && $(this).attr('data-noterecordid') == 0) {
                iud = 'INSERT';
            } else if (!$(this).is(':checked') && $(this).attr('data-noterecordid') > 0) {
                iud = 'DELETE';
            }
            if (iud != '') {

                data.AddHearingTypes.push({
                    'CodeID': $(this).attr('data-id'),
                    'IUD': iud,
                    'QuickNoteDependentID': $(this).attr('data-noterecordid'),

                });
            }

        }
    });

    return data;
}