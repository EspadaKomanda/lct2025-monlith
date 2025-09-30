using LctMonolith.Models.Database;

namespace LctMonolith.Services.Interfaces;

public interface IDialogueService
{
    Task<Dialogue?> GetDialogueByMissionIdAsync(Guid missionId);
    Task<Dialogue> CreateDialogueAsync(Dialogue dialogue);
    Task<DialogueMessage?> GetDialogueMessageByIdAsync(Guid messageId);
    Task<IEnumerable<DialogueMessageResponseOption>> GetResponseOptionsAsync(Guid messageId);
    Task<DialogueMessage?> ProcessDialogueResponseAsync(Guid messageId, Guid responseOptionId, Guid playerId);
}
