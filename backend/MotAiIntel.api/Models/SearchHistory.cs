namespace MotAiIntel.api.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string Registration { get; set; } = "";
        public DateTime SearchedAt { get; set; } = DateTime.UtcNow;

        public string AiSummary { get; set; } = "";

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
