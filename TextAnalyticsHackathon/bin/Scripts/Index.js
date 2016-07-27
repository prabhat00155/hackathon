var intervalId;

function ProgressBarIntervalHandler() {
    var progressBarStr = document.getElementById('ProgressBar').innerHTML;
    var substrings = progressBarStr.split('.');
    // Limit maximum dots in progress bar to 5.
    if (substrings.length === 6) {
        progressBarStr = '. ';
    } else {
        progressBarStr += '. ';
    }
    document.getElementById('ProgressBar').innerHTML = progressBarStr;
}

function UpdateOnBegin() {
    var results = $('#AnalysisResults');
    results.html('');
    results.addClass('hidden');
    $('#WaitingForResults').removeClass('hidden');
    $('#Analyze').prop('disabled', true);
    intervalId = setInterval('ProgressBarIntervalHandler()', 300);
}
function UpdateOnSuccess() {
    clearInterval(intervalId);
    $('#Analyze').prop('disabled', false);
    $('#AnalysisResults').removeClass('hidden');
    $('#WaitingForResults').addClass('hidden');
}
function UpdateOnError(ajaxContext) {
    clearInterval(intervalId);
    var results = $('#AnalysisResults');
    results.html('Unfortunately your request errored out [HTTP status code ' + ajaxContext.status +
        ']. Please retry your request');
    results.removeClass('hidden');
    $('#WaitingForResults').addClass('hidden');
    $('#Analyze').prop('disabled', false);
}

function FillSampleText(sampleText) {
    $("#inputHelpBlock").val(sampleText);
}