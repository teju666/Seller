
using Microsoft.AspNetCore.Mvc;
using Seller.Api.Models;
using Seller.Api.Services;

namespace Seller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SellersController : ControllerBase
{
    private readonly ISellerService _svc;

    public SellersController(ISellerService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SellerCreateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var created = await _svc.CreateAsync(dto, ct);

            // Map to your response DTO. Adjust fields based on your Seller entity & DTOs.
            var result = new SellerDto(
                created.Id,
                created.Name
                // , created.Email
                // , created.Phone
                // , created.Address
                // , created.Code
                // , created.GstNumber
            );

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            // For conflicts like unique key violations
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
        }
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = "name",
        [FromQuery] string? sortDir = "asc",
        CancellationToken ct = default)
    {
        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

        var (items, total) = await _svc.ListAsync(page, pageSize, search, sortBy, desc, ct);

        var dtos = items.Select(s => new SellerDtos(
            s.Id,
            s.Name
            // , s.Email
            // , s.Phone
            // , s.Address
            // , s.Code
            // , s.GstNumber
        )).ToList();

        return Ok(new { items = dtos, totalCount = total, page, pageSize });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var seller = await _svc.GetByIdAsync(id, ct);
        return seller is null
            ? NotFound()
            : Ok(new SellerDtos(
                seller.Id,
                seller.Name
                // , seller.Email
                // , seller.Phone
                // , seller.Address
                // , seller.Code
                // , seller.GstNumber
            ));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SellerUpdateDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var updated = await _svc.UpdateAsync(id, dto, ct);
            if (updated is null) return NotFound();

            var result = new SellerDtos(
                updated.Id,
                updated.Name
                // , updated.Email
                // , updated.Phone
                // , updated.Address
                // , updated.Code
                // , updated.GstNumber
            );

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] bool soft = false, CancellationToken ct = default)
    {
        var ok = await _svc.DeleteAsync(id, soft, ct);
        return ok ? NoContent() : NotFound();
    }
}
