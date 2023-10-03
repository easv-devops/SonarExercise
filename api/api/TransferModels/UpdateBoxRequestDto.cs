using System.ComponentModel.DataAnnotations;
using api.CustomDataAnnotations;

namespace api.TransferModels;

public class UpdateBoxRequestDto
{
    public int Id { get; set; }
    
    [Required]
    [ValueIsOneOf(new string[] {"small", "medium", "big", "large"}, "Must be proper size! (small, medium, big or large)")]
    public string Size { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public float Weight { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
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