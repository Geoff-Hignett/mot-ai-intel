using MotAiIntel.api.Models;

namespace MotAiIntel.api.Services
{
    public class AiService
    {
        public async Task<string> Analyse(string motData, User? user)
        {
            return $"AI summary (mock). Mileage: {user?.YearlyMileage}";
        }
    }
}
