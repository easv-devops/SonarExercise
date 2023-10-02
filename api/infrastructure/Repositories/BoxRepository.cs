using Dapper;
using infrastructure.DataModels;
using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class BoxRepository
{
    private NpgsqlDataSource _dataSource;
    
    public BoxRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }

    public IEnumerable<InStockBoxes> GetBoxStock()
    {
        string sql = $@"
            SELECT id as {nameof(InStockBoxes.Id)},
                weight as {nameof(InStockBoxes.Weight)},
                quantity as {nameof(InStockBoxes.Quantity)} FROM box_factory.boxes;
            ";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<InStockBoxes>(sql);
        }

    }
    
    public Box CreateBox(string size,float weight, float price, string material, string color, int quantity)
    {
        var sql = $@"
INSERT INTO box_factory.boxes (size, weight, price, material, color, quantity) 
VALUES (@size, @weight, @price, @material, @color, @quantity)
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

    public Box UpdateBox(int id, string size, float weight, float price, string material, string color, int quantity)
    {
        var sql = $@"
UPDATE box_factory.boxes SET size = @size, weight = @weight, price = @price, material = @material, color = @color, quantity = @quantity
WHERE id = @id
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
            return conn.QueryFirst<Box>(sql, new { id, size, weight, price, material, color, quantity });
        }
    }

    public bool DeleteBox(int id)
    {
        var sql = @"DELETE FROM box_factory.boxes WHERE id = @id";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { id }) == 1;
        }
    }

    public Box getBoxById(int id)
    {
        var sql = $@"SELECT id as {nameof(Box.Id)},
            size as {nameof(Box.Size)},
        weight as {nameof(Box.Weight)},
        price as {nameof(Box.Price)},
        material as {nameof(Box.Material)},
        color as {nameof(Box.Color)},
        quantity as {nameof(Box.Quantity)} FROM box_factory.boxes WHERE id = @id;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Box>(sql, new { id });
        }

    }
}