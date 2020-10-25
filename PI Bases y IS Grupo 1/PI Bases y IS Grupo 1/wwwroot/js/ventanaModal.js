var ventana = document.getElementById("vtn-modal");

var botonVerModal = document.getElementById("ver");

var botonCerrar = document.getElementsByClassName("close")[0];


botonCerrar.addEventListener('click', cerrarConBoton);
botonVerModal.addEventListener('click', abrir);

function cerrarConBoton() {
    ventana.style.display = "none";
}

function abrir() {
    ventana.style.display = "block";
    window.onclick = function (event) {
        if (event.target == ventana) {
            ventana.style.display = "none";
        }
    }
}