var page = (function () {
    return {
        $form: $('#CountyCounselAddEdit-search-form'),
        $table: $('#CountyCounselAddEdit'),
        mainKey: 'CountyCounselAddEdit-search-form',
        resultPageIdKey: 'CountyCounselAddEdit-search-form-page#',
        rowSelectedKey: 'CountyCounselAddEdit-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

$(document).ready(function () {
    var personId = $("#Id").val();
    $.ajax({
        type: "GET", url: '/CountyCounselList/GetAllCountyCheckboxes/' + personId,
        success: function (data) {
            setData(data);
            eventChangeBind();
        },
        dataType: 'json'
    });
});
var oTable = $('#CountyCounselAddEdit').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        {
            "render": function (data, type, full, meta) {
                if (full.Selected == 1) {
                    return ('<input type="checkbox" id="' + full.AgencyID + '" value="' + full.AgencyID + '" data-role-id="' + full.RoleID + '" data-role-start-date="' + full.RoleStartDate + '" data-checked="true" checked="checked" >');
                } else {
                    return ('<input type="checkbox" id="' + full.AgencyID + '" value="' + full.AgencyID + '" data-role-id="' + full.RoleID + '" data-role-start-date="' + full.RoleStartDate + '" data-checked="false">');
                }
            }
        },
    { "data": "AgencyName" }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "fnDrawCallback": function (oSettings) {
        $(oSettings.nTHead).hide();
    },
    "deferRender": true
});

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#CountyCounselAddEdit').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#CountyCounselAddEdit_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#CountyCounselAddEdit_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$("#save").on("click", function () {
    saveData();
});

$("#btnCancel").on("click", function () {
    window.location.href = "/CountyCounselList/Search";
});

function eventChangeBind() {
    //$("input[type='checkbox']").bind("change", function () {
    //    var attr = $(this).attr('changed');
    //    if (typeof attr !== typeof undefined && attr !== false) {
    //        $(this).removeAttr("changed");
    //    }
    //    else {
    //        $(this).attr("changed", "");
    //    }
    //});
}
function saveData() {
    IPadKeyboardFix();
    if ($("#LastName").val() == "") {
        notifyDanger("Last Name is required.")
        $("#LastName").focus();
        return true;
    }
    if ($("#FirstName").val() == "") {
        notifyDanger("First Name is required.")
        $("#FirstName").focus();
        return true;
    }
    if ($("#StartDate").val() == "") {
        notifyDanger("Start Date is required.")
        $("#StartDate").focus();
        return true;
    }

    if ($("#EndDate").val() != '' && new Date($("#StartDate").val()) > new Date($("#EndDate").val())) {
        $("#EndDate").focus();
        notifyDanger("End date can not be earlier than start date.");
        return false;
    }


    if ($('#CountyCounselAddEdit input:checkbox:checked').length == 0) {
        notifyDanger("At least one agency must be selected.")
        $('#CountyCounselAddEdit input:checkbox:first').focus();
        return true;
    }

    var checkboxes = [];
    var form = getData();
    $("input[type='checkbox']").each(function () {
        //     if ($(this).data("checked") != $(this).is(":checked")) {
        checkboxes.push({
            AgencyId: $(this).val(),
            Selected: $(this).prop('checked'),
            Changed: $(this).data("checked") != $(this).is(":checked"),

            RoleID: ($(this).data("role-id") == 'null') ? 0 : $(this).data("role-id"),
            RoleStartDate: $("#StartDate").val()
                ,
            RoleEndDate: $("#EndDate").val()
        });
        //  }
    });
    var data = {
        person: form,
        checkboxes: checkboxes
    }
    $.ajax({
        type: "POST", url: '/CountyCounselList/UpdateCountyCounsel',
        data: data,
        success: function (data) {
            window.location.replace("/CountyCounselList");
        },
        dataType: 'json'
    });
}
function getData() {
    var fData = $('#CountyCounselAddEdit-search-form').serializeObject();
    fData.AgencyID = $model.AgencyID;
    fData.RecordStateID = $model.RecordStateID;
    fData.RoleID = $model.RoleID;
    return fData;
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);


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
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}

(function ($) {
    $.fn.serializeObject = function () {

        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };


        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };

        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each($(this).serializeArray(), function () {

            // skip invalid keys
            if (!patterns.validate.test(this.name)) {
                return;
            }

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while ((k = keys.pop()) !== undefined) {

                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                    // fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }

                    // named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }

            json = $.extend(true, json, merge);
        });

        return json;
    };
})(jQuery);