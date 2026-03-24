namespace MotAiIntel.api.Models
{
    public class VehicleResponse
    {
        public VehicleInfo Vehicle { get; set; } = new();
        public DefectInfo Defects { get; set; } = new();
        public AiResult Ai { get; set; } = new();
    }

    public class VehicleInfo
    {
        public string Registration { get; set; } = "";
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public string? LastTestResult { get; set; }
        public string? LastTestDate { get; set; }
    }

    public class DefectInfo
    {
        public List<string> Major { get; set; } = new();
        public List<string> Minor { get; set; } = new();
        public List<string> Advisory { get; set; } = new();
    }
}
