using System.Text;
using ChatSimulador.Shared.Models;

namespace ChatSimulador.Shared.Services
{
    public class CsvService
    {
        private readonly CsvConversor _csvConversor = new();

        public List<Mensagem> LerCsv(Stream arquivoCsv)
        {
            var mensagens = new List<Mensagem>();
            using (var reader = new StreamReader(arquivoCsv, Encoding.UTF8))
            {
                reader.ReadLine(); // pula cabeçalho
                string? linha;
                while ((linha = reader.ReadLine()) != null)
                {
                    var colunas = ParseCsvLine(linha);
                    if (colunas.Length != 15) continue;

                    try
                    {
                        var dto = new MensagemCsvDTO
                        {
                            Plataforma = colunas[0],
                            DataHora = colunas[1],
                            Remetente = colunas[2],
                            NomeExibicao = colunas[3],
                            Avatar = colunas[4],
                            TipoMensagem = colunas[5],
                            Conteudo = colunas[6],
                            Midia = colunas[7],
                            StatusLeitura = colunas[8],
                            BorrarNome = colunas[9],
                            BorrarAvatar = colunas[10],
                            BorrarMidia = colunas[11],
                            EhChamada = colunas[12],
                            TipoChamada = colunas[13],
                            Duracao = colunas[14]
                        };

                        var mensagem = _csvConversor.Converter(dto);
                        mensagem.Id = Guid.NewGuid();
                        mensagens.Add(mensagem);
                    }
                    catch { }
                }
            }
            return mensagens.OrderBy(m => m.DataHora).ToList();
        }

        private string[] ParseCsvLine(string line)
        {
            line = line.Trim();
            if (line.StartsWith("\"") && line.EndsWith("\""))
                line = line.Substring(1, line.Length - 2);

            var result = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else inQuotes = !inQuotes;
                }
                else if (c == ';' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else current.Append(c);
            }
            result.Add(current.ToString());
            while (result.Count < 15) result.Add("");
            return result.ToArray();
        }
    }
}