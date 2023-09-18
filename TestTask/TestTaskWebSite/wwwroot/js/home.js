var TT = TT || {}

//***********************************************************//
//
// start page
//
//***********************************************************//
TT.Start = {
    // Initialize and load default data
    Load: function () {
        TT.Start.LoadAvailableTests();
    },

    // Request all the availables test 
    LoadAvailableTests: function () {
        TT.Utils.ShowLoading();

        TT.ApiService.Get("availabletests", TT.Start.GetAvailableTestsSuccess);
    },

    // Fill the test's dropdown with the returned tests
    GetAvailableTestsSuccess: function (response) {
        if (response) {
            $.each(response, function (i, test) {
                $("#testSelect").append($("<option>", {
                    value: test.id,
                    text: test.name
                }));
            });
        }

        TT.Utils.HideLoading();
    },

    // Request the creation of a new Test compilation for the specified Person name
    CreateNewTestCompilationAndStart: function () {
        let personName = $("#personName").val();
        let testId = $("#testSelect").val();
        if (personName && testId) {
            let request = { test_id: testId, person_name: personName };
            TT.ApiService.Post("newtestcompilation", request, TT.Start.CreateNewTestCompilationSuccess);
        }
    },

    // Redirect to Questions Page with the returned compilation id
    CreateNewTestCompilationSuccess: function (response) {
        if (response?.id) {
            let url = "testquestions.html?id=" + response.id;
            location.replace(url);
        }

        TT.Utils.HideLoading();
    },

    // Validate the form data and trigger the Start process if all ok
    ValidateDataAndSubmit: function (event) {
        event.preventDefault();
        if (!event.target.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
            $(this).addClass("was-validated");
        }
        else {
            TT.Start.CreateNewTestCompilationAndStart();
        }
    }
}

$(document).ready(function () {
    $("#startForm").on("submit", TT.Start.ValidateDataAndSubmit);

    // Load the test dropdown with the available tests
    TT.Start.Load();
});