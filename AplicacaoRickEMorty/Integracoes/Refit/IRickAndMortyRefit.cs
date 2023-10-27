using System.Threading.Tasks;
using AplicacaoRickEMorty.Models;
using Refit;

namespace AplicacaoRickEMorty.Services
{
    public interface IRickAndMortyRefit
    {
        [Get("/api/character")]
        Task<Characters> GetCharacters();
        Task<Characters> GetFilteredCharacters(string url);
    }
}
