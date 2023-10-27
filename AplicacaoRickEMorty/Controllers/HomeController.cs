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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
