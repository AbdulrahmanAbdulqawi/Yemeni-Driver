const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

hubConnection.start().then(() => {
    console.log("Passenger Connected to NotificationHub");
}).catch(err => console.error(err.toString()));

// Handle the new ride request event
hubConnection.on("ReceiveNotification", (newRequest) => {
    // Handle the notification, e.g., show a pop-up or update the UI
    alert(`New ride request: ${newRequest}`);
});