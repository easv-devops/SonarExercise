using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace test;

public class DeleteBoxEntry
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }

    [Test]
    public async Task GetBoxTest()
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
}