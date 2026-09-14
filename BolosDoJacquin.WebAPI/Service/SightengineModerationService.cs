using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BolosDoJacquin.WebAPI.Services
{
    public class SightengineModerationService
    {
        private readonly HttpClient _http;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        public SightengineModerationService(IConfiguration configuration)
        {
            _http = new HttpClient();
            _apiUser = configuration["SightEngine:ApiUser"]!;
            _apiSecret = configuration["SightEngine:ApiSecret"]!;
        }

        // 1. Moderação de Imagem (Usado na ProdutoController)
        public async Task<bool> ImagemEValidaAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0) return false;

            using var content = new MultipartFormDataContent();
            using var stream = arquivo.OpenReadStream();

            content.Add(new StreamContent(stream), "media", arquivo.FileName);
            content.Add(new StringContent("nudity-2.0,offensive"), "models");
            content.Add(new StringContent(_apiUser), "api_user");
            content.Add(new StringContent(_apiSecret), "api_secret");

            var resposta = await _http.PostAsync("https://api.sightengine.com/1.0/check.json", content);
            if (!resposta.IsSuccessStatusCode) return false;

            using var doc = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
            var root = doc.RootElement;

            if (root.TryGetProperty("status", out var status) && status.GetString() == "success")
            {
                if (root.TryGetProperty("nudity", out var nudity))
                {
                    if (nudity.TryGetProperty("sexual_activity", out var activity) && activity.GetDouble() > 0.5)
                        return false;
                }
                return true;
            }

            return false;
        }

        // 2. Moderação de Texto (Usado na AvaliacaoController)
        public async Task<bool> ModerarTextoAsync(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return true;

            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["text"] = texto,
                ["lang"] = "pt",
                ["mode"] = "ml",
                ["api_user"] = _apiUser,
                ["api_secret"] = _apiSecret
            });

            var resposta = await _http.PostAsync("https://api.sightengine.com/1.0/text/check.json", form);
            if (!resposta.IsSuccessStatusCode) return false;

            using var doc = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
            var root = doc.RootElement;

            if (root.TryGetProperty("status", out var status) && status.GetString() == "success")
            {
                if (root.TryGetProperty("profanity", out var profanity))
                {
                    if (profanity.TryGetProperty("matches", out var matches) && matches.GetArrayLength() > 0)
                        return false;
                }
                return true;
            }

            return false;
        }
    }
}