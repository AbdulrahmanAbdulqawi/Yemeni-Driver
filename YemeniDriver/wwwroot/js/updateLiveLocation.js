
    function updateLiveLocation() {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            fetch('/api/updateLiveLocation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ latitude: latitude, longitude: longitude }),
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    console.log('Live location updated successfully.');
                })
                .catch(error => {
                    console.error('Error updating live location:', error);
                });
        })};
        

    setInterval(updateLiveLocation, 5000);
