namespace AplicacaoRickEMorty.Repositorio.Response
{

    public class RickAndMortyResponse
    {
        public Models.Info Info { get; set; }
        public List<Models.Character> Results { get; set; }
    }

    public class Info
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
    }
}
