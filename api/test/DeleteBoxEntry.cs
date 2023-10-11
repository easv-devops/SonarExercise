using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace test;

[TestFixture]
public class DeleteTests : PageTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    public async Task DeleteBoxFromHttpClient()
    {
        Helper.TriggerRebuild();

        var box = new Box()
        {
            Id = 1,
            Size = "small",
            Weight = 1,
            Price = 1,
            Material = "plastic",
            Color = "green",
            Quantity = 1,
        };

        var sql = $@"
            insert into box_factory.boxes (size, weight, price, material, color, quantity) VALUES(@size, @weight,
                @price, @material, @color, @quantity)";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, box);
        }

        var url = "http://localhost:5000/api/boxes/1";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.DeleteAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        using (new AssertionScope())
        {
            using (var conn = Helper.DataSource.OpenConnection())
            {
                (conn.ExecuteScalar<int>($"SELECT COUNT(*) FROM box_factory.boxes WHERE Id = 1;") == 0).Should()
                    .BeTrue();
            }

            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }


    [Test]
    public async Task DeleteBoxFromUI()
    {
        Helper.TriggerRebuild();
        


        var box = new Box()
        {
            Id = 1,
            Size = "small",
            Weight = 10,
            Price = 2,
            Material = "plastic",
            Color = "red",
            Quantity = 10,
        };

        var sql = $@"
            insert into box_factory.boxes (size, weight, price, material, color, quantity) VALUES(@size, @weight,
                @price, @material, @color, @quantity)";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, box);
            
        }

Page.SetDefaultTimeout(3000);
        await Page.GotoAsync(Helper.ClientAppBaseUrl);
        var card = Page.GetByTestId("card_" + box.Id);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        await Page.GotoAsync(Helper.ClientAppBaseUrl);
        await Expect(card).Not.ToBeVisibleAsync();
        await using (var conn = await Helper.DataSource.OpenConnectionAsync())

        {
            int count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM box_factory.boxes WHERE id=@BoxId;",
                new { BoxId = box.Id });
            count.Should().Be(0);
        }
    }
}