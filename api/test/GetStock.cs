using Microsoft.Playwright.NUnit;

namespace test;

using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

public class GetStock : PageTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    public async Task GetStockTest()
    {
        Helper.TriggerRebuild();
        var expected = new List<object>();
        for (var i = 1; i < 10; i++)
        {
            var box = new Box()
            {
                Id = i,
                Size = "small",
                Weight = 1,
                Price = 1,
                Material = "plastic",
                Color = "green",
                Quantity = 1,
            };
            expected.Add(box);
            var sql = $@"
            insert into box_factory.boxes (size, weight, price, material, color, quantity) VALUES(@size, @weight,
                @price, @material, @color, @quantity)";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, box);
            }
        }

        var url = "http://localhost:5000/api/stock";
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

        IEnumerable<InStockBoxes> boxes;
        try
        {
            boxes = JsonConvert.DeserializeObject<IEnumerable<InStockBoxes>>(
                        await response.Content.ReadAsStringAsync()) ??
                    throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            foreach (var box in boxes)
            {
                box.Id.Should().BeGreaterThan(0);
            }
        }
    }

    // Checks that each box added will be visible in stock
    [Test]
    public async Task StockUiCheck()
    {
        Helper.TriggerRebuild();
        var expected = new List<object>();
        for (var i = 1; i < 10; i++)
        {
            var box = new Box()
            {
                Id = i,
                Size = "small",
                Weight = 1,
                Price = 1,
                Material = "plastic",
                Color = "green",
                Quantity = 1,
            };
            expected.Add(box);
            var sql = $@"
            insert into box_factory.boxes (size, weight, price, material, color, quantity) VALUES(@size, @weight,
                @price, @material, @color, @quantity)";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, box);
            }
            
            Page.SetDefaultTimeout(3000);
            await Page.GotoAsync(Helper.ClientAppBaseUrl);


            foreach (Box b in expected)
            {
                var card = Page.GetByTestId("card_" + b.Id);
                await Expect(card).ToBeVisibleAsync();
            }
        }
    }
}