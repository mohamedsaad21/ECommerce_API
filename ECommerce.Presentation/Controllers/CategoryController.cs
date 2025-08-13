using Asp.Versioning;
using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Dtos.Category;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ECommerce_API.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategories(int pageSize = 3, int pageNumber = 1)
        {
            _response.Result = _mapper.Map<IEnumerable<CategoryDTO>>(await _unitOfWork.Category.GetAllAsync(pageSize:pageSize, pageNumber:pageNumber));
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Category = await _unitOfWork.Category.GetAsync(u => u.Id == id, false);
            if(Category == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            _response.Result = _mapper.Map<CategoryDTO>(Category);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO categoryDTO)
        {
            if(categoryDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Category = _mapper.Map<Category>(categoryDTO);
            await _unitOfWork.Category.CreateAsync(Category);
            await _unitOfWork.SaveAsync();

            _response.Result = CreatedAtRoute("GetCategory", new {Id = Category.Id}, Category);
            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
        }
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [Authorize(Roles = SD.Role_Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Category = await _unitOfWork.Category.GetAsync(u => u.Id == id, false);
            if (Category == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            await _unitOfWork.Category.RemoveAsync(Category);
            await _unitOfWork.SaveAsync();
            return Ok(_response);
        }
        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [Authorize(Roles = SD.Role_Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, [FromBody] CartUpdateDTO categoryDTO)
        {
            if(id == 0 ||  categoryDTO == null || id != categoryDTO.Id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var Category = await _unitOfWork.Category.GetAsync(u => u.Id == id, false);
            if (Category == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            Category = _mapper.Map<Category>(categoryDTO);
            await _unitOfWork.Category.UpdateAsync(Category);
            await _unitOfWork.SaveAsync();

            return Ok(_response);
        }
    }
}