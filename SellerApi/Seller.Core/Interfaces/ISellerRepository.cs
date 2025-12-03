using Seller.Core.Entities;
 
namespace Seller.Core.Interfaces;
 
public interface ISellerRepository
{
    Task<Seller?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Seller?> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task<(IReadOnlyList<Seller> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, bool desc, CancellationToken ct = default);
    Task AddAsync(Seller seller, CancellationToken ct = default);
    Task UpdateAsync(Seller seller, CancellationToken ct = default);
    Task DeleteAsync(Seller seller, bool softDelete = false, CancellationToken ct = default);
}