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
    [Range(0, 1000, ErrorMessage = "Enter weight number between 0 to 1000")]
    public float Weight { get; set; }
    
    [Required]
    [Range(0, 1000, ErrorMessage = "Enter price number between 0 to 1000")]
    public float Price { get; set; }
    
    [Required]
    [ValueIsOneOf(new string[] {"paper", "metal", "plastic", "wood"}, "Must be proper material! (paper, metal, plastic or wood)")]
    public string Material { get; set; }
    
    [Required]
    [ValueIsOneOf(new string[] {"clear", "red", "blue", "green"}, "Must be proper color! (clear, red, blue, green)")]
    public string Color { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}