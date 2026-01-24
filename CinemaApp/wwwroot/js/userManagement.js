// userManagement.js

function setRoleValue(userId, inputPrefix) {
    // Find the dropdown and the hidden input
    const dropdown = document.getElementById(`roleDropdown-${userId}`);
    const hiddenInput = document.getElementById(`${inputPrefix}-${userId}`);

    if (!dropdown || !hiddenInput) {
        console.warn("Role dropdown or hidden input not found for user:", userId);
        return false;
    }

    // Set the hidden input value
    hiddenInput.value = dropdown.value;

    // Optional: Validate
    if (!hiddenInput.value) {
        alert("Please select a role before submitting.");
        return false;
    }

    return true;
}

// Confirmation for delete
document.addEventListener("DOMContentLoaded", function () {
    const deleteForms = document.querySelectorAll("form[action*='DeleteUser']");

    deleteForms.forEach(form => {
        form.addEventListener("submit", function (e) {
            const confirmed = confirm("Are you sure you want to delete this user?");
            if (!confirmed) {
                e.preventDefault();
            }
        });
    });
});
