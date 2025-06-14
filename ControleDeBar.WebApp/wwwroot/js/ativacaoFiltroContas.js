﻿document.addEventListener("DOMContentLoaded", function () {
    const params = new URLSearchParams(window.location.search);
    const status = params.get("status") ?? "";

    const botoesFiltro = document.querySelectorAll('.btn-filtro');

    for (const btn of botoesFiltro) {
        const href = btn.getAttribute('href');

        if (!href)
            continue;

        const url = new URL(href, window.location.origin);
        const btnStatus = url.searchParams.get("status") ?? "";

        const btnStatusAtivo = status === btnStatus;

        btn.classList.toggle('btn-primary', btnStatusAtivo);
        btn.classList.toggle('btn-outline-primary', !btnStatusAtivo);
    }
});