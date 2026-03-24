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

        public async Task<string> Analyse(string motData, User? user)
        {
            var apiKey = _config["OpenAiKey"];

            // 👇 Determine user state
            var isGuest = user == null;
            var hasProfile =
                user?.YearlyMileage != null &&
                user?.DrivingType != null &&
                user?.MechanicalKnowledge != null;

            // 👇 Guidance message
            string guidance = "";

            if (isGuest)
            {
                guidance = "This recommendation can be improved if you register and provide your driving details.";
            }
            else if (!hasProfile)
            {
                guidance = "You can get more accurate recommendations by completing your profile.";
            }

            var prompt = $@"
Analyse this MOT data and provide:
- Risk level (Low/Medium/High)
- Short explanation
- 2 recommendations

User context:
Mileage: {(user?.YearlyMileage?.ToString() ?? "Unknown")}
Driving type: {user?.DrivingType ?? "Unknown"}
Mechanical knowledge: {user?.MechanicalKnowledge ?? "Unknown"}

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

            // Handle API errors cleanly
            if (!response.IsSuccessStatusCode)
            {
                return $"OpenAI error: {json}";
            }

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("choices", out var choices))
            {
                return $"Unexpected response: {json}";
            }

            var aiText = choices[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";

            // Append guidance AFTER AI (guaranteed UX)
            if (!string.IsNullOrWhiteSpace(guidance))
            {
                aiText += $"\n\n{guidance}";
            }

            return aiText;
        }
    }
}