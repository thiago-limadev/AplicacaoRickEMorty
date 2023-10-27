document.addEventListener("DOMContentLoaded", function () {
    if (document.getElementById("status")) {
        document.getElementById("status").addEventListener("change", function () {
            applyFilters(getPageNumberFromUrl(window.location.href), '', this.value, document.getElementById("gender").value);
        });
    }

    if (document.getElementById("gender")) {
        document.getElementById("gender").addEventListener("change", function () {
            applyFilters(getPageNumberFromUrl(window.location.href), '', document.getElementById("status").value, this.value);
        });
    }

    if (document.getElementById("previous-page")) {
        document.getElementById("previous-page").addEventListener("click", function () {
            var currentPage = getPageNumberFromUrl(window.location.href);
            if (currentPage > 1) {
                var statusFilter = document.getElementById("status").value;
                var genderFilter = document.getElementById("gender").value;
                applyFilters(currentPage - 1, '', statusFilter, genderFilter);
            }
        });
    }

    if (document.getElementById("next-page")) {
        document.getElementById("next-page").addEventListener("click", function () {
            var currentPage = getPageNumberFromUrl(window.location.href);
            var statusFilter = document.getElementById("status").value;
            var genderFilter = document.getElementById("gender").value;
            applyFilters(currentPage + 1, '', statusFilter, genderFilter);
        });
    }

    document.getElementById("status").value; // Define "Todos" como o valor padrão
    applyFilters(getPageNumberFromUrl(window.location.href), '', 'Todos', 'Todos');
});

function applyFilters(page, nameFilter, statusFilter, genderFilter) {
    console.log('entrou aqui');
    var url = "https://rickandmortyapi.com/api/character/?";

    if (page) {
        url += `page=${page}`;
    } else {
        url += "page=1";
    }

    if (nameFilter) {
        url += `&name=${nameFilter}`;
    }

    if (statusFilter && statusFilter !== "Todos" && statusFilter !== "") {
        url += `&status=${statusFilter}`;
    }

    if (genderFilter && genderFilter !== "Todos" && genderFilter !== "") {
        url += `&gender=${genderFilter}`;
    }

    fetch(url)
        .then(response => response.json())
        .then(data => {
            updateDisplay(data);
        })
        .catch(error => {
            console.error('Houve um erro ao buscar os personagens:', error);
        });
}

function getPageNumberFromUrl(url) {
    if (!url) return 1;

    const urlSearchParams = new URLSearchParams(new URL(url).search);
    return parseInt(urlSearchParams.get("page")) || 1;
}

function updateDisplay(data) {
    var container = document.querySelector(".row"); // Seletor para o contêiner dos personagens

    // Limpar a exibição atual
    container.innerHTML = "";

    // Atualizar a exibição com os novos dados
    if (data.results) {
        data.results.forEach(function (character) {
            var card = document.createElement("div");
            card.className = "col-md-3 mt-3";
            card.innerHTML = `
                <div class="card">
                    <img src="${character.image}" class="card-img-top" alt="No image" />
                    <div class="card-body">
                        <p class="card-text"><strong>${character.name}</strong></p>
                        <p class="card-text">Espécie: ${character.species}</p>
                        <p class="card-text">Status: ${character.status}</p>
                        <p class="card-text">Origem: ${character.origin.name}</p>
                        <p class="card-text">Localização: ${character.location.name}</p>
                    </div>
                </div>
            `;
            container.appendChild(card);
        });
    } else {
        console.log("Nenhum personagem encontrado.");
    }
}
