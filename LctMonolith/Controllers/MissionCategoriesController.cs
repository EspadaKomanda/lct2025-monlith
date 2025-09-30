using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;
using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/mission-categories")]
[Authorize]
public class MissionCategoriesController : ControllerBase
{
    private readonly IMissionCategoryService _service;
    public MissionCategoriesController(IMissionCategoryService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllCategoriesAsync();
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var c = await _service.GetCategoryByIdAsync(id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMissionCategoryDto dto)
    {
        var c = await _service.CreateCategoryAsync(new MissionCategory { Title = dto.Title });
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateMissionCategoryDto dto)
    {
        var c = await _service.GetCategoryByIdAsync(id);
        if (c == null) return NotFound();
        c.Title = dto.Title;
        await _service.UpdateCategoryAsync(c);
        return Ok(c);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteCategoryAsync(id);
        return ok ? NoContent() : NotFound();
    }
}

