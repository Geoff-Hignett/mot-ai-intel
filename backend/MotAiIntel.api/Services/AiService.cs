using MotAiIntel.api.Models;

namespace MotAiIntel.api.Services
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;

    public class AiService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<AiResult> Analyse(string motData, User? user)
        {
            var apiKey = _config["OpenAiKey"];

            var isGuest = user == null;
            var hasProfile =
                user?.YearlyMileage != null &&
                user?.DrivingType != null &&
                user?.MechanicalKnowledge != null;

            string guidance = "";

            if (isGuest)
            {
                guidance = "Include a short note telling the user they can improve results by registering.";
            }
            else if (!hasProfile)
            {
                guidance = "Include a short note telling the user to complete their profile for better results.";
            }

            var prompt = $@"
Analyse this MOT data and return ONLY JSON in this format:

{{
  ""risk"": ""Low|Medium|High"",
  ""summary"": ""short explanation"",
  ""recommendations"": [""rec1"", ""rec2""]
}}

Rules:
- Do NOT include markdown
- Do NOT include extra text
- Only valid JSON

User context:
Mileage: {(user?.YearlyMileage?.ToString() ?? "Unknown")}
Driving type: {user?.DrivingType ?? "Unknown"}
Mechanical knowledge: {user?.MechanicalKnowledge ?? "Unknown"}

Additional instruction:
{guidance}

MOT Data:
{motData}
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI error: {json}");
            }

            using var doc = JsonDocument.Parse(json);

            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("Empty AI response");

            try
            {
                return JsonSerializer.Deserialize<AiResult>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch
            {
                throw new Exception($"Failed to parse AI JSON: {content}");
            }
        }
    }
}