$(function () {


    if ($('.chkPage').length == $('.chkPage:checked').length) {
        $('#chkAll').prop('checked', true);

    }
    $('#chkAll').click(function () {

        $('.chkPage').prop('checked', $(this).is(':checked'));
    });

    $('#btnSave').click(function (e) {
        e.preventDefault();

        SaveData(1);
    });
    $('#btnSaveAndAdd').click(function (e) {
        e.preventDefault();

        SaveData(2);
    });


    setInitialFormValues('NoteAdd');
});

var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    SaveData(3);

});
function Validation() {



    if ($('#Subject').val() == '') {
        notifyDanger('Subject is required.');
        $('#Subject').focus();
        return false;
    }
    if ($('#Notes').val().length == 0) {
        notifyDanger('Notes is required.');
        $('#Notes').focus();
        return false;
    } if ($('#NoteTypeID').val().length == 0) {
        notifyDanger('Note Type is required.');
        $('#NoteTypeID').focus();
        return false;
    }
    return true;
}
function SaveData(typeId) {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged('NoteAdd')) {
        if (typeId == 1) {
            document.location.href = '/Case/Main';
            return false;
        } else if (typeId == 3) {
            document.location.href = wizardUrl;
            return false;
        }
        notifyDanger('Nothing was changed.');
        return false;
    }


    var isValid = Validation();
    if (isValid) {
        var model = GetData();
        $.ajax({
            type: "POST",
            url: "/CaseOpening/NoteSave",
            data: { 'model': model },
            dataType: 'json',
            success: function (data) {
                if (data.Status == 'Done') {
                    notifySuccess('Data Saved!.');
                    RequestSubmitted();

                    if (typeId == 1) {
                        document.location.href = '/Case/Main';

                    } else if (typeId == 3) {
                        document.location.href = wizardUrl;
                        return false;
                    } else {
                        document.location.href = '/CaseOpening/Notes';

                    }


                } else {
                    document.location.href = data.URL;
                }
            }
        });
    }
}
function GetData() {
    var pageIds = '';
    $('.chkPage:checked').each(function () {

        if (pageIds != '')
            pageIds += ',';
        pageIds += $(this).attr('data-pageid');

    });

    var data = {
        'NoteTypeID': $('#NoteTypeID').val(),

        'Subject': $('#Subject').val(),
        'Notes': $('#Notes').val(),
        'SelectedPageIds': pageIds




    };
    return data;
}