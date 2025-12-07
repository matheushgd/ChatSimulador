using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChatSimulador.Shared.Services;

namespace ChatSimulador.Pages.WhatsApp
{
    public class UploadModel : PageModel
    {
        private readonly CsvService _csvService;

        public UploadModel(CsvService csvService)
        {
            _csvService = csvService;
        }

        public void OnGet() { }

        public IActionResult OnPost(IFormFile arquivoCsv)
        {
            if (arquivoCsv == null || arquivoCsv.Length == 0)
            {
                ModelState.AddModelError("", "Selecione um arquivo CSV válido");
                return Page();
            }

            try
            {
                using var stream = arquivoCsv.OpenReadStream();
                var mensagens = _csvService.LerCsv(stream);

                if (mensagens.Count == 0)
                {
                    ModelState.AddModelError("", "Nenhuma mensagem foi carregada do CSV");
                    return Page();
                }

                TempData["Mensagens"] = System.Text.Json.JsonSerializer.Serialize(mensagens);
                return RedirectToPage("/WhatsApp/Chat");  // ✅ CORRIGIDO
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao processar CSV: {ex.Message}");
                return Page();
            }
        }
    }
}
