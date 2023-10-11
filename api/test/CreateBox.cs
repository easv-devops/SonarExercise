using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Newtonsoft.Json;
using NUnit.Framework;

namespace test;

[TestFixture]
public class CreateBox : PageTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }


    [TestCase("small", 10, 10, "wood", "green", 10)]
    public async Task BoxCanSuccessfullyBeCreatedFromUi(string size, float weight, float price, string material,
        string color, int quantity)
    {
        //ARRANGE
        Helper.TriggerRebuild();

        //ACT
        await Page.GotoAsync("http://localhost:4200/boxes");

        await Page.GetByTestId("createBox").GetByRole(AriaRole.Img).Nth(1).ClickAsync();

        await Page.GetByText("SizePick size").ClickAsync();

        await Page.GetByRole(AriaRole.Radio, new() { Name = "small" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

        await Page.GetByLabel("Weight of the box").ClickAsync();

        await Page.GetByLabel("Weight of the box").FillAsync("10");

        await Page.GetByLabel("Price of the box").ClickAsync();

        await Page.GetByLabel("Price of the box").FillAsync("10");

        await Page.GetByText("MaterialPick material").ClickAsync();

        await Page.GetByRole(AriaRole.Radio, new() { Name = "wood" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

        await Page.GetByText("ColorPick color").ClickAsync();

        await Page.GetByRole(AriaRole.Radio, new() { Name = "green" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

        await Page.GetByLabel("Quantity").ClickAsync();

        await Page.GetByLabel("Quantity").FillAsync("10");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Create New Box" }).ClickAsync();

        //ASSERT
        await Expect(Page.GetByTestId("card_" + 1)).ToBeVisibleAsync(); //Exists in UI after creation
        await using (var conn = await Helper.DataSource.OpenConnectionAsync())
        {
            var expected = new Box()
            {
                Id = 1,
                Size = size,
                Weight = weight,
                Price = price,
                Material = material,
                Color = color,
                Quantity = quantity
            }; //Article object from test case

            conn.QueryFirst<Box>("SELECT * FROM box_factory.boxes;").Should()
                .BeEquivalentTo(expected); //Should be equal to article found in DB
        }
    }

    [Test]
    public async Task ShouldSuccessfullyCreateBox()
    {
        Helper.TriggerRebuild();
        var box = new Box()
        {
            Size = "small",
            Weight = 10,
            Price = 100,
            Material = "wood",
            Color = "red",
            Quantity = 30
        };
        var url = "http://localhost:5000/api/boxes";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, box);
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
    }

    [TestCase("super big", 10, 10, "plastic", "green", 10)]
    [TestCase("small", -10, 10, "plastic", "green", 10)]
    [TestCase("small", 10, -5, "plastic", "green", 10)]
    [TestCase("small", 10, 5.5F, "wrong-material", "green", 10)]
    [TestCase("small", 10, 5.5F, "plastic", "wrong-color", 10)]
    [TestCase("small", 10, 5.5F, "plastic", "green", -10)]
    public async Task ShouldFailDueToDataValidation(string size, float weight, float price, string material,
        string color, int quantity)
    {
        var box = new Box()
        {
            Size = size,
            Weight = weight,
            Price = price,
            Material = material,
            Color = color,
            Quantity = quantity
        };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/boxes", box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        response.IsSuccessStatusCode.Should().BeFalse();
    }
}