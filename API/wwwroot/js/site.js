// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showLoader() {
    document.getElementById("loader").style.display = "block";
    document.getElementById("main-content").style.display = "none";
    document.body.setAttribute("style", "background-color: #7373749b");
}

// Function to hide the loader
function hideLoader() {
    document.getElementById("loader").style.display = "none";
    document.getElementById("main-content").style.display = "block";
}

// Trigger showLoader when the page is being loaded
window.addEventListener("beforeunload", showLoader);

// Trigger hideLoader when the page has finished loading
window.addEventListener("load", hideLoader);

