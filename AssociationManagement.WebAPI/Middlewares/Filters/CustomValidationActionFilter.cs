using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace AssociationManagement.WebAPI.Middlewares.Filters {
    public class CustomValidationActionFilter : IActionFilter {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context) {
            if(!context.ModelState.IsValid) {
                //var errors = context.ModelState.Values
                //.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                //.ToList();
                //throw new ValidationException(errors.First());
            }
        }
    }
}
