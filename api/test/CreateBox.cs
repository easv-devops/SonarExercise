using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace test;

public class CreateBox
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }


    [Test]
    public async Task ShouldSuccessfullyCreateBox()
    {
        Helper.TriggerRebuild();
        var article = new Box()
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
            response = await _httpClient.PostAsJsonAsync(url, article);
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

    [TestCase("super big", 10, 10, "plastic", "green", 10 )]
    public async Task ShouldFailDueToDataValidation(string size, int weight, int price, string material, string color, int quantity)
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