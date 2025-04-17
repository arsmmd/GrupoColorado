using AutoMapper;
using GrupoColorado.API.DTOs;
using GrupoColorado.Business.Entities;

namespace GrupoColorado.API.Mappings
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<UsuarioDto, Usuario>().ReverseMap();
      CreateMap<ClienteDto, Cliente>().ReverseMap();
      CreateMap<TipoTelefoneDto, TipoTelefone>().ReverseMap();
      CreateMap<TelefoneDto, Telefone>().ReverseMap();
    }
  }
}