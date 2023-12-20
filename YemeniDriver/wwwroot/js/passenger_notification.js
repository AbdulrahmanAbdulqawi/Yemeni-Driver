

        // function refreshPage() {
        //     // Reload the current page
        //     location.reload();
        // }
const connection = new signalR.HubConnectionBuilder()
.withUrl("/notificationHub")
.build();

connection.start().then(() => {
console.log("Connection established.");
}).catch(err => {
console.error(err.toString());
});

connection.on("ReceiveRequestNotification", (message, driverId, tripId) => {
    // Handle the incoming request notification
    console.log("Received request notification:", message);

    setTimeout(() => {
        // After the delay, redirect to the rating box
        window.location.href = `/Rating/ShowRatingAndReviewBox?driverId=${driverId}&tripId=${tripId}`;
    }, 10000); // 10000 milliseconds = 10 seconds

    //Update the UI or take any other actions
    Toastify({
        text: message,
        duration: 5000,  // Duration in milliseconds
        close: true,
        gravity: "bottom",  // Toast position
    }).showToast();
});

