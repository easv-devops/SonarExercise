using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure.Repositories;

public class BoxRepository
{
    private NpgsqlDataSource _dataSource;
    
    public BoxRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }
    
    public Box CreateBox(string size,int weight, int price, string material, string color, int quantity)
    {
        var sql = $@"
INSERT INTO box_factory.boxes (size, weight, price, material, color, quantity) 
VALUES (@size, @weight, @price, @material), @color, @quantity
RETURNING id as {nameof(Box.Id)},
       size as {nameof(Box.Size)},
        weight as {nameof(Box.Weight)},
        price as {nameof(Box.Price)},
        material as {nameof(Box.Material)},
        color as {nameof(Box.Color)},
        quantity as {nameof(Box.Quantity)};
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Box>(sql, new { size, weight, price, material, color, quantity });
        }
    }
}