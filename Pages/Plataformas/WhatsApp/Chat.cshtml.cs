using Microsoft.AspNetCore.Mvc.RazorPages;
using ChatSimulador.Shared.Models;
using System.Text.Json;
using System.Linq;

namespace ChatSimulador.Pages.Plataformas.WhatsApp
{
    public class ChatModel : PageModel
    {
        public List<Mensagem> Mensagens { get; set; } = new List<Mensagem>();
        public string NomeContato { get; set; } = "Contato Desconhecido";
        public string ContatoAvatar { get; set; } = "/whatsapp/wwwroot/avatares/joao.png"; 

        public void OnGet()
        {
            if (TempData["Mensagens"] is string mensagensJson)
            {
                Mensagens = JsonSerializer.Deserialize<List<Mensagem>>(mensagensJson) ?? new List<Mensagem>();

                if (Mensagens.Any())
                {
                    var outroRemetente = Mensagens.FirstOrDefault(m => m.Remetente != "Você");
                    if (outroRemetente != null)
                    {
                        NomeContato = !string.IsNullOrEmpty(outroRemetente.NomeExibicao) ? outroRemetente.NomeExibicao : "João";
                        ContatoAvatar = !string.IsNullOrEmpty(outroRemetente.Avatar) ? $"/whatsapp//wwwroot/avatares/{outroRemetente.Avatar}" : "/whatsapp/wwwroot/avatares/joao.png";
                    }

                    // Ordena por DataHora e agrupa para balões de data
                    Mensagens = Mensagens.OrderBy(m => m.DataHora).ToList();
                    var grouped = Mensagens.GroupBy(m => m.DataHora.Date);
                    foreach (var group in grouped)
                    {
                        group.First().DataBalao = group.Key;  // Atribui data ao primeiro da grupo
                    }
                }
            }
        }
    }
}
