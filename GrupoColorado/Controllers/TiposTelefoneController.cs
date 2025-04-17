using GrupoColorado.DTOs;
using GrupoColorado.DTOs.Core;
using GrupoColorado.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrupoColorado.Controllers
{
  [Authorize]
  public class TiposTelefoneController : Controller
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public TiposTelefoneController(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TipoTelefoneDto tipoTelefone)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.PostAsync("TiposTelefone", tipoTelefone.Serialize().CreateStringContent());
      string json = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
      {
        DefaultResponse<List<ValidationError>> result = json.Deserialize<DefaultResponse<List<ValidationError>>>();
        return BadRequest(ValidationError.FormatOutput(result.Data));
      }
      else
      {
        DefaultResponse<UsuarioDto> result = json.Deserialize<DefaultResponse<UsuarioDto>>();
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

      HttpResponseMessage response = await client.GetAsync($"TiposTelefone{queryParameters.ToQueryString()}");
      if (!(response.IsSuccessStatusCode))
        return NoContent();

      string json = await response.Content.ReadAsStringAsync();
      DefaultResponse<List<TipoTelefoneDto>> result = json.Deserialize<DefaultResponse<List<TipoTelefoneDto>>>();
      return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByPkAsync([FromQuery] int codigoTipoTelefone)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.GetAsync($"TiposTelefone/{codigoTipoTelefone}");
      string json = await response.Content.ReadAsStringAsync();
      DefaultResponse<TipoTelefoneDto> result = json.Deserialize<DefaultResponse<TipoTelefoneDto>>();
      return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] TipoTelefoneDto tipoTelefone)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);
      HttpResponseMessage response = await client.PutAsync($"TiposTelefone/{tipoTelefone.CodigoTipoTelefone}", tipoTelefone.Serialize().CreateStringContent());
      string json = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
      {
        DefaultResponse<List<ValidationError>> result = json.Deserialize<DefaultResponse<List<ValidationError>>>();
        return BadRequest(ValidationError.FormatOutput(result.Data));
      }
      else
      {
        DefaultResponse<UsuarioDto> result = json.Deserialize<DefaultResponse<UsuarioDto>>();
        return Json(result);
      }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] int codigoTipoTelefone)
    {
      if (!ModelState.IsValid)
        return BadRequest();

      HttpClient client = _httpClientFactory.CreateAuthenticatedClient(base.Request);

      try
      {
        HttpResponseMessage response = await client.DeleteAsync($"TiposTelefone/{codigoTipoTelefone}");
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