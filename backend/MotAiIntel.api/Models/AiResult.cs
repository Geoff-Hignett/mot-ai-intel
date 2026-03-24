namespace MotAiIntel.api.Models
{
    public class AiResult
    {
        public string Risk { get; set; } = "";
        public string Summary { get; set; } = "";
        public List<string> Recommendations { get; set; } = new();
    }
}
