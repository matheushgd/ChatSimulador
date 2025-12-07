namespace ChatSimulador.Shared.Models
{
    public class CsvConversor
    {
        public Mensagem Converter(MensagemCsvDTO dto)
        {
            return new Mensagem
            {
                Id = Guid.NewGuid(),
                Plataforma = Enum.Parse<PlataformaChat>(dto.Plataforma, true),
                DataHora = DateTime.Parse(dto.DataHora),
                Remetente = dto.Remetente,
                NomeExibicao = dto.NomeExibicao,
                Avatar = dto.Avatar,
                TipoMensagem = Enum.Parse<TipoMensagem>(dto.TipoMensagem, true),
                Conteudo = dto.Conteudo,
                Midia = dto.Midia,
                StatusLeitura = Enum.Parse<StatusLeitura>(dto.StatusLeitura, true),
                BorrarNome = dto.BorrarNome?.ToLower() == "sim",
                BorrarAvatar = dto.BorrarAvatar?.ToLower() == "sim",
                BorrarMidia = dto.BorrarMidia?.ToLower() == "sim",
                EhChamada = dto.EhChamada?.ToLower() == "sim",
                TipoChamada = string.IsNullOrEmpty(dto.TipoChamada) ? null : Enum.Parse<TipoChamada>(dto.TipoChamada, true),
                DuracaoEmSegundos = string.IsNullOrEmpty(dto.Duracao) ? null : int.Parse(dto.Duracao)
            };
        }
    }
}