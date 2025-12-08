using ChatSimulador.Shared.Models;
using System.Globalization;

namespace ChatSimulador.Shared.Services
{
    public class CsvService
    {
        public async Task<List<Mensagem>> LerCsvAsync(IFormFile file)
        {
            var mensagens = new List<Mensagem>();

            using var reader = new StreamReader(file.OpenReadStream(), System.Text.Encoding.UTF8);

            // Pula o header
            var header = await reader.ReadLineAsync();
            Console.WriteLine($"Header do CSV: {header}");

            int linhaNumero = 1;
            while (!reader.EndOfStream)
            {
                var linha = await reader.ReadLineAsync();
                linhaNumero++;

                if (string.IsNullOrWhiteSpace(linha)) continue;

                // Remove aspas duplas da linha inteira
                linha = linha.Trim('"');

                var campos = linha.Split(';');

                if (campos.Length < 9)
                {
                    Console.WriteLine($"Linha {linhaNumero} ignorada: menos de 9 campos");
                    continue;
                }

                try
                {
                    // Verifica se é chamada
                    bool ehChamada = campos[12].Trim().ToLower() == "sim";

                    var mensagem = new Mensagem
                    {
                        Plataforma = ParseEnum<PlataformaChat>(campos[0]),
                        DataHora = ParseDateTime(campos[1]),
                        Remetente = campos[2].Trim(),
                        NomeExibicao = campos[3].Trim(),
                        Avatar = campos[4].Trim(),
                        TipoMensagem = ehChamada ? TipoMensagem.Chamada : ParseEnum<TipoMensagem>(campos[5]),
                        Conteudo = campos[6].Trim(),
                        Midia = campos[7].Trim(),
                        StatusLeitura = ParseEnum<StatusLeitura>(campos[8]),
                        BorrarNome = campos[9].Trim().ToLower() == "sim",
                        BorrarAvatar = campos[10].Trim().ToLower() == "sim",
                        BorrarMidia = campos[11].Trim().ToLower() == "sim",
                        TipoChamada = ehChamada && campos.Length > 13 ? ParseEnumNullable<TipoChamada>(campos[13]) : null,
                        DuracaoEmSegundos = campos.Length > 14 && int.TryParse(campos[14], out var duracao) ? duracao : null
                    };

                    mensagens.Add(mensagem);
                    Console.WriteLine($"Linha {linhaNumero} processada: {mensagem.NomeExibicao} - {mensagem.TipoMensagem}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar linha {linhaNumero}: {ex.Message}");
                }
            }

            Console.WriteLine($"Total de mensagens processadas: {mensagens.Count}");
            return mensagens;
        }

        private DateTime ParseDateTime(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DateTime.Now;

            // Formato esperado: 2025-02-01 08:45:00
            string[] formatos = new[]
            {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm",
                "dd/MM/yyyy HH:mm:ss",
                "dd/MM/yyyy HH:mm"
            };

            foreach (var formato in formatos)
            {
                if (DateTime.TryParseExact(value.Trim(), formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out var resultado))
                {
                    return resultado;
                }
            }

            if (DateTime.TryParse(value, out var resultadoGenerico))
            {
                return resultadoGenerico;
            }

            Console.WriteLine($"AVISO: Data '{value}' não reconhecida, usando DateTime.Now");
            return DateTime.Now;
        }

        private T ParseEnum<T>(string? value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return default;

            // Remove espaços e tenta parse
            value = value.Trim();

            // Tenta parse case-insensitive
            if (Enum.TryParse<T>(value, true, out var result))
                return result;

            // Tenta remover acentos e espaços (ex: "Voz Perdida" -> "VozPerdida")
            value = value.Replace(" ", "");
            if (Enum.TryParse<T>(value, true, out result))
                return result;

            Console.WriteLine($"AVISO: Valor '{value}' não mapeado para enum {typeof(T).Name}, usando default");
            return default;
        }

        private T? ParseEnumNullable<T>(string? value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Trim().Replace(" ", "");

            return Enum.TryParse<T>(value, true, out var result) ? result : null;
        }
    }
}
