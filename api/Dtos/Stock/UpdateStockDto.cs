using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock;

public class UpdateStockDto
{
    // include only information needed from the user
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 over characters")]
    public string Symbol { get; set; } = String.Empty;
    
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 over characters")]
    public string CompanyName { get; set; } = String.Empty;
    
    [Required]
    [Range(1, 1000000000)]
    public decimal Purchase { get; set; }
    
    [Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    
    [Required]
    [MaxLength(20, ErrorMessage = "Industry cannot be over 20 characters")]
    public string Industry { get; set; } = String.Empty;
    
    [Range(1, 50000000000)]
    public long MarketCap { get; set; }
}