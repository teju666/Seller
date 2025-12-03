using System.ComponentModel.DataAnnotations;
 
namespace Seller.Api.Models;
 
public record SellerDto(Guid Id, string Name, string? Description, decimal Price, string Sku);
 
public class SellerCreateDto
{
    [Required, MinLength(3), MaxLength(100)]
    public string Name { get; set; } = default!;
    [MaxLength(500)]
    public string? Description { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    [Required, MinLength(3), MaxLength(30), RegularExpression(@"^[A-Za-z0-9\-]+$")]
    public string Sku { get; set; } = default!;
}
 
public class SellerUpdateDto
{
    [MinLength(3), MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
    [MinLength(3), MaxLength(30), RegularExpression(@"^[A-Za-z0-9\-]+$")]
    public string? Sku { get; set; }
}