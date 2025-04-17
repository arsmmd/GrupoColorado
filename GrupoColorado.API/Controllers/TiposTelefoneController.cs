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
  public class TiposTelefoneController : ControllerBase
  {
    private readonly ITipoTelefoneService _service;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TiposTelefoneController> _logger;

    public TiposTelefoneController(ITipoTelefoneService service, IUserContext userContext, IMapper mapper, ILogger<TiposTelefoneController> logger)
    {
      _service = service;
      _userContext = userContext;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      DefaultResponse<IEnumerable<TipoTelefone>> defaultResponseDto = new();

      try
      {
        GrupoColorado.Business.Shared.PagedResults<TipoTelefone> results = await _service.GetPagedAsync(queryParameters);
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

    [HttpGet("{codigoTipoTelefone:int}")]
    public async Task<IActionResult> GetByPkAsync(int codigoTipoTelefone)
    {
      DefaultResponse<TipoTelefoneDto> defaultResponseDto = new();

      try
      {
        TipoTelefone tipoTelefone = await _service.GetByPkAsync(codigoTipoTelefone);
        if (tipoTelefone != null)
        {
          defaultResponseDto.Data = _mapper.Map<TipoTelefoneDto>(tipoTelefone);
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
    public async Task<ActionResult<TipoTelefoneDto>> InsertAsync([FromBody] TipoTelefoneDto tipoTelefoneDto)
    {
      DefaultResponse<TipoTelefoneDto> defaultResponseDto = new();

      try
      {
        tipoTelefoneDto.UsuarioInsercao = _userContext.GetNameIdentifierAsInt();
        TipoTelefone tipoTelefone = _mapper.Map<TipoTelefone>(tipoTelefoneDto);
        await _service.AddAsync(tipoTelefone);
        tipoTelefone = await _service.GetByPkAsync(tipoTelefone.CodigoTipoTelefone);

        defaultResponseDto.Data = _mapper.Map<TipoTelefoneDto>(tipoTelefone);
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

    [HttpPut("{codigoTipoTelefone:int}")]
    public async Task<IActionResult> UpdateAsync(int codigoTipoTelefone, [FromBody] TipoTelefoneDto tipoTelefoneDto)
    {
      DefaultResponse<TipoTelefoneDto> defaultResponseDto = new();

      try
      {
        TipoTelefone tipoTelefone = await _service.GetByPkAsync(codigoTipoTelefone);
        if (tipoTelefone != null)
        {
          tipoTelefoneDto.CodigoTipoTelefone = codigoTipoTelefone;
          tipoTelefone = _mapper.Map<TipoTelefone>(tipoTelefoneDto);
          await _service.UpdateAsync(tipoTelefone);
          tipoTelefone = await _service.GetByPkAsync(codigoTipoTelefone);

          defaultResponseDto.Data = _mapper.Map<TipoTelefoneDto>(tipoTelefone);
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

    [HttpDelete("{codigoTipoTelefone:int}")]
    public async Task<IActionResult> DeleteAsync(int codigoTipoTelefone)
    {
      DefaultResponse<TipoTelefoneDto> defaultResponseDto = new();

      try
      {
        TipoTelefone tipoTelefone = await _service.GetByPkAsync(codigoTipoTelefone);
        if (tipoTelefone != null)
        {
          await _service.DeleteAsync(tipoTelefone);
          defaultResponseDto.Data = _mapper.Map<TipoTelefoneDto>(tipoTelefone);
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