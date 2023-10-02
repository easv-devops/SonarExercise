using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace test;

public class UpdateBox
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    public async Task SuccessfullyUpdateBox()
    {
        Helper.TriggerRebuild();
        var box = new Box()
        {
            Id = 1,
            Size = "medium",
            Weight = 2.5f,
            Price = 10.99f,
            Material = "wood",
            Color = "red",
            Quantity = 50
        };
        var sql = $@"
        INSERT INTO box_factory.boxes (size, weight, price, material, color, quantity) 
        VALUES (@size, @weight, @price, @material, @color, @quantity);
    ";

        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, box);
        }

        var url = "http://localhost:5000/api/boxes/" + box.Id;

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync(url, box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        Box responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<Box>(
                await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            responseObject.Should().BeEquivalentTo(box, Helper.MyBecause(responseObject, box));
        }
    }


    [Test]
    [TestCase("invalid-size", -2.5f, 15.99f, "invalid-material", "invalid-color", -10)]
    [TestCase("small", 2.5f, -15.99f, "paper", "clear", 10)]
    [TestCase("medium", -2.5f, 15.99f, "metal", "blue", 0)]
    [TestCase("invalid-size", 2.5f, 15.99f, "metal", "blue", 0)]
    [TestCase("medium", -2.5f, 15.99f, "invalid-material", "blue", 0)]
    [TestCase("medium", -2.5f, 15.99f, "metal", "invalid-color", 0)]
    [TestCase("medium", -2.5f, 15.99f, "metal", "blue", -10)]
    public async Task UpdateBoxShouldFailDueToDataValidation(string size, float weight, float price, string material,
        string color, int quantity)
    {
        Helper.TriggerRebuild();
        var box = new Box()
        {
            Size = size,
            Weight = weight,
            Price = price,
            Material = material,
            Color = color,
            Quantity = quantity
        };
        
        var url = "http://localhost:5000/api/boxes/" + 1;
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync(url, box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}