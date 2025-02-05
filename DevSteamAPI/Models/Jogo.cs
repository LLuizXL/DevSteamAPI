namespace DevSteamAPI.Models
{
    public class Jogo
    {
        public Guid Jogoid { get; set; }
        public string Nome { get; set; }
        public int CLassificacao { get; set; }
        public string? Descricao { get; set; }
        // o ? permite inserir valores nulos
        public string? Imagem { get; set; }

        public Guid CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
