namespace LctMonolith.Models.Database;

public class Dialogue
{
    public Guid Id { get; set; }

    public Guid MissionId { get; set; }
    public required Mission Mission { get; set; }

    public Guid InitialDialogueMessageId { get; set; }
    public Dialogue? InitialDialogueMessage { get; set; }
    public Guid InterimDialogueMessageId { get; set; }
    public Dialogue? InterimDialogueMessage { get; set; }
    public Guid EndDialogueMessageId { get; set; }
    public Dialogue? EndDialogueMessage { get; set; }
}
