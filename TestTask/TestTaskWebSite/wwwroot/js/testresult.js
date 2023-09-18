var TT = TT || {}

//***********************************************************//
//
// test result page
//
//***********************************************************//
TT.Results = {

    // holds the compilation Id 
    CompilationId: null,

    // Initialize and load results data
    Load: function () {
        TT.Results.GetCompilationId();
        TT.Results.LoadTestResults();
    },

    // Get compilation Id from query param and set property
    GetCompilationId: function () {
        TT.Results.CompilationId = TT.Utils.GetUrlParam("id");
    },

    // Request the result of the compiled test
    LoadTestResults: function (number) {

        if (!TT.Results.CompilationId) return;

        TT.Utils.ShowLoading();
        let request = "testresults/" + TT.Results.CompilationId;
        TT.ApiService.Get(request, TT.Results.GetTestResultsSuccess);
    },

    // Set the test results in page controls
    GetTestResultsSuccess: function (response) {
        if (response) {
            let title = "Thank you, " + response.person_name;
            $("#responseTitle").html(title);

            let resultMessage = "You have answered correctly on " + response.correct_answers + " out of " + response.number_questions + " questions"
            $("#responseResults").text(resultMessage);
        }

        TT.Utils.HideLoading();
    },
}

$(document).ready(function () {
    // Load the test result
    TT.Results.Load();
});