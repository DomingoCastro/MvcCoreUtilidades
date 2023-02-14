// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const toggleDarkModeButton = document.querySelector("#toggle-dark-mode");
toggleDarkModeButton.addEventListener("click", () => {
    const body = document.querySelector("body");
    body.classList.toggle("dark-mode");

    // Hacer una llamada Ajax para establecer la preferencia de modo oscuro en la cookie
    const xhr = new XMLHttpRequest();
    xhr.open("POST", "/Alumnos/ToggleDarkMode", true);
    xhr.send();
});

// Comprobar si la preferencia de modo oscuro está establecida en la cookie
const isDarkModeEnabled = document.cookie.includes("dark-mode=true");
if (isDarkModeEnabled) {
    const body = document.querySelector("body");
    body.classList.add("dark-mode");
}
