document.addEventListener("DOMContentLoaded", function () {
    const mensaje = document.getElementById("Message").value;
    if (mensaje) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: `${mensaje}`
        });
    }
});
