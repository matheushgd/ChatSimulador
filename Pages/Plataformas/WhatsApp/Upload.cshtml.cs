using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChatSimulador.Shared.Services;
using System.Text.Json;

namespace ChatSimulador.Pages.Plataformas.WhatsApp
{
    public class UploadModel : PageModel
    {
        private readonly CsvService _csvService;

        public UploadModel(CsvService csvService)
        {
            _csvService = csvService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Arquivo inválido");
                return Page();
            }

            var mensagens = await _csvService.LerCsvAsync(file);

            Console.WriteLine($"Total de mensagens lidas: {mensagens.Count}");

            if (mensagens.Count == 0)
            {
                ModelState.AddModelError("", "Nenhuma mensagem foi carregada do CSV");
                return Page();
            }

            TempData["Mensagens"] = JsonSerializer.Serialize(mensagens);
            return RedirectToPage("/Plataformas/WhatsApp/Chat");

        }
    }
}
