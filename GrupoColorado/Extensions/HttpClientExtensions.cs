using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GrupoColorado.Extensions
{
  public static class HttpClientExtensions
  {
    public static HttpClient CreateAuthenticatedClient(this IHttpClientFactory factory, HttpRequest request)
    {
      HttpClient apiClient = factory.CreateClient("Api");
      string token = request.Cookies["access_token"];

      if (!string.IsNullOrWhiteSpace(token))
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      return apiClient;
    }

    public static StringContent CreateStringContent(this string json)
    {
      return new StringContent(json, Encoding.UTF8, "application/json");
    }
  }
}