using LctMonolith.Models.Enums;

namespace LctMonolith.Models.Database;

public class DialogueMessage
{
    public Guid Id { get; set; }
    public Character CharacterLeft { get; set; } = Character.None;
    public Character CharacterRight { get; set; } = Character.None;
    public CharacterAnimation CharacterLeftAnim { get; set; } = CharacterAnimation.Neutral;
    public CharacterAnimation CharacterRightAnim { get; set; } = CharacterAnimation.Neutral;
    public string CharacterLeftMessage { get; set; } = string.Empty;
    public string CharacterRightMessage { get; set; } = string.Empty;
    public MessageStyle CharacterLeftMessageStyle { get; set; } = MessageStyle.Normal;
    public MessageStyle CharacterRightMessageStyle { get; set; } = MessageStyle.Normal;
    public bool AllowMessageAi { get; set; }
    public string MessageAiButtonText { get; set; } = string.Empty;

    public Guid InitialDialogueId { get; set; }
    public Dialogue? InitialDialogue { get; set; }
    public Guid InterimDialogueId { get; set; }
    public Dialogue? InterimDialogue { get; set; }
    public Guid EndDialogueId { get; set; }
    public Dialogue? EndDialogue { get; set; }

    public ICollection<DialogueMessageResponseOption> DialogueMessageResponseOptions = new List<DialogueMessageResponseOption>();
}
