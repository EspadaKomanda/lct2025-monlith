using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class DialogueService : IDialogueService
{
    private readonly IUnitOfWork _uow;
    public DialogueService(IUnitOfWork uow) => _uow = uow;

    public async Task<Dialogue?> GetDialogueByMissionIdAsync(Guid missionId)
    {
        try { return await _uow.Dialogues.Query(d => d.MissionId == missionId).FirstOrDefaultAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetDialogueByMissionIdAsync failed {MissionId}", missionId); throw; }
    }

    public async Task<Dialogue> CreateDialogueAsync(Dialogue dialogue)
    {
        try
        {
            dialogue.Id = Guid.NewGuid();
            await _uow.Dialogues.AddAsync(dialogue);
            await _uow.SaveChangesAsync();
            return dialogue;
        }
        catch (Exception ex) { Log.Error(ex, "CreateDialogueAsync failed {MissionId}", dialogue.MissionId); throw; }
    }

    public async Task<DialogueMessage?> GetDialogueMessageByIdAsync(Guid messageId)
    {
        try { return await _uow.DialogueMessages.GetByIdAsync(messageId); }
        catch (Exception ex) { Log.Error(ex, "GetDialogueMessageByIdAsync failed {MessageId}", messageId); throw; }
    }

    public async Task<IEnumerable<DialogueMessageResponseOption>> GetResponseOptionsAsync(Guid messageId)
    {
        try { return await _uow.DialogueMessageResponseOptions.Query(o => o.ParentDialogueMessageId == messageId).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetResponseOptionsAsync failed {MessageId}", messageId); throw; }
    }

    public async Task<DialogueMessage?> ProcessDialogueResponseAsync(Guid messageId, Guid responseOptionId, Guid playerId)
    {
        try
        {
            var option = await _uow.DialogueMessageResponseOptions.Query(o => o.Id == responseOptionId && o.ParentDialogueMessageId == messageId).FirstOrDefaultAsync();
            if (option == null) return null;
            if (option.DestinationDialogueMessageId == null) return null; // end branch
            return await _uow.DialogueMessages.GetByIdAsync(option.DestinationDialogueMessageId);
        }
        catch (Exception ex) { Log.Error(ex, "ProcessDialogueResponseAsync failed {MessageId} {ResponseId}", messageId, responseOptionId); throw; }
    }
}
