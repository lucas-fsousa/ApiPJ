using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Methods {
  public class FilterValidState: ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
      if(!context.ModelState.IsValid) {
        var isValid = new IsValidFieldsOutput(context.ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage));
        context.Result = new BadRequestObjectResult(isValid);
      }
    }
  }

  internal class IsValidFieldsOutput {
    public IEnumerable<string> Errors { get; private set; }

    public IsValidFieldsOutput(IEnumerable<string> errors) {
      this.Errors = errors;
    }
  }
}
