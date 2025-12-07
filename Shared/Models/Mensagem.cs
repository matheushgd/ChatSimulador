namespace ChatSimulador.Shared.Models
{
    public class Mensagem
    {
        public Guid Id { get; set; }
        public PlataformaChat Plataforma { get; set; }
        public DateTime DataHora { get; set; }
        public string Remetente { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public TipoMensagem TipoMensagem { get; set; }
        public string? Conteudo { get; set; }
        public string? Midia { get; set; }
        public StatusLeitura StatusLeitura { get; set; }
        public bool BorrarNome { get; set; }
        public bool BorrarAvatar { get; set; }
        public bool BorrarMidia { get; set; }
        public bool EhChamada { get; set; }
        public TipoChamada? TipoChamada { get; set; }
        public int? DuracaoEmSegundos { get; set; }
    }
}