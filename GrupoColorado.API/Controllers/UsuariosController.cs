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
  public class UsuariosController : ControllerBase
  {
    private readonly IUsuarioService _service;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService service, IUserContext userContext, IMapper mapper, ILogger<UsuariosController> logger)
    {
      _service = service;
      _userContext = userContext;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      DefaultResponse<IEnumerable<UsuarioDto>> defaultResponseDto = new();

      try
      {
        GrupoColorado.Business.Shared.PagedResults<Usuario> results = await _service.GetPagedAsync(queryParameters);
        if (results.Count > 0)
          defaultResponseDto.Data = results.Items.Select(i => _mapper.Map<UsuarioDto>(i)).ToList();

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

    [HttpGet("{codigoUsuario:int}")]
    public async Task<IActionResult> GetByPkAsync(int codigoUsuario)
    {
      DefaultResponse<UsuarioDto> defaultResponseDto = new();

      try
      {
        Usuario usuario = await _service.GetByPkAsync(codigoUsuario);
        if (usuario != null)
        {
          defaultResponseDto.Data = _mapper.Map<UsuarioDto>(usuario);
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
    public async Task<ActionResult<UsuarioDto>> InsertAsync([FromBody] UsuarioDto usuarioDto)
    {
      DefaultResponse<UsuarioDto> defaultResponseDto = new();

      try
      {
        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
        await _service.AddAsync(usuario);
        usuario = await _service.GetByPkAsync(usuario.CodigoUsuario);

        defaultResponseDto.Data = _mapper.Map<UsuarioDto>(usuario);
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

    [HttpPut("{codigoUsuario:int}")]
    public async Task<IActionResult> UpdateAsync(int codigoUsuario, [FromBody] UsuarioDto usuarioDto)
    {
      DefaultResponse<UsuarioDto> defaultResponseDto = new();

      try
      {
        Usuario usuario = await _service.GetByPkAsync(codigoUsuario);
        if (usuario != null)
        {
          usuarioDto.CodigoUsuario = codigoUsuario;
          usuario = _mapper.Map<Usuario>(usuarioDto);
          await _service.UpdateAsync(usuario);
          usuario = await _service.GetByPkAsync(codigoUsuario);

          defaultResponseDto.Data = _mapper.Map<UsuarioDto>(usuario);
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

    [HttpDelete("{codigoUsuario:int}")]
    public async Task<IActionResult> DeleteAsync(int codigoUsuario)
    {
      DefaultResponse<UsuarioDto> defaultResponseDto = new();

      try
      {
        Usuario usuario = await _service.GetByPkAsync(codigoUsuario);
        if (usuario != null)
        {
          await _service.DeleteAsync(usuario, _userContext.GetNameIdentifierAsInt());
          defaultResponseDto.Data = _mapper.Map<UsuarioDto>(usuario);
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