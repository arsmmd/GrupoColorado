using FluentValidation;
using FluentValidation.Results;
using GrupoColorado.API.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoColorado.API.Filters
{
  public class ValidationFilter : IActionFilter
  {
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
      List<object> errors = new();

      foreach (object argument in context.ActionArguments.Values)
      {
        if (argument == null)
          continue;

        Type argType = argument.GetType();
        Type validatorType = typeof(IValidator<>).MakeGenericType(argType);
        IValidator validator = _serviceProvider.GetService(validatorType) as IValidator;
        if (validator == null)
          continue;

        ValidationResult validationResult = validator.Validate(new ValidationContext<object>(argument));
        if (validationResult.IsValid)
          continue;

        errors.AddRange(
          validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .Select(group => new
            {
              Property = group.Key,
              Messages = group.Select(g => g.ErrorMessage)
            })
        );
      }

      if (!errors.Any())
        return;

      context.Result = new BadRequestObjectResult(new DefaultResponse<object>()
      {
        Data = errors,
        ExitCode = 400,
        Message = "Ocorreram erros de validação.",
        Count = errors.Count
      });
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
  }
}
