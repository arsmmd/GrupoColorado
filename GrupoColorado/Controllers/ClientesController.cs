using GrupoColorado.DTOs;
using GrupoColorado.DTOs.Core;
using GrupoColorado.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrupoColorado.Controllers
{
  [Authorize]
  public class ClientesController : Controller
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientesController(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> DetailsAsync([FromQuery] int codigoCliente)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      if (codigoCliente == 0)
        return View(new ClienteDto());

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.GetAsync($"Clientes/{codigoCliente}");
      if (!response.IsSuccessStatusCode)
        return BadRequest();

      string json = await response.Content.ReadAsStringAsync();
      DefaultResponse<ClienteDto> result = json.Deserialize<DefaultResponse<ClienteDto>>();
      if (result.Count == 0)
        result.Data = new ClienteDto();

      return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ClienteDto usuario)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.PostAsync("Clientes", usuario.Serialize().CreateStringContent());
      string json = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
      {
        DefaultResponse result = json.Deserialize<DefaultResponse>();
        return BadRequest(result.Message);
      }
      else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
      {
        DefaultResponse<List<ValidationError>> result = json.Deserialize<DefaultResponse<List<ValidationError>>>();
        return BadRequest(ValidationError.FormatOutput(result.Data));
      }
      else
      {
        DefaultResponse<ClienteDto> result = json.Deserialize<DefaultResponse<ClienteDto>>();
        return Json(result);
      }
    }

    [HttpPost]
    public async Task<IActionResult> ReadAsync([FromForm] DataTableRequest request)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);

      Dictionary<string, string> filters = request.Columns
        .Where(c => c.Searchable && !string.IsNullOrWhiteSpace(c.Data) && !string.IsNullOrWhiteSpace(c.Search.Value))
        .Select(c => new { c.Data, c.Search.Value })
        .ToDictionary(t => t.Data, t => t.Value);

      var queryParameters = new
      {
        Filters = filters,
        OrderBy = request.Columns[request.Order[0].Column].Data,
        OrderDescending = request.Order[0].Dir == "desc",
        Page = (request.Start / request.Length) + 1,
        PageSize = request.Length
      };

      HttpResponseMessage response = await client.GetAsync($"Clientes?{queryParameters.ToQueryString()}");
      if (!(response.IsSuccessStatusCode))
        return NoContent();

      string json = await response.Content.ReadAsStringAsync();
      DefaultResponse<List<ClienteDto>> result = json.Deserialize<DefaultResponse<List<ClienteDto>>>();
      return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByPkAsync([FromQuery] int codigoCliente)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.GetAsync($"Clientes/{codigoCliente}");
      string json = await response.Content.ReadAsStringAsync();
      DefaultResponse<ClienteDto> result = json.Deserialize<DefaultResponse<ClienteDto>>();
      return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ClienteDto usuario)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.PutAsync($"Clientes/{usuario.CodigoCliente}", usuario.Serialize().CreateStringContent());
      string json = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
      {
        DefaultResponse result = json.Deserialize<DefaultResponse>();
        return BadRequest(result.Message);
      }
      else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
      {
        DefaultResponse<List<ValidationError>> result = json.Deserialize<DefaultResponse<List<ValidationError>>>();
        return BadRequest(ValidationError.FormatOutput(result.Data));
      }
      else
      {
        DefaultResponse<ClienteDto> result = json.Deserialize<DefaultResponse<ClienteDto>>();
        return Json(result);
      }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] int codigoCliente)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);

      try
      {
        HttpResponseMessage response = await client.DeleteAsync($"Clientes/{codigoCliente}");
        if (response.IsSuccessStatusCode)
          return Ok();

        string json = await response.Content.ReadAsStringAsync();
        DefaultResponse defaultResponse = json.Deserialize<DefaultResponse>();

        return BadRequest(defaultResponse.Message);
      }
      catch
      {
        return StatusCode(500, "Erro interno.");
      }
    }
  }
}