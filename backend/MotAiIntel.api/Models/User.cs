namespace MotAiIntel.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        public int? YearlyMileage { get; set; }
        public string? DrivingType { get; set; }
        public string? MechanicalKnowledge { get; set; }

        public List<SearchHistory> Searches { get; set; } = new();
    }
}
