using AutoMapper;
using GrupoColorado.API.DTOs;
using GrupoColorado.API.DTOs.Core;
using GrupoColorado.API.Extensions;
using GrupoColorado.API.Helpers.Interfaces;
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoColorado.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/v1/[controller]")]
  public class ClientesController : ControllerBase
  {
    private readonly IClienteService _service;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteService service, IUserContext userContext, IMapper mapper, ILogger<ClientesController> logger)
    {
      _service = service;
      _userContext = userContext;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      DefaultResponse<IEnumerable<ClienteDto>> defaultResponseDto = new();

      try
      {
        GrupoColorado.Business.Shared.PagedResults<Cliente> results = await _service.GetPagedAsync(queryParameters);
        if (results.Count > 0)
          defaultResponseDto.Data = results.Items.Select(i => _mapper.Map<ClienteDto>(i)).ToList();

        defaultResponseDto.Count = results.Count;
        defaultResponseDto.ExitCode = 200;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);

        defaultResponseDto.ClearObjects();
        defaultResponseDto.Message = ex.Message;
        defaultResponseDto.ExitCode = 500;
      }

      return StatusCode(defaultResponseDto.ExitCode, defaultResponseDto);
    }

    [HttpGet("{codigoCliente:int}")]
    public async Task<IActionResult> GetByPkAsync(int codigoCliente)
    {
      DefaultResponse<ClienteDto> defaultResponseDto = new();

      try
      {
        Cliente cliente = await _service.GetByPkAsync(codigoCliente);
        if (cliente != null)
        {
          defaultResponseDto.Data = _mapper.Map<ClienteDto>(cliente);
          defaultResponseDto.Count = 1;
        }

        defaultResponseDto.ExitCode = 200;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);

        defaultResponseDto.ClearObjects();
        defaultResponseDto.Message = ex.Message;
        defaultResponseDto.ExitCode = 500;
      }

      return StatusCode(defaultResponseDto.ExitCode, defaultResponseDto);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> InsertAsync([FromBody] ClienteDto clienteDto)
    {
      DefaultResponse<ClienteDto> defaultResponseDto = new();

      try
      {
        clienteDto.UsuarioInsercao = _userContext.GetNameIdentifierAsInt();
        Cliente cliente = _mapper.Map<Cliente>(clienteDto);
        await _service.AddAsync(cliente);
        cliente = await _service.GetByPkAsync(cliente.CodigoCliente);

        defaultResponseDto.Data = _mapper.Map<ClienteDto>(cliente);
        defaultResponseDto.Count = 1;
        defaultResponseDto.ExitCode = 201;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);

        defaultResponseDto.ClearObjects();
        defaultResponseDto.Message = ex.Message;
        defaultResponseDto.ExitCode = 500;
      }

      return StatusCode(defaultResponseDto.ExitCode, defaultResponseDto);
    }

    [HttpPut("{codigoCliente:int}")]
    public async Task<IActionResult> UpdateAsync(int codigoCliente, [FromBody] ClienteDto clienteDto)
    {
      DefaultResponse<ClienteDto> defaultResponseDto = new();

      try
      {
        Cliente cliente = await _service.GetByPkAsync(codigoCliente);
        if (cliente != null)
        {
          clienteDto.CodigoCliente = codigoCliente;
          cliente = _mapper.Map<Cliente>(clienteDto);
          await _service.UpdateAsync(cliente);
          cliente = await _service.GetByPkAsync(codigoCliente);

          defaultResponseDto.Data = _mapper.Map<ClienteDto>(cliente);
          defaultResponseDto.Count = 1;
        }

        defaultResponseDto.ExitCode = 200;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);

        defaultResponseDto.ClearObjects();
        defaultResponseDto.Message = ex.Message;
        defaultResponseDto.ExitCode = 500;
      }

      return StatusCode(defaultResponseDto.ExitCode, defaultResponseDto);
    }

    [HttpDelete("{codigoCliente:int}")]
    public async Task<IActionResult> DeleteAsync(int codigoCliente)
    {
      DefaultResponse<ClienteDto> defaultResponseDto = new();

      try
      {
        Cliente cliente = await _service.GetByPkAsync(codigoCliente);
        if (cliente != null)
        {
          await _service.DeleteAsync(cliente);
          defaultResponseDto.Data = _mapper.Map<ClienteDto>(cliente);
          defaultResponseDto.Count = 1;
        }

        defaultResponseDto.ExitCode = 200;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);

        defaultResponseDto.ClearObjects();
        defaultResponseDto.Message = ex.Message;
        defaultResponseDto.ExitCode = 500;
      }

      return StatusCode(defaultResponseDto.ExitCode, defaultResponseDto);
    }
  }
}