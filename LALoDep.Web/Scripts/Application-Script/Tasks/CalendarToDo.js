
var oTable = $('#searchToDoList').dataTable({
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "searching": false,
    "bSort": false,
    "columns": [
  {
      "data": "ActionType",
      "render": function (data, type, full, meta) {
          return '<a class="go-to-edit" href="' + $('#simplewizard li.active a').attr('href') + '&pdActionId=' + full.PDActionEcryptedID + '" >' + data + '</a>';
      }
  },


 { "data": "ActionNote" },
 { "data": "ActionReminderDate" },
 { "data": "ActionDueDate" }, { "data": "AssignedTo" },

 {
     "render": function (data, type, full, meta) {
         if (full.Completed ==0) {
             return ( '<input type="checkbox" data-id="' + full.PDActionID + '" id="checkboxStatus' + full.PDActionEcryptedID + '" class="chkCompleted form-control input-sm"   />')
         }

         else {
             return '';
         }
     }
 }

    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true,
    fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

        if (aData.PDActionID == $('#PdAction_PDActionID').val())
        {
            $(nRow).addClass('highLightBlue');
        
        }
    }

});



function PopulateData(showAll) {



    $.ajax({
        type: "POST", url: '/Task/CalendarToDoHistory/' + $('#HearingID').val() + '?showAll=' + showAll,
        dataType: "json",
        success: function (data) {
            setData(data);


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });





}


function Validation() {
     
        
    if($('#ActionTypeID').val().length>0 || 
        $('#DueDate').val().length>0 || 
        $('#ReminderDate').val().length>0  
        ){
        if ($('#DueDate').val() != '' && $('#ReminderDate').val() == '') {
            $('#ReminderDate').val($('#DueDate').val());
        } if ($('#ReminderDate').val() != '' && $('#DueDate').val() == '') {
            $('#DueDate').val($('#ReminderDate').val());
        }
  
        if ($('#ActionTypeID').val().length == 0) {
            notifyDanger('Action Type is required.')
            $('#ActionTypeID').focus();
            return false;
        }

        if ($('#DueDate').val().length == 0) {
            notifyDanger('Due Date is required.');
            $('#DueDate').focus();
            return false;
        }
        if ($('#ReminderDate').val().length == 0) {
            notifyDanger('Reminder Date is required.');
            $('#ReminderDate').focus();
            return false;
        }
        if ($('#AssignToStaffID').val().length == 0) {
            notifyDanger('Assign To Staff is required.');
            $('#AssignToStaffID').focus();
            return false;
        }  
    }
    return true;
}
 
function setData(data) {

    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    }

}

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
                if ($(this).attr("id") == 'loading')
                    return;
                calc_height = calc_height - $(this).outerHeight(true);
            }
        });
        _offset = _offset + $(this).outerHeight(true) - $(this).height();
    });

    //console.log("calc :" + calc_height + " offset: " + _offset);
    calc_height = calc_height - _offset;
    //console.log("total: " + calc_height);
    $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
    oTable.fnAdjustColumnSizing();
    //new $.fn.dataTable.FixedHeader(oTable);
    return calc_height;
}


$(window).bind('resize', function () {
    $('#searchToDoList').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});


$(function () {
    PopulateData(0);

    $('#btnShowAll').click(function () {

        if ($(this).val() == 'Show All') {
            $(this).val('Show Active Only');
            PopulateData(1);
        } else {
            $(this).val('Show All');
            PopulateData(0);
        }
    });
    $('#btnSave').click(function () {


        Save(1);
    })
    $('#btnSaveBackToCalendar').click(function () {

              Save(2);
    })
    $('#btnCancel').click(function () {

        document.location.href = $('#simplewizard li.active a').attr('href');
        
    })
    setInitialFormValues('calendartodo-form', true);
});

function BackToCalendar() {
    window.location.href = '/Task/QuickCalMyCalendar?HearingDate=' + $('.hearingDate').data('date');

}
function GetData() {
    var ids='';

    $('.chkCompleted:checked').each(function(){
        if(ids!='')
            ids+=',';
        ids+=$(this).data('id')
    })
    var model = {

        PdActionOldModel: pdActionModel,
        ActionTypeID: $('#ActionTypeID').val()
         , DueDate: $('#DueDate').val()
        , ReminderDate: $('#ReminderDate').val()
        , ActionNote: $('#ActionNote').val()
        , AssignToStaffID: $('#AssignToStaffID').val(),
        PDActionCompletedIds: ids
    };
    return model;


}
function Save(buttonId) {
    IPadKeyboardFix();

    if (!hasFormChanged('calendartodo-form') && $('.chkCompleted:checked').length==0) {

        if (buttonId == 2) {
            BackToCalendar();
            isValid = false;
            return false;
        }
        else if (buttonId == 1) {
            document.location.href = $('#simplewizard li.active a').attr('href');
        }

    }



    if (!IsValidFormRequest()) {
        return;
    }
    var isvalid = Validation();


    if (isvalid) {
        var params = GetData();

        $.ajax({
            type: "POST",
            url: '/Task/CalendarToDo',
            data: { model: params }, 
            dataType: 'json',
            success: function (result) {

                if (result.Status == "Done") {
                    notifySuccess('Data saved successfully!');
                    setTimeout(function () {
                        if (buttonId == 2) {
                            BackToCalendar();
                        }
                        else
                            document.location.href = $('#simplewizard li.active a').attr('href');
                    }, 1000);
                }


            }

        });
    }
}