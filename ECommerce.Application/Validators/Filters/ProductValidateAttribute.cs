using ECommerce.Application.Common;
using ECommerce.Application.Dtos.Product;
using ECommerce.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ECommerce.Application.Filters
{
    public class ProductValidateAttribute : Attribute, IActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductValidateAttribute(IUnitOfWork unitOfWork)
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
            .FirstOrDefault(v => v is ProductCreateDTO || v is ProductUpdateDTO);

            if (dto == null)
            {
                context.Result = new BadRequestObjectResult(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Invalid product data" }
                });
                return;
            }

            var errors = new List<string>();

            int categoryId = 0;
            decimal price = 0;
            int stock = 0;

            if (dto is ProductCreateDTO createDTO)
            {
                categoryId = createDTO.CategoryId;
                price = createDTO.Price;
                stock = createDTO.stock;
            }
            else if (dto is ProductUpdateDTO updateDTO)
            {
                categoryId = updateDTO.CategoryId;
                price = updateDTO.Price;
                stock = updateDTO.stock;
            }

            if (price < 0)
                errors.Add("Price must be a positive value");

            if (stock < 0)
                errors.Add("Stock must be a positive value");

            var category = _unitOfWork.Category.GetAsync(u => u.Id == categoryId).Result;
            if (category is null)
                errors.Add("Category doesn't exist");

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
