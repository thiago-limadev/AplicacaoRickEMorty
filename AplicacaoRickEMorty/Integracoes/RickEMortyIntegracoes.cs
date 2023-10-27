using System;
using System.Threading.Tasks;
using AplicacaoRickEMorty.Integracoes.Interface;
using AplicacaoRickEMorty.Models;
using AplicacaoRickEMorty.Repositorio.Response;
using AplicacaoRickEMorty.Services;
using Newtonsoft.Json;

namespace AplicacaoRickEMorty.Integracoes
{
    public class RickEMortyIntegracoes : IRickAndMortyIntegracao
    {
        private readonly IRickAndMortyRefit _rickAndMortyApi;

        public RickEMortyIntegracoes(IRickAndMortyRefit rickAndMortyApi)
        {
            _rickAndMortyApi = rickAndMortyApi;
        }

        public async Task<RickAndMortyResponse> ObterPersonagens()
        {
            try
            {
                var characters = await _rickAndMortyApi.GetCharacters();
                var response = new RickAndMortyResponse
                {
                    Info = characters.Info,
                    Results = characters.Results
                };
                return response;
            }
            catch (Exception ex)
            {
                // Lide com a exceção de acordo com a lógica de negócios do seu aplicativo
                // Aqui, estamos relançando a exceção para os níveis superiores
                throw;
            }
        }


        public async Task<RickAndMortyResponse> CarregarPagina(int pageNumber)
        {
            try
            {
                var characters = await _rickAndMortyApi.GetCharacters(); 
                var response = new RickAndMortyResponse
                {
                    Info = characters.Info,
                    Results = characters.Results
                };
                return response;
            }
            catch (Exception ex)
            {
                // Lide com a exceção de acordo com a lógica de negócios do seu aplicativo
                // Aqui, estamos relançando a exceção para os níveis superiores
                throw;
            }
        }

        public async Task<RickAndMortyResponse> FiltrarPersonagens(string name = null, string status = null, string species = null, string type = null, string gender = null)
        {
            try
            {
                // Lógica para construir a URL com base nos parâmetros de filtro
                var url = "https://rickandmortyapi.com/api/character/"; // URL base

                if (!string.IsNullOrEmpty(name))
                {
                    url += $"?name={name}";
                }

                if (!string.IsNullOrEmpty(status))
                {
                    url += string.IsNullOrEmpty(name) ? "?" : "&";
                    url += $"status={status}";
                }

                if (!string.IsNullOrEmpty(species))
                {
                    url += string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) ? "?" : "&";
                    url += $"species={species}";
                }

                if (!string.IsNullOrEmpty(type))
                {
                    url += string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(species) ? "?" : "&";
                    url += $"type={type}";
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    url += string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(species) && string.IsNullOrEmpty(type) ? "?" : "&";
                    url += $"gender={gender}";
                }

                // Chamada à API com a URL de filtro construída
                var filteredCharacters = await _rickAndMortyApi.GetFilteredCharacters(url);

                var response = new RickAndMortyResponse
                {
                    Info = filteredCharacters.Info,
                    Results = filteredCharacters.Results
                };
                return response;
            }
            catch (Exception ex)
            {
                // Lide com a exceção de acordo com a lógica de negócios do seu aplicativo
                // Aqui, estamos relançando a exceção para os níveis superiores
                throw;
            }
        }



    }
}
