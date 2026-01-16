document.addEventListener("DOMContentLoaded", () => {

    // open buy ticket modal
    document.querySelectorAll(".buy-ticket-btn").forEach(btn => {
        btn.addEventListener("click", () => {

            const cinemaId = btn.dataset.cinemaId;
            const cinemaName = btn.dataset.cinemaName;
            const movieId = btn.dataset.movieId;
            const showtimes = btn.dataset.showtimes ? JSON.parse(btn.dataset.showtimes) : [];

            document.getElementById("cinemaNamePlaceholder").textContent = cinemaName;
            document.getElementById("cinemaId").value = cinemaId;
            document.getElementById("movieId").value = movieId;

            // fill showtime options
            const showtimeSelect = document.getElementById("showtimeSelect");
            showtimeSelect.innerHTML = ""; // Clear previous options

            if (showtimes.length === 0) {
                const option = document.createElement("option");
                option.value = "";
                option.textContent = "No showtimes available";
                showtimeSelect.appendChild(option);
                showtimeSelect.disabled = true;
            } else {
                showtimeSelect.disabled = false;
                showtimes.forEach(st => {
                    const option = document.createElement("option");
                    option.value = st;
                    option.textContent = new Date(st).toLocaleString();
                    showtimeSelect.appendChild(option);
                });
            }

            document.getElementById("quantity").value = 1;
            document.getElementById("errorMessage").classList.add("d-none");

            const ticketModal = new bootstrap.Modal(document.getElementById("buyTicketModal"));
            ticketModal.show();
        });
    });

    // Confirm purchase
    const buyButton = document.getElementById("buyTicketButton");
    buyButton.addEventListener("click", async () => {

        const cinemaId = document.getElementById("cinemaId").value;
        const movieId = document.getElementById("movieId").value;
        const quantity = parseInt(document.getElementById("quantity").value);
        const showtime = document.getElementById("showtimeSelect").value;
        const errorMessage = document.getElementById("errorMessage");

        errorMessage.classList.add("d-none");

        if (!cinemaId || !movieId || !showtime || isNaN(quantity) || quantity < 1) {
            errorMessage.textContent = "Please select a showtime and valid ticket quantity.";
            errorMessage.classList.remove("d-none");
            return;
        }

        try {
            const response = await fetch("/api/tickets/buy", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    cinemaId: cinemaId,
                    movieId: movieId,
                    showtime: showtime,
                    quantity: quantity
                })
            });

            if (!response.ok) {
                const errorData = await response.json();
                errorMessage.textContent = errorData.message || "Failed to purchase tickets.";
                errorMessage.classList.remove("d-none");
                return;
            }

            // Successful purchase
            const ticketModalEl = document.getElementById("buyTicketModal");
            const ticketModal = bootstrap.Modal.getInstance(ticketModalEl);
            ticketModal.hide();

            alert("Tickets purchased successfully!");
        }
        catch (err) {
            console.error(err);
            errorMessage.textContent = "An error occurred. Please try again.";
            errorMessage.classList.remove("d-none");
        }
    });

});
