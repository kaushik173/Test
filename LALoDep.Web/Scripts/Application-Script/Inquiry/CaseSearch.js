var oTable = null;
var rowActive = false; var pageDone = false, paramPassed = '', formDataPased='';
var page = (function () {
    return {
        $form: $('#case-search-form'),
        $table: $('#searchCase'),
        mainKey: 'case-search-form' + GetWindowID(),
        resultPageIdKey: GetWindowID() + 'case-search-form-page#',
        rowSelectedKey: GetWindowID() + 'case-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

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

//
// Pipelining function for DataTables. To be used to the `ajax` option of DataTables
//
$.fn.dataTable.pipeline = function (opts) {
    // Configuration options
    var conf = $.extend({
        pages: 5,     // number of pages to cache
        url: '',      // script url
        data: null,   // function or object with parameters to send to the server
        // matching how `ajax.data` works in DataTables
        method: 'GET' // Ajax HTTP method
    }, opts);

    var pageState = simpleStorage.get(page.mainKey);
    pageState = pageState ? pageState : {};
    // Private variables for storing the cache
    var cacheObject = $.extend({
        cacheLower: -1,
        cacheUpper: null,
        cacheLastRequest: null,
        cacheLastJson: null
    }, pageState.result );

    

    return function (request, drawCallback, settings) {

        var ajax = false;
        var requestLength = request.length;

        var pageNumber = GetPageNumber();
        if (pageNumber > 0) {
            request.start = pageNumber * requestLength;
            pageDone = false;
        }
        
        var requestStart = request.start;
        var drawStart = request.start;
        var requestEnd = requestStart + requestLength;

        if (settings.clearCache) {
            // API requested that the cache be cleared            
            ajax = true;
            settings.clearCache = false;
            ResetPageState();
        }
        else if (cacheObject.cacheLower < 0 || requestStart < cacheObject.cacheLower || requestEnd > cacheObject.cacheUpper) {
            // outside cached data - need to make a request
            ajax = true;
        }
        else if (JSON.stringify(request.order) !== JSON.stringify(cacheObject.cacheLastRequest.order) ||
            JSON.stringify(request.columns) !== JSON.stringify(cacheObject.cacheLastRequest.columns) ||
            JSON.stringify(request.search) !== JSON.stringify(cacheObject.cacheLastRequest.search)
        ) {
            // properties changed (ordering, columns, searching)
            ajax = true;
        }
        else if (formDataPased !== $('#case-search-form').serialize()) {
            ajax = true;
        }

        formDataPased = $('#case-search-form').serialize();

        // Store the request for checking next time around
        cacheObject.cacheLastRequest = $.extend(true, {}, request);

        if (ajax) {

            if (pageNumber > 0) {
                requestStart = pageNumber * requestLength;
                pageDone = false;
            }

            // Need data from the server
            if (requestStart < cacheObject.cacheLower) {
                requestStart = requestStart - (requestLength * (conf.pages - 1));

                if (requestStart < 0) {
                    requestStart = 0;
                }
            }

            cacheObject.cacheLower = requestStart;
            cacheObject.cacheUpper = requestStart + (requestLength * conf.pages);

            request.start = requestStart;
            request.length = requestLength * conf.pages;

            // Provide the same `data` options as DataTables.
            if (typeof conf.data === 'function') {
                // As a function it is executed with the data object as an arg
                // for manipulation. If an object is returned, it is used as the
                // data object to submit
                var d = conf.data(request);
                if (d) {
                    $.extend(request, d);
                }
            }
            else if ($.isPlainObject(conf.data)) {
                // As an object, the data given extends the default
                $.extend(request, conf.data);
            }

            settings.jqXHR = $.ajax({
                "type": conf.method,
                "url": conf.url,
                "data": request,
                "dataType": "json",
                "cache": false,
                "success": function (json) {
                    cacheObject.cacheLastJson = $.extend(true, {}, json);                    
                    SaveFormState($.extend(true, {}, cacheObject));

                    if (cacheObject.cacheLower !== drawStart) {
                        json.data.splice(0, drawStart - cacheObject.cacheLower);
                    }
                    if (requestLength >= -1) {
                        json.data.splice(requestLength, json.data.length);
                    }

                    $('#CountSearchResult').text(json.recordsTotal)
                    drawCallback(json);                    
                }
            });
        }
        else {                       
            
            //if (pageNumber > 0) {
            //    request.start = pageNumber * requestLength;
            //    pageDone = false;
            //}

            requestStart = request.start;
            drawStart = request.start;

            //json = $.extend(true, {}, cacheLastJson);
            json = $.extend(true, {}, cacheObject.cacheLastJson);

            json.draw = request.draw; // Update the echo for each response
            json.data.splice(0, requestStart - cacheObject.cacheLower);
            json.data.splice(requestLength, json.data.length);
            $('#CountSearchResult').text(json.recordsTotal)
            drawCallback(json);            
        }
    };
};

// Register an API method that will empty the pipelined data, forcing an Ajax
// fetch on the next draw (i.e. `table.clearPipeline().draw()`)
$.fn.dataTable.Api.register('clearPipeline()', function () {
    return this.iterator('table', function (settings) {
        console.log("clearPipeline called");
        settings.clearCache = true;
    });
});

var ResetPageState = function () {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
};

//var SaveFormState = function () {
//    var formData = {};
//    page.$form.serializeArray().map(function (item) {
//        formData[item.name] = item.value;
//    });
//    var pageState = { formData: formData };
//    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
//};

var SaveFormState = function (resultData) {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData, result: resultData };
    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
};


var GetPageNumber = function () {
    var pageNumber = simpleStorage.get(page.resultPageIdKey);
    if (pageNumber && pageNumber > 0 && !pageDone) {
        pageDone = true;
        return pageNumber;
    }
    else {
        pageNumber = 0;
    }
    return pageNumber;
};
var GetCaseSearchGuidID = function () {
    var guid = simpleStorage.get(GetWindowID() + '_SearchGuidID');
    if (guid && guid.length > 0) {
        return guid;
    }
    return "";
};

var ClearCaseSearchGuidID = function () {
    simpleStorage.deleteKey(GetWindowID() + '_SearchGuidID');
    simpleStorage.deleteKey(page.resultPageIdKey);
};


var LoadPreviousFormState = function () {
    var flag = false;
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData) {
        /*Load Form inputs*/
        var formData = pageState.formData;
        page.$form.find('*').filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')]) {
                $this.val(formData[$this.attr('id')]);
                flag = true;

            }
        });
    }

    formDataPased = $('#case-search-form').serialize();
    return flag;
};


function getData() {
    var fData = $('#case-search-form').serializeObject();
    return fData;
}

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    if (oTable !== null) {
        calc_height = $(window).height();
        var _offset = 25;

        $("#divSearchResult .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
            $(this).siblings().each(function () {
                if (calc_height > $(this).outerHeight(true) && $(this).css('display') !== 'none') {
                    if ($(this).attr("id") === 'loading')
                        return;
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        calc_height = calc_height - _offset;
        if (calc_height < 100) {

            $('#divSearchResult .dataTables_scrollBody').css('max-height', '100px');
        }
        else {
            $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        }
        //oTable.fnAdjustColumnSizing();
        oTable.DataTable().columns.adjust();
    }
    return calc_height;
}

function Search() {
    //if mobile or tablet
    IPadKeyboardFix();
    if (!IsInvalidCharactersNotExistsInSearchFields($('#case-search-form')))
        return false;
    if ($('#LastName').val().length === 0 && $('#FirstName').val().length === 0 && $('#DocketNumber').val().length === 0 && $('#JcatsNumber').val().length === 0 && $('#HHSA').val().length === 0) {
        notifyDanger('At least one of the following is required: Last Name, First Name, Case #, Jcats #, HHSA #');
        $('#LastName').focus();
        return false;
    }
    if ($('#LastName').val().length > 0 && $('#FirstName').val().length === 0 && $('#LastName').val().length < 2) {
        notifyDanger('At least two characters required for Last Name, if First Name is blank.');
        $('#LastName').focus();
        return false;
    }
    if ($('#FirstName').val().length > 0 && $('#LastName').val().length === 0 && $('#FirstName').val().length < 2) {
        notifyDanger('At least two characters required for First Name, if Last Name is blank.');
        $('#FirstName').focus();
        return false;
    }

    if ($('#DocketNumber').val().length !== 0 && $('#DocketNumber').val().length < 4) {
        notifyDanger("At least four characters required for Case #");
        $('#DocketNumber').focus();
        return false;
    }

    if ($('#JcatsNumber').val().length !== 0 && $('#JcatsNumber').val().length < 5) {
        notifyDanger("At least five characters required for Jcats #");
        $('#JcatsNumber').focus();
        return false;
    }

    if ($('#HHSA').val().length !== 0 && $('#HHSA').val().length < 5) {
        notifyDanger("At least five characters required for HHSA #");
        $('#HHSA').focus();
        return false;
    }

    //SaveFormState();

    if (oTable !== null) {
        oTable.fnDraw();
    } else {
        oTable = $('#searchCase').dataTable({

            searching: false,
            ordering: false,
            lengthChange: false,
            serverSide: true,
            responsive: true,
            pageLength: 50,
            scrollY: "auto", // Need to make this calculated            
            loadingRecords: "Loading...",
            processing: "Processing...",
            ajax: $.fn.dataTable.pipeline({
                url: "/Case/Search",
                pages: 5, // number of pages to cache
                method: "POST",
                data: function (d) {
                    //var pageNumber = GetPageNumber();

                    //if (pageNumber > 0) {
                    //    d.start = pageNumber * d.length;
                    //    pageDone = false;
                    //}

                    var formData = getData();
                    d = $.extend({}, d, formData || {});


                    var serlisedForm = $('#case-search-form').serialize();

                    if (GetCaseSearchGuidID().length > 0) {
                        serlisedForm += '&SearchGuid=' + GetCaseSearchGuidID();
                        d.SearchGuid = GetCaseSearchGuidID();
                    }

                    d.ParamChanged = paramPassed !== serlisedForm;
                    paramPassed = serlisedForm;
                    return d;
                },
                dataSrc: function (result) {
                    //Make your callback here.
                    
                    $('#CountSearchResult').text(result.recordsTotal);
                    return result.data;
                }
            }),
            //{
            //    url: "/Case/Search", type: "POST",
            //    data: function (d) {
            //        var pageNumber = GetPageNumber();

            //        if (pageNumber > 0) {
            //            d.start = pageNumber * d.length;
            //            pageDone = false;
            //        }

            //        var formData = getData();
            //        d = $.extend({}, d, formData || {});


            //        var serlisedForm = $('#case-search-form').serialize();

            //        if (GetCaseSearchGuidID().length > 0) {
            //            serlisedForm += '&SearchGuid=' + GetCaseSearchGuidID();
            //            d.SearchGuid = GetCaseSearchGuidID();
            //        }

            //        d.ParamChanged = paramPassed !== serlisedForm;
            //        paramPassed = serlisedForm;
            //        return d;
            //    },
            //    dataSrc: function (result) {
            //        //Make your callback here.
            //        $('#CountSearchResult').text(result.recordsTotal);
            //        return result.data;
            //    }
            //},

            columns: [
                { "data": "PersonNameLast" },
                { "data": "PersonNameFirst" },
                { "data": "DOB" },
                { "data": "Sex", "title": "Gender" },
                {
                    "render": function (data, type, full, meta) {
                        if (full.ConflictFlag === 1) {
                            return '<label>' + full.Role + '</label>' + " " + '<label class="red"> (Conflict)</label>';
                        }
                        else {
                            return full.Role;
                        }
                    }
                },
                { "data": "HHSANumber" },
                {
                    "data": "CaseNumber",
                    "render": function (data, type, full, meta) {
                        if (hasUserAccessToSecurityItemId('1500')) {
                            return '<span>' + full.CaseNumber + '</span>';
                        } else {
                            return '<a href="/Case/Main/' + full.EncryptedCaseID + '" data-guid="' + full.wtCaseSearchGUID + '" class="lnkCaseNumber" data-secure-linkremovediffound-id="1500">' + full.CaseNumber + '</a>';
                        }
                        

                    }
                },
                {
                    "data": "ClosedDate",
                    "render": function (data, type, full, meta) {
                        return '<span class="red">' + (data !== null ? data : '') + '</span>';
                    }
                },
                { "data": "PetitionType" },

                {
                    "data": "PetitionCloseDate",
                    "render": function (data, type, full, meta) {
                        if (data !== null)
                            return '<a style="cursor:pointer;" class="narativeLink" data-id="' + full.EncryptedRoleID + '">' + data + '</a>';
                        else
                            return '';
                    }
                },
                { "data": "PetitionDocketNumber" },
                { "data": "LeadAttorney" }
            ],
            fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.RoleClient === 1) {
                    $(nRow).addClass('highLightBlue');
                    removeLinkFromUnAuthorizedAnchorElements(secureIds);
                }
            }
        });
    }

    if (device.mobile() || device.tablet()) {
        var middleColumn = oTable.DataTable().column(2);
        middleColumn.visible(false);
    }

}



page.$table.on('draw.dt', function () {
    var rowIndex = simpleStorage.get(page.rowSelectedKey);
    if (rowIndex >= 0 && rowActive === false) {
        rowActive = true;

        page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
        page.$table.find("tr:nth-child(" + (rowIndex + 1) + ") td").css("background", "#f3f7b4");

        var pageNumber = simpleStorage.get(page.resultPageIdKey);
        if (pageNumber) page.$table.dataTable().fnPageChange(pageNumber);
    }

    //if mobile or tablet
    if (device.mobile() || device.tablet()) {
        $('#searchCase_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchCase_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

    setTimeout(function () {
        fitCalculatedHeightForSearchDataTable();
       
        
    },1000);
    
    

});

page.$table.on('page.dt', function () {

    var table = page.$table.dataTable();
    var oSettings = table.fnSettings();
    var pageNumber = oSettings._iDisplayLength === -1 ? 0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength);

    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
});

$('#search').on('click', function () {
    ClearCaseSearchGuidID();
    Search();
});


$('body').on('hidden.bs.modal', '.modal', function () {
    $(this).removeData('bs.modal');
});

$('body').on('click', '.narativeLink', function () {
    OpenPopup('/Case/PetitionAndRolePopUp/' + $(this).attr('data-id'), 'Petition Info');
});

$('body').on("click", "a.lnkCaseNumber", function (e) {
    var $this = $(this);
    e.preventDefault();
    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
    simpleStorage.set(GetWindowID() + '_SearchGuidID', $this.data('guid'), { TTL: page.ttl });
    document.location = $this.attr('href');

});

$('#reset').on('click', function (e) {
    e.preventDefault();
    var $formErrorContainer = $('#search-validation-error');
    $formErrorContainer.addClass('hidden');
    $('#case-search-form *').val('');
    $('#CountSearchResult').text('0');
    $('#case-search-form input:text:first').focus();
    ResetPageState();
    oTable.fnClearTable();
});

$(window).bind('resize', function () {
    $('#searchCase').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

$(document).ready(function () {
    if ($onViewLoad === "True") {
        ResetPageState();
        Search();
    } else {
        enableLoadDataFromCache();
        /*Load Default state of the form*/
        if ($isFromMain === "1" || isLoadDataFromCache())
            if (LoadPreviousFormState()) {
                Search();
            }
            else
                ResetPageState();
    }
});