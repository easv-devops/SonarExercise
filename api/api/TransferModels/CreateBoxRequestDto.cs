using System.ComponentModel.DataAnnotations;
using api.CustomDataAnnotations;

namespace api.TransferModels;

public class CreateBoxRequestDto
{
    [ValueIsOneOf(new string[] {"small", "medium"}, "Must be proper size!")]
    public string Size { get; set; }
    
    public int Weight { get; set; }
    public int Price { get; set; }
    
    [MinLength(5)]
    public string Material { get; set; }
    
    public string Color { get; set; }
    public int Quantity { get; set; }
    
  
    

}