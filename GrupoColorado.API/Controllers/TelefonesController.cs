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
using System.Threading.Tasks;

namespace GrupoColorado.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/v1/[controller]")]
  public class TelefonesController : ControllerBase
  {
    private readonly ITelefoneService _service;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TelefonesController> _logger;

    public TelefonesController(ITelefoneService service, IUserContext userContext, IMapper mapper, ILogger<TelefonesController> logger)
    {
      _service = service;
      _userContext = userContext;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      DefaultResponse<IEnumerable<Telefone>> defaultResponseDto = new();

      try
      {
        GrupoColorado.Business.Shared.PagedResults<Telefone> results = await _service.GetPagedAsync(queryParameters);
        defaultResponseDto.Data = results.Items;
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

    [HttpGet("{codigoCliente:int}/{numeroTelefone}")]
    public async Task<IActionResult> GetByPkAsync(int codigoCliente, string numeroTelefone)
    {
      DefaultResponse<TelefoneDto> defaultResponseDto = new();

      try
      {
        Telefone telefone = await _service.GetByPkAsync(codigoCliente, numeroTelefone);
        if (telefone != null)
        {
          defaultResponseDto.Data = _mapper.Map<TelefoneDto>(telefone);
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
    public async Task<ActionResult<TelefoneDto>> InsertAsync([FromBody] TelefoneDto telefoneDto)
    {
      DefaultResponse<TelefoneDto> defaultResponseDto = new();

      try
      {
        telefoneDto.UsuarioInsercao = _userContext.GetNameIdentifierAsInt();
        Telefone telefone = _mapper.Map<Telefone>(telefoneDto);
        await _service.AddAsync(telefone);
        telefone = await _service.GetByPkAsync(telefone.CodigoCliente, telefone.NumeroTelefone);

        defaultResponseDto.Data = _mapper.Map<TelefoneDto>(telefone);
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

    [HttpPut("{codigoCliente:int}/{numeroTelefone}")]
    public async Task<IActionResult> UpdateAsync(int codigoCliente, string numeroTelefone, [FromBody] TelefoneDto telefoneDto)
    {
      DefaultResponse<TelefoneDto> defaultResponseDto = new();

      try
      {
        Telefone telefone = await _service.GetByPkAsync(codigoCliente, numeroTelefone);
        if (telefone != null)
        {
          telefoneDto.CodigoCliente = codigoCliente;
          telefoneDto.NumeroTelefone = numeroTelefone;
          telefone = _mapper.Map<Telefone>(telefoneDto);
          await _service.UpdateAsync(telefone);
          telefone = await _service.GetByPkAsync(codigoCliente, numeroTelefone);

          defaultResponseDto.Data = _mapper.Map<TelefoneDto>(telefone);
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

    [HttpDelete("{codigoCliente:int}/{numeroTelefone}")]
    public async Task<IActionResult> DeleteAsync(int codigoCliente, string numeroTelefone)
    {
      DefaultResponse<TelefoneDto> defaultResponseDto = new();

      try
      {
        Telefone telefone = await _service.GetByPkAsync(codigoCliente, numeroTelefone);
        if (telefone != null)
        {
          await _service.DeleteAsync(telefone);
          defaultResponseDto.Data = _mapper.Map<TelefoneDto>(telefone);
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