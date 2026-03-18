namespace MotAiIntel.api.Services
{
    public class DvsaService
    {
        private readonly HttpClient _http;

        public DvsaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetMotData(string reg)
        {
            return $"Mock MOT data for {reg}";
        }
    }
}
