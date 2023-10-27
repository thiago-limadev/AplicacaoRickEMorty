using System.Threading.Tasks;
using AplicacaoRickEMorty.Repositorio.Response;

namespace AplicacaoRickEMorty.Integracoes.Interface
{
    public interface IRickAndMortyIntegracao
    {
        Task<RickAndMortyResponse> ObterPersonagens();
        Task<RickAndMortyResponse> FiltrarPersonagens(string name = null, string status = null, string species = null, string type = null, string gender = null);

    }
}
