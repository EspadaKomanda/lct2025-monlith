using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/dialogue")]
[Authorize]
public class DialogueController : ControllerBase
{
    private readonly IDialogueService _dialogueService;

    public DialogueController(IDialogueService dialogueService)
    {
        _dialogueService = dialogueService;
    }

    [HttpGet("mission/{missionId:guid}")]
    public async Task<IActionResult> GetByMission(Guid missionId)
    {
        var d = await _dialogueService.GetDialogueByMissionIdAsync(missionId);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpGet("message/{messageId:guid}")]
    public async Task<IActionResult> GetMessage(Guid messageId)
    {
        var m = await _dialogueService.GetDialogueMessageByIdAsync(messageId);
        return m == null ? NotFound() : Ok(m);
    }

    [HttpGet("message/{messageId:guid}/options")]
    public async Task<IActionResult> GetOptions(Guid messageId)
    {
        var opts = await _dialogueService.GetResponseOptionsAsync(messageId);
        return Ok(opts);
    }

    public record DialogueResponseRequest(Guid ResponseOptionId, Guid PlayerId);

    [HttpPost("message/{messageId:guid}/respond")]
    public async Task<IActionResult> Respond(Guid messageId, DialogueResponseRequest req)
    {
        var next = await _dialogueService.ProcessDialogueResponseAsync(messageId, req.ResponseOptionId, req.PlayerId);
        if (next == null) return Ok(new { end = true });
        return Ok(next);
    }

    public class CreateDialogueRequest
    {
        public Guid MissionId { get; set; }
        public Guid InitialDialogueMessageId { get; set; }
        public Guid InterimDialogueMessageId { get; set; }
        public Guid EndDialogueMessageId { get; set; }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateDialogueRequest dto)
    {
        var d = new Dialogue
        {
            Id = Guid.NewGuid(),
            MissionId = dto.MissionId,
            InitialDialogueMessageId = dto.InitialDialogueMessageId,
            InterimDialogueMessageId = dto.InterimDialogueMessageId,
            EndDialogueMessageId = dto.EndDialogueMessageId,
            Mission = null! // EF will populate if included
        };
        d = await _dialogueService.CreateDialogueAsync(d);
        return CreatedAtAction(nameof(GetByMission), new { missionId = d.MissionId }, d);
    }
}
