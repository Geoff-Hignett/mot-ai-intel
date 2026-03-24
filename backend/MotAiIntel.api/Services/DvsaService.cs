namespace MotAiIntel.api.Services
{
    using System.Net.Http.Headers;
    using System.Text.Json;
    using MotAiIntel.api.Models;

    public class DvsaService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public DvsaService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<string> GetMotData(string reg)
        {
            var token = await GetAccessToken();

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://history.mot.api.gov.uk/v1/trade/vehicles/registration/{reg}"
            );

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("x-api-key", _config["Dvsa:ApiKey"]);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _http.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"DVSA error: {json}";
            }

            return json;
        }

        private async Task<string> GetAccessToken()
        {
            var tokenUrl = _config["Dvsa:TokenUrl"];

            var body = new Dictionary<string, string>
        {
            { "client_id", _config["Dvsa:ClientId"]! },
            { "client_secret", _config["Dvsa:ClientSecret"]! },
            { "scope", _config["Dvsa:Scope"]! },
            { "grant_type", "client_credentials" }
        };

            var response = await _http.PostAsync(tokenUrl, new FormUrlEncodedContent(body));

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Token error: {json}");
            }

            var token = JsonSerializer.Deserialize<DvsaTokenResponse>(json);

            return token!.access_token;
        }
    }
}
