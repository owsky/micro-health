using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WorkoutService.Tests.Common;

public abstract class EndpointTestBase(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
  protected readonly TestWebApplicationFactory Factory = factory;

  protected static readonly JsonSerializerOptions JsonOptions = new()
  {
    Converters = { new JsonStringEnumConverter() },
    PropertyNameCaseInsensitive = true,
  };

  private static string MakeXUserinfoHeader(string username)
  {
    var json = JsonSerializer.Serialize(new { preferred_username = username });
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
  }

  protected HttpClient CreateAuthenticatedClient(string username = "alice")
  {
    var client = Factory.CreateClient();
    client.DefaultRequestHeaders.Add("X-Userinfo", MakeXUserinfoHeader(username));
    return client;
  }
}
