using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Dtos.ShoppingCart;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
using ECommerce_API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ECommerce_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.Role_Customer)]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public CartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetCartItems(int pageSize = 3, int pageNumber = 1)
        {
            try
            {
                var UserId = User.FindFirst("uid")?.Value;
                _response.Result = _mapper.Map<IEnumerable<ShoppingCartDTO>>
                    (await _unitOfWork.ShoppingCart.GetAllAsync(u => u.ApplicationUserId == UserId, pageSize:pageSize, pageNumber:pageNumber));
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ServiceFilter(typeof(ShoppingCartValidateAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> AddItem([FromBody] ShoppingCartCreateDTO ShoppingCartDTO)
        {
            try
            {
                var product = await _unitOfWork.Product.GetAsync(u => u.Id == ShoppingCartDTO.ProductId, false);
                var UserId = User.FindFirst("uid")?.Value;
                var CartFromDB = await _unitOfWork.ShoppingCart.GetAsync(u => u.ProductId == product.Id && u.ApplicationUserId == UserId, false);
                if (CartFromDB != null)
                {
                    CartFromDB.Count += ShoppingCartDTO.Count;
                    await _unitOfWork.ShoppingCart.UpdateAsync(CartFromDB);
                }
                else
                {
                    var shoppingCart = _mapper.Map<ShoppingCart>(ShoppingCartDTO);
                    shoppingCart.ApplicationUserId = UserId!;
                    await _unitOfWork.ShoppingCart.CreateAsync(shoppingCart);
                    _response.StatusCode = HttpStatusCode.Created;
                }
                await _unitOfWork.SaveAsync();                
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("{ProductId:int}", Name = "RemoveItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> RemoveItem(int ProductId)
        {
            try
            {
                if (ProductId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var UserId = User.FindFirst("uid")?.Value;
                var Item = await _unitOfWork.ShoppingCart.GetAsync(u => u.ProductId == ProductId && u.ApplicationUserId == UserId, false);
                if (Item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                await _unitOfWork.ShoppingCart.RemoveAsync(Item);
                await _unitOfWork.SaveAsync();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("plus/{ProductId:int}", Name = "Plus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> Plus(int ProductId)
        {
            try
            {
                if (ProductId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var UserId = User.FindFirst("uid")?.Value;
                var Item = await _unitOfWork.ShoppingCart.GetAsync(u => u.ProductId == ProductId && u.ApplicationUserId == UserId, false);
                if (Item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                Item.Count++;

                await _unitOfWork.ShoppingCart.UpdateAsync(Item);
                await _unitOfWork.SaveAsync();

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("minus/{ProductId:int}", Name = "Minus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> Minus(int ProductId)
        {
            try
            {
                if (ProductId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var UserId = User.FindFirst("uid")?.Value;
                var Item = await _unitOfWork.ShoppingCart.GetAsync(u => u.ProductId == ProductId && u.ApplicationUserId == UserId, false);
                if (Item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                if(Item.Count > 1)
                {
                    Item.Count--;
                    await _unitOfWork.ShoppingCart.UpdateAsync(Item);
                }
                else
                {
                    await RemoveItem(ProductId);
                }                
                await _unitOfWork.SaveAsync();

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
