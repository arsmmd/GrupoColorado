using GrupoColorado.API.CrossCutting;
using GrupoColorado.API.Filters;
using GrupoColorado.API.Helpers;
using GrupoColorado.API.Helpers.Interfaces;
using GrupoColorado.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GrupoColorado.API
{
  public static class Program
  {
    public static async Task Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      // Autenticação
      string authKey = builder.Configuration["Jwt:Key"];
      string authAudience = builder.Configuration["Jwt:Audience"];
      string authIssuer = builder.Configuration["Jwt:Issuer"];

      builder.Services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidAudience = authAudience,
          ValidIssuer = authIssuer,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey))
        };
      });
      builder.Services.AddAuthorization();
      builder.Services.AddScoped<IUserContextHelper, UserContextHelper>();


      // Configuração do Mapper, para transformação das DTOs em Entities.
      builder.Services.AddAutoMapper(typeof(Program));

      // Configuração da conexão com o SQL Server
      builder.Services.AddDbContext<AppDbContext>(options =>
      {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
      });

      // Configuração dos Validadores, Serviços e Repositórios
      IoC.RegisterValidators(builder.Services);
      IoC.RegisterServices(builder.Services);
      IoC.RegisterRepositories(builder.Services);

      // Controllers com Filters do FluentValidation
      builder.Services.AddControllers(options =>
      {
        options.Filters.Add<ValidationFilter>();
      })
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
      });

      // CORS
      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      });

      // Swagger
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Grupo Colorado", Version = "v1" });
        c.SchemaFilter<HideDictionaryAdditionalProperties>();

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Description = "Insira o valor do JWT Token:"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }, Array.Empty<string>() } });
      });

      builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      WebApplication app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Grupo Colorado v1"));
      }

      app.UseHttpsRedirection();

      app.UseCors("AllowAll");

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllers();
      app.UseStaticFiles();

      await app.RunAsync();
    }
  }
}