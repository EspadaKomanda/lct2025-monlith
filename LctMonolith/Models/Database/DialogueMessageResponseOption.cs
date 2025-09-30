using LctMonolith.Models.Enums;

namespace LctMonolith.Models.Database;

public class DialogueMessageResponseOption
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "...";
    public MessageStyle MessageStyle { get; set; } = MessageStyle.Normal;
    public int z { get; set; }

    public Guid ParentDialogueMessageId { get; set; }
    public required DialogueMessage ParentDialogueMessage { get; set; }
    public Guid DestinationDialogueMessageId { get; set; }
    public DialogueMessage? DestinationDialogueMessage { get; set; }
}
