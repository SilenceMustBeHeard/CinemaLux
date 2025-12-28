
﻿/* =====================================================
   CINEMA APP – Luxury JS
   Dark + Light Mode • Glass Buttons • Movie Cards
   ===================================================== */

// -------------------------
// Light / Dark Mode Toggle
// -------------------------
const toggleBtn = document.getElementById("themeToggle");
const body = document.body;

function applyTheme(theme) {
    if (theme === "light") {
        body.classList.add("light-mode");
        if (toggleBtn) toggleBtn.textContent = "🌞";
    } else {
        body.classList.remove("light-mode");
        if (toggleBtn) toggleBtn.textContent = "🌙";
    }
}

document.addEventListener("DOMContentLoaded", () => {
    // Load saved theme
    const savedTheme = localStorage.getItem("theme") || "dark";
    applyTheme(savedTheme);

    // Toggle button click
    if (toggleBtn) {
        toggleBtn.addEventListener("click", () => {
            const isLight = body.classList.toggle("light-mode");
            toggleBtn.textContent = isLight ? "🌞" : "🌙";
            localStorage.setItem("theme", isLight ? "light" : "dark");
        });
    }

    // -------------------------
    // Movie Card: Show Info Button
    // -------------------------
    const infoButtons = document.querySelectorAll(".show-info-btn");
    infoButtons.forEach(btn => {
        btn.addEventListener("click", () => {
            const overlay = btn.closest(".movie-card").querySelector(".movie-card-overlay");
            if (overlay) {
                overlay.classList.toggle("show-full-info");
                overlay.style.transition = "opacity 0.4s ease";
            }
        });
    });

    // -------------------------
    // Navbar Toggler Hover Glow
    // -------------------------
    const navbarTogglers = document.querySelectorAll(".navbar-toggler");
    navbarTogglers.forEach(btn => {
        btn.addEventListener("mouseenter", () => {
            btn.style.background = "rgba(212,175,55,0.15)";
            btn.style.boxShadow = "0 6px 20px rgba(212,175,55,0.5)";
        });
        btn.addEventListener("mouseleave", () => {
            btn.style.background = "rgba(255,255,255,0.05)";
            btn.style.boxShadow = "0 4px 10px rgba(0,0,0,0.25)";
        });
    });

    // -------------------------
    // Luxury Hover Effects for All Buttons
    // -------------------------
    const allButtons = document.querySelectorAll(".btn, .glass-btn");
    allButtons.forEach(btn => {
        btn.addEventListener("mouseenter", () => {
            btn.style.background = "rgba(212,175,55,0.15)";
            btn.style.color = "var(--accent-gold)";
            btn.style.boxShadow = "0 6px 20px rgba(212,175,55,0.5)";
            btn.style.transform = "translateY(-2px)";
        });
        btn.addEventListener("mouseleave", () => {
            btn.style.background = "rgba(255,255,255,0.05)";
            btn.style.color = "var(--text-main)";
            btn.style.boxShadow = "0 4px 10px rgba(0,0,0,0.25)";
            btn.style.transform = "translateY(0)";
        });
    });
});

