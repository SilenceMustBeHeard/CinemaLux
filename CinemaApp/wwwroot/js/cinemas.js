document.addEventListener("DOMContentLoaded", () => {
    const searchInput = document.getElementById("cinemaSearch");
    const citySelect = document.getElementById("cityFilter");
    const cinemaCards = document.querySelectorAll(".cinema-card");

    function filterCinemas() {
        const searchValue = searchInput?.value.toLowerCase() || "";
        const selectedCity = citySelect?.value.toLowerCase() || "";

        cinemaCards.forEach(card => {
            const name = card.querySelector(".card-title").textContent.toLowerCase();
            const city = card.dataset.city;

            const matchesSearch = name.includes(searchValue);
            const matchesCity = !selectedCity || city === selectedCity;

            if (matchesSearch && matchesCity) {
                card.classList.remove("d-none");
            } else {
                card.classList.add("d-none");
            }
        });
    }

    if (searchInput) {
        searchInput.addEventListener("input", filterCinemas);
    }

    if (citySelect) {
        citySelect.addEventListener("change", filterCinemas);
    }
});
