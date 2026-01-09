document.addEventListener("DOMContentLoaded", () => {

    const detailsModalElement = document.getElementById("movieDetailsModal");
    const detailsModal = new bootstrap.Modal(detailsModalElement);

    const detailsTitle = document.getElementById("movieDetailsLabel");
    const detailsContent = document.getElementById("movieDetailsContent");
    const addToWatchlistBtn = document.getElementById("add-to-watchlist-btn");

    let currentMovieId = null;

 // view details
    document.querySelectorAll(".view-details-btn").forEach(btn => {
        btn.addEventListener("click", async () => {
            const movieId = btn.dataset.movieId;
            currentMovieId = movieId;

            try {
                const response = await fetch(`/Movie/Details/${movieId}`);
                if (!response.ok) throw new Error("Failed to load movie details");

                const data = await response.json();

                detailsTitle.textContent = data.title;

                detailsContent.innerHTML = `
                    <div class="row">
                        <div class="col-md-4 text-center">
                            <img src="${data.imageUrl}"
                                 class="img-fluid rounded shadow-sm mb-3"
                                 alt="${data.title}">
                        </div>
                        <div class="col-md-8">
                            <p><strong>Director:</strong> ${data.director}</p>
                            <p><strong>Genre:</strong> ${data.genre}</p>
                            <p><strong>Duration:</strong> ${data.duration} min</p>
                            <p><strong>Release Year:</strong> ${data.releaseYear}</p>
                            <hr />
                            <p>${data.description}</p>
                        </div>
                    </div>
                `;

                detailsModal.show();
            }
            catch (err) {
                console.error(err);
                alert("Could not load movie details.");
            }
        });
    });

  // add to watchlist
    if (addToWatchlistBtn) {
        addToWatchlistBtn.addEventListener("click", async () => {

            if (!currentMovieId) return;

            try {
                const response = await fetch(`/Watchlist/Add/${currentMovieId}`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "RequestVerificationToken":
                            document.querySelector('input[name="__RequestVerificationToken"]')?.value
                    }
                });

                if (!response.ok) throw new Error();

                addToWatchlistBtn.textContent = "Added ?";
                addToWatchlistBtn.classList.remove("btn-warning");
                addToWatchlistBtn.classList.add("btn-success");
                addToWatchlistBtn.disabled = true;
            }
            catch {
                alert("Failed to add movie to watchlist.");
            }
        });
    }

    // buy tickets
    document.querySelectorAll(".buy-ticket-btn").forEach(btn => {
        btn.addEventListener("click", () => {

            const cinemaId = btn.dataset.cinemaId;
            const cinemaName = btn.dataset.cinemaName;
            const movieId = btn.dataset.movieId;
            const movieName = btn.dataset.movieName;

            document.getElementById("ticketCinemaName").textContent = cinemaName;
            document.getElementById("ticketMovieName").textContent = movieName;

            document.getElementById("CinemaIdInput").value = cinemaId;
            document.getElementById("MovieIdInput").value = movieId;

            const ticketModal = new bootstrap.Modal(
                document.getElementById("buyTicketModal")
            );

            ticketModal.show();
        });
    });

});
