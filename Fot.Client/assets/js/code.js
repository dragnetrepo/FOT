'use strict';

var bundle = null;
var instructionTemplate = null;
var starterTemplate = null;
var assessmentTemplate = null;
var essayTemplate = null;
var essayStarterTemplate = null;
var assessmentEndTemplate = null;



$(function () {

    $("#img").show();
    $("#img").css("display", "block");

    var loader_url = $("#loader").data("request-url-get-test");





    $.get(loader_url).done(function (data) {

        bundle = data;

        if (bundle.flagsuccess === true) {

            if (bundle.current_assessment_index < 0) {

                bundle.current_assessment_index = 0;

            }

           

            if (bundle.assessments[bundle.current_assessment_index].started) {

                instructionPage.moveNext();

            } else {

              
                $("#img").prop("src", "data:image/png;base64," + bundle.bundle_icon);
               $("#bttn").show(); 
            }



        } else {
            var error = "<h1> " + bundle.error_message + "</h1>";

            $("#content").html(error);
        }

    });



    //compile templates.

    instructionTemplate = Handlebars.compile($("#instruction").html());
    starterTemplate = Handlebars.compile($("#starter").html());
    assessmentTemplate = Handlebars.compile($("#assessment").html());
    essayTemplate = Handlebars.compile($("#essay").html());
    essayStarterTemplate = Handlebars.compile($("#essayStarter").html());
    assessmentEndTemplate = Handlebars.compile($("#assessmentEnd").html());


});



var firstPage = (function () {

    var moveNext = function () {

        var html = instructionTemplate(bundle.assessments[bundle.current_assessment_index]);

        $("#container").html(html);
    }

    return {
        moveNext: moveNext
    };

})();


var instructionPage = (function () {


    var moveNext = function () {

        var timed = bundle.assessments[bundle.current_assessment_index].timed;
        var duration = bundle.assessments[bundle.current_assessment_index].duration;

        var html = '';
        var assessment_name = bundle.assessments[bundle.current_assessment_index].assessment_name;
        var btn_text = '';

        if (bundle.assessments[bundle.current_assessment_index].assessment_type === 'MCQ') {

        var message = "";
         btn_text = "Start Test";

        if (timed == true) {

            if (bundle.assessments[bundle.current_assessment_index].started == true) {
                message = "You have started this assessment previously and you have " + bundle.assessments[bundle.current_assessment_index].time_remaining + " minutes left to complete it. The timer will continue when you click on 'Continue Test' .";
                btn_text = "Continue Test";
            } else {
                message = "You will have " + duration + " minutes to work on this test. The timer will start when you click on 'Start Test' .";
            }

        } else {

            message = "You have no time limit on this assesment. You may click on 'Start Test' to begin.";
        }



        var context = { assessment_name: assessment_name, message: message, btn_text: btn_text };

        

            html = starterTemplate(context);
        } else {

            var essay_message = "";
             btn_text = "Start Test";

            if (timed == true) {

                if (bundle.assessments[bundle.current_assessment_index].started == true) {
                    essay_message = "You have started this assessment previously and you have " + bundle.assessments[bundle.current_assessment_index].time_remaining + " minutes left to complete it. The timer will continue when you click on 'Continue Test' .";
                    btn_text = "Continue Test";
                } else {
                    essay_message = "You will have " + duration + " minutes to work on this test. The timer will start when you click on 'Start Test'. Select your preferred essay topic from the list below.";
                }

            } else {

                essay_message = "You have no time limit on this assesment. You may click on 'Start Test' to begin.";
            }


            var essay_context = { assessment_name: assessment_name, message: essay_message, btn_text: btn_text };


            
            html = essayStarterTemplate(essay_context);
        }

        

        $("#container").html(html);


        if (bundle.assessments[bundle.current_assessment_index].assessment_type !== 'MCQ') {

            var optionsHtml = '';

            var selectedTopic = '';

            for (var x = 0; x < bundle.assessments[bundle.current_assessment_index].essays.length; x++) {

                if (bundle.assessments[bundle.current_assessment_index].essays[x].selected) {

                    optionsHtml += "<option value='" + x + "' selected>Topic " + (x + 1) + " </option>";

                    selectedTopic = bundle.assessments[bundle.current_assessment_index].essays[x].topic;

                    

                } else {

                    optionsHtml += "<option value='" + x + "'>Topic " + (x + 1) + " </option>";
                    
                }
                

            }



            $("#listTopics").html(optionsHtml);

            if (selectedTopic !== '') $("#listTopics").prop("disabled", true);

            $("#divTopic").html(selectedTopic === '' ? bundle.assessments[bundle.current_assessment_index].essays[0].topic : selectedTopic);




        }

    }

    return {
        moveNext: moveNext

    };

})();



var starterPage = (function () {


    var startTest = function () {



        var assessment_name = bundle.assessments[bundle.current_assessment_index].assessment_name;
        var questions = bundle.assessments[bundle.current_assessment_index].questions;

        if (bundle.assessments[bundle.current_assessment_index].started) {

            if (bundle.assessments[bundle.current_assessment_index].timed) {
                bundle.assessments[bundle.current_assessment_index].duration = bundle.assessments[bundle.current_assessment_index].time_remaining;
            }


        } else {
            bundle.assessments[bundle.current_assessment_index].current_question_index = 0;
            bundle.assessments[bundle.current_assessment_index].questions[0].seen = true;
        }



        var question_count = "Question " + (bundle.assessments[bundle.current_assessment_index].current_question_index + 1) + " of " + questions.length;

        var item_img = questions[bundle.assessments[bundle.current_assessment_index].current_question_index].item_img;

        var question_text = questions[bundle.assessments[bundle.current_assessment_index].current_question_index].question_text;

        var question_options = getOptions(questions[bundle.assessments[bundle.current_assessment_index].current_question_index]);

        var context = {
            assessment_name: assessment_name,
            question_count: question_count,
            item_img: item_img,
            question_text: question_text,
            question_options: question_options
        };
        var html = assessmentTemplate(context);

        $("#container").html(html);

        assessmentPage.init();


    }

    var getOptions = function (question) {


        var localQuestionIndex = bundle.assessments[bundle.current_assessment_index].current_question_index;
        var options = [];
        var optionsHtml = '';
        var letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];

        for (var i = 0; i < question.answers.length; i++) {

            var checked = question.answers[i].selected ? "checked" : "";

            if (question.options_layout_is_vertical == true) {

                if (question.answer_type === "Single") {


                    if (question.answers[i].is_image == true) {

                        options.push('<div class="radio"><span style="margin-right: 20px;font-weight: bold; color: #666;">[', letters[i], ']</span><label><input type="radio" name="', question.question_id, '" id="', localQuestionIndex + "_" + i, '" value="', i, '" ', checked, ' onclick="assessmentPage.saveRadioState(this)">', '<img src="data:image/png;base64,', question.answers[i].answer_image, '" style="max-height: 90px;" />', '</label></div>');

                    } else {
                        
                        options.push('<div class="radio"><span style="margin-right: 20px;font-weight: bold; color: #666;">[', letters[i], ']</span><label><input type="radio" name="', question.question_id, '" id="', localQuestionIndex + "_" + i, '" value="', i, '" ', checked, ' onclick="assessmentPage.saveRadioState(this)">', question.answers[i].answer_text, '</label></div>');


                    }

                } else {

                    if (question.answers[i].is_image == true) {
                        options.push('<div class="checkbox"><span style="margin-right: 20px;font-weight:bold; color: #666;">[', letters[i], ']</span><label><input type="checkbox" id="', localQuestionIndex + "_" + i, '"  value="', i, '" ', checked, ' onclick="assessmentPage.saveCheckBoxState(this)">', '<img src="data:image/png;base64,', question.answers[i].answer_image, '" style="max-height: 90px;" />' , '</label></div>');

                    } else {
                        
                        options.push('<div class="checkbox"><span style="margin-right: 20px;font-weight:bold; color: #666;">[', letters[i], ']</span><label><input type="checkbox" id="', localQuestionIndex + "_" + i, '"  value="', i, '" ', checked, ' onclick="assessmentPage.saveCheckBoxState(this)">', question.answers[i].answer_text, '</label></div>');
                    }
                }

            } else {
                

                if (question.answer_type === "Single") {


                    if (question.answers[i].is_image == true) {

                        options.push('<label class="radio-inline">', '<span style="margin-right: 20px;font-weight: bold; color: #666;">[', letters[i], ']</span>', '<input type="radio" name="', question.question_id, '" id="', localQuestionIndex + "_" + i, '" value="', i, '" ', checked, ' onclick="assessmentPage.saveRadioState(this)">', '<img src="data:image/png;base64,', question.answers[i].answer_image, '" style="max-height: 90px;" />', '</label>');

                    } else {

                        options.push('<label class="radio-inline">', '<span style="margin-right: 20px;font-weight: bold; color: #666;">[', letters[i], ']</span>', '<input type="radio" name="', question.question_id, '" id="', localQuestionIndex + "_" + i, '" value="', i, '" ', checked, ' onclick="assessmentPage.saveRadioState(this)">', question.answers[i].answer_text, '</label>');


                    }

                } else {

                    if (question.answers[i].is_image == true) {
                        options.push('<label class="checkbox-inline">', '<span style="margin-right: 20px;font-weight:bold; color: #666;">[', letters[i], ']</span>', '<input type="checkbox" id="', localQuestionIndex + "_" + i, '"  value="', i, '" ', checked, ' onclick="assessmentPage.saveCheckBoxState(this)">', '<img src="data:image/png;base64,', question.answers[i].answer_image, '" style="max-height: 90px;" />', '</label>');

                    } else {

                        options.push('<label class="checkbox-inline">', '<span style="margin-right: 20px;font-weight:bold; color: #666;">[', letters[i], ']</span>', '<input type="checkbox" id="', localQuestionIndex + "_" + i, '"  value="', i, '" ', checked, ' onclick="assessmentPage.saveCheckBoxState(this)">', question.answers[i].answer_text, '</label>');
                    }
                }

            }


        }


        optionsHtml = options.join('');

        if (question.options_layout_is_vertical == false) {

            optionsHtml = '<div style="padding-top: 20px;">' + optionsHtml + '</div>';
        }

        return optionsHtml;
    }

    return {
        startTest: startTest,
        getOptions: getOptions

    };

})();



var essayStarterPage = (function () {


    var startTest = function () {



        var assessment_name = bundle.assessments[bundle.current_assessment_index].assessment_name;
        var essays = bundle.assessments[bundle.current_assessment_index].essays;

        if (bundle.assessments[bundle.current_assessment_index].started) {

            if (bundle.assessments[bundle.current_assessment_index].timed) {
                bundle.assessments[bundle.current_assessment_index].duration = bundle.assessments[bundle.current_assessment_index].time_remaining;
            }


        } else {
            bundle.assessments[bundle.current_assessment_index].current_question_index = parseInt($("#listTopics").val());
            essays[bundle.assessments[bundle.current_assessment_index].current_question_index].selected = true;
        }



        var essay_topic = essays[bundle.assessments[bundle.current_assessment_index].current_question_index].topic;

        var candidate_response = essays[bundle.assessments[bundle.current_assessment_index].current_question_index].candidate_response;

        var context = {
            assessment_name: assessment_name,
            topic: essay_topic,
            candidate_response: candidate_response
        };
        var html = essayTemplate(context);

        $("#container").html(html);

        $("#editor").kendoEditor();

        essayPage.init(); 


    }


    var changeTopic = function() {

        var topic_index = parseInt($("#listTopics").val());

        $("#divTopic").html(bundle.assessments[bundle.current_assessment_index].essays[topic_index].topic);

    }


    return {
        startTest: startTest,
        changeTopic: changeTopic
        

    };

})();

var assessmentPage = (function () {


    var IsLastPage = false;

    var interval;
    var seconds = 0;
    var minutes = 0;


    var init = function () {

        $("#bttnPrevious").hide();


        if (bundle.assessments[bundle.current_assessment_index].timed) {

            minutes = bundle.assessments[bundle.current_assessment_index].duration;

            showTime(minutes, 0);

            interval = setInterval(timerTick, 1000);

            $("#divTimerWrapper").show();

        } else {

            $("#divTimerWrapper").hide();
        }

        if (bundle.assessments[bundle.current_assessment_index].started) {

            gotoPage(bundle.assessments[bundle.current_assessment_index].current_question_index);


        } else {
            bundle.assessments[bundle.current_assessment_index].started = true;
        }

        LoadNav();

        $("body").on('keyup', function (key) {

            if (key.keyCode == 39 && IsLastPage == false) {
                moveNext();
            }
            else if (key.keyCode == 37 && bundle.assessments[bundle.current_assessment_index].current_question_index > 0) {
                movePrevious();
            }

            else if ((key.keyCode >= 65 && key.keyCode <= 74) && (bundle.assessments[bundle.current_assessment_index].current_question_index >= 0 && IsLastPage == false)) {

                selectOption(bundle.assessments[bundle.current_assessment_index].current_question_index, getAnswerIndex(key.keyCode));

            }


        });
    }


    var selectOption = function (questionIndex, optionIndex) {


        var selectObj = $("#" + questionIndex + "_" + optionIndex);

        if (selectObj) {

            var selType = $(selectObj).prop("type");

            if (selType == "radio") {
                $(selectObj).prop("checked", true).trigger("click");
            } else {
                $(selectObj).click();
            }


        }


    }


    var getAnswerIndex = function (keyCode) {

        var answerIndex = 0;

        switch (keyCode) {
            case 65:
                answerIndex = 0;
                break;
            case 66:
                answerIndex = 1;
                break;
            case 67:
                answerIndex = 2;
                break;
            case 68:
                answerIndex = 3;
                break;
            case 69:
                answerIndex = 4;
                break;
            case 70:
                answerIndex = 5;
                break;
            case 71:
                answerIndex = 6;
                break;
            case 72:
                answerIndex = 7;
                break;
            case 73:
                answerIndex = 8;
                break;
            case 74:
                answerIndex = 9;
                break;
        }


        return answerIndex;
    }

    var LoadNav = function () {


        var current = bundle.assessments[bundle.current_assessment_index].current_question_index;

        var prefix = "<ul class='pagination' style='margin: 0;'>";
        var suffix = "</ul>";
        var navArray = [];

        for (var ctr = 0; ctr < bundle.assessments[bundle.current_assessment_index].questions.length; ctr++) {

            var num = ctr + 1;

            var numStr = "<li><a href='javascript:void(0)' onclick='assessmentPage.moveTo(" + ctr + ");'> " + num + "</a></li>";

            if (ctr == current) {

                numStr = "<li class='active'><a href='javascript:void(0)'> " + num + "</a></li>";
            }
            else {

                if (bundle.assessments[bundle.current_assessment_index].questions[ctr].seen == true) {

                    numStr = "<li><a href='javascript:void(0)' onclick='assessmentPage.moveTo(" + ctr + ");' style='background-color: #fb9494;color: #000;'> " + num + "</a></li>";
                }

                if (bundle.assessments[bundle.current_assessment_index].questions[ctr].answered == true) {

                    numStr = "<li><a href='javascript:void(0)' onclick='assessmentPage.moveTo(" + ctr + ");' style='background-color: #a0eb9d;color: #333;'> " + num + "</a></li>";
                }
            }
            navArray.push(numStr);

        }

        var navHtml = prefix + navArray.join('') + suffix;

        $("#navDiv").html(navHtml);

    }

    function timerTick() {

        if (seconds == 0) {
            seconds = 60; //60
            minutes--;

            bundle.assessments[bundle.current_assessment_index].time_remaining = minutes;


            //save state

            var tempBundle = JSON.parse(JSON.stringify(bundle));

            tempBundle.bundle_name = '';
            tempBundle.bundle_icon = '';
            tempBundle.candidate_photo = '';


            for (var j = 0; j < tempBundle.assessments.length; j++) {

                tempBundle.assessments[j].assessment_name = '';
                tempBundle.assessments[j].instruction_image = '';

                if (tempBundle.assessments[j].assessment_type === 'MCQ') {

                    for (var i = 0; i < tempBundle.assessments[j].questions.length; i++) {

                        tempBundle.assessments[j].questions[i].item_img = '';
                        tempBundle.assessments[j].questions[i].question_text = '';

                        for (var t = 0; t < tempBundle.assessments[j].questions[i].answers.length; t++) {

                            tempBundle.assessments[j].questions[i].answers[t].answer_image = '';
                            tempBundle.assessments[j].questions[i].answers[t].answer_text = '';


                        }

                    }

                } else {

                    for (var i = 0; i < tempBundle.assessments[j].essays.length; i++) {

                        tempBundle.assessments[j].essays[i].topic = '';

                    }
                }

            }

           

            var timeRemaining = bundle.assessments[bundle.current_assessment_index].time_remaining;
            var assessmentName = bundle.assessments[bundle.current_assessment_index].assessment_name;

            var saveData = { bundle: tempBundle, assessmentName: assessmentName, timeRemaining: timeRemaining };

            var saver_url = $("#saver").data("request-url-save-test");

            $.ajax({
                url: saver_url,
                data: JSON.stringify(saveData),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                timeout: 5000
            })
                .done(function (result) {
                    
                })
                .fail(function () {
                   
                });

        }

        showTime(minutes, --seconds);

        if (minutes <= 0 && seconds <= 0) {


            progressAssessment();

        }

    }


    function progressAssessment() {

        if (bundle.assessments[bundle.current_assessment_index].timed) {

            clearInterval(interval);
        }

     
        if (bundle.current_assessment_index == (bundle.assessments.length - 1)) {

            $("body").off('keyup');

            //end assessment session

            var html = assessmentEndTemplate(bundle);

            $("#container").html(html);

            assessmentEndPage.submitAssessment();


        } else {

            //progress assessment 
            bundle.current_assessment_index++;

            minutes = 0;
            seconds = 0;

            firstPage.moveNext();

        }




    }


    var doEnd = function () {

        if (confirm("Are you sure you want to end the assessment ?")) {

            progressAssessment();
        }
    }

    var moveTo = function (pageNum) {

        gotoPage(pageNum);
    }

    var moveNext = function () {


        if (bundle.assessments[bundle.current_assessment_index].current_question_index < (bundle.assessments[bundle.current_assessment_index].questions.length - 1)) {

            gotoPage(++bundle.assessments[bundle.current_assessment_index].current_question_index);

            if (bundle.assessments[bundle.current_assessment_index].current_question_index > 0) {
                $("#bttnPrevious").show();
            }

        } else {

            showLastPage();
        }
    }

    function hideItems(flag) {

        if (flag == true) {

            $("#divQuestionCount").hide();
            $("#divQuestion").hide();
            $("#divAdditional").hide();
            $("#divOptions").hide();
            $("#bttnNext").hide();
            $("#navDiv").hide();

        } else {

            $("#divQuestionCount").show();
            $("#divQuestion").show();
            $("#divAdditional").show();
            $("#divOptions").show();
            $("#bttnNext").show();
            $("#navDiv").show();
        }
    }

    function showLastPage() {

        hideItems(true);
        $("#divEnd").show();
        $("#bttnEnd").show();
        IsLastPage = true;
    }

    function showTime(min, sec) {

        var totalMins = min;
        var hrs = 0;
        var mins = totalMins;

        if (totalMins >= 60) {
            hrs = parseInt(totalMins / 60);
            mins = totalMins % 60;
        }
        var hrStr = hrs + "";
        var minStr = mins + "";

        if (sec == 60) {

            sec = 0;
        }
        var secStr = sec + "";

        if (secStr.length == 1) secStr = "0" + secStr;

        if (hrStr.length == 1) {
            hrStr = "0" + hrStr;
        }
        if (minStr.length == 1) {
            minStr = "0" + minStr;
        }

        $("#divTimer").text(hrStr + ":" + minStr + ":" + secStr);

    }


    var movePrevious = function () {

        if (IsLastPage == true) {

            hideItems(false);
            gotoPage(bundle.assessments[bundle.current_assessment_index].current_question_index);
            IsLastPage = false;
            $("#divEnd").hide();
            $("#bttnEnd").hide();


        } else {

            gotoPage(--bundle.assessments[bundle.current_assessment_index].current_question_index);
            if (bundle.assessments[bundle.current_assessment_index].current_question_index == 0) {
                $("#bttnPrevious").hide();
            }

        }



    }

    function gotoPage(index) {



        $("#item_img").prop("src", "data:image/png;base64," + bundle.assessments[bundle.current_assessment_index].questions[index].item_img);

        $("#divAdditional").text(bundle.assessments[bundle.current_assessment_index].questions[index].question_text);

        $("#divQuestionCount").text("Question " + (index + 1) + " of " + bundle.assessments[bundle.current_assessment_index].questions.length);

        bundle.assessments[bundle.current_assessment_index].questions[index].seen = true;
        bundle.assessments[bundle.current_assessment_index].current_question_index = index;


        var optionsHtml = starterPage.getOptions(bundle.assessments[bundle.current_assessment_index].questions[index]);

        $("#divOptions").html(optionsHtml);




        if (bundle.assessments[bundle.current_assessment_index].current_question_index == 0) {
            $("#bttnPrevious").hide();
        } else {
            $("#bttnPrevious").show();
        }

        LoadNav();
    }

    var saveRadioState = function (element) {




        if (element.checked) {



            var answer_index = parseInt($(element).val());

            for (var i = 0; i < bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answers.length; i++) {

                bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answers[i].selected = false;
            }

            bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answers[answer_index].selected = true;

            bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answered = true;



        }

    }

    var saveCheckBoxState = function (element) {




        var answer_index = parseInt($(element).val());


        bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answers[answer_index].selected = element.checked;

        for (var x = 0; x < bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answers.length; x++) {

            if (bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answers[x].selected == true) {
                bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answered = true;
                break;
            }
            bundle.assessments[bundle.current_assessment_index].questions[bundle.assessments[bundle.current_assessment_index].current_question_index].answered = false;
        }

    }


    return {
        moveNext: moveNext,
        movePrevious: movePrevious,
        doEnd: doEnd,
        init: init,
        saveRadioState: saveRadioState,
        saveCheckBoxState: saveCheckBoxState,
        moveTo: moveTo

    };

})();



var essayPage = (function () {


    var interval;
    var seconds = 0;
    var minutes = 0;


    var init = function () {

      


        if (bundle.assessments[bundle.current_assessment_index].timed) {

            minutes = bundle.assessments[bundle.current_assessment_index].duration;

            showTime(minutes, 0);

            interval = setInterval(timerTick, 1000);

            $("#divTimerWrapper").show();

        } else {

            $("#divTimerWrapper").hide();
        }

        if (bundle.assessments[bundle.current_assessment_index].started) {

          //  gotoPage(bundle.assessments[bundle.current_assessment_index].current_question_index);


        } else {
            bundle.assessments[bundle.current_assessment_index].started = true;
        }

    

 
    }





    function timerTick() {

        if (seconds == 0) {
            seconds = 60; //60
            minutes--;

            bundle.assessments[bundle.current_assessment_index].time_remaining = minutes;


            //save state

            saveCurrentResponse();

            var tempBundle = JSON.parse(JSON.stringify(bundle));

            tempBundle.bundle_name = '';
            tempBundle.bundle_icon = '';
            tempBundle.candidate_photo = '';


            for (var j = 0; j < tempBundle.assessments.length; j++) {

                tempBundle.assessments[j].assessment_name = '';
                tempBundle.assessments[j].instruction_image = '';

                if (tempBundle.assessments[j].assessment_type === 'MCQ') {

                  //shouldn't get here.

                } else {

                    for (var i = 0; i < tempBundle.assessments[j].essays.length; i++) {

                        tempBundle.assessments[j].essays[i].topic = '';

                    }
                }

            }


            var timeRemaining = bundle.assessments[bundle.current_assessment_index].time_remaining;
            var assessmentName = bundle.assessments[bundle.current_assessment_index].assessment_name;

            var saveData = { bundle: tempBundle, assessmentName: assessmentName, timeRemaining: timeRemaining };

            var saver_url = $("#saver").data("request-url-save-test");

            $.ajax({
                url: saver_url,
                data: JSON.stringify(saveData),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                timeout: 5000
            })
                .done(function (result) {

                })
                .fail(function () {
                    
                });

        }

        showTime(minutes, --seconds);

        if (minutes <= 0 && seconds <= 0) {


            progressAssessment();

        }

    }


    function progressAssessment() {

        if (bundle.assessments[bundle.current_assessment_index].timed) {

            clearInterval(interval);
        }


        saveCurrentResponse();


        if (bundle.current_assessment_index == (bundle.assessments.length - 1)) {

            //end assessment session

            $("body").off('keyup');

            var html = assessmentEndTemplate(bundle);

            $("#container").html(html);

            assessmentEndPage.submitAssessment();


        } else {

            //progress assessment 
            bundle.current_assessment_index++;
          
            minutes = 0;
            seconds = 0;

            firstPage.moveNext();

        }


    }


    var saveCurrentResponse = function() {
        
        var editor = $("#editor").data("kendoEditor");

        var response = editor.value();
        var currentIndex = bundle.current_assessment_index;

        bundle.assessments[currentIndex].essays[bundle.assessments[currentIndex].current_question_index].candidate_response = response;

    }


    var doEnd = function () {

        if (confirm("Are you sure you want to end this assessment ?")) {

            progressAssessment();
        }
    }




    function showTime(min, sec) {

        var totalMins = min;
        var hrs = 0;
        var mins = totalMins;

        if (totalMins >= 60) {
            hrs = parseInt(totalMins / 60);
            mins = totalMins % 60;
        }
        var hrStr = hrs + "";
        var minStr = mins + "";

        if (sec == 60) {

            sec = 0;
        }
        var secStr = sec + "";

        if (secStr.length == 1) secStr = "0" + secStr;

        if (hrStr.length == 1) {
            hrStr = "0" + hrStr;
        }
        if (minStr.length == 1) {
            minStr = "0" + minStr;
        }

        $("#divTimer").text(hrStr + ":" + minStr + ":" + secStr);

    }







    return {

        doEnd: doEnd,
        init: init

    };

})();

var assessmentEndPage = (function () {

    var submitAssessment = function () {

        $("#divProgress").show();
        $("#bttnResend").hide();


        var temp = [];

        
         

        for (var ctr = 0; ctr < bundle.assessments.length; ctr++) {
            
            if (bundle.assessments[ctr].assessment_type === 'MCQ') {

                temp.push({ assessment_id: bundle.assessments[ctr].assessment_id, result: getResult(bundle.assessments[ctr]) });


            } else {

                temp.push({ assessment_id: bundle.assessments[ctr].assessment_id, is_essay: true, essay_id: getEssayId(bundle.assessments[ctr]), result: getEssayResponse(bundle.assessments[ctr]) });
            }

        }



        var data = { responses: temp };

        var submitter_url = $("#submitter").data("request-url-submit-test");

        $.ajax({
            url: submitter_url,
            data: JSON.stringify(data),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json'
        })
        .done(function (result) {

            if (result.Succeeded == true) {

                $("#divStatus").html('<div class="alert alert-success alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>Test submitted successfully.</div>');

                $("#divProgress").hide();
                if (bundle.show_results_on_submit === true && result.resultList.length > 0) {

                    $("#divResult").show();

                    var morePractice = '';

                    if (bundle.external_link && bundle.external_link.length > 0) {
                        morePractice = "<br/><br/><div><p>Please visit this <a href='" + bundle.external_link + "' target='_blank'>link</a> as part of your assessment</p></div>";

                    } else {
                        morePractice = "<br/><br/><div><p>For more practice test questions go to <a href='http://mytestpractice.com' target='_blank'>http://mytestpractice.com</a></p></div>";
                    }


                    $("#divResult").html(getScoreTable(result.resultList) + morePractice);

                } else {
                    var morePractice = '';

                    if (bundle.external_link && bundle.external_link.length > 0) {
                        morePractice = "<br/><br/><div><p>Please visit this <a href='" + bundle.external_link + "' target='_blank'>link</a> as part of your assessment</p></div>";

                        var htmlStr = $("#divStatus").html();

                        $("#divStatus").html(htmlStr + morePractice);
                    } 
                   
                }

                if (bundle.min_aggregate_score != null) {

                    var totalScore = getTotalScore(result.resultList);

                    $("#lblPassFail").show();
                    $("#lblPassFail").html(totalScore >= bundle.min_aggregate_score ? "<strong style='color: green; font-size: 18px;'>Congratulations. Your attempt is successful, you have passed the test.</strong>" : "<strong style='color: red; font-size: 18px;'>Sorry, your attempt has not been successful.</strong>");

                }
                
                if (ProctorClient3) {
                    ProctorClient3.stop(function () {
                        console.log("completed the proctoring session");
                    });
                }
               
            } else {

                $("#divProgress").hide();
                $("#bttnResend").show();
                $("#divStatus").html('<div class="alert alert-danger alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><strong>Error!</strong> ' + result.ErrorMessage + '. Please click on "Submit Test" below.</div>');

            }

           

        })
        .fail(function () {

            $("#divProgress").hide();
            $("#bttnResend").show();
            $("#divStatus").html('<div class="alert alert-danger alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><strong>Error!</strong> Could not submit test. Please click on "Submit Test" below.</div>');

        });

    }


    function getTotalScore(results) {
        
        var total = 0;

        for (var i = 0; i < results.length; i++) {

            total += results[i].Score;
        }

        return total;
    }

    function getScoreTable(results) {

        var prefix = '<div><table class="table table-bordered table-hover table-striped"><tbody><tr><th>Assessment</th><th style="width: 140px;">Score</th></tr>';

        var total = 0;

        for (var i = 0; i < results.length; i++) {

            var temp = "<tr><td>" + results[i].AssessmentName + "</td><td>" + results[i].Score + "</td> </tr>";

            prefix += temp;

            total += results[i].Score;
        }

        prefix += "<tr><th>Aggregate</th><th>" + total + "</th> </tr>";

        prefix += "</tbody></table></div>";


        return prefix;

    }


    function getEssayResponse(ass) {

        for (var i = 0; i < ass.essays.length; i++) {

            if (ass.essays[i].selected) {

                return ass.essays[i].candidate_response;
            }
        }
    }

    function getEssayId(ass) {

        for (var i = 0; i < ass.essays.length; i++) {

            if (ass.essays[i].selected) {

                return ass.essays[i].essay_id;
            }
        }

    }

    function getResult(ass) {

        var ans_options = '';

        for (var i = 0; i < ass.questions.length; i++) {

            var q_id = ass.questions[i].question_id + '';

            var ret = '';

            for (var x = 0; x < ass.questions[i].answers.length; x++) {
                if (ass.questions[i].answers[x].selected) {
                    if (ret.length > 0) ret += ",";
                    ret += ass.questions[i].answers[x].answer_id + '';
                }


            }
            if (ret.length == 0) ret = "0";

            if (ans_options.length > 0) ans_options += ";";
            ans_options += q_id + ":" + ret;

        }


        return ans_options;
    }

    function DoProgress() {

        bundle.current_assessment_index = bundle.assessments.length - 1;

        //end assessment session

        $("body").off('keyup');

        var html = assessmentEndTemplate(bundle);

        $("#container").html(html);

        assessmentEndPage.submitAssessment();
    }

    return {

        submitAssessment: submitAssessment,
        getEssayResponse: getEssayResponse,
        DoProgress: DoProgress
    };


})();
