namespace LctMonolith.Models.Database;

public class Dialogue
{
    public Guid Id { get; set; }

    public Guid MissionId { get; set; }
    public required Mission Mission { get; set; }

    public Guid InitialDialogueMessageId { get; set; }
    public DialogueMessage? InitialDialogueMessage { get; set; }
    public Guid InterimDialogueMessageId { get; set; }
    public DialogueMessage? InterimDialogueMessage { get; set; }
    public Guid EndDialogueMessageId { get; set; }
    public DialogueMessage? EndDialogueMessage { get; set; }
}
