namespace LctMonolith.Models.DTO;

public class MissionValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? SuggestedExperience { get; set; }
}
