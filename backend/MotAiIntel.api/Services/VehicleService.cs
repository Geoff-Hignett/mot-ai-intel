using MotAiIntel.api.Data;
using MotAiIntel.api.Models;
using System.Text.Json;

namespace MotAiIntel.api.Services
{
    public class VehicleService
    {
        private readonly DvsaService _dvsa;
        private readonly AiService _ai;
        private readonly AppDbContext _db;

        public VehicleService(DvsaService dvsa, AiService ai, AppDbContext db)
        {
            _dvsa = dvsa;
            _ai = ai;
            _db = db;
        }

        public async Task<VehicleResponse> GetVehicle(string reg, int? userId)
        {
            var motJson = await _dvsa.GetMotData(reg);

            var doc = JsonDocument.Parse(motJson);
            var root = doc.RootElement;

            var tests = root.GetProperty("motTests");

            if (tests.GetArrayLength() == 0)
                throw new Exception("No MOT tests found");

            var latest = tests[0];

            // Vehicle info
            var vehicle = new VehicleInfo
            {
                Registration = root.GetProperty("registration").GetString() ?? "",
                Make = root.GetProperty("make").GetString() ?? "",
                Model = root.GetProperty("model").GetString() ?? "",
                LastTestResult = latest.GetProperty("testResult").GetString(),
                LastTestDate = latest.GetProperty("completedDate").GetString()
            };

            // Defects parsing
            var defects = latest.GetProperty("defects");

            var major = new List<string>();
            var minor = new List<string>();
            var advisory = new List<string>();

            foreach (var d in defects.EnumerateArray())
            {
                var type = d.GetProperty("type").GetString();
                var text = d.GetProperty("text").GetString();

                if (type == "MAJOR") major.Add(text ?? "");
                if (type == "MINOR") minor.Add(text ?? "");
                if (type == "ADVISORY") advisory.Add(text ?? "");
            }

            var defectInfo = new DefectInfo
            {
                Major = major,
                Minor = minor,
                Advisory = advisory
            };

            // Get user
            User? user = null;

            if (userId != null)
            {
                user = await _db.Users.FindAsync(userId);
            }

            // AI input (clean + structured)
            var aiInput = $@"
Vehicle: {vehicle.Make} {vehicle.Model}
Latest result: {vehicle.LastTestResult}
Major defects: {string.Join("; ", major)}
Minor defects: {string.Join("; ", minor)}
Advisories: {string.Join("; ", advisory)}
";

            var ai = await _ai.Analyse(aiInput, user);

            // Save search
            _db.Searches.Add(new SearchHistory
            {
                Registration = reg,
                SearchedAt = DateTime.UtcNow,
                AiSummary = JsonSerializer.Serialize(ai),
                UserId = userId
            });

            await _db.SaveChangesAsync();

            return new VehicleResponse
            {
                Vehicle = vehicle,
                Defects = defectInfo,
                Ai = ai
            };
        }
    }
}
