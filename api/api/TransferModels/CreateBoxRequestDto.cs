using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using api.CustomDataAnnotations;

namespace api.TransferModels;

public class CreateBoxRequestDto
{
    [Required]
    [ValueIsOneOf(new string[] {"small", "medium", "big", "large"}, "Must be proper size! (small, medium, big or large)")]
    public string Size { get; set; }
    
    [Required]
    [RegularExpression(@"^[0-9]+\d*(\.\d{1,2})?$")]
    public float Weight { get; set; }
    
    [Required]
    [RegularExpression(@"^0|[1-9]+\d*(\.\d{1,2})?$")]
    public float Price { get; set; }
    
    [Required]
    [ValueIsOneOf(new string[] {"paper", "metal", "plastic", "wood"}, "Must be proper material! (paper, metal, plastic or wood)")]
    public string Material { get; set; }
    
    [Required]
    [ValueIsOneOf(new string[] {"clear", "red", "blue", "green"}, "Must be proper color! (clear, red, blue, green)")]
    public string Color { get; set; }
    
    [Required]
    [RegularExpression(@"^(0|[1-9][0-9]*)$")]
    public int Quantity { get; set; }
}