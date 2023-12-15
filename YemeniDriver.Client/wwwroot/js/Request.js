$(document).ready(function () {
    // Handler for form submission
    $("#requestRideBtn").click(function () {
        // Show progress bar and cancel button
        $("#requestProgress").show();
        $("#cancelRequestBtn").show();

        // Capture the selected driver and drop-off location
        var selectedDriver = $("#selectedDriver").val();
        var dropoffLocation = $("#dropoffLocation").val();

        // Validate that a driver is selected
        if (!selectedDriver) {
            alert("Please select a driver.");
            // Hide progress bar
            $("#requestProgress").hide();
            return;
        }

        // Create a request using AJAX
        $.ajax({
            url: '/api/request/createRequest', // Replace with your actual API endpoint
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json', // Set the content type to JSON
            data: JSON.stringify({
                DriverId: selectedDriver,
                DropoffLocation: dropoffLocation
            }),
            success: function (data) {
                console.log("Request created successfully");
                // Optionally, you can redirect the user or update the UI as needed
            },
            error: function (error) {
                console.error('Error creating request:', error);
                // Optionally, handle the error and update the UI accordingly
            },
            complete: function () {
                // Hide progress bar
                $("#requestProgress").hide();
            }
        });
    });

    // Handler for cancel button click
    $("#cancelRequestBtn").click(function () {
        // Hide progress bar and cancel button
        $("#requestProgress").hide();
        $("#cancelRequestBtn").hide();

        // Perform logic to cancel the request and remove it from the database
        $.ajax({
            url: '/api/request/cancelRequest', // Replace with your actual API endpoint
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                console.log("Request cancelled successfully");
                // Optionally, you can redirect the user or update the UI as needed
            },
            error: function (error) {
                console.error('Error cancelling request:', error);
                // Optionally, handle the error and update the UI accordingly
            }
        });
    });
});