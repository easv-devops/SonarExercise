using infrastructure.DataModels;
using infrastructure.Repositories;

namespace service;

public class BoxService
{
    private readonly BoxRepository _boxRepository;
    
    public BoxService(BoxRepository boxRepository)
    {
        _boxRepository = boxRepository;
    }
    public Box CreateBox(string size,int weight, int price, string material, string color, int quantity)
    {
        return _boxRepository.CreateBox(size, weight, price, material, color, quantity);
    }
}
