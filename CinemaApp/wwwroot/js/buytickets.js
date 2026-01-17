document.addEventListener("DOMContentLoaded", () => {

    const modalEl = document.getElementById("buyTicketModal");
    const showtimeSelect = document.getElementById("showtimeSelect");
    const quantityInput = document.getElementById("quantity");
    const availableTicketsSpan = document.getElementById("availableTickets");
    const errorMessage = document.getElementById("errorMessage");

    // OPEN MODAL
    document.querySelectorAll(".buy-ticket-btn").forEach(btn => {
        btn.addEventListener("click", async () => {

            const cinemaId = btn.dataset.cinemaId;
            const cinemaName = btn.dataset.cinemaName;
            const movieId = btn.dataset.movieId;

            document.getElementById("cinemaId").value = cinemaId;
            document.getElementById("movieId").value = movieId;
            document.getElementById("cinemaNamePlaceholder").textContent = cinemaName;

            errorMessage.classList.add("d-none");
            quantityInput.value = 1;
            quantityInput.disabled = true;
            availableTicketsSpan.textContent = "–";

            await loadShowtimes(cinemaId, movieId);

            new bootstrap.Modal(modalEl).show();
        });
    });

    // LOAD SHOWTIMES
    async function loadShowtimes(cinemaId, movieId) {
        showtimeSelect.innerHTML = `<option value="">Loading...</option>`;
        showtimeSelect.disabled = true;

        try {
            const res = await fetch(`/api/CinemaMovieApi/showtimes?cinemaId=${cinemaId}&movieId=${movieId}`);

            if (!res.ok) {
                showtimeSelect.innerHTML = `<option>No showtimes available</option>`;
                return;
            }

            const showtimes = await res.json();
            showtimeSelect.innerHTML = `<option value="">Select showtime</option>`;

            showtimes.forEach(st => {
                const opt = document.createElement("option");
                opt.value = st;
                opt.textContent = new Date(st).toLocaleString();
                showtimeSelect.appendChild(opt);
            });

            showtimeSelect.disabled = false;
        }
        catch {
            showtimeSelect.innerHTML = `<option>Error loading showtimes</option>`;
        }
    }

    // ON SHOWTIME CHANGE ? LOAD AVAILABLE TICKETS
    showtimeSelect.addEventListener("change", async () => {

        const cinemaId = document.getElementById("cinemaId").value;
        const movieId = document.getElementById("movieId").value;
        const showtime = showtimeSelect.value;

        if (!showtime) return;

        quantityInput.disabled = true;
        availableTicketsSpan.textContent = "…";

        try {
            const res = await fetch(
                `/api/CinemaMovieApi/AvailableTickets?cinemaId=${cinemaId}&movieId=${movieId}&showtime=${encodeURIComponent(showtime)}`
            );

            if (!res.ok) {
                availableTicketsSpan.textContent = "0";
                return;
            }

            const available = await res.json();
            availableTicketsSpan.textContent = available;
            quantityInput.max = available;
            quantityInput.disabled = available <= 0;
        }
        catch {
            availableTicketsSpan.textContent = "–";
        }
    });

    // CONFIRM PURCHASE
    document.getElementById("buyTicketButton").addEventListener("click", async () => {

        const cinemaId = document.getElementById("cinemaId").value;
        const movieId = document.getElementById("movieId").value;
        const showtime = showtimeSelect.value;
        const quantity = Number(quantityInput.value);
        const available = Number(quantityInput.max);

        errorMessage.classList.add("d-none");

        if (!showtime || quantity < 1 || quantity > available) {
            errorMessage.textContent = "Invalid ticket quantity.";
            errorMessage.classList.remove("d-none");
            return;
        }

        // TODO: POST /api/tickets/buy
        alert("Tickets purchased successfully!");
        bootstrap.Modal.getInstance(modalEl).hide();
    });

});
