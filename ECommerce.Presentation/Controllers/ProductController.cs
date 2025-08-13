using Asp.Versioning;
using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Dtos.Product;
using ECommerce.Application.Filters;
using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ECommerce_API.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._response = new();
            this._fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts(int? categoryId, int pageSize = 3, int pageNumber = 1)
        {
            var products = _mapper.Map<IEnumerable<ProductDTO>>(await _unitOfWork.Product
                .GetAllAsync(categoryId != null ? p => p.CategoryId == categoryId : null, includeProperties: "Category", pageSize: pageSize, pageNumber: pageNumber));

            //products.Where(p => p.Images is null).Select(p => p.Images = _fileService.GetByUrls("Products", p.Id));
            foreach (var product in products)
            {
                product.images = _fileService.GetByUrls(product.Id, _webHostEnvironment.WebRootPath, "Products");
            }
            _response.Result = products;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Product = await _unitOfWork.Product.GetAsync(u => u.Id == id, false, includeProperties: "Category");
            if (Product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var product = _mapper.Map<ProductDTO>(Product);
            product.images = _fileService.GetByUrls(product.Id, _webHostEnvironment.WebRootPath, "Products");
            _response.Result = product;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ServiceFilter(typeof(ProductValidateAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromForm] ProductCreateDTO productDTO)
        {
            var Product = _mapper.Map<Product>(productDTO);
            await _unitOfWork.Product.CreateAsync(Product);
            await _unitOfWork.SaveAsync();
            await _fileService.UploadAsync(Product.Id, productDTO.images!, _webHostEnvironment.WebRootPath, "Products");

            _response.Result = CreatedAtRoute("GetProduct", new { Id = Product.Id }, Product);
            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
        }
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [Authorize(Roles = SD.Role_Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Product = await _unitOfWork.Product.GetAsync(u => u.Id == id, false);
            if (Product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            await _unitOfWork.Product.RemoveAsync(Product);
            await _unitOfWork.SaveAsync();
            _fileService.DeleteAsync(Product.Id, _webHostEnvironment.WebRootPath, "Products");
            return Ok(_response);
        }
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [Authorize(Roles = SD.Role_Admin)]
        [ServiceFilter(typeof(ProductValidateAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] ProductUpdateDTO productDTO)
        {
            if (id == 0 || id != productDTO.Id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Product = await _unitOfWork.Product.GetAsync(u => u.Id == id, false);
            if (Product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            Product = _mapper.Map<Product>(productDTO);

            await _unitOfWork.Product.UpdateAsync(Product);
            await _unitOfWork.SaveAsync();

            return Ok(_response);
        }
    }
}