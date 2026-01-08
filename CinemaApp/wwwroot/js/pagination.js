document.addEventListener("DOMContentLoaded", () => {
    const loadMoreBtn = document.getElementById("loadMoreBtn");
    const cinemaCards = document.querySelectorAll(".cinema-card");

    const cardsPerPage = 8;
    let visibleCount = cardsPerPage;

    function updateVisibility() {
        cinemaCards.forEach((card, index) => {
            if (index < visibleCount) {
                card.classList.remove("d-none");
            } else {
                card.classList.add("d-none");
            }
        });

        if (visibleCount >= cinemaCards.length) {
            loadMoreBtn?.classList.add("d-none");
        }
    }

    updateVisibility();

    loadMoreBtn?.addEventListener("click", () => {
        visibleCount += cardsPerPage;
        updateVisibility();
    });
});
