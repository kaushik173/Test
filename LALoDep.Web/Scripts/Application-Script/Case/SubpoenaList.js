
function GetData() {
    var data = {
        SubpoenaList: []
    };

    var subpoenaTr = $("#tblSubpoena tbody tr");
    for (var indx = 0; indx < subpoenaTr.length; indx++) {
        $tr = subpoenaTr.eq(indx);
        $date = $tr.find(".subpoenaServerdDate");

        if ($date.val() != $tr.data("subpoenaserveddate")) {
            var subpoenaList = {
                SubpoenaID: $tr.data("subpoenaid"),
                AgencyID: $tr.data("agencyid"),
                HearingID: $tr.data("hearingid"),
                SubpoenaServedToRoleID: $tr.data("subpoenaservedtoroleid"),
                SubpoenaServedDate: $date.val(),
                RecordStateID: $tr.data("recordstateid"),
            };
            data.SubpoenaList.push(subpoenaList);
        }
    }
    return data;
}

function SaveData() {
    var subpoenaTr = $("#tblSubpoena tbody tr");
    var isChange = false;
    for (var indx = 0; indx < subpoenaTr.length; indx++) {
        $tr = subpoenaTr.eq(indx);
        $date = $tr.find(".subpoenaServerdDate");

        if ($date.val() != $tr.data("subpoenaserveddate")) {
            isChange = true;
            break;
        }
    }
    if (!isChange) {
        notifyDanger('No changes were entered.');
        return false;
    }
    var data = JSON.stringify(GetData());
    $.ajax({
        type: "POST", dataType: 'json', url: '/Case/SubpoenaListSave', data: data, contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                notifyDanger('Subpoena has been saved successfully.');
            }
            else {
                notifyDanger('There is something wrong while processing request.');
            }
        }
    });

}

$('.chkPrintAll').click(function () {
    $('.chkPrint').prop('checked', $(this).is(':checked'));
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Case/SubpoenaDelete/' + id,
                dataType: "json",
                success: function (data) {
                    tr.remove();
                    notifyDanger('Record delete successfully.');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
});

$('#btnAddNew').on("click", function () {
    if ($('#HearingID').val() == '') {
        $('#HearingID').focus();
        notifyDanger('Hearing is required to add a subpoena.');
        return false;
    }
    else {
        window.location.href = '/Case/SubpoenaAddEdit/' + $('#HearingID').val();
    }
})

$('#btnSave').on("click", function() {
    SaveData();
});
$('#btnPrint').on("click", function () {
    var ids = '';
    $('.chkPrint:checked').each(function() {
        if (ids != '')
            ids += ",";
        ids += $(this).attr('data-id');
    });
    var data = {
        id: ids

    }
    
   $.download($('#hdnCurrentSessionGuidPath').val()+'/Case/SubpoenaPrint/' + ids, data, "POST", 'target="_blank"');

});

$('#btnPrintBlank').on("click", function () {
    var ids = '';
    
    var data = {
        id: ids

    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/Case/SubpoenaPrint/' + ids, data, "POST", 'target="_blank"');

});
