document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("status").value; // Define "Todos" como o valor padrão
    applyFilters(getPageNumberFromUrl(window.location.href));
});
function getPageNumberFromUrl(url) {
    var pageNumber = 1; // Define o número da página como 1 por padrão
    var urlParams = new URLSearchParams(new URL(url).search);
    if (urlParams.has('page')) {
        pageNumber = parseInt(urlParams.get('page'));
    }
    return pageNumber;
}

function applyFilters(page) {
    var nameFilter = document.getElementById("name").value;
    var statusFilter = document.getElementById("status").value;
    var genderFilter = document.getElementById("gender").value;
    var url = new URL("https://rickandmortyapi.com/api/character/");

    var params = new URLSearchParams(url.search);
    if (page) {
        params.set('page', page);
    } else {
        params.set('page', '1');
    }

    if (nameFilter) {
        params.set('name', nameFilter);
    }

    if (statusFilter && statusFilter !== "Todos" && statusFilter !== "") {
        params.set('status', statusFilter);
    }

    if (genderFilter && genderFilter !== "Todos" && genderFilter !== "") {
        params.set('gender', genderFilter);
    }

    url.search = params.toString();

    fetch(url)
        .then(response => response.json())
        .then(data => {
            updateDisplay(data);
            history.pushState({}, '', '?' + params.toString());
        })
        .catch(error => {
            console.error('Houve um erro ao buscar os personagens:', error);
        });
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
