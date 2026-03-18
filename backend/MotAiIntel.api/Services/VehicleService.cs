using MotAiIntel.api.Data;
using MotAiIntel.api.Models;

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

        public async Task<object> GetVehicle(string reg, int? userId)
        {
            var motData = await _dvsa.GetMotData(reg);

            User? user = null;

            if (userId != null)
            {
                user = await _db.Users.FindAsync(userId);
            }

            var aiResult = await _ai.Analyse(motData, user);

            var search = new SearchHistory
            {
                Registration = reg,
                AiSummary = aiResult,
                UserId = userId
            };

            _db.Searches.Add(search);
            await _db.SaveChangesAsync();

            return new
            {
                motData,
                ai = aiResult
            };
        }
    }
}
