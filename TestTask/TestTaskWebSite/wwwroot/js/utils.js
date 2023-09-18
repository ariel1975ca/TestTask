var TT = TT || {}

//***********************************************************//
//
// General utils
//
//***********************************************************//
TT.Utils = {
    // show the loaging overlay panel
    ShowLoading: function () {
        $(".overlay").show();
    },

    // hide the loaging overlay panel
    HideLoading: function () {
        $(".overlay").hide();
    },

    // show an errors message as an alert dialog
    ShowError: function (message) {
        alert(message);
    },

    // get from query params the value of the specified param name
    GetUrlParam: function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)')
            .exec(window.location.search);

        return (results !== null) ? results[1] || 0 : false;
    }
}

//***********************************************************//
//
// Handle API Calls
//
//***********************************************************//
TT.ApiService = {

    // API Base url
    ApiBaseUrl: "http://localhost:5201/testtask",

    // make a get call to the Web API
    Get: function (operationName, onSuccess, onError) {
        $.ajax({
            type: "GET",
            url: TT.ApiService.ApiBaseUrl + "/" + operationName,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                onSuccess(response);
            },
            error: function (request, message, error) {
                if (onError) {
                    onError(request, message, error);
                }
                else {
                    TT.ApiService.HandleException(request, message, error);
                }
            }
        });
    },

    // make a post call to the Web API
    Post: function (seviceName, operationParamObject, onSuccess, onError) {
        $.ajax({
            type: "POST",
            url: TT.ApiService.ApiBaseUrl + "/" + seviceName,
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(operationParamObject),
            dataType: "json",
            success: function (response) {
                onSuccess(response);
            },
            error: function (request, message, error) {
                if (onError) {
                    onError(request, message, error);
                }
                else {
                    TT.ApiService.HandleException(request, message, error);
                }
            }
        });
    },

    // Handles the API call errors with an alert dialog
    HandleException: function (request, message, error) {
        let msg = "";
        msg += "Code: " + request.status + "\n";
        msg += "Text: " + request.statusText + "\n";
        if (request.responseJSON && request.responseJSON.error) {
            msg += "Message: " + request.responseJSON.error.message + "\n";
        }
        TT.Utils.ShowError(msg);

        TT.Utils.HideLoading();
    }
}