// Write your Javascript code.
function refreshJobs() {

    $.get("api/pi/list", function (data) {

        var jobsDiv = $('#jobs');
        jobsDiv.hide();
        jobsDiv.empty();
        data.forEach(function(element) {

            var startDate = new Date(element.startTime);
            var endDate = new Date(element.endTime);

            var rowHtml = '<tr>';
            rowHtml += '<td>' + element.id + '</td>';
            rowHtml += '<td>' + element.digits + '</td>';
            rowHtml += '<td>' + element.iterations + '</td>';
            rowHtml += '<td>' + element.result + '</td>';
            rowHtml += '<td>' + element.status + '</td>';
            rowHtml += '<td>' + startDate.toDateString() + '</td>';
            rowHtml += '<td>' + startDate.toLocaleTimeString() + '</td>';
            rowHtml += '<td class="text-right">' + (endDate - startDate) + '</td>';
            rowHtml += '</tr>'

            jobsDiv.append(rowHtml);
        }, this);

        jobsDiv.fadeIn(300);
    });
        
}

// Wire up events
$('#queue-job').click(function () {
    $.post("api/pi/queuejob", { digits: $('#digits').val(), iterations: $('#iterations').val() });
    refreshJobs();
});

$('#refresh').click(function () {
    refreshJobs();
})

// Ready
refreshJobs();