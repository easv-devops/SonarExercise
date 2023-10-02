using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace test;

public class GetBoxInfo
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    public async Task GetBoxInfoTest()
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
           response = await _httpClient.GetAsync(url);
           TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
       }
       catch (Exception e)
       {
           throw new Exception(Helper.NoResponseMessage, e);
       }

       var content = await response.Content.ReadAsStringAsync();
       Box actual;
       try
       {
           actual = JsonConvert.DeserializeObject<Box>(content) ??
                    throw new InvalidOperationException();
       }
       catch (Exception e)
       {
           throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
       }

       using (new AssertionScope())
       {
           actual.Id.Should().BeGreaterThan(0);
           response.IsSuccessStatusCode.Should().BeTrue();
           actual.Material.Should().BeEquivalentTo(box.Material);
           actual.Color.Should().BeEquivalentTo(box.Color);
           actual.Quantity.Should().BeGreaterThan(0);
       }
    }
}