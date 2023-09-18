var TT = TT || {}

//***********************************************************//
//
// test questions page
//
//***********************************************************//
TT.Questions = {

    // holds the compilation Id 
    CompilationId: null,
    // holds the current Test been used
    Test: null,
    // holds the current question been answered
    CurrentQuestion: null,

    // Initialize and load first question
    Load: function () {
        TT.Questions.GetCompilationId();
        TT.Questions.LoadTestDetails();
    },

    // Get compilation Id from query param and set property
    GetCompilationId: function () {
        TT.Questions.CompilationId = TT.Utils.GetUrlParam("id");
    },

    // Set the details controls with test data
    SetTestDetails: function (test) {
        if (!test) return;

        let title = test.name + " (" + test.number_questions + " questions)";
        $("#testName").html(title);
    },

    // Request the details of the test to compile
    LoadTestDetails: function (number) {

        if (!TT.Questions.CompilationId) return;

        TT.Utils.ShowLoading();
        let request = "testbycompilation?id=" + TT.Questions.CompilationId;
        TT.ApiService.Get(request, TT.Questions.GetTestByCompilationSuccess);
    },

    // Set the test's details in page and load first question
    GetTestByCompilationSuccess: function (response) {
        if (response) {
            TT.Questions.Test = response;
            TT.Questions.SetTestDetails(response);
            // Load the first question
            TT.Questions.LoadQuestion(1);
        }
        else {
            TT.Utils.HideLoading();
        }
    },

    // Request the specified question
    LoadQuestion: function (number) {
        TT.Utils.ShowLoading();

        let request = "test/" + TT.Questions.Test.id + "/question/" + number;
        TT.ApiService.Get(request, TT.Questions.GetQuestionSuccess);
    },

    // set as current question the requested question and set the controls data 
    GetQuestionSuccess: function (response) {
        if (response) {
            TT.Questions.CurrentQuestion = response;
            TT.Questions.SetQuestionDetails(response);
            TT.Questions.SetTestProgress(response);
        }

        TT.Utils.HideLoading();
    },

    // Set the question controls with current question data
    SetQuestionDetails: function (question) {
        if (!question) return;

        let title = "Question " + question.number;
        $("#questionTitle").html(title);
        $("#questionName").text(question.name);

        TT.Questions.SetQuestionAnswers(question.answers);
    },

    // Set the test progress control
    SetTestProgress: function (question) {
        let progress = 0;
        if (TT.Questions.CurrentQuestion && TT.Questions.Test && TT.Questions.Test.number_questions > 0) {
            // here we use prev question number for progress calculation. 
            // the progress shows the percent of answered question; this means the last question will not show a 100% completion in the bar. 
            progress = Math.round((TT.Questions.CurrentQuestion.number - 1) * 100 / TT.Questions.Test.number_questions);
        }

        $("#testProgress").prop("aria-valuenow", progress);
        $("#testProgressBar").css("width", progress + "%");
        $("#testProgressBar").html(progress + "%");
    },

    // Set the question's answer controls
    SetQuestionAnswers: function (answers) {
        if (!answers) return;

        $("#answerContainer").empty();

        $.each(answers, function (i, answer) {
            $("#answerContainer").append(TT.Questions.BuildQuestionAnswer(answer));
        });
    },

    // create the html string that holds a question's answer controls
    BuildQuestionAnswer: function (answer) {
        if (!answer) return;

        let answerCtrl =
            "<div class='col-4 my-3'>" +
            "<input type='radio' class='btn-check' name = 'question-answers' id='answer_" + answer.number + "' value='" + answer.number + "' autocomplete='off'>" +
            "<label class='btn' for='answer_" + answer.number + "'>" + answer.name + "</label>" +
            "</div>";

        return answerCtrl;
    },

    // Save current question anwser and move to next question
    LoadNextQuestion: function () {
        if (!TT.Questions.CurrentQuestion || !TT.Questions.Test) return;

        let selectedAnswer = $("input[name='question-answers']:checked").val();
        if (!selectedAnswer) {
            TT.Utils.ShowError("An answer must be selected before moving to the next question.");
            return;
        }
        
        TT.Questions.SaveQuestionAnswer(TT.Questions.CompilationId, TT.Questions.CurrentQuestion.number, selectedAnswer);        
    },

    // Request the save of the answer selection for the current question
    SaveQuestionAnswer: function (testCompilationId, questionNumber, answerNumber) {
        if (testCompilationId && questionNumber && answerNumber) {
            TT.Utils.ShowLoading();
            debugger;

            let request = { test_compilation_id: testCompilationId, question_number: questionNumber, answer_number: answerNumber };
            TT.ApiService.Post("saveselectedanswer", request, TT.Questions.SaveQuestionAnswerSuccess);
        }
    },

    // load next question or go to Result page
    SaveQuestionAnswerSuccess: function (response) {
        TT.Utils.HideLoading();

        if (response) {
            if (TT.Questions.CurrentQuestion.number == TT.Questions.Test.number_questions) {
                let url = "testresult.html?id=" + TT.Questions.CompilationId;
                location.replace(url);
            }
            else {
                TT.Questions.LoadQuestion(TT.Questions.CurrentQuestion.number + 1);
            }
        }
    },
}

$(document).ready(function () {
    $("#nextQuestion").on("click", TT.Questions.LoadNextQuestion);

    // Load general test data and first question
    TT.Questions.Load();
});