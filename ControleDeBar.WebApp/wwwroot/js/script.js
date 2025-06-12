const hamburguer = document.querySelector('.toggle-btn');
const toggler = document.querySelector('.toggle-btn-icon');
var x = window.matchMedia("(max-width: 992px)")

function sidebarToggler() {
    document.querySelector('.sidebar').classList.toggle('expand');
    toggler.classList.toggle('bi-chevron-double-left');
    toggler.classList.toggle('bi-chevron-double-right');
}

hamburguer.addEventListener('click', sidebarToggler);

sidebarToggler(x);

x.addEventListener("change", function () {
    sidebarToggler(x);
});