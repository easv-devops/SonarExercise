using System.ComponentModel.DataAnnotations;
using api.CustomDataAnnotations;

namespace api.TransferModels;

public class UpdateBoxRequestDto
{
    public int Id { get; set; }
    
    [ValueIsOneOf(new string[] {"small", "medium", "big", "large"}, "Must be proper size! (small, medium, big or large)")]
    public string Size { get; set; }
    
    public int Weight { get; set; }
    public int Price { get; set; }
    
    [MinLength(4)]
    public string Material { get; set; }
    
    [MinLength(3)]
    public string Color { get; set; }
    public int Quantity { get; set; }

}