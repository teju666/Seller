
using Seller.Core.Entities;
using Seller.Core.Interfaces;
using Seller.Api.Models;

namespace Seller.Api.Services;

public interface ISellerService
{
    Task<Seller> CreateAsync(SellerCreateDto dto, CancellationToken ct);
    Task<(IReadOnlyList<Seller> Items, int Total)> ListAsync(
        int page, int pageSize, string? search, string? sortBy, bool desc, CancellationToken ct);
    Task<Seller?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Seller?> UpdateAsync(Guid id, SellerUpdateDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, bool softDelete, CancellationToken ct);
}

public class SellerService : ISellerService
{
    private readonly ISellerRepository _repo;
    private readonly ILogger<SellerService> _log;

    public SellerService(ISellerRepository repo, ILogger<SellerService> log)
    {
        _repo = repo;
        _log = log;
    }

    public async Task<Seller> CreateAsync(SellerCreateDto dto, CancellationToken ct)
    {
        // TODO: If your Seller has a unique field (e.g., Email, Code, GstNumber), 
        // uncomment and adapt one of the checks below based on your repository and DTO.
        //
        // if (!string.IsNullOrWhiteSpace(dto.Email) &&
        //     await _repo.GetByEmailAsync(dto.Email.Trim(), ct) is not null)
        //     throw new InvalidOperationException($"Email '{dto.Email}' already exists.");
        //
        // if (!string.IsNullOrWhiteSpace(dto.Code) &&
        //     await _repo.GetByCodeAsync(dto.Code.Trim(), ct) is not null)
        //     throw new InvalidOperationException($"Code '{dto.Code}' already exists.");

        var seller = new Seller
        {
            Name = dto.Name.Trim(),
            // Map the rest according to your Seller entity & DTOs:
            // Email = dto.Email?.Trim(),
            // Phone = dto.Phone?.Trim(),
            // Address = dto.Address?.Trim(),
            // Code = dto.Code?.Trim(),
            // GstNumber = dto.GstNumber?.Trim(),
            // etc...
        };

        await _repo.AddAsync(seller, ct);
        _log.LogInformation("Seller created with Id {Id}", seller.Id);
        return seller;
    }

    public Task<(IReadOnlyList<Seller> Items, int Total)> ListAsync(
        int page, int pageSize, string? search, string? sortBy, bool desc, CancellationToken ct)
        => _repo.GetPagedAsync(page, pageSize, search, sortBy, desc, ct);

    public Task<Seller?> GetByIdAsync(Guid id, CancellationToken ct)
        => _repo.GetByIdAsync(id, ct);

    public async Task<Seller?> UpdateAsync(Guid id, SellerUpdateDto dto, CancellationToken ct)
    {
        var seller = await _repo.GetByIdAsync(id, ct);
        if (seller is null) return null;

        // TODO: If changing a unique field, add the appropriate uniqueness check:
        // if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email.Trim() != seller.Email)
        // {
        //     if (await _repo.GetByEmailAsync(dto.Email.Trim(), ct) is not null)
        //         throw new InvalidOperationException($"Email '{dto.Email}' already exists.");
