using AplicacaoRickEMorty.Integracoes.Interface;
using AplicacaoRickEMorty.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AplicacaoRickEMorty.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRickAndMortyIntegracao _rickAndMortyIntegracao;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IRickAndMortyIntegracao rickAndMortyIntegracao)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _rickAndMortyIntegracao = rickAndMortyIntegracao;
        }

        public async Task<IActionResult> Index(string name, string status, string species, string type, string gender, int page = 1)
        {
            try
            {
                HttpClient httpClient = _clientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync($"https://rickandmortyapi.com/api/character?page={page}&name={name}&status={status}&species={species}&type={type}&gender={gender}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Characters characters = JsonConvert.DeserializeObject<Characters>(responseBody);


                return View(characters);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na solicitação HTTP: {ex.Message}");
                return StatusCode(500, "Erro ao buscar os personagens. Por favor, tente novamente mais tarde.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro. Por favor, tente novamente mais tarde.");
            }
        }

       

        public async Task<IActionResult> CharacterDetails(int id)
        {
            try
            {
                HttpClient httpClient = _clientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync($"https://rickandmortyapi.com/api/character/{id}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Character character = JsonConvert.DeserializeObject<Character>(responseBody);

                List<string> episodeNames = new List<string>();
                List<int> episodeIds = new List<int>();
                foreach (var episodeUrl in character.Episode)
                {
                    var episodeId = GetEpisodeIdFromUrl(episodeUrl);
                    string episodeName = await GetEpisodeName(episodeId);
                    episodeNames.Add(episodeName);
                    episodeIds.Add(episodeId);
                    
                }
                character.EpisodeNames = episodeNames;
                character.NumEpisodes = episodeIds;

                return View(character); 
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na solicitação HTTP: {ex.Message}");
                return StatusCode(500, "Erro ao buscar o personagem. Por favor, tente novamente mais tarde.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro. Por favor, tente novamente mais tarde.");
            }
        }

        private int GetEpisodeIdFromUrl(string url)
        {
            var segments = new Uri(url).Segments;
            var idString = segments[segments.Length - 1].Trim('/');
            if (int.TryParse(idString, out int id))
            {
                return id;
            }
            return -1; 
        }

        public async Task<string> GetEpisodeName(int episodeId)
        {
            try
            {
                HttpClient httpClient = _clientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync($"https://rickandmortyapi.com/api/episode/{episodeId}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic episode = JsonConvert.DeserializeObject(responseBody);

                return episode.name;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na solicitação HTTP: {ex.Message}");
                return "Nome do episódio não encontrado";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                return "Ocorreu um erro ao buscar o nome do episódio";
            }
        }

    }
}
