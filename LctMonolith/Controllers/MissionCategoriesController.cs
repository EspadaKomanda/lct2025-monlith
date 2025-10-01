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

    public MissionCategoriesController(IMissionCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllCategoriesAsync();
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var category = await _service.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMissionCategoryDto dto)
    {
        var created = await _service.CreateCategoryAsync(new MissionCategory { Title = dto.Title });
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateMissionCategoryDto dto)
    {
        var existing = await _service.GetCategoryByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }
        existing.Title = dto.Title;
        await _service.UpdateCategoryAsync(existing);
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var removed = await _service.DeleteCategoryAsync(id);
        if (!removed)
        {
            return NotFound();
        }
        return NoContent();
    }
}
