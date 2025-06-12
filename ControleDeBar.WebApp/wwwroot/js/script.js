const hamburguer = document.querySelector('.toggle-btn');
const toggler = document.querySelector('.toggle-btn-icon');

function sidebarToggler() {
    document.querySelector('.sidebar').classList.toggle('expand');
    toggler.classList.toggle('bi-chevron-double-right');
    toggler.classList.toggle('bi-chevron-double-left');
}

hamburguer.addEventListener('click', sidebarToggler);