
using infrastructure.DataModels;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class BoxService
{
    private readonly BoxRepository _boxRepository;
    
    public BoxService(BoxRepository boxRepository)
    {
        _boxRepository = boxRepository;
    }

    public IEnumerable<InStockBoxes> GetInStockBoxes()
    {
        return _boxRepository.GetBoxStock();
    }
    public Box CreateBox(string size,float weight, float price, string material, string color, int quantity)
    {
        return _boxRepository.CreateBox(size, weight, price, material, color, quantity);
    }

    public Box UpdateBox(int id, string size, float weight, float price, string material, string color, int quantity)
    {
        return _boxRepository.UpdateBox(id, size, weight, price, material, color, quantity);
    }

    public void DeleteBox(int id)
    {
        var result = _boxRepository.DeleteBox(id);
        if (!result)
        {
            throw new AggregateException("Could not delete Box");
        }
    }

    public Box GetBoxById(int id)
    {
        return _boxRepository.getBoxById(id);
    }

    public IEnumerable<InStockBoxes> SearchBox(String searchterm)
    {
        try
        {
            return _boxRepository.SearchBox(searchterm);
        }
        catch (Exception)
        {
            throw new Exception("Could not search for boxes");
        }
    }
}
