var validateTabClose = true;

$(function () {
    if ($("#NoteID").val() != "" && $("#NoteID").val() != 0) {
        $("#NoteTypeCodeID").prop("disabled", true);
    }

    $(window).on("keydown", handleHotkey);
    function handleHotkey(e) {
        if (!e.ctrlKey) return;
        switch (String.fromCharCode(e.keyCode).toLowerCase()) {
            case 'c':
                $('#btnCancel').trigger('click');
                e.preventDefault();
                break;
            case 'p':
                $('#btnPrint').trigger('click');
                e.preventDefault();
                break;
            case 's':
            case 'a':
                $('#btnSave').trigger('click');
                e.preventDefault();
                break;
            default:
                break;
        }
    }
    if ($('.chkPage').length == $('.chkPage:checked').length) {
        $('#chkAll').prop('checked', true);

    }
    $('#chkAll').click(function () {

        $('.chkPage').prop('checked', $(this).is(':checked'));
    });
    $('#chkAllClient').click(function () {

        $('.chkClient').prop('checked', $(this).is(':checked'));
    });
    
    $('#btnCancel').click(function (e) {
        e.preventDefault();
        validateTabClose = false;
        document.location.href = '/Note/Index';
    });

    $('#btnSaveandRecordtime').click(function (e) {
        e.preventDefault();
        SaveData(2);
        //document.location.href = '/Note/Index';
    });

    $('#btnSave').click(function (e) {
        e.preventDefault();
        SaveData(1);
    });
    setInitialFormValues('NoteAddEdit',true);
});


function Validation() {
    if ($('#NoteTypeCodeID').val().length == 0 && !$('#NoteTypeCodeID').is(':hidden')) {
        Notify('Note Type is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $('#NoteTypeCodeID').focus();
        return false;
    }
   else if ($('#NoteSubject').val() == '') {
        Notify('Subject is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $('#NoteSubject').focus();
        return false;
   }
   else if ($('#NoteEntry').GetText() == '') {
       Notify('Note is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
       $('#NoteEntry').focus();
       return false;
   }
    
    return true;
}
function SaveData(buttonID) {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged('NoteAddEdit')) {
        if (buttonID == 1) {
            document.location.href = '/Note/Index';
            return false;
        } else if (buttonID == 2) {
            document.location.href = '/Case/RecordTime';
            return false;
        }//need to change

        notifyDanger('Nothing was changed.');
        return false;
    }

    var isValid = Validation();
    if (isValid) {
        var data = GetData();
        $.ajax({
            type: "POST",
            url: "/Note/NoteAddEditSave",
            data: data,
            dataType: 'json',
            success: function (data) {
                if (data.isSuccess) {
                    notifySuccess('Note Update Successfully!');

                    RequestSubmitted();
                    validateTabClose = false;
                    if (buttonID==1)
                        document.location.href = '/Note/Index';
                    else
                        document.location.href = '/Case/RecordTime';//need to change
                } else {
                }
            }
        });
    }
    else
    {
        return false;
    }
}

function GetData() {
    var data = {
        'NoteID': $('#NoteID').val(),
        'AgencyID': $('#AgencyID').val(),
        'NoteEntityCodeID': $('#NoteEntityCodeID').val(),
        'NoteEntityTypeCodeID': $('#NoteEntityTypeCodeID').val(),
        'EntityPrimaryKeyID': $('#EntityPrimaryKeyID').val(),
        'NoteTypeCodeID': $('#NoteTypeCodeID').val(),
        'NoteSubject': $('#NoteSubject').val(),
        'NoteEntry': $('#NoteEntry').GetHtml(),
        'CaseID': $('#CaseID').val(),
        'PetitionID': $('#PetitionID').val(),
        'HearingID': $('#HearingID').val(),
        'RecordStateID': $('#RecordStateID').val(),
        'PanelList': [],
        'NoteClientListForAddEdit':[]
    };
    var currentStatus = 0;
    
    var creaditTypeTRs = $(".panelRow .ckePanel");
    for (var indx = 0; indx < creaditTypeTRs.length; indx++) {
        var tr = creaditTypeTRs.eq(indx);
        var checkBox = tr.find(".chkPage");
        if (checkBox.is(":checked"))
            currentStatus = 1;
        else
            currentStatus = 0;

        var paneldata = {
            'CodeID': checkBox.attr('data-codeid'),
            'IsCurrentSelected': currentStatus,
            'Selected': checkBox.attr('data-selected'),
            'NotePanelKey': checkBox.attr('data-notepanelkey'),
        };
        data.PanelList.push(paneldata);
    }
    

    $('.chkClient').each(function () {
      
      
        if ($(this).is(":checked"))
            currentStatus = 1;
        else
            currentStatus = 0;
        
        var paneldata = {
            'NotePersonID': $(this).attr('data-notepersonid'),           
            'Selected': currentStatus,
            'PersonID': $(this).attr('data-personid'),
        };
        data.NoteClientListForAddEdit.push(paneldata);
    });     
    
    //$('.chkPage').each(function () {
    //    if ($(this).is(":checked"))
    //        currentStatus = 1;
    //    else
    //        currentStatus = 0;

        
    //        var paneldata = {
    //            'CodeID': $(this).attr('data-codeid'),
    //            'IsCurrentSelected': currentStatus,
                
    //        };
    //        data.PanelList.push(paneldata);
        
        
    //});
    

    return data;
}



window.onbeforeunload = function (e) {

    if (hasFormChanged('NoteAddEdit') && validateTabClose) {
        return 'There is unsaved data.';
    }
    return undefined;
}