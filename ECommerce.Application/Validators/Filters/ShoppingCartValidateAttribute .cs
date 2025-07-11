using ECommerce.Application.Common;
using ECommerce.Application.Dtos.ShoppingCart;
using ECommerce.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ECommerce_API.Filters
{
    public class ShoppingCartValidateAttribute : Attribute, IActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartValidateAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var dto = context.ActionArguments.Values
            .FirstOrDefault(v => v is ShoppingCartCreateDTO) as ShoppingCartCreateDTO;

            if (dto == null)
            {
                context.Result = new BadRequestObjectResult(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Invalid item data" }
                });
                return;
            }

            var errors = new List<string>();
            

            if (dto.Count < 0)
                errors.Add("Count must be a positive value");

            
            var product = _unitOfWork.Product.GetAsync(u => u.Id == dto.ProductId).Result;
            if (product is null)
                errors.Add("Product doesn't exist");

            if (errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = errors
                });
            }

        }
    }
}
