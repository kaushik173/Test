
$(document).ajaxStart(function () {
    $('#loading').show();
    isBottomBarAccessible(false);
    HideLoadingPanel();
});
$(document).ajaxStop(function () {
    $('#loading').hide();
    isBottomBarAccessible(true);
});
$(document).ajaxComplete(function () {
    $('#loading').hide();
    isBottomBarAccessible(true);

});
$(document).ajaxError(function (event, xhr, settings) {
    $('#loading').hide();
    isBottomBarAccessible(true);
    if (xhr.statusText === "User is not authenticated!") {
        document.location.href = '/Account/Login';
    } else if (xhr.status == 500) {
        //   document.location.href = '/Home/Error';

    }
});
$(document).ajaxSuccess(function () {
    $('#loading').hide();
    isBottomBarAccessible(true);
});
var loadingPanelTimeout;
function HideLoadingPanel() {
    clearTimeout(loadingPanelTimeout);
    loadingPanelTimeout = setTimeout(function () {
        $('#loading').hide();
        isBottomBarAccessible(true);
    }, 50000);
}
$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    options.url = options.url;
    if (Contains(options.url, '?')) {
        options.url = options.url + '&_uniquerequest=' + randomRange(1, 1000000000000) + '&CurrentSessionGuid=' + $('#hdnCurrentSessionGuid').val();
    } else {
        options.url = options.url + '?_uniquerequest=' + randomRange(1, 1000000000000) + '&CurrentSessionGuid=' + $('#hdnCurrentSessionGuid').val();

    }


});
//Toast Notifications
var notifyDanger = function (message) { Notify(message, 'bottom-right', '4000', 'danger', 'fa-warning', true); }
var notifyWarning = function (message) { Notify(message, 'bottom-right', '4000', 'warning', 'fa-warning', true); }
var notifyInfo = function (message) { Notify(message, 'bottom-right', '3000', 'info', 'fa-info', true); }
var notifySuccess = function (message) { Notify(message, 'bottom-right', '3000', 'success', 'fa-check', true); }
var notifyBlue = function (message) { Notify(message, 'bottom-right', '3000', 'blue', 'fa-info', true); }

function isBottomBarAccessible(access) {
    if (access) {
        setTimeout(function () {
            $('button,[type=submit],[type=button],a', '#fixed-button-bar').not('.btn-disabled').prop("disabled", !access);
            $('button,[type=submit],[type=button],a', '.fixedbar').not('.btn-disabled').prop("disabled", !access);
            $('button,[type=submit],[type=button],a', '.boxfloat').not('.btn-disabled').prop("disabled", !access);
            $('.fixedbar button,.fixedbar [type=submit],.fixedbar [type=button]').not('.btn-disabled').prop("disabled", !access);
            $('button,[type=submit],[type=button]').not('.btn-disabled').prop("disabled", !access);
        }, 3000)
        $('.btn-normal', '#fixed-button-bar').not('.btn-disabled').prop("disabled", !access);
        $('.btn-normal', '.fixedbar').not('.btn-disabled').prop("disabled", !access);
        $('.btn-normal', '.boxfloat').not('.btn-disabled').prop("disabled", !access);
        $('.fixedbar .btn-normal').not('.btn-disabled').prop("disabled", !access);
        $('button,[type=submit],[type=button]').not('.btn-disabled').not('.btn-ignore').prop("disabled", !access);

    } else {
        $('button,[type=submit],[type=button],a', '#fixed-button-bar').prop("disabled", !access);
        $('button,[type=submit],[type=button],a', '.fixedbar').prop("disabled", !access);
        $('button,[type=submit],[type=button],a', '.boxfloat').prop("disabled", !access);
        $('.fixedbar button,fixedbar [type=submit],fixedbar [type=button]').prop("disabled", !access);
        $('button,[type=submit],[type=button]').not('.btn-disabled').not('.btn-ignore').prop("disabled", !access);
    }

}

function randomRange(min, max) {
    return (Math.random() * (max - min + 1)) + min;
}

function GetWindowID() {

    return $('#hdnCurrentSessionGuid').val();//sessionStorage.tabID;
}
function RefreshWindowID() {

    sessionStorage.tabID = 'SEAppId' + (new Date()).getTime();
}
function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function GenerateWindowID() {
    if (Contains(document.location.href, '?newSession=True')) {
        RefreshWindowID();

        document.location.href = '/Case/Search';
    } else {
        //var tabIDFromCoookie = sessionStorage.tabID ? sessionStorage.tabID : sessionStorage.tabID = readCookie('CurrentWindowID');
        var tabIDFromCoookie = (sessionStorage.tabID != null && sessionStorage.tabID != "null") ? sessionStorage.tabID : sessionStorage.tabID = readCookie('CurrentWindowID');
    }

    //var tabID = sessionStorage.tabID ? sessionStorage.tabID : sessionStorage.tabID = 'SEAppId' + (new Date()).getTime();
    var tabID = (sessionStorage.tabID != null && sessionStorage.tabID != "null") ? sessionStorage.tabID : sessionStorage.tabID = 'SEAppId' + (new Date()).getTime();


    strCookie = 'CurrentWindowID=' + tabID + ';';
    strCookie += ' path=/;';
    if (window.location.protocol.toLowerCase() == 'https:') {
        strCookie += ' secure;';
    }
    document.cookie = strCookie;
}


$(function () {
    //Events 
    GenerateWindowID();
    $(window).focus(function () { GenerateWindowID(); });
    $(window).mouseover(function () {
        GenerateWindowID();
    });

    $("a.menu-item").on("click", function (e) {
        e.preventDefault();

        var element = $(this);
        simpleStorage.deleteKey(element.attr("data-main-key") + GetWindowID());
        simpleStorage.deleteKey(GetWindowID() + element.attr("data-result-page-id-key"));
        simpleStorage.deleteKey(GetWindowID() + element.attr("data-row-selected-key"));

        document.location.href = $(this).attr('href');
    });

    $('.setSelectedValue').each(function () {

        $(this).val($(this).attr('data-default-value'));
    });

    //unbind shortcuts
    for (i = 0; i < 10; i++) {
        setTimeout(function () {
            $(window).unbind("keydown");
        }, i * 1000);

    }
    $('body').on("keypress", '.number', function (event) {
        return isNumber(event, this);
    });
    //For Convert Loercase to upercase
    $('.uppercase').on('blur', function () {
        $(this).val(($(this).val()).toUpperCase());
    });

    //Prevent char in numeric field
    $('body').on('keypress', '.numeric-val', function (event) {
        if ($(this).hasClass('allowDash')) {
            return isNumberWithCommaAndDash(event);
        }
        return isNumberWithDecimal(event);
    });

    $('body').on('keypress', '.number-only', function (event) {
        return isNumberOnly(event);
    });

    $('.phone_us').on('input', function () {
        var str = $(this).val();
        if (str.match(/[^0-9()\s-]|.{14}\s/)) {
            $(this).unmask().val(str);
        }
        else {
            $(this).mask('(000) 000-0000');
        }
    });

    //--JQuery Autosize--
    $('textarea').autosize({ append: "\n" });
    $('input:text').attr('autocomplete', 'off');

    $('.date-picker').datepicker().on("changeDate", function (e) {

        $(e.target).trigger('blur');
    }).off('focus');


    $("body").on("click", ".datepicker-trigger", function () {
        //if ($('.date-picker').val() == '') {
        //    $('.date-picker').datepicker("setValue", getDate()).datepicker("update");
        //}
        //$(".date-picker").datepicker("show");
        var $txtDatePicker = $(this).parents(".input-group").find(".date-picker");
        if (!$txtDatePicker.is(':disabled')) {
            if ($txtDatePicker.val() == '') {
                $txtDatePicker.datepicker("setValue", getDate()).datepicker("update");
            }
            $txtDatePicker.datepicker("show");
        }
    });


    $('.time-picker').timepicker({ minuteStep: 1 });

    $('.time-picker').on("keypress", function (event) {
        //keycode 9 = tab
        if (event.keyCode == '9')
            $('.time-picker').timepicker('hideWidget')
    });

    $("body").on('changeDate', '.date-picker', function (ev) {
        DateOfBirthCheck($(this));
        // $(this).datepicker('hide');
    });

    $("body").on("blur", '.date-picker', function () {
        $value = $(this).val();
        if ($value != '' && $value != null) {
            $date = dateFormat($value);
            if ($date == '' || !moment($date, 'M/D/YYYY').isValid()) { $(this).val(''); notifyDanger('Invalid date'); $(this).focus(); return false; }
            else if (moment($date, 'M/D/YYYY') < moment('01/01/1800', 'M/D/YYYY')) { notifyDanger('Invalid date. Date must be later than 01/01/1800'); $(this).val(''); $(this).focus(); return false; }
            else {
                if ($date != $value) {
                    $(this).val($date);
                    $(this).datepicker("update");
                }
            }
            DateOfBirthCheck($(this));
        }
    });



    $('.date-picker-control').datepicker().off('focus');
    $("body").on("click", ".datepicker-control-trigger", function () {

        if (!$(this).prev().is(':disabled')) {
            if ($(this).prev().val() == '') {
                $(this).prev().datepicker("setValue", getDate()).datepicker("update");

            }
            $(this).prev().datepicker("show");
        }

    });


    $('body').on('changeDate', '.date-picker-control', function (ev) {
        DateOfBirthCheck($(this));
        // $(this).datepicker('hide');
    });

    $('body').on("blur", '.date-picker-control', function () {
        $value = $(this).val();


        if ($value != '' && $value != null) {
            $date = dateFormat($value);
            if ($date == '' || !moment($date, 'M/D/YYYY').isValid()) {
                $(this).val('');
                notifyDanger('Invalid date');

                $(this).focus();
                return false;
            }
            else if (moment($date, 'M/D/YYYY') < moment('01/01/1800', 'M/D/YYYY')) { notifyDanger('Invalid date. Date must be later than 01/01/1800'); $(this).val(''); $(this).focus(); return false; }
            else {
                if ($date != $value) {
                    $(this).val($date);
                    $(this).datepicker("update");
                }
            }
            DateOfBirthCheck($(this));
        }
    });
    $('body').on('focusout', '.date-picker,.date-picker-control', function (ev) {

        DateOfBirthCheck($(this));

    });
    var date = $('.date-picker');

    var _oldStopCallback = Mousetrap.stopCallback;

    Mousetrap.stopCallback = function (e, element, combo) {
        if (combo == 'u' || combo == 'd' || combo == 't') {
            console.log(!$(element).hasClass("date-picker") && !$(element).hasClass("date-picker-control"))
            return !$(element).hasClass("date-picker") && !$(element).hasClass("date-picker-control");
        }
        return _oldStopCallback(e, element, combo);
    }

    Mousetrap.bind('u', function (e) {
        if ($(e.target).hasClass("date-picker")) {
            $(e.target).val(IncrementDecrimentDate($(e.target).val(), true));
        } else if ($(e.target).hasClass("date-picker-control")) {
            $(e.target).val(IncrementDecrimentDate($(e.target).val(), true));
        }
        e.preventDefault();
    });
    Mousetrap.bind('d', function (e) {
        if ($(e.target).hasClass("date-picker")) {
            $(e.target).val(IncrementDecrimentDate($(e.target).val(), false));
        } else if ($(e.target).hasClass("date-picker-control")) {
            $(e.target).val(IncrementDecrimentDate($(e.target).val(), false));
        }
        e.preventDefault();
    });

    Mousetrap.bind('t', function (e) {
        if ($(e.target).hasClass("date-picker")) {
            $(e.target).val(todayDate());
        } else if ($(e.target).hasClass("date-picker-control")) {
            $(e.target).val(todayDate());
        }
        e.preventDefault();
    });
    Mousetrap.bind(['ctrl+n'], function (e) {

        if (e.preventDefault) {
            e.preventDefault();
        } else {

            // internet explorer
            e.returnValue = false;
        }
        jQuery('<form  target="_blank" action="/" method="GET"><input type="hidden" name="newSession" value="True"/></form>')
            .appendTo('body').submit().remove();
        return false;
    });

    function todayDate() { return getDate(); }

    function getDate(d) {
        d = (d || new Date());

        var month = d.getMonth() + 1;
        var day = d.getDate();
        return (month < 10 ? '0' : '') + month + '/' +
            (day < 10 ? '0' : '') + day + '/' + d.getFullYear();
    }
    function IncrementDecrimentDate(value, bIncriment) {
        var date = value;
        if (date.indexOf('/') != -1) {
            $datePart = date.split('/');
            $countPart = $datePart.length;
            $digitsSlashes = /^[0-9\/]*$/.test(date);

            if ($countPart != 3) {
                value = '';
            }
        } else {
            value = '';
        }
        if (value == '') {
            value = todayDate();
        }
        dt = new Date(value);
        if (bIncriment)
            dt.setDate(dt.getDate() + 1);
        else
            dt.setDate(dt.getDate() - 1);
        return getDate(dt);
    }

    var $userbox = $('#userbox');

    $userbox.on('click', function (e) {
        if (!e.target.href) {
            e.preventDefault();
            e.stopPropagation();
            var $this = $(this);
            $this.toggleClass('open');
            var $dd = $this.children('.dropdown-menu');
            if ($this.hasClass('open')) {
                $dd.show();
            } else {
                $dd.hide();
            }
        }
    });

    jQuery.download = function (url, data, method, target) {

        method = (method || 'POST');
        target = (target || '');
        //url and data options required
        if (url && data != null) {
            //data can be string of parameters or array/object
            data = typeof data == 'string' ? data : (method == "POST") ? unescape(jQuery.param(data)) : jQuery.param(data);
            //split params into form inputs
            var inputs = '';
            jQuery.each(data.split('&'), function () {
                var pair = this.split('=');
                var name = pair[0]; var value = '';
                if (pair.length > 1 && pair[1] != null) value = pair[1].split("+").join(" ");
                inputs += '<input type="hidden" name="' + name + '" value="' + value + '" />';
            });
            //send request
            jQuery('<form  ' + target + ' action="' + url + '" method="' + method + '">' + inputs + '</form>')
                .appendTo('body').submit().remove();
        };
    };

    //// Theme changing event.
    //$('.colorpicker .colorpick-btn').on('click', function (e) {
    //    e.preventDefault();
    //    var themeUrl = $(this).attr('rel');
    //    $.ajax({
    //        type: "POST",
    //        url: '/Home/SelectTheme',
    //        data: { 'ThemeUrl': themeUrl },
    //        dataType: 'json',
    //        success: function (data) {
    //            if (data.Result == 'OK') {
    //                document.location.href = document.location.href;
    //            } else {
    //                Notify(data.Message, 'bottom-right', '3000', 'danger', 'fa-warning', true);

    //            }
    //        }
    //    });

    //});

    $('.note-detail').click(function (e) {
        e.preventDefault();

        $.ajax({
            type: "POST",
            url: $(this).attr('data-detail-url'),


            success: function (data) {
                $('#ModelContent').html(data);
                $('#pageModal').modal('show');

                $('#pageModalLabel').text('Notes');
            },
            error: function (returnValue) { }
        });

    });
    $('body').on('click', '.note-link', function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: $(this).attr('data-detail-url'),


            success: function (data) {
                $('#ModelContent').html(data);

            },
            error: function (returnValue) { }
        });
    });
    //$('.widget-buttons *[data-toggle="collapse"]').on("click", function (event) {
    //    event.preventDefault();
    //    var widget = $(this).parents(".widget").eq(0);
    //    var body = widget.find(".widget-body");
    //    var button = $(this).find("i");
    //    var down = "fa-plus";
    //    var up = "fa-minus";
    //    var slidedowninterval = 300;
    //    var slideupinterval = 200;
    //    if (widget.hasClass("collapsed")) {
    //        if (button) {
    //            button.addClass(up).removeClass(down);
    //        }
    //        widget.removeClass("collapsed");
    //        body.slideUp(0, function () {
    //            body.slideDown(slidedowninterval);
    //        });
    //    } else {
    //        if (button) {
    //            button.addClass(down)
    //                .removeClass(up);
    //        }
    //        body.slideUp(slideupinterval, function () {
    //            widget.addClass("collapsed");
    //        });
    //    }
    //});


    $('#PagePopup').on('show.bs.modal', function (e) {

        $('#PagePopup iframe').attr('src', $('#PagePopup').attr('data-src'));
        $('#PagePopup iframe').attr('height', $('#PagePopup').attr('data-height'));
    });
    $('#PagePopup').on('hidden.bs.modal', function (e) {

        $('#PagePopup iframe').attr('src', '');
    });

    //Code for prevent backspace for select dropdown
    $('select').keypress(function (event) { return cancelBackspace(event) });
    $('select').keydown(function (event) { return cancelBackspace(event) });

    //daterangePicker with no linked calendar
    $('.daterange').daterangepicker({ autoClose: true, "linkedCalendars": false, }, function (start, end) {

        $('#StartDate').val(start.format('MM/DD/YYYY'));
        $('#EndDate').val(end.format('MM/DD/YYYY'));
    });

    $(document).on("keydown", function (e) {
        if (e.altKey) { $("#fixed-button-bar button:first").focus(); }
    });
    setHotKeys();

    if (!$("#fixed-button-bar button.default:first").hasClass('btn-primary')) {
        $("#fixed-button-bar button.default:first").addClass('btn-primary').removeClass('btn-default');
    }


    $('body').on('change', 'input:text, textarea', function (e) {

        $(this).val($.trim($(this).val()));
    });
});
function dateFormat(date) {
    if (date.indexOf('/') != -1) {
        $datePart = date.split('/');
        $countPart = $datePart.length;
        if ($countPart == 3) {


            console.log(moment(date).isValid())
            $month = parseInt($datePart[0]);
            $day = parseInt($datePart[1]);
            $year = $datePart[2];
            if (!moment(date).isValid()) {
                date = '';
                return date;
            }
            if ($month < 10) {
                $month = '0' + $month;
            }
            if ($day < 10) {
                $day = '0' + $day;
            }
            if ($year.toString().length == 2) {
                if (parseInt($year) > 30) { $year = '19' + $year; }
                else { $year = '20' + $year; }
            }
            else if ($year.toString().length == 4) {
                $year = $year;
            }
            else {
                date = '';
                return date;
            }

            if ((parseInt($month) == 4 || parseInt($month) == 6 || parseInt($month) == 9 || parseInt($month) == 11) && $day <= 30) {
                date = $month + '/' + $day + '/' + $year;
            }
            else if ((parseInt($month) == 2 && $day <= 28) || ((parseInt($month) == 2 && $day == 29) && (((parseInt($year) % 4 == 0) && (parseInt($year) % 100 != 0)) || (parseInt($year) % 400 == 0)))) {
                date = $month + '/' + $day + '/' + $year;
            }
            else if ((parseInt($month) == 1 || parseInt($month) == 3 || parseInt($month) == 5 || parseInt($month) == 7 || parseInt($month) == 8 || parseInt($month) == 10 || parseInt($month) == 12) && $day <= 31) {
                date = $month + '/' + $day + '/' + $year;
            }
            else {
                date = '';
            }
            return date;
        }
        else {
            date = '';
            return date;
        }
    }
    else if (date.indexOf('-') != -1) {
        $datePart = date.split('-');
        $countPart = $datePart.length;
        $digitsSlashes = /^[0-9\/]*-/.test(date);

        if ($countPart == 3 && $digitsSlashes) {
            $month = parseInt($datePart[0]);
            $day = parseInt($datePart[1]);
            $year = $datePart[2];
            if (!moment(date).isValid()) {
                date = '';
                return date;
            }
            if ($month < 10) {
                $month = '0' + $month;
            }
            if ($day < 10) {
                $day = '0' + $day;
            }
            if ($year.toString().length == 2) {
                if (parseInt($year) > 30) { $year = '19' + $year; }
                else { $year = '20' + $year; }
            }
            else if ($year.toString().length == 4) {
                $year = $year;
            }
            else {
                date = '';
                return date;
            }

            if ((parseInt($month) == 4 || parseInt($month) == 6 || parseInt($month) == 9 || parseInt($month) == 11) && $day <= 30) {
                date = $month + '/' + $day + '/' + $year;
            }
            else if ((parseInt($month) == 2 && $day <= 28) || ((parseInt($month) == 2 && $day == 29) && (((parseInt($year) % 4 == 0) && (parseInt($year) % 100 != 0)) || (parseInt($year) % 400 == 0)))) {
                date = $month + '/' + $day + '/' + $year;
            }
            else if ((parseInt($month) == 1 || parseInt($month) == 3 || parseInt($month) == 5 || parseInt($month) == 7 || parseInt($month) == 8 || parseInt($month) == 10 || parseInt($month) == 12) && $day <= 31) {
                date = $month + '/' + $day + '/' + $year;
            }
            else {
                date = '';
            }
            return date;
        }
        else {
            date = '';
            return date;
        }
    } else {
        //mmddyyyy
        if (date.length == 8) {
            $month = parseInt(date.substr(0, 2));
            $day = parseInt(date.substr(2, 2));
            $year = date.substr(4, 4);
        }
        //mmddyy

        else if (date.length == 6) {
            $month = parseInt(date.substr(0, 2));
            $day = parseInt(date.substr(2, 2));
            $year = date.substr(4, 2);
            if (parseInt($year) > 30) { $year = '19' + $year; }
            else { $year = '20' + $year; }
        }
        //mdyy
        else if (date.length == 4) {
            $month = parseInt(date.substr(0, 1));
            $day = parseInt(date.substr(1, 1));
            $year = date.substr(2, 2);
            if (parseInt($year) > 30) { $year = '19' + $year; }
            else { $year = '20' + $year; }
        }
        else {
            date = '';
            return date;
        }
        if ($month < 10) {
            $month = '0' + $month;
        }
        if ($day < 10) {
            $day = '0' + $day;
        }
        //check for valid date
        if ((parseInt($month) == 4 || parseInt($month) == 6 || parseInt($month) == 9 || parseInt($month) == 11) && $day <= 30) {
            date = $month + '/' + $day + '/' + $year;
        }
        else if ((parseInt($month) == 2 && $day <= 28) || ((parseInt($month) == 2 && $day == 29) && (((parseInt($year) % 4 == 0) && (parseInt($year) % 100 != 0)) || (parseInt($year) % 400 == 0)))) {
            date = $month + '/' + $day + '/' + $year;
        }
        else if ((parseInt($month) == 1 || parseInt($month) == 3 || parseInt($month) == 5 || parseInt($month) == 7 || parseInt($month) == 8 || parseInt($month) == 10 || parseInt($month) == 12) && $day <= 31) {
            date = $month + '/' + $day + '/' + $year;
        }
        else {
            date = '';
        }


        return date;
    }
}
var dateOfBirthCheckInterval;
function DateOfBirthCheck(el) {
    if (!$(el).hasClass('DOBValidation') && !$(el).hasClass('WeekendValidation')) {
        return false;
    }
    var date = $(el).val();
    if (date.indexOf('/') != -1) {
        $datePart = date.split('/');
        $countPart = $datePart.length;
        $digitsSlashes = /^[0-9\/]*$/.test(date);

        if ($countPart == 3) {
            if ($datePart[2].length != 4) {
                console.log($datePart[2])
                return false;
            }
        }
    } else {
        return false;
    }
    var flag = true;
    clearTimeout(dateOfBirthCheckInterval);
    dateOfBirthCheckInterval = setTimeout(function () {
        $date = dateFormat($(el).val());
        if ($(el).hasClass('DOBValidation')) {

            if ($(el).val().length > 0 && moment($(el).val()) > moment()) {
                var msg = 'DOB cannot be in the future';


                notifyDanger(msg);
                //   $(el).val('');
                $(el).focus();
                flag = false;
                return false;
            }
        }
        else if ($(el).hasClass('WeekendValidation')) {

            if ($(el).val().length > 0 && (moment($(el).val()).day() == 0 || moment($(el).val()).day() == 6)) {
                var msg = $(el).data('label') + ' cannot be on a weekend';


                notifyDanger(msg);
                //   $(el).val('');
                $(el).focus();
                flag = false;
                return false;
            }
        }

    }, 1000);
    return flag;

}
$(document).on({
    'click': function (e) {
        e.preventDefault();
        var _target = $("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';
        //$.download($(this).attr("href"), "", "POST", $(this).attr("target") !== undefined ? 'target="_blank"' : '');

        if (Contains($(this).attr("href"), '/g/')) {
            $.download($(this).attr("href"), "", "POST", _target);
        } else {
            $.download('/g/' + $('#hdnCurrentSessionGuid').val() + $(this).attr("href"), "", "POST", _target);
        }
    }
}, ".auto-download");


/*This is to make the one button as default button in every page. All we have to do is add default to the class list of the button */
$(document).on({
    "keypress": function (e) {

        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            if ($('.confirm-box').is(':visible')) {
                $('.confirm-box .btn-primary').click();
                return;
            }
            if ($(".note-editable").is(":focus") || $(e.target).attr('class') == 'note-editable') {


            } else if ($(".ignore-enterkey").is(":focus")) {

            }
            else {


                if ($("select").is(":focus")) {
                    if ($("select:focus").hasClass('eventDDl')) {

                        return true;
                    }
                    var defaultBtn = $('button[type=submit].default');
                    if (defaultBtn.length > 0 && !$(defaultBtn).is(':focus')) {

                        defaultBtn[0].click();

                    }
                    return false;


                }
                else if (!$("textarea").is(":focus")) {
                    if ($("input.date-picker-control, input.date-picker").is(":focus")) {
                        var input = $('input.date-picker-control:focus, input.date-picker:focus');
                        if (input.val().length > 0) {
                            $date = dateFormat(input.val());
                            if ($date == '' || !moment($date, 'M/D/YYYY').isValid()) {
                                $(input).val('');
                                notifyDanger('Invalid date');

                                $(input).focus();

                                return false;
                            }
                            else if (moment($date, 'M/D/YYYY') < moment('01/01/1800', 'M/D/YYYY')) {
                                $(input).val('');
                                notifyDanger('Invalid date. Date must be later than 01/01/1800'); $(input).focus(); return false;
                            }
                            else {
                                $(input).val($date);
                                $(input).datepicker("update");
                            }
                            DateOfBirthCheck(input);
                        }

                    }

                    var defaultBtn = $('button[type=submit].default');
                    if (defaultBtn.length > 0 && !$(defaultBtn).is(':focus')) {
                        defaultBtn[0].click();
                        return false;
                    }



                }
            }

        }
    }
}, "body");

$(document).on({
    "keydown": function (e) {
        if ($('.confirm-box').is(':visible')) {
            $('.confirm-box .btn-primary').click();
            return;
        }
        if ($(e.target).is(':input')) {
            if ($(e.target).val() != '' && $(e.target).val()[0] == ' ') {
                $(e.target).val($.trim($(e.target).val()))
            }

        }
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {

            if ($("input:checkbox").is(":focus")) {

                ///   $("input:checkbox:focus").prop('checked', !$("input:checkbox:focus").is(':checked'))
                var defaultBtn = $('button[type=submit].default');
                if (defaultBtn.length > 0 && !$(defaultBtn).is(':focus')) {
                    defaultBtn[0].click();

                }

                return false;

            } else if ($("select").is(":focus")) {
                var defaultBtn = $('button[type=submit].default');
                if (defaultBtn.length > 0 && !$(defaultBtn).is(':focus')) {
                    defaultBtn[0].click();

                }
                return false;



            }


        }
    }
}, "body");
function hideAllUnAuthorizedInputElements(secureIds) {
    var attr = "data-secure-id";
    $("[data-secure-id]").each(function (e) {
        var secureId = $(this).attr(attr);
        if ($.inArray(secureId, secureIds) < 0)
            $(this).remove();
    });

    attr = "data-secure-disabled-id";
    $("[data-secure-disabled-id]").each(function (e) {
        var secureId = $(this).attr(attr);
        if ($.inArray(secureId, secureIds) < 0)
            $(this).attr('disabled', 'disabled');
    });
    attr = "data-secure-disablediffound-id";
    $("[data-secure-disablediffound-id]").each(function (e) {
        var secureId = $(this).attr(attr);
        if ($.inArray(secureId, secureIds) > -1) {
            $(this).attr('disabled', 'disabled');
            $(this).addClass('disabled-permanent')
        }

    });
}

function removeLinkFromUnAuthorizedAnchorElements(secureIds) {
    var attr = "data-secure-link-id";
    $("[data-secure-link-id]").each(function (e) {
        var secureId = $(this).attr(attr);

        if ($.inArray(secureId, secureIds) < 0) {

            $(this).removeAttr('href');
            $(this).addClass('linktospan');
            $(this).attr('disabled', 'disabled');
        }
    });
    attr = "data-secure-linkremovediffound-id";
    $("[data-secure-linkremovediffound-id]").each(function (e) {
        var secureId = $(this).attr(attr);

        if ($.inArray(secureId, secureIds) > -1) {

            $(this).removeAttr('href');

            $(this).removeAttr('class');
            $(this).addClass('linktospan');
            $(this).attr('disabled', 'disabled');
        }
    });
}

function hasUserAccessToSecurityItemId(secureId) {

    return $.inArray(secureId, secureIds) > -1;
}



jQuery.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
    return {
        "iStart": oSettings._iDisplayStart,
        "iEnd": oSettings.fnDisplayEnd(),
        "iLength": oSettings._iDisplayLength,
        "iTotal": oSettings.fnRecordsTotal(),
        "iFilteredTotal": oSettings.fnRecordsDisplay(),
        "iPage": oSettings._iDisplayLength === -1 ?
            0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
        "iTotalPages": oSettings._iDisplayLength === -1 ?
            0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
    };
};

//prevante backspace for dropdown
function cancelBackspace(event) {
    if (event.keyCode == 8) {
        return false;
    }
}

// Zoom changing event
var timeZoomInOut;
function ZoomIn() {
    clearTimeout(timeZoomInOut);
    if (!$('body').hasClass('zoom')) {
        $('body').addClass('zoom');
        $('#zoom-out').css('cursor', 'pointer');
        $('#zoom-out').css('opacity', '1.0');
        $('#zoom-in').css('cursor', 'not-allowed');
        $('#zoom-in').css('opacity', '0.5');
        timeZoomInOut = setTimeout(function () {
            SaveZoomSetting();
        }, 1000);

        //  return;
    }
    //if (!$('body').hasClass('zoom-plus')) {
    //    $('body').addClass('zoom-plus');
    //    $('#zoom-in').css('cursor', 'not-allowed');
    //    $('#zoom-in').css('opacity', '0.5');
    //    timeZoomInOut = setTimeout(function () {
    //        SaveZoomSetting();
    //    }, 1000);

    //}

};
function ZoomOut() {
    clearTimeout(timeZoomInOut);
    //if ($('body').hasClass('zoom-plus')) {
    //    $('body').removeClass('zoom-plus');
    //    $('#zoom-in').css('cursor', 'pointer');
    //    $('#zoom-in').css('opacity', '1.0');
    //    timeZoomInOut = setTimeout(function () {
    //        SaveZoomSetting();
    //    }, 1000);
    //    return;
    //}

    if ($('body').hasClass('zoom')) {
        $('body').removeClass('zoom');
        $('#zoom-in').css('cursor', 'pointer');
        $('#zoom-in').css('opacity', '1.0');
        $('#zoom-out').css('cursor', 'not-allowed');
        $('#zoom-out').css('opacity', '0.5');
        timeZoomInOut = setTimeout(function () {
            SaveZoomSetting();
        }, 1000);


    }

};

function SaveZoomSetting() {
    var zoomCssClass = '';
    if ($('body').hasClass('zoom')) {
        zoomCssClass = 'zoom ';
    }
    if ($('body').hasClass('zoom-plus')) {
        zoomCssClass += ' zoom-plus';
    }


    $.ajax({
        type: "POST",
        url: '/Home/ZoomInOut',
        data: { 'ZoomCssClass': zoomCssClass },
        dataType: 'json',
        success: function (data) {

        }
    });
}
function isNumber(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 13) {
        return true;
    }
    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function isNumberOnly(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode;

    if ((charCode < 48 || charCode > 57))
        return false;

    return true;
}

//Used for Prevent chars in Numeric fields and allow period(.) for decimal 
function isNumberWithDecimal(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 13) {
        return true;
    }
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

//Used for Prevent chars in Numeric fields and allow period(.) for decimal 
function isNumberWithCommaAndDash(evt) {
    evt = (evt) ? evt : window.event;

    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode != 45 && charCode != 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function IsValidData(type, str) {
    var flag = false;

    switch (type) {
        case 'email':
            flag = validateEmail(str);
            break;
    }
    return flag;
}

function validateEmail(sEmail) {
    //  var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    var filter = /^[\w-']+(\.[\w-']+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$/;
    if (filter.test(sEmail)) {
        if (filter.test(sEmail.toLowerCase())) {
            return true;
        }
        else {
            return false;
        }
    }
function Contains(str, value) {
    return str.indexOf(value) >= 0;
}

var initial_values = new Array();

// Gets all form elements from the entire document.
function getAllFormElements(formId) {
    // Return variable.
    var all_form_elements = Array();

    // The form.
    var form_activity_report = document.getElementById(formId);

    // Different types of form elements.
    var inputs = form_activity_report.getElementsByTagName('input');
    var textareas = form_activity_report.getElementsByTagName('textarea');
    var selects = form_activity_report.getElementsByTagName('select');

    // We do it this way because we want to return an Array, not a NodeList.
    var i;
    for (i = 0; i < inputs.length; i++) {
        all_form_elements.push(inputs[i]);
    }
    for (i = 0; i < textareas.length; i++) {
        all_form_elements.push(textareas[i]);
    }
    for (i = 0; i < selects.length; i++) {
        all_form_elements.push(selects[i]);
    }

    return all_form_elements;
}

// Sets the initial values of every form element.
function setInitialFormValues(formId, createOldValueFields) {
    initial_values = new Array();
    createOldValueFields = createOldValueFields || false;
    var inputs = getAllFormElements(formId);
    if (createOldValueFields) {
        for (var i = 0; i < inputs.length; i++) {
            if ($(inputs[i]).is(':checkbox')) {
                $('#' + formId).append('<input type="hidden" value="' + $(inputs[i]).is(':checked') + '" name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');

            } else if ($(inputs[i]).is(':radio')) {

                $('#' + formId).append('<input type="hidden" value="' + $(inputs[i]).is(':checked') + '" name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');

            } else if ($(inputs[i]).attr('type') != 'hidden' && !$(inputs[i]).hasClass('summernote')) {

                $('#' + formId).append('<input type="hidden"   name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');
                $('#' + $(inputs[i]).attr('name') + '_oldValue').val($(inputs[i]).val());

            } else if ($(inputs[i]).attr('type') != 'hidden' && $(inputs[i]).hasClass('summernote')) {

                $('#' + formId).append('<input type="hidden"  name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');
                $('#' + $(inputs[i]).attr('name') + '_oldValue').val($(inputs[i]).GetHtml());
            }
        }
    }

    inputs = getAllFormElements(formId);
    for (var i = 0; i < inputs.length; i++) {
        var initValue = '';
        if ($(inputs[i]).attr('type') != 'hidden' && $(inputs[i]).hasClass('summernote')) {
            initValue = $(inputs[i]).GetHtml();
        } else {
            initValue = $(inputs[i]).GetHtml();
        }

        if ($(inputs[i]).is(':checkbox')) {
            $(inputs[i]).attr('data-old-value-on-pageload', $(inputs[i]).is(':checked'));

        } else if ($(inputs[i]).is(':radio')) {
            $(inputs[i]).attr('data-old-value-on-pageload', $(inputs[i]).is(':checked'));

        } else {
            $(inputs[i]).attr('data-old-value-on-pageload', initValue);
        }

        if (inputs[i].type == "checkbox") {
            initial_values.push($(inputs[i]).is(':checked'));
        } else if (inputs[i].type == "radio") {
            initial_values.push($(inputs[i]).is(':checked'));
        } else {
            initial_values.push(initValue);
        }

    }

}
function setInitialFormValuesWithOutSummernote(formId, createOldValueFields) {
    initial_values = new Array();
    createOldValueFields = createOldValueFields || false;
    var inputs = getAllFormElements(formId);
    if (createOldValueFields) {
        for (var i = 0; i < inputs.length; i++) {
            if ($(inputs[i]).is(':checkbox')) {
                $('#' + formId).append('<input type="hidden" value="' + $(inputs[i]).is(':checked') + '" name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');

            } else if ($(inputs[i]).is(':radio')) {

                $('#' + formId).append('<input type="hidden" value="' + $(inputs[i]).is(':checked') + '" name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');

            } else if ($(inputs[i]).attr('type') != 'hidden' && !$(inputs[i]).hasClass('summernote')) {

                $('#' + formId).append('<input type="hidden"   name="' + $(inputs[i]).attr('name') + '_oldValue' + '"  id="' + $(inputs[i]).attr('name') + '_oldValue' + '"/>');
                $('#' + $(inputs[i]).attr('name') + '_oldValue').val($(inputs[i]).val());

            }
        }
    }

    inputs = getAllFormElements(formId);
    for (var i = 0; i < inputs.length; i++) {
        var initValue = '';
        if ($(inputs[i]).attr('type') != 'hidden' && $(inputs[i]).hasClass('summernote')) {
            initValue = $(inputs[i]).GetHtml()
        } else {
            initValue = $(inputs[i]).GetHtml()
        }

        if ($(inputs[i]).is(':checkbox')) {
            $(inputs[i]).attr('data-old-value-on-pageload', $(inputs[i]).is(':checked'));

        } else if ($(inputs[i]).is(':radio')) {
            $(inputs[i]).attr('data-old-value-on-pageload', $(inputs[i]).is(':checked'));

        } else {
            $(inputs[i]).attr('data-old-value-on-pageload', initValue);
        }

        if (inputs[i].type == "checkbox") {
            initial_values.push($(inputs[i]).is(':checked'));
        } else if (inputs[i].type == "radio") {
            initial_values.push($(inputs[i]).is(':checked'));
        } else {
            initial_values.push(initValue);
        }

    }

}

function hasFormChanged(formId) {
    var has_changed = false;
    var elements = getAllFormElements(formId);

    for (var i = 0; i < elements.length; i++) {
        if ($(elements[i]).hasClass("exclude-in-form-change")) {
            continue;
        }

        if (elements[i].type == "checkbox") {
            if ($(elements[i]).is(':checked') != initial_values[i]) {
                has_changed = true;
                break;
            }
        } else if (elements[i].type == "radio") {
            if ($(elements[i]).is(':checked') != initial_values[i]) {
                has_changed = true;
                break;
            }
        } else if ($(elements[i]).hasClass("summernote")) {
            if ($(elements[i]).GetHtml() !== initial_values[i]) {
                has_changed = true;
                break;

            }

        } else {
            if (elements[i].value != initial_values[i]) {
                has_changed = true;
                break;
            }
        }

    }


    return has_changed;
}

function AlertBox(messageStr, customClass) {
    bootbox.dialog({
        className: customClass,// "yellow",
        size: 400,
        message: messageStr,
        title: "Confirmation",
        header: "<i class='fa fa-warning'></i>",
        buttons: {
            yes: {
                label: "OK",
                className: "btn-secondary",
                callback: function () {

                }
            }


        }
    });
}

// customization of bootbox
var confirmBox = function (message, callback) {
    bootbox.confirm({
        closeButton: false,
        className: 'modal-custom modal-message modal-primary confirm-box',
        title: '<i class="fa fa-warning themeprimary"></i>',
        message: '<div style="color: #737373;font-size: 17px;margin-bottom: 3px">Confirmation</div>' + message,
        buttons: { "confirm": { className: "btn-primary", label: "No" }, "cancel": { className: "btn-default", label: "Yes" } },
        callback: function (result) { callback(!result); }
    })
}

function OpenCustomPopup(src, width, height, title) {
    $('#PagePopup').attr('data-src', src);
    $('#PagePopup').attr('data-height', height);
    $('#PagePopupLabel').html(title);

    $('#PagePopup').modal('show');
    $('#PagePopup .modal-dialog').css('width', width);
}

function OpenPopup(src, title) {
    var _offset = 200;
    var headerHeight = $(".navbar.navbar-fixed-top:first").height();
    var modelHeight = $(window).height() - headerHeight - _offset;

    OpenCustomPopup(src, "98%", modelHeight, title);

    _offset = 80;
    var headerHeight = $(".navbar.navbar-fixed-top:first").height() + _offset;
    $('#PagePopup .modal-dialog').css({ marginTop: headerHeight + "px" });

}

function ClosePopup() {
    $('#PagePopup').modal('hide');
}

//trigger date picker on click
function fnDatePickerTrigger(id) {
    if ($('#' + id).val() == '') {
        $('#' + id).datepicker("setValue", getDate()).datepicker("update");
    }
    $("#" + id).datepicker("show");
}

var setHotKeys = function () {
    var totalButtons = $("#fixed-button-bar button").length;
    if (totalButtons > 1) {
        var events = [];

        for (var index = 1; index < totalButtons; index++) {
            var btn = $("#fixed-button-bar button").eq(index);
            var lowerText = btn.text();
            if (lowerText.toLowerCase().indexOf("cancel") < 0 && lowerText.toLowerCase().indexOf("delete") < 0) {
                btn.text(lowerText + " [" + index + "]");
                btn.prop("title", "Alt + " + index);
                btn.addClass("option-" + index);
                events.push('option+' + index);
            }
        }

        //console.log(events);
        Mousetrap.bind(events, function (e, combo) {
            var key = combo.split('+')[1];
            $('.btn.option-' + key).trigger('click');
            return false;
        });
    }
}

$.fn.IsValueChanged = function () {
    return $(this).GetValue() != $(this).attr('data-old-value-on-pageload');
};

$.fn.GetValue = function () {
    return $(this).val() == null ? '' : $(this).val();
};
$.fn.hasValue = function () {
    var val = $(this).val();

    return val != null && val.length > 0;
};

function OpenCustomModal(html, title, width, height) {
    $('#pageModal #ModelContent').html(html);
    $('#pageModalLabel').html(title);

    $('#pageModal').attr('data-height', height);
    $('#pageModal').modal('show');

    var _offset = 60;
    var headerHeight = $(".navbar.navbar-fixed-top:first").height() + _offset;

    $('#pageModal .modal-dialog').css({ 'width': width, 'height': height, 'marginTop': headerHeight });
    $('#pageModal .modal-body').css('height', height);

}

function OpenModal(html, title) {
    var _offset = 200;
    var headerHeight = $(".navbar.navbar-fixed-top:first").height();
    var modelHeight = $(window).height() - headerHeight - _offset;
    OpenCustomModal(html, title, "98%", modelHeight);
}



function CloseModal() {
    $('#pageModal').modal('hide');
}
//To valid form data is not submitted twice by do back button in browser
function IsValidFormRequest() {


    var flag = false;
    if ($("#hidBackButtonButtonValue").val() == null || $("#hidBackButtonButtonValue").val() == '') {
        flag = true;
    } else {
        flag = false;
        bootbox.dialog({
            message: "You must refresh this page before it can be processed again. Please click Cancel if you need to copy any information to your clipboard before refreshing.",
            title: "Warning",
            header: "<i class='fa fa-warning'></i>",
            buttons: {
                yes: {
                    label: "OK",
                    className: "btn-secondary",
                    callback: function () {
                        window.location.href = window.location.href;

                    }
                }
                , no: {
                    label: "Cancel",
                    className: "btn-secondary",
                    callback: function () {
                    }
                }
            }
        });
    }
    return flag;
}

function RequestSubmitted() {
    $("#hidBackButtonButtonValue").val('submitted');
}
function IPadKeyboardFix() {
    $("input, textarea").each(function () {
        if (!$(this).is(":button") && !$(this).is(":submit") && !$(this).is(":file")) {
            var value = $(this).val();
            $(this).val($.trim(value));
        }
    });
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('form').blur();
        $('form input').blur();
    }
}

function getParameterByName(name) {
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
}

function doCopy(textElementToCopy) {
    //var textToCopy = document.getElementById('main-case-link');
    var range = document.createRange();

    window.getSelection().removeAllRanges();
    range.selectNodeContents(textElementToCopy);
    window.getSelection().addRange(range);

    //try {
    //    document.execCommand('copy');
    //    Notify('Case number is copied to clipboard.', 'bottom-right', '4000', 'info', 'fa-info-o', true);
    //}
    //catch (e)
    //{
    //    Notify('Unable to copy, Please use "ctrl/cmd + C".' , 'bottom-right', '4000', 'danger', 'fa-frown-o', true);
    //}
}

function AlertBoxWithCallback(messageStr, callback) {
    bootbox.dialog({
        className: "modal-custom modal-message modal-primary confirm-box ",// "yellow",
        size: 400,
        message: messageStr,
        title: "Confirmation",
        header: "<i class='fa fa-warning'></i>",
        buttons: {
            yes: {
                label: "OK",
                className: "btn-secondary",
                callback: function () {
                    callback();
                }
            }


        }
    });
}

function AlertBoxWithTitleAndCallback(title, messageStr, callback) {
    if (messageStr.length <= 0) {
        messageStr = "     "
            ;
    }
    bootbox.dialog({
        className: "modal-custom modal-message modal-primary confirm-box ",// "yellow",
        size: 400,
        message: messageStr,
        title: title,
        header: "<i class='fa fa-warning'></i>",
        buttons: {
            yes: {
                label: "OK",
                className: "btn-secondary",
                callback: function () {
                    callback();
                }
            }


        }
    });
}
function enableLoadDataFromCache() {
    $("#hidResultCache").val("yes");
}

function disableLoadDataFromCache() {
    $("#hidResultCache").val() == "no";
}

function isLoadDataFromCache() {
    return $("#hidResultCache").val() == "yes";
}
$.fn.IsCheckboxChanged = function () {
    return $(this).is(':checked').toString() != $(this).attr('data-old-value-on-pageload');
};

$.fn.IsRadioChanged = function () {
    return $(this).is(':checked').toString() != $(this).attr('data-old-value-on-pageload');
};
$('body').on('focusout', '.phone_format', function () {
    var originalText = $(this).val();
    var numbers = GetNumberFromStartString($(this).val() + ' ');
    var text = numbers;
    if ($.isNumeric(text)) {
        if (text.length == 7) {
            text = text.replace(/(\d\d\d)(\d\d\d\d)/, '$1-$2') + ' ';

            text = originalText.replace(numbers, text);
            $(this).val(text);
        } else if (text.length == 10) {
            text = text.replace(/(\d\d\d)(\d\d\d)(\d\d\d\d)/, '($1) $2-$3') + ' ';

            text = originalText.replace(numbers, text);
            $(this).val(text);
        } else if (text.length == 12) {
            text = text.replace(/(\d\d)(\d\d\d)(\d\d\d)(\d\d\d\d)/, '$1 ($2) $3-$4') + ' ';
            text = originalText.replace(numbers, text);
            $(this).val(text);
        }
    }

})
String.prototype.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};

$('body').on('focus', '.phone_format', function () {
    var text = $(this).val();
    text = text.replace('(', '').replaceAll(')', '').replaceAll(' ', '').replaceAll('-', '');
    if ($.isNumeric(text)) {
        if (text.length == 10 || text.length == 12) {
            $(this).val(text);
        }
    }


})
function GetNumberFromStartString(str) {
    return str.replace(/(^\d+)(.+$)/i, '$1'); //=> '123'
}



function strEndsWith(haystack, needle) {
    return needle === haystack.substr(0 - needle.length);
}

(function ($) {
    $.fn.serializeFormObject = function () {

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
$('.date-picker,.date-picker-control').each(function () {
    $value = $(this).val();
    if ($value != '' && $value != null) {
        $date = dateFormat($value);


        if ($date != $value) {
            $(this).val($date);

        }



    }
});

function IsInvalidCharactersNotExistsInSearchField(el) {
    var flag = true;
    console.log($(el).val())
    if (Contains($(el).val(), '%') || Contains($(el).val(), '^') || Contains($(el).val(), '[') || Contains($(el).val(), ']') || Contains($(el).val(), '_')) {
        flag = false;
        notifyDanger("Invalid search parameter. The following characters are not allowed: %_[]^");
        $(el).focus();
    }


    return flag;
}
function IsInvalidCharactersNotExistsInSearchFields(formEl) {
    var flag = true;

    $(formEl).find('input:text').each(function () {
        if ($(this).val().length > 0) {

            if (flag) {

                flag = IsInvalidCharactersNotExistsInSearchField($(this));

            }
        }

    })

    return flag;
}

$.fn.GetHtml = function () {
    if (!$(this).hasClass('summernote')) {
        if ($(this).val() !== '' && $(this).val() !== null) {
            return $(this).val();//($(this).val().replace(/[^\x00-\x7F]|/g, ''));
        }
        return '';
    }
    if ($.trim($(this).GetText()).length > 0) {
        return $(this).code();//($(this).code().replace(/[^\x00-\x7F]|/g, ''));
    }
    return '';
};

$.fn.GetHtmlWithEscape = function () {
    if (!$(this).hasClass('summernote')) {
        if ($(this).val() !== '' && $(this).val() !== null) {
            return $(this).val();//($(this).val().replace(/[^\x00-\x7F]|/g, ''));
        }
        return '';
    }
    if ($.trim($(this).GetText()).length > 0) {
        return $(this).code();//escape($(this).code().replace(/[^\x00-\x7F]|/g, ''));
    }
    return '';
};
function stripHTML(dirtyString) {
    var container = document.createElement('div');
    var text = document.createTextNode(dirtyString);
    container.appendChild(text);
    return container.innerHTML; // innerHTML will be a xss safe string
}
$.fn.GetText = function () {
    if (!$(this).hasClass('summernote')) {
        if ($(this).val() !== '' && $(this).val() !== null) {
            return $(this).val();//($(this).val().replace(/[^\x00-\x7F]|/g, ''));
        }
        return '';
    }
    return stripHTML($(this).code().replace(/<.*?>/g, ''));//stripHTML($(this).code().replace(/[^\x00-\x7F]|/g, '').replace(/<.*?>/g, ''));
};
function ApplySummernote() {


    if ($('#hidServerEnvironment').val() == 'Dev') {
        $('.summernote').summernote({
            // height: '150px',
            tabDisable: false,
            lineHeight: 20,
            fontSizes: ['12', '14', '16', '18', '24', '36', '48'],
            toolbar: [
                ['style', ['style']],
                ['font', ['bold', 'italic', 'underline', 'clear']],
                //  ['fontname', ['fontname']],
                //   ['font', ['fontsize']],
                // ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['view', ['fullscreen', 'codeview']],
                //  ['insert', ['link', 'picture', 'video']]
            ]
        });
        $('.summernote').each(function () {
            $(this).parent().find('.note-editable').attr('tabindex', $(this).attr('tabindex'))
        })
    }
    else {
        $('.summernote').summernote({
            // height: '150px',
            lineHeight: 20,
            fontSizes: ['12', '14', '16', '18', '24', '36', '48'],
            toolbar: [
                ['style', ['style']],
                ['font', ['bold', 'italic', 'underline', 'clear']],
                //  ['fontname', ['fontname']],
                //   ['font', ['fontsize']],
                // ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['view', ['fullscreen']],
                //  ['insert', ['link', 'picture', 'video']]
            ]
        });
        $('.summernote').each(function () {
            $(this).parent().find('.note-editable').attr('tabindex', $(this).attr('tabindex'))
        })
    }
    delete $.summernote.options.keyMap.pc.TAB;
    delete $.summernote.options.keyMap.mac.TAB;
    delete $.summernote.options.keyMap.pc['SHIFT+TAB'];
    delete $.summernote.options.keyMap.mac['SHIFT+TAB'];
    $.summernote.pluginEvents['untab'] = function (event, editor, layoutInfo) {
        event.preventDefault();
    };
    $('button[data-event="justifyFull"]').hide()
    $('a[data-value="blockquote"]').parent('li').hide()
    $('a[data-value="p"]').parent('li').hide()
    $('a[data-value="pre"]').parent('li').hide()

    $('<li><a data-event="formatBlock" data-value="p"><h6>Paragraph</h6></a></li>').appendTo($('a[data-value="pre"]').parent('li').parent('ul'))

}
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.indexOf(searchString, position) === position;
    };
}
function encodeUrl(str) {
    return encodeURIComponent(str)
}
function RoundHours(number, increment, offset) {
    return Math.ceil((number - offset) / increment) * increment + offset;
}
function encryptText(str) {

    return CryptoJS.AES.encrypt(str, "My Secret Passphrase").toString();
}

function decryptText(str) {
    var decryptedBytes = CryptoJS.AES.decrypt(str, "My Secret Passphrase");
    var plaintext = decryptedBytes.toString(CryptoJS.enc.Utf8);
    return plaintext;
}
function AutoSaveNote(noteElementId, keyName, loadData, caption, callback) {
    // console.log('AutoSaveNote')
    caption = caption || '';
    var mainKey = keyName;
    if (loadData == 1) {
        var pageState = simpleStorage.get(mainKey);
        if (pageState && pageState.data) {
            if ($('#' + noteElementId).GetHtml() !== decryptText(pageState.data)) {
                confirmBox('You have an unsaved ' + caption + '. Do you want to recover it?', function (result) {

                    if (result) {
                        var formData = decryptText(pageState.data);
                        $('#' + noteElementId).code(formData);
                    } else {
                        simpleStorage.deleteKey(mainKey);
                    }
                    callback();
                });
            } else {
                simpleStorage.deleteKey(mainKey);
			}

      
        } else {
            callback();
        }
    } else if (loadData == 2) {
        simpleStorage.deleteKey(mainKey);

    }
    else {


        if ($('#' + noteElementId).data("old-value-on-pageload") !== $('#' + noteElementId).GetHtml()) {
            var pageState = { data: encryptText($('#' + noteElementId).GetHtml()) };
            simpleStorage.set(mainKey, pageState, { TTL: 3600000 });

        }

    }

}

function ConvertToFloat(str) {

    return parseFloat(str != '' ? str : 0);
}

function ConvertToInt(str) {

    return parseInt(str != '' ? str : 0);
}