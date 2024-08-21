
var oTable = $('#searchCalendarNumbering').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": true,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [

        {
            "data": "CalNbr",
            "render": function (data, type, full, meta) {
                if (type === "display") {
                    return '<input tabindex="4" type="text" class="form-control input-sm number txtCalNbr" data-id="' + full.HearingID + '" data-oldvalue="' + (full.CalNbr !== null ? full.CalNbr : "") + '" value="' + (full.CalNbr !== null ? full.CalNbr : "") + '"/>';
                }
                else {
                    return data; 
                }
            }
        },
        { "data": "CaseNbr" },
        { "data": "HearingType" },
        { "data": "CaseName" },
        { "data": "Minors" }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true


});


$("#searchCalendarNumbering_wrapper th").on("click", function () {
    //$("#searchCalendarNumbering_wrapper th").removeClass('active');
    //$(this).addClass('active');
    //Search();
});
$("#btnSearch").on("click", function () {
    if (IsAnyChange()) {
        
        confirmBox("You have changed one or more of the CAL # values, do you want to save your changes?", function (result2) {
            if (result2) {
                SaveData(true);
            }
            else {
                Search()
            }
        });
     
    }else{
    Search()
    }
});

$("#btnSave").on("click", function () {
    if (IsAnyChange()) {
        SaveData(true);
    } else {
        Notify('Nothing was changed.', 'bottom-right', '5000', 'info', 'fa-info', true);
      
    }

});
function SaveData(search) {
    var arr = [];
    var el; var flag = false;
    var emptyExists = false;
    var outside = false;

    $('.txtCalNbr').each(function () {
        if ($(this).val() != '' && $(this).val() != '99' && flag == false) {
            if (arr.length > 0 && arr.indexOf($(this).val()) > -1) {
                flag = true;
                el = $(this);

            }
            if ($(this).val() <= 0 || $(this).val() > 25) {
                outside = true;
            }
            arr.push($(this).val())

        } else if ($(this).val() === '') {
            emptyExists = true;
        }

    });
    if (flag) {
        Notify('Duplicate CAL #s are disallowed unless the value is 99.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $(el).focus();
        return false;
    }

    if (emptyExists) {

        confirmBox("There are hearing without a CAL #, do you want to continue?", function (result) {
            if (result) {
                if (outside) {
                    confirmBox("A CAL # is outside of 1-25 or 99, do you want to continue?", function (result2) {
                        if (result2) {
                            SaveCalendarNumber();
                        }
                        else {

                        }
                    });
                } else {
                    SaveCalendarNumber();
                }
            }
            else {

            }
        });
    } else if (outside) {
        confirmBox("A CAL # is outside of 1-25 or 99, do you want to continue?", function (result2) {
            if (result2) {
                SaveCalendarNumber();
            }
            else {

            }
        });
    }

}
function SaveCalendarNumber() {
    var model = { 'CalNbrs': [] };

    $('.txtCalNbr').each(function () {
        if ($(this).val() != $(this).attr('data-oldvalue')) {
            model.CalNbrs.push({
                'CalNbr': $(this).val(),
                'HearingID': $(this).attr('data-id')
            });
        }
    });
    var params = model;
    $.ajax({
        type: "POST", url: '/Task/CalendarNumberingUpdate', data: { model: params  },
        success: function (result) {
            if (result.Status == "Done") {
                Notify('Calendar Number Updated Successfully!.', 'bottom-right', '3000', 'success', 'fa-smile-o', true);
                Search();
            }
        }
    });
}
function IsAnyChange() {
    var flag = false;
    $('.txtCalNbr').each(function () {
        if ($(this).val() != $(this).attr('data-oldvalue')) {
            flag = true;
        }
    });
    return flag;
}
$('#searchCalendarNumbering').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchCalendarNumbering_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchCalendarNumbering_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }
});

function Search() {

    var $form = $('#searchCalendarNumbering-form');
    IPadKeyboardFix();
    if ($("#Date").val() == '') {
        Notify('Date is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $("#Date").focus();
        return false;
    }
    if ($("#DepartmentID").val() == '') {
        Notify('Department is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        $("#DepartmentID").focus();
        return false;
    }


    var data = getData();
    $.ajax({
        type: "POST", url: '/Task/CalendarNumbering', data: data,
        success: function (data) {
            setData(data);
        },
        dataType: 'json'
    });

}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        $('#btnSave').removeClass('hidden');
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else {
        $('#btnSave').addClass('hidden')
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);

    }


}

function getData() {
    
    var fData = $('#searchCalendarNumbering-form').serialize() + '&sortoption=' + $("#searchCalendarNumbering_wrapper th.active").attr('data-col');
    return fData;
}

$(window).bind('resize', function () {

    fitCalculatedHeightForSearchDataTable();
});
function onCaseNumberClick(e) {
    var $this = $(this);
    e.preventDefault();
    document.location = $this.attr('href');
}

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    if (oTable != null) {
        calc_height = $(window).height();
        var _offset = 25;

        $("#divSearchResult .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
            $(this).siblings().each(function () {
                if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                    if ($(this).attr("id") == 'loading')
                        return;
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        calc_height = calc_height - _offset;
        calc_height = calc_height < 250 ? 250 : calc_height;
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}
