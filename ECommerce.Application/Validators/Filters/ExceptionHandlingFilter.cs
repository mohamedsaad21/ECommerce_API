using ECommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ECommerce.Application.Validators.Filters
{
    public class ExceptionHandlingFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new APIResponse
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { context.Exception.ToString() }
            };
            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
