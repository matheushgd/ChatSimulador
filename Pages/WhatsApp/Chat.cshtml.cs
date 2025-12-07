using Microsoft.AspNetCore.Mvc.RazorPages;
using ChatSimulador.Shared.Models;

namespace ChatSimulador.Pages.WhatsApp
{
    public class ChatModel : PageModel
    {
        public List<Mensagem> Mensagens { get; set; } = new();

        public void OnGet()
        {
            var json = TempData["Mensagens"] as string;
            if (!string.IsNullOrEmpty(json))
            {
                var todasMensagens = System.Text.Json.JsonSerializer.Deserialize<List<Mensagem>>(json) ?? new();
                // FILTRA APENAS MENSAGENS DO WHATSAPP
                Mensagens = todasMensagens.Where(m => m.Plataforma == PlataformaChat.WhatsApp).ToList();
            }
        }
    }
}