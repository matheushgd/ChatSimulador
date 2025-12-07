namespace ChatSimulador.Shared.Models
{
    public class MensagemCsvDTO
    {
        public string Plataforma { get; set; } = string.Empty;
        public string DataHora { get; set; } = string.Empty;
        public string Remetente { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string TipoMensagem { get; set; } = string.Empty;
        public string? Conteudo { get; set; }
        public string? Midia { get; set; }
        public string StatusLeitura { get; set; } = string.Empty;
        public string? BorrarNome { get; set; }
        public string? BorrarAvatar { get; set; }
        public string? BorrarMidia { get; set; }
        public string? EhChamada { get; set; }
        public string? TipoChamada { get; set; }
        public string? Duracao { get; set; }
    }
}