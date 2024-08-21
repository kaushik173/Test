/**
* jtutorial.js
* purpose   - provide interactive, on-page flexible guide for user tasks and page training to reduce application training load
* version   - 0.1 (update 1)
* author    - Jack Barrett
* date      - 02/02/2020
*
* TODO      - fix animation rapid queue
*           - provide more error checking for configuration failiure
*           - fix step events not working for some targets like dropdowns
*           - eliminate page markup reliance, request and insert markup retroactively on first open
*           - split into two scripts, one that validates application for use and inserts open button if so, and the rest to be requested upon open
*           - host a prettier ES7 version with classes and host a Babel transformed ES5 version for IE on hand if
*           - fix bug where refreshing on saveStateForNext type step will take you next anyway
*/

var tutorialState = {
    // display/watch state
    "ready": false,
    "open": false,
    "tab": "tutorialList",
    "playing": false,
    "maskPath": { "primary": "", "holes": "", "containerOnly": "", "baseOnly": "" },
    "maskReady": false,
    "maskTargets": [],
    "playingTransitions": [],
    "scrollLevel": 0,
    "stepNum": null,
    "lastStepNum": null,
    "errors": [],

    // title/key state
    "tutorialName": null,
    "stepName": null,
    "pageName": null,

    // data/value state
    "fullData": null,
    "tutorialData": null,
    "stepData": null

}

// clone, don't copy reference
const tutorialStateInit = $.extend(true, {}, tutorialState);

// clicking on stacking elements can sometimes be complicated,
// triggering on an element might not have desired effects as there is missing information
var customClick = new jQuery.Event("click")

$(function () {
    /**
    * jQuery setup directive on page load
    * takes   - none
    * returns - none
    */

    // if session was passed a tutorial state, use it
    if (sessionStorage['tutorialState']) {
        tutorialState = JSON.parse(sessionStorage['tutorialState'])
        $('.tutorial-list').replaceWith(fullListToElement())
        if (tutorialState.hasOwnProperty('callNext') && tutorialState.callNext) {
            tutorialNext()
        }
        delete sessionStorage['tutorialState']
    }

    // locate page label and append open button
    var foundPageTitleElement = $('.page-header .form-group').last()
    if (!foundPageTitleElement || !foundPageTitleElement.children('label').first().html()) {
        console.warn('jcats tutorial was unable to locate a page title to append the open button')
        return
    }
    foundPageTitleElement.append($('<i>', { 'class': 'fa fa-question-circle tutorial-button tutorial-open available' }))
    tutorialState.pageName = foundPageTitleElement.children('label').first().html()

    // setup button handles

    $('i.tutorial-button').click(function (e) {
        e.preventDefault();
        if (!$(this).hasClass('available')) {
            return
        }

        if ($(this).hasClass('tutorial-close')) {
            tutorialClose()
        } else if ($(this).hasClass('tutorial-open')) {
            if (!tutorialState.open) {
                tutorialOpen()
            } else {
                tutorialClose()
            }
        } else if ($(this).hasClass('tutorial-play-pause')) {
            if (!tutorialState.playing) {
                tutorialPlay()
            } else {
                tutorialPause()
            }
        } else if ($(this).hasClass('tutorial-next')) {
            tutorialNext()
        } else if ($(this).hasClass('tutorial-prev')) {
            tutorialPrev()
        } else if ($(this).hasClass('tutorial-show-list')) {
            tutorialState.tab = (tutorialState.tab == 'tutorialList' && tutorialState.tutorialData != null) ? 'selectedTutorial' : 'tutorialList'
            refreshAll()
        }
    });

    // svg tactic needs a bit of help on zoom and resize, obstruct and recalculate paths when 'done'
    var resizeTime;
    var resizing = false;
    var resizeDelta = 300;
    $(window).on('resize', function () {
        if (tutorialState.stepData && tutorialState.stepData.hasOwnProperty('mask') && tutorialState.stepData.mask.length > 0) {
            $('.tutorial-mask-primary').css('background-color', '#000')
            $('.tutorial-mask-primary path').attr('stroke-width', '0px')
            $('.tutorial-mask-holes path').attr('stroke-width', '0px')
            tutorialState.maskReady = false
            applyMask('M0 0')

            resizeTime = new Date()
            if (resizing == false) {
                resizing = true;
                setTimeout(resizeEnd, resizeDelta);
            }
        }

        // recursive timer helper for this resize only
        function resizeEnd() {
            if (new Date() - resizeTime < resizeDelta) {
                setTimeout(resizeEnd, resizeDelta);
            } else {
                resizing = false;
                refreshMask()
            }
        }
    });

    $('.tutorial-navigate').click(function (e) {
        e.preventDefault()
        sessionStorage['tutorialState'] = JSON.stringify(tutorialState)
        window.location.href = $(this).attr('value')
    })

    refreshAll()
});

function fullListToElement() {
    /**
    * Constructs the full DOM element for the menu list from JSON retrieved on open
    * takes   - none
    * returns - the constructed jQuery element for the full menu
    */

    var fullListElement = $('<div>', { 'class': 'tutorial-list' })

    // need to use the keys().map() workaround of Object.values() for IE11
    Object.keys(tutorialState.fullData.groups).map(function(e) { return tutorialState.fullData.groups[e] } ).forEach(function (group, i) {
        var groupTitle = Object.keys(tutorialState.fullData.groups)[i]
        var groupElement = $('<div>', { 'class': 'tutorial-list-group' })
        var priorityFound = false
        Object.keys(group).map(function(e) { return group[e] } ).forEach(function (item, j) {
            var listItemElement = listItemToElement(Object.keys(group)[j], Object.keys(group).map(function(e) { return group[e] } )[j])
            if (!priorityFound && listItemElement.hasClass('priority')) {
                groupElement.prepend(listItemElement)
                priorityFound = true
            } else {
                groupElement.append(listItemElement)
            }
        })

        if (!priorityFound) {
            // no priority was found for the group, say so if the group should have one
            if (groupTitle == 'Pages') {
                groupElement.prepend($('<div>', { 'class': 'tutorial-list-item no-priority' }).append($('<div>', {'class': 'tutorial-list-item-title', 'text': 'Current page tutorial not found.'})))
            }
        }

        // prepend title element last to account for priority item, more elegant than a set up for $.after()
        groupElement.prepend($('<div>', { 'class': 'tutorial-list-group-title', 'text': groupTitle }))
        fullListElement.append(groupElement)
    })

    // bind them
    $(tutorialState.fullData.meta.widgetSelector).on('click', '.tutorial-list-item', function (e) {
        selectTutorial($(this).children('.tutorial-list-item-title').first().html())
    })

    return fullListElement
}

function listItemToElement(title, config) {
    /**
    * Constructs the DOM element for a single list item in the menu
    * takes   - title of tutorial as a string, the configuration object for the item
    * returns - the constructed jQuery element for the list item
    */

    var itemElement = $('<div>', { 'class': 'tutorial-list-item' })
    var tagsElement = $('<div>', { 'class': 'tutorial-list-item-tags' })

    if (title == tutorialState.pageName) {
        itemElement.addClass('priority')
    }

    // TODO: consider loading up user completion data from some UserTutorial table for this checkmark tag

    if (config.hasOwnProperty('startCondition')) {
        tagsElement.append($('<i>', { 'class': 'fa fa-info-circle tutorial-tag tutorial-tag-info', 'title': 'Start Condition: ' + config.startCondition }))
    }

    itemElement.append($('<div>', { 'class': 'tutorial-list-item-title', 'text': title })).append(tagsElement)
    return itemElement
}

function selectTutorial(title) {
    /**
    * Selects the tutorial with the given name and switches the tab to it
    * takes   - title of tutorial as a string
    * returns - none
    */

    // search data state for corresponding tutorial data, unset as search success criteria

    tutorialState.tutorialData = null
    Object.keys(tutorialState.fullData.groups).map(function(e) { return tutorialState.fullData.groups[e] } ).forEach(function (tutorialGroup) {
        if (tutorialGroup.hasOwnProperty(title)) {
            tutorialState.tutorialData = tutorialGroup[title]
            tutorialState.tutorialName = title
            tutorialState.stepNum = 1
            tutorialState.lastStepNum = Object.keys(tutorialState.tutorialData.steps).map(function(e) { return tutorialState.tutorialData.steps[e] } ).length || 1
            tutorialState.stepName = Object.keys(tutorialState.tutorialData.steps)[0]
            tutorialState.stepData = Object.keys(tutorialState.tutorialData.steps).map(function(e) { return tutorialState.tutorialData.steps[e] } )[0]
        }
    })

    if (!tutorialState.tutorialData) {
        // on faliure reset state, shouldn't happen, only existing ones get links
        tutorialState = tutorialStateInit
        console.warn(tutorialState.fullData.meta.errorTypes.tutorialNotFound)
        return
    }
    tutorialState.tab = 'selectedTutorial'
    $('.tutorial-related .tutorial-related-item').remove()

    refreshAll()
}

function refreshAll() {
    /**
    * updates how the tutorial widget, content, buttons, mask, and effects should display
    * takes   - none
    * returns - none
    */

    // TODO: refactor sections into state watchers so they only refresh when the relevant property changes

    if (tutorialState.fullData != null) {
        refreshValidate()
        refreshDisplayClass()
        refreshContent()
        triggerEvents()
        refreshMask()
    }
}

function refreshValidate() {
    /**
    * updates the validation state and access
    * takes   - none
    * returns - none
    */
    if (tutorialState.tab == 'selectedTutorial') {
        validateTutorial()
        var errorsElement = $('.tutorial-errors')

        errorsElement.html('')
        if (tutorialState.errors.length) {
            tutorialState.errors.forEach(function (error) {
                errorsElement.append($('<li>', { 'class': 'tutorial-errors-item', 'text': error }))
            })
            errorsElement.removeClass('tutorial-aspect-hidden')
        } else {
            errorsElement.addClass('tutorial-aspect-hidden')
        }
    }
}

function refreshDisplayClass() {
    /**
    * updates css classes that control tabs and button availiblity
    * takes   - none
    * returns - none
    */
    $(tutorialState.fullData.meta.wrapperSelector).toggleClass('tutorial-open', tutorialState.open).toggleClass('tutorial-playing', tutorialState.playing)
    $(tutorialState.fullData.meta.widgetSelector).toggleClass('tutorial-aspect-hidden', !tutorialState.open)
    $('.tutorial-play-pause').toggleClass('fa-play-circle', !tutorialState.playing).toggleClass('fa-pause', tutorialState.playing)
    $('.tutorial-list-caption').toggleClass('tutorial-aspect-hidden', tutorialState.tab != 'tutorialList' || !tutorialState.tab)
    $('.tutorial-selected-caption').html(tutorialState.stepNum + '/' + tutorialState.lastStepNum).toggleClass('tutorial-aspect-hidden', tutorialState.tab != 'selectedTutorial')
    $('.tutorial-list').toggleClass('tutorial-aspect-hidden', tutorialState.tab != 'tutorialList')
    $('.tutorial-content').toggleClass('tutorial-aspect-hidden', tutorialState.tab != 'selectedTutorial' || !tutorialState.tab)
    $('i.tutorial-button.tutorial-next').toggleClass('available', tutorialState.stepNum < tutorialState.lastStepNum && !tutorialState.errors.length
                                                                    && !(tutorialState.stepData != null && tutorialState.stepData['saveStateForNext']))
    $('i.tutorial-button.tutorial-prev').toggleClass('available', tutorialState.stepNum > 1)
    $('i.tutorial-button.tutorial-play-pause').toggleClass('available', tutorialState.tutorialData != null && tutorialState.lastStepNum > 1 && !tutorialState.errors.length)
    $('i.tutorial-button.tutorial-show-list').toggleClass('available', tutorialState.tab == 'selectedTutorial' || tutorialState.tutorialData != null)
                                        .toggleClass('fa-bars', tutorialState.tab == 'selectedTutorial')
                                        .toggleClass('fa-file', tutorialState.tab != 'selectedTutorial' || !tutorialState.tab)
                                        .attr('title', tutorialState.tab == 'selectedTutorial' ? 'show tutorial list' : 'show selected tutorial')


    if (tutorialState.errors.length) {
        $(tutorialState.fullData.meta.wrapperSelector).removeClass('tutorial-playing')
        $('.tutorial-next').removeClass('available')
        $('.tutorial-play-pause').removeClass('available')
    }
}

function refreshContent() {
    /**
    * updates how the tutorial content should display
    * takes   - none
    * returns - none
    */

    if (tutorialState.stepData && tutorialState.tab == 'selectedTutorial') {
        $('.tutorial-title').html(tutorialState.tutorialName)
        $('.tutorial-subtitle').html(tutorialState.stepName)
        $('.tutorial-body').html(tutorialState.stepData.body.join(''))
        if (tutorialState.stepData.hasOwnProperty('note')) {
            $('.tutorial-note').html(tutorialState.stepData.note)
        } else {
            $('.tutorial-note').html('')
        }
        if (tutorialState.tutorialData && tutorialState.tutorialData.hasOwnProperty('relatedTutorials')
            && tutorialState.tutorialData.relatedTutorials
            && tutorialState.stepNum == tutorialState.lastStepNum)
        {
            if (!$('.tutorial-related .tutorial-related-item').length) {
                tutorialState.tutorialData.relatedTutorials.forEach(function (relatedTutorialName) {
                    var matches = $('.tutorial-list-item-title').filter(function () { return $(this).html() === relatedTutorialName })
                    if (matches) {
                        $('.tutorial-related').append(matches.first().parent().clone().addClass('tutorial-related-item').get(0))

                    }
                })
            }
            $('.tutorial-related').removeClass('tutorial-aspect-hidden')
        } else {
            $('.tutorial-related .tutorial-related-item').remove()
            $('.tutorial-related').addClass('tutorial-aspect-hidden')
        }
    }
}

function triggerEvents() {
    // TODO: finish events

    if (tutorialState.stepData && tutorialState.stepData.hasOwnProperty('events')) {
        tutorialState.stepData.events.forEach(function (eventConfig) {
            // grab the element target
            var candidates = null
            var targetIndex = null
            if (eventConfig.targetType == "css") {
                candidates = $(eventConfig.target)
            }
            if (candidates == null) {
                console.warn(tutorialState.fullData.meta.errorTypes.eventTargetNotFound)
                return
            } else if (candidates.length == 1) {
                targetIndex = 0
            } else if (candidates.length > 1) {
                if (eventConfig.hasOwnProperty('filter')) {
                    if (eventConfig.hasOwnProperty('filterType') && eventConfig.filterType == 'index') {
                        if (!isNaN(parseInt(eventConfig.filter)) && eventConfig.filter > -1 && eventConfig.filter < candidates.length) {
                            targetIndex = eventConfig.filter
                        } else {
                            console.warn(tutorialState.fullData.meta.errorTypes.eventBadTargetIndexFilter)
                        }
                    } else {
                        // assume text, default
                        candidates.each(function (i) {
                            if (targetIndex == null && $(this).text().indexOf(eventConfig.filter) > -1) {
                                targetIndex = i
                            }
                        })
                    }
                } else {
                    console.warn(tutorialState.fullData.meta.errorTypes.eventTargetAmbiguous)
                    return
                }
            }

            // elemeent found, trigger event
            if ( targetIndex != null) {
                var el = candidates.eq(targetIndex)
                // console.log('tutorial event target found: triggering - ' + eventConfig.type)
                if (el.hasClass('dropdown-toggle')) {
                    console.warn("programatic dropdown toggle failure is a known bug, working on it")
                    el.dropdown('toggle');
                } else if (el.is('select')) {
                    console.warn("programatic dropdown toggle failure is a known bug, working on it")
                    el.trigger('open')
                } else {
                    el.trigger(eventConfig.type)
                }

            } else  {
                console.warn(tutorialState.fullData.meta.errorTypes.eventTargetAmbiguous)
            }
        })
    }
}

function setupMaskScroll () {
    // focus/scroll to highest mask target, then disable scrolling while tutorial active
    var scrollInit = $("html, body").scrollTop()
    $("html, body").scrollTop(0)

    // define scroll level for scroll transition
    tutorialState.scrollLevel = tutorialState.maskTargets[0].offset().top
                              - $(tutorialState.fullData.meta.widgetSelector).offset().top
                              - parseFloat($(tutorialState.fullData.meta.widgetSelector).css('padding-top') || 0)
                              - parseFloat($(tutorialState.fullData.meta.widgetSelector).css('border-top') || 0)
    var scrollMax = $(document).height() - $(window).height()
    if (tutorialState.scrollLevel > scrollMax) {
        tutorialState.scrollLevel = scrollMax
    } else if (tutorialState.scrollLevel < 0) {
        tutorialState.scrollLevel = 0
    }

    $("html, body").scrollTop(scrollInit)
}

function refreshMask() {
    /**
    * updates how the mask and effects should display
    * takes   - none
    * returns - none
    */
    if (tutorialState.errors.length) {
        $(tutorialState.fullData.meta.wrapperSelector).removeClass('tutorial-masked')
        return
    }

    if (tutorialState.open && tutorialState.stepData && tutorialState.stepData.hasOwnProperty('mask') && tutorialState.stepData.mask.length > 0) {
        tutorialState.maskReady = false
        $('html').css('overflow', 'hidden')
        getMaskTargets()
        if (tutorialState.maskTargets.length > 0) {
            $(tutorialState.fullData.meta.wrapperSelector).addClass('tutorial-masked')
            $('i.tutorial-button.tutorial-scroll-lock').removeClass('tutorial-aspect-hidden')
            getMaskPath([])
            applyMask(tutorialState.maskPath.containerOnly)
            setupMaskScroll()
            validateTransitions()

            playStepTransitions(0)


        }
        } else {
            $('html').css('overflow', 'auto')
            tutorialState.maskPath = tutorialStateInit.maskPath
            applyMask(tutorialState.maskPath)
            $(tutorialState.fullData.meta.wrapperSelector).removeClass('tutorial-masked')
            $('i.tutorial-button.tutorial-scroll-lock').addClass('tutorial-aspect-hidden')
        }
}

function afterTransition(transitionType, i) {
    if (transitionType == 'scrollMask') {
        getMaskPath(tutorialState.maskTargets)
        applyMask(tutorialState.maskPath)

        $('.tutorial-mask-primary').css('background-color', '')
        $('.tutorial-mask-primary path').attr('stroke-width', '3px')
        $('.tutorial-mask- path').attr('stroke-width', '5px')
        tutorialState.maskReady = true
    } else if (transitionType == 'blinkMask') {

    }
    playStepTransitions(i + 1)
}

function validateTransitions() {
    var unique = []
    if (tutorialState.errors.length) {
        return unique
    }
    if (tutorialState.stepData && !tutorialState.stepData.hasOwnProperty('transitions')) {
        tutorialState.stepData.transitions = []
    }

    tutorialState.stepData.transitions.unshift('scrollMask')

    tutorialState.stepData.transitions.forEach(function (transition) {
        var seen = false
        unique.forEach(function (item) {
            if (item == transition) {
                seen = true
            }
        })
        if (!seen) {
            if (!tutorialState.fullData.meta.animations.hasOwnProperty(transition)) {
                console.warn(tutorialState.fullData.meta.errorTypes.animationNotFound, 'name: ' + transition)
                return
            }
            unique.push(transition)
        }
    })
    tutorialState.stepData.transitions = unique
}

function applyMask(maskPath) {
    /**
    * apply to the DOM the given mask path object accordingly, or string to all masks, or path from state if no argument given
    * takes   - maskPath Obj, string, or undefined
    * returns - none
    */
    var maskPath = (typeof maskPath !== 'undefined') ? maskPath : tutorialState.maskPath
    $('.tutorial-mask-primary path').attr('d', maskPath && maskPath.hasOwnProperty('primary') ? maskPath.primary : maskPath)
    $('.tutorial-mask-holes path').attr('d', maskPath.hasOwnProperty('holes') ? maskPath.holes : "")
}

function getMaskTargets() {
    /**
    * overlay an svg and append counter-clockwise box draws to cut holes in the mask 
    * (more elegant solutions exist in css blend-modes, but no version of IE or Edge supports them)
    * takes   - none
    * returns - jQuery elements to pass to getMaskPath()
    */
    var foundTargets = []
    var focusIndex = 0
    tutorialState.maskTargets = []
    tutorialState.stepData.mask.forEach(function (item, i) {
        if (item.targetType == 'css') {
            foundTargets.push($(item.target))
        } else if (item.targetType == 'widgetCaption') {
            $('.widget').each(function (element) {
                if ($(this).find('.widget-caption').first().html().toLowerCase().indexOf(item.target.toLowerCase()) >= 0) {
                    foundTargets.push($(this))
                    return false
                }
            })
            if (item.hasOwnProperty('targetAlternate')) {
                // try alternate targets, sometimes there are variable spellings or plurals
                $('.widget').each(function (element) {
                    if ($(this).find('.widget-caption').first().html().toLowerCase().indexOf(item.targetAlternate.toLowerCase()) >= 0) {
                        foundTargets.push($(this))
                        return false
                    }
                })
            }
        }

        if (foundTargets.length == i) {
            // nothing was added for this configuration item, add an empty entry
            console.warn(tutorialState.fullData.meta.errorTypes.maskNotFound)
            foundTargets.push(null)
        }

        if (i > focusIndex && foundTargets[i] != null) {
            if (foundTargets[focusIndex] == null || foundTargets[i].offset().top < foundTargets[focusIndex].offset().top) {
                focusIndex = i
            }
        }
    })
    // swap focus index to be first item (don't care where the other one goes)
    if (focusIndex > 0) {
        var temp = foundTargets[0]
        foundTargets[0] = foundTargets[focusIndex]
        foundTargets[focusIndex] = temp
    }

    tutorialState.maskTargets = foundTargets
}

function getMaskPath(maskTargets) {
    /**
    * overlay an svg and append counter-clockwise box draws to cut holes in the mask 
    * (more elegant solutions exist in css blend-modes, but no version of IE or Edge supports them)
    * takes   - number of aspects to chop off the front, default 0
    * returns - string to be placed in the 'd' element of the svg path
    */

    var maskElement = $('.tutorial-mask-primary')
    var runningPath = 'M0 0 h' + maskElement.width() + ' v' + maskElement.height() + ' h-' + maskElement.width() + ' z '
    var holesPath = ''

    runningPath += getHolePath($(tutorialState.fullData.meta.widgetSelector).children().eq(0), -1)
    var containerOnlyPath = runningPath

    maskTargets.forEach(function (target, i) {
        if (target) {
            var hole = getHolePath(target, i)
            runningPath += hole
            holesPath += hole
        }
    })

    tutorialState.maskPath = { 'primary': runningPath, 'holes': holesPath, 'containerOnly': containerOnlyPath }
}

function getHolePath(target, index) {
    /**
    * builds a sub-path string to append to a running mask path for a given element
    * takes   - config for one mask JSON object, overlay overflow to take into consideration
    * returns - svg 'd' attribute sub-path string
    */

    var runningPath = ''

    var box = [0, 0, 0, 0]
    var parentOverflow = -parseFloat($('.tutorial-mask-primary').css('top'))
    var scroll = tutorialState.scrollLevel || 0

    box[0] = target.offset().top - parseFloat(target.css('padding-top')) + parentOverflow - scroll
    box[1] = target.offset().left - parseFloat(target.css('padding-left')) + parentOverflow
    box[2] = target.innerHeight()
    box[3] = target.innerWidth()

    if (index < 0) {
        // it is an inherent item with no respective configuration (i.e. the tutorial content box)
        return 'M' + box[1] + ' ' + box[0] + ' v' + box[2] + ' h' + box[3] + ' v-' + box[2] + 'z '
    }

    var respectiveConfig = tutorialState.stepData.mask[index]

    if (respectiveConfig.hasOwnProperty('modifiers')) {
        for (var i = 0; i < Object.keys(respectiveConfig.modifiers).length; i++) {
            // translate top-left-right-left modifiers into the top-left-height-width that path expects
            if (respectiveConfig.modifiers.hasOwnProperty('addTop')) {
                var mod = parseFloat(respectiveConfig.modifiers.addTop)
                box[0] -= mod
                box[2] += mod
            }
            if (respectiveConfig.modifiers.hasOwnProperty('addRight')) {
                box[3] += parseFloat(respectiveConfig.modifiers.addRight)
            }
            if (respectiveConfig.modifiers.hasOwnProperty('addBottom')) {
                box[2] += parseFloat(respectiveConfig.modifiers.addBottom)
            }
            if (respectiveConfig.modifiers.hasOwnProperty('addLeft')) {
                var mod = parseFloat(respectiveConfig.modifiers.addLeft)
                box[1] -= mod
                box[3] += mod
            }

            // TODO: add new modifiers for misbehaving boxes
        }
    }
    runningPath += 'M' + box[1] + ' ' + box[0] + ' v' + box[2] + ' h' + box[3] + ' v-' + box[2] + 'z '

    return runningPath
}

function playStepTransitions(pass) {
    /**
    * checks if step has a mask and effects, then plays them
    * takes   - none
    * returns - none
    */

    var pass = (typeof pass !== undefined) ? pass : 0

    if (tutorialState.stepData.hasOwnProperty('transitions') && tutorialState.stepData.transitions.length > 0) {
        //quick bounds check
        if (pass < 0) {
            pass = 0
        } else if (pass >= tutorialState.stepData.transitions.length) {
            // finished playing all
            return
        }

        var stepAnimation = tutorialState.stepData.transitions[pass]

        // set up initial values and references from generic information
        var stateMapGet = {'scrollLevel': tutorialState.scrollLevel }
        var config = tutorialState.fullData.meta.animations[stepAnimation]
        var animationEl = $(config.valueTarget)
            
        var setup = {
            'animationEl': $(config.valueTarget),
            'valueInit': config.valueType == "property" ? animationEl[config.value] : animationEl.css(config.value),
            'animationObj': {},
            'periods': config.hasOwnProperty('periodEase') ? config.periods : 1,
            'periodEase': config.hasOwnProperty('periodEase') && config.periodEase,
            'remain': config.hasOwnProperty('valueRemain') && config.valueRemain
        }

        if (config.valueEnd == "stateMapValue" && config.hasOwnProperty('stateMapValue')) {
            if (stateMapGet.hasOwnProperty(config.stateMapValue)) {
                setup.animationObj[config.value] = stateMapGet[config.stateMapValue]
            }
        } else {
            setup.animationObj[config.value] = config.valueEnd
        }
        if (!setup.animationObj) {
            console.warn(tutorialState.fullData.meta.errorTypes.animationMapError)
            return
        }

        if (tutorialState.fullData.meta.useAnimations) {
            runAnimation(1)
        } else {
            animationEl.animate(setup.animationObj, 0, function () {
                afterTransition(stepAnimation, pass)
            })
        }
    }

    // helper recursive animation
    function runAnimation(period) {
        if (period > setup.periods) {
            // TODO: sometimes when entering animations rapidly value does not revert to intended original
            setup.animationObj[config.value]
            afterTransition(stepAnimation, pass)
            return
        }
        var revert = (period !== 1 && period < setup.periods) || (!setup.remain && period == setup.periods)

        animationEl.animate(setup.animationObj, config.duration, function () {
            if (revert) {
                setup.animationObj[config.value] = config.valueOrig || setup.valueInit
                animationEl.animate(setup.animationObj, setup.periodEase ? config.duration : 0, function () {
                    runAnimation(period + 1)
                })
            } else {
                runAnimation(period + 1)
            }
        })
    }
}

function validateTutorial() {
    /**
    * conditions checked before a tutorial starts/navigates
    * takes   - one tutorial config JSON object
    * returns - none, error stored in state
    */
    var errors = []

    if (!tutorialState.tutorialData.hasOwnProperty('steps')) {
        errors.push('This tutorial has no steps.')
    }

    if (tutorialState.tutorialData.hasOwnProperty('startCondition')) {
        // select a case: Check the case status bar for a JCATS number
        if (tutorialState.tutorialData.startCondition == 'select a case' && !$('.copy-case-info').html()) {
            errors.push(tutorialState.fullData.meta.errorTypes.selectCase)
        }
        
        // TODO: define new conditions here
    }

    // if user is not on the appropriate page for this step, show the navigate button with the appropriate link
    var foundStartPage = tutorialState.stepData.startPage || tutorialState.tutorialData.startPage || ''
    var startPageInURL = window.location.href.indexOf(foundStartPage) >= 0
    if (!tutorialState.stepData.hasOwnProperty('overrideStartPage') || !tutorialState.stepData.overrideStartPage) {
        if (foundStartPage && !startPageInURL) {
            $('.tutorial-navigate').attr('value', foundStartPage).html('Go to ' + foundStartPage + '   ').append($('<i>', { 'class': 'fa fa-long-arrow-right' })).removeClass('tutorial-aspect-hidden')
            errors.push(tutorialState.fullData.meta.errorTypes.navigate)
        } else {
            $('.tutorial-navigate').attr('value', '').html('').addClass('tutorial-aspect-hidden')
        }
    }

    if (tutorialState.stepData != null) {
        if (tutorialState.stepData.hasOwnProperty('saveStateForNext') && tutorialState.stepData.saveStateForNext) {
            // in case the page intends to navigate away outside the control of a tutorial button
            // clone of state, not same references
            var temp = $.extend(true, {}, tutorialState);
            temp['callNext'] = true
            sessionStorage['tutorialState'] = JSON.stringify(temp)
        } else if (sessionStorage.hasOwnProperty['tutorialState']) {
             delete sessionStorage['tutorialState']
        }
    }

    tutorialState.errors = errors
}

function tutorialClose() {
    /**
    * Make appropriate state changes to close the tutorial and update
    * takes   - none
    * returns - none
    */

    tutorialState.playing = false
    tutorialState.open = false
    refreshAll()
}

function fetchData() {
    // initialize data
    $.ajax({
        "url": "/Help/TutorialConfig",
        "dataType": "text",
        "cache": false,
        "success": function (serial) {
            tutorialState.fullData = JSON.parse(serial)
        }
    }).then(function () {
        if (!tutorialState.fullData) {
            // the rest of the errors should be contained in tutorialState.fullData.meta.errorTypes
            console.error('jcats tutorial was unable to load configuration data from file')
            return
        }
        $('.tutorial-list').replaceWith(fullListToElement())

        if (!$('.tutorial-list')) {
            console.error(tutorialState.fullData.meta.errorTypes.listElementFail)
            return false
        }
        tutorialState.ready = true
        refreshAll()
    })
}

function tutorialOpen() {
    /**
    * Make appropriate state changes to open the tutorial and update, load content if not already loaded
    * takes   - none
    * returns - none
    */

    tutorialState.open = true

    if (!tutorialState.ready || tutorialState.fullData == null) {
        fetchData()
    } else {
        refreshAll()
    }
}

function tutorialNext() {
    /**
    * Advance to the next step in the selected tutorial, upate state references
    * takes   - none
    * returns - none
    */

    if (tutorialState.stepNum < tutorialState.lastStepNum && !tutorialState.errors.length) {
        tutorialState.stepNum++
        if (tutorialState.tutorialData) {
            tutorialState.stepName = Object.keys(tutorialState.tutorialData.steps)[tutorialState.stepNum - 1]
            tutorialState.stepData = Object.keys(tutorialState.tutorialData.steps).map(function(e) { return tutorialState.tutorialData.steps[e] } )[tutorialState.stepNum - 1]
        }
    }
    refreshAll()
}

function tutorialPrev() {
    /**
    * Return to the previous step in the selected tutorial, upate state references
    * takes   - none
    * returns - none
    */

    if (tutorialState.stepNum > 1) {
        tutorialState.stepNum--
        if (tutorialState.tutorialData) {
            tutorialState.stepName = Object.keys(tutorialState.tutorialData.steps)[tutorialState.stepNum - 1]
            tutorialState.stepData = Object.keys(tutorialState.tutorialData.steps).map(function(e) { return tutorialState.tutorialData.steps[e] } )[tutorialState.stepNum - 1]
        }
    }
    refreshAll()
}

function tutorialPlay() {
    // TODO: re-implement
    return
}

function tutorialPause() {
    // TODO: re-implement
    return
}