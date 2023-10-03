using test;

namespace DefaultNamespace;

using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

public class SearchBox
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    [TestCase("Small")]
    [TestCase("medium")]
    [TestCase("Red")]
    [TestCase("Cardboard")]
    public async Task SuccessfullBoxSearch(string searchterm)
    {
        Helper.TriggerRebuild();
        var expected = new List<object>();

        for (var i = 1; i <= 10; i++)
        {
            var box = new Box()
            {
                Id = i,
                Size = i % 2 == 0 ? "Small" : "Medium",
                Weight = 5,
                Price = 2,
                Material = i % 2 == 0 ? "Cardboard" : "Plastic",
                Color = i % 2 == 0 ? "Red" : "Blue",
                Quantity = 1
            };
            expected.Add(box);

            var sql = $@"INSERT INTO box_factory.boxes (size, weight, price, material, color, quantity)
                    VALUES (@Size, @Weight, @Price, @Material, @Color, @Quantity);
                ";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, box);
            }
        }

        var url = $"http://localhost:5000/api/boxes?searchTerm={searchterm}";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        var content = await response.Content.ReadAsStringAsync();
        IEnumerable<Box> boxes;
        try
        {
            boxes = JsonConvert.DeserializeObject<IEnumerable<Box>>(content) ??
                    throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
           
        }
    }

    [Test]
    [TestCase("NonExistentSearch")]
    public async Task BoxSearchNoResults(string searchterm)
    {
        var expected = new List<object>();
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(
                $"http://localhost:5000/api/boxes?searchTerm={searchterm}");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            expected.Should().BeEmpty();
        }
    }
}