using Asp.Versioning;
using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Dtos.ShoppingCart;
using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
using ECommerce_API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ECommerce_API.Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Company},{SD.Role_Customer}")]
    [ApiVersion("1")]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CartController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._response = new();
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
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
                var cart = await _unitOfWork.ShoppingCart.GetAsync
                    (u => u.ApplicationUserId == UserId, includeProperties: "ShoppingCartItems");
                _response.Result = _mapper.Map<IEnumerable<ShoppingCartDTO>>(cart.ShoppingCartItems);
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
                var CartFromDB = await _unitOfWork.ShoppingCart.GetAsync(u => u.ApplicationUserId == UserId, false);
                if (CartFromDB != null)
                {
                    var cartItem = AddCartItem(ShoppingCartDTO, CartFromDB.Id);
                    CartFromDB.ShoppingCartItems.Add(cartItem);
                    await _unitOfWork.ShoppingCart.UpdateAsync(CartFromDB);
                }
                else
                {
                    var newCart = new ShoppingCart
                    {
                        ApplicationUserId = UserId,
                        CreatedOn = DateTime.Now,
                    };
                    await _unitOfWork.ShoppingCart.CreateAsync(newCart);
                    await _unitOfWork.SaveAsync();
                    var cartItem = AddCartItem(ShoppingCartDTO, newCart.Id);
                    newCart.ShoppingCartItems.Add(cartItem);
                }
                _response.StatusCode = HttpStatusCode.Created;
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
        public async Task<ActionResult<APIResponse>> RemoveItem(int CartItemId)
        {
            try
            {
                if (CartItemId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var UserId = User.FindFirst("uid")?.Value;
                var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.ApplicationUserId == UserId, false);
                if(cart == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var item = cart.ShoppingCartItems.Where(i => i.Id == CartItemId).FirstOrDefault();
                if (item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                cart.ShoppingCartItems.Remove(item);
                if (cart.ShoppingCartItems.Any())
                {
                    await _unitOfWork.ShoppingCart.UpdateAsync(cart);
                }
                else
                {
                    await _unitOfWork.ShoppingCart.RemoveAsync(cart);
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
        [HttpPut("plus/{ProductId:int}", Name = "Plus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> Plus(int CartItemId)
        {
            try
            {
                if (CartItemId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var UserId = User.FindFirst("uid")?.Value;
                var cart = await _unitOfWork.ShoppingCart.GetAsync(u =>  u.ApplicationUserId == UserId, false, includeProperties: "ShoppingCartItems");
                if (cart == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var item = cart.ShoppingCartItems.Where(i => i.Id == CartItemId).FirstOrDefault();
                if (item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                item.Quantity++;

                await _unitOfWork.ShoppingCart.UpdateAsync(cart);
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
        public async Task<ActionResult<APIResponse>> Minus(int CartItemId)
        {
            try
            {
                if (CartItemId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var UserId = User.FindFirst("uid")?.Value;
                var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.ApplicationUserId == UserId, false, includeProperties: "ShoppingCartItems");
                if (cart == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var item = cart.ShoppingCartItems.Where(i => i.Id == CartItemId).FirstOrDefault();
                if (item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    await _unitOfWork.ShoppingCart.UpdateAsync(cart);
                }
                else
                {
                    cart.ShoppingCartItems.Clear();
                    await _unitOfWork.ShoppingCart.RemoveAsync(cart);
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
        private ShoppingCartItem AddCartItem(ShoppingCartCreateDTO itemDTO, int CartId)
        {
            return new ShoppingCartItem
            {
                ProductId = itemDTO.ProductId,
                ProductName = itemDTO.ProductName,
                Quantity = itemDTO.Quantity,
                UnitPrice = itemDTO.UnitPrice,
                ShoppingCartId = CartId,
                ImageUrl = _fileService.GetByUrls(itemDTO.ProductId, _webHostEnvironment.WebRootPath, "Products").FirstOrDefault()!
            };
        }
    }
}
