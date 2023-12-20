function refreshPage() {
    // Reload the current page
    location.reload();
}
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.start().then(() => {
    console.log("Connection established.");
}).catch(err => {
    console.error(err.toString());
});

connection.on("ReceiveRequestNotification", (message) => {
    // Handle the incoming request notification
    console.log("Received request notification:", message);

    //Update the UI or take any other actions
    // Toastify({
    //     text: message,
    //     duration: 5000,  // Duration in milliseconds
    //     close: true,
    //     gravity: "top",  // Toast position
    // }).showToast();

    refreshPage();
});