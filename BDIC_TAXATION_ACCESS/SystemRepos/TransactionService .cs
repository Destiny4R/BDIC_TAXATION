using BDIC_TAXATION_MODELS.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;
        private readonly string _baseUrl;

        public TransactionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUrl = configuration["CredoApi:BaseUrl"] ?? throw new ArgumentNullException("BaseUrl not found in configuration");
            _secretKey = configuration["CredoApi:SecretKey"] ?? throw new ArgumentNullException("SecretKey not found in configuration");

            // Configure HttpClient
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(_secretKey);
        }

        public async Task<VerifyPaymentResponse> GetTransactionDetailsAsync(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                throw new ArgumentException("Transaction ID cannot be null or empty", nameof(reference));

            try
            {
                var endpoint = $"transaction/{reference}/verify";
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                return JsonSerializer.Deserialize<VerifyPaymentResponse>(content, options)
                    ?? throw new InvalidOperationException("Deserialization returned null");
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error verifying transaction with reference: {reference}", ex);
            }
            catch (JsonException jsonEx)
            {
                throw new ApplicationException("Failed to deserialize transaction verification response", jsonEx);
            }
        }
    }

}
