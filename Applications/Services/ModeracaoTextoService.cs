using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BolosDoJacquin.Infra.Sightengine
{
    public class ModeracaoTextoService : IModeracaoTextoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        // String com TODAS as categorias de segurança ativadas
        private const string Categorias = "profanity,personal,link,drug,weapon,spam,extremism,violence,self-harm";

        public ModeracaoTextoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUser = configuration["Sightengine:ApiUser"] ?? string.Empty;
            _apiSecret = configuration["Sightengine:ApiSecret"] ?? string.Empty;
        }

        public async Task<bool> ContemConteudoImproprio(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;

            var parametros = new Dictionary<string, string>
            {
                { "text", texto },
                { "lang", "pt" },
                { "mode", "rules,ml" },
                { "categories", Categorias }, // <-- Enviando o arsenal completo aqui!
                { "api_user", _apiUser },
                { "api_secret", _apiSecret }
            };

            using var conteudo = new FormUrlEncodedContent(parametros);

            using var resposta = await _httpClient.PostAsync("text/check.json", conteudo);
            if (!resposta.IsSuccessStatusCode)
            {
                var erroDaIA = await resposta.Content.ReadAsStringAsync();
                throw new BolosDoJacquin.Domain.Exceptions.DomainException("ERRO_IA", $"A IA respondeu isso: {erroDaIA}");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var resultado = JsonSerializer.Deserialize<SightengineTextResponseDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return resultado != null && resultado.ContemViolacao;
        }
    }
}