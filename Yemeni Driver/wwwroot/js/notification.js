

function showNotification(message) {
    // Create a Bootstrap modal to display the notification
    var modalHtml = `
        <div class="modal fade" id="notificationModal" tabindex="-1" aria-labelledby="notificationModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="notificationModalLabel">New Ride Request</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        ${message}
                    </div>
                </div>
            </div>
        </div>`;

    // Append the modal HTML to the body
    $('body').append(modalHtml);

    // Show the modal
    $('#notificationModal').modal('show');

    // Remove the modal from the DOM after it's closed
    $('#notificationModal').on('hidden.bs.modal', function () {
        $(this).remove();
    });
}
