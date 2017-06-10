// Write your Javascript code.
function refreshJobs() {

    $.get("api/pi/list", function (data) {

        var jobsDiv = $('#jobs');
        jobsDiv.hide();
        jobsDiv.empty();
        data.forEach(function(job) {

            // Calculate dates
            var duration = null;
            var startDate = new Date(job.startTime);
            if (job.endTime)
            {
                duration = new Date(job.endTime) - startDate;
            };

            // Get status colors
            var statusClass = "";
            if(job.status === "Complete")
            {
                statusClass = "label label-success";
            } 
            else if (job.status === "Processing")
            {
                statusClass = "label label-warning";
            }

            // Render row
            var rowHtml = '<tr>';
            
            rowHtml += '<td>' + job.id + '</td>';
            rowHtml += '<td>' + job.digits + '</td>';
            rowHtml += '<td>' + job.iterations + '</td>';
            rowHtml += '<td>' + job.result + '</td>';
            rowHtml += '<td><span class="' + statusClass + '">' + job.status + '</span></td>';
            rowHtml += '<td>' + startDate.toDateString() + '</td>';
            rowHtml += '<td>' + startDate.toLocaleTimeString() + '</td>';
            rowHtml += '<td class="text-right">' + duration + '</td>';
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