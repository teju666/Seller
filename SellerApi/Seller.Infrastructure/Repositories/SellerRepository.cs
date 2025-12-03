using Seller.EntityFrameworkCore;
using Seller.Core.Entities;
using Seller.Core.Interfaces;
using Seller.Infrastructure.Data;
 
namespace Seller.Infrastructure.Repositories;
 
public class SellerRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public SellerRepository(AppDbContext db) => _db = db;
 
    public Task<Seller?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Seller.FirstOrDefaultAsync(p => p.Id == id, ct);
 
    public Task<Seller?> GetBySkuAsync(string sku, CancellationToken ct = default)
        => _db.Seller.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Sku == sku, ct);
 
    public async Task<(IReadOnlyList<Seller> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, bool desc, CancellationToken ct = default)
    {
        var q = _db.Seller.AsQueryable();
 
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));
 
        q = (sortBy?.ToLowerInvariant()) switch
        {
            "name" => desc ? q.OrderByDescending(p => p.Name) : q.OrderBy(p => p.Name),
            "price" => desc ? q.OrderByDescending(p => p.Price) : q.OrderBy(p => p.Price),
            "createdatutc" => desc ? q.OrderByDescending(p => p.CreatedAtUtc) : q.OrderBy(p => p.CreatedAtUtc),
            _ => q.OrderBy(p => p.Name)
        };
 
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }
 
    public async Task AddAsync(Seller seller, CancellationToken ct = default)
    {
        await _db.Seller.AddAsync(seller, ct);
        await _db.SaveChangesAsync(ct);
    }
 
    public async Task UpdateAsync(Seller seller, CancellationToken ct = default)
    {
        _db.Seller.Update(product);
        await _db.SaveChangesAsync(ct);
    }
 
    public async Task DeleteAsync(Seller Seller, bool softDelete = false, CancellationToken ct = default)
    {
        if (softDelete)
        {
            Seller.IsDeleted = true;
            _db.Seller.Update(Seller);
        }
        else
        {
            _db.Seller.Remove(Seller);
        }
        await _db.SaveChangesAsync(ct);
    }
}