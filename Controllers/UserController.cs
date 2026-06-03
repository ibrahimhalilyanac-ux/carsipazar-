using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.CargoServices.CargoCustomerServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly ICommentService _commentService;
        private readonly IOrderAddressService _orderAddressService;

        public UserController(IUserService userService, IOrderOderingService orderOderingService, ICommentService commentService, IOrderAddressService orderAddressService)
        {
            _userService = userService;
            _orderOderingService = orderOderingService;
            _commentService = commentService;
            _orderAddressService = orderAddressService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var values = await _userService.GetUserInfo();
                return View(values);
            }
            catch
            {
                // If token is invalid or IdentityServer is unavailable, force a logout to clear broken session.
                return RedirectToAction("LogOut", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserDetailViewModel model)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(model);
                if (result)
                {
                    TempData["SuccessMessage"] = "Bilgileriniz başarıyla güncellendi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Bilgileriniz güncellenirken bir hata oluştu.";
                }
                
                var values = await _userService.GetUserInfo();
                return View(values);
            }
            catch
            {
                return RedirectToAction("LogOut", "Login");
            }
        }
        public async Task<IActionResult> MyOrders()
        {
            var values = await _userService.GetUserInfo();
            var orders = await _orderOderingService.GetOrderingByUserId(values.Id);
            return View(orders);
        }

        public async Task<IActionResult> MyReviews()
        {
            var values = await _userService.GetUserInfo();
            ViewBag.UserEmail = values.Email;
            var comments = await _commentService.CommentListByEmail(values.Email);
            return View(comments);
        }

        public async Task<IActionResult> MyAddress()
        {
            var values = await _userService.GetUserInfo();
            var addresses = await _orderAddressService.GetAddressListByUserIdAsync(values.Id);
            return View(addresses);
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(CreateOrderAddressDto createOrderAddressDto)
        {
            var values = await _userService.GetUserInfo();
            createOrderAddressDto.UserId = values.Id;
            createOrderAddressDto.Name = string.IsNullOrWhiteSpace(createOrderAddressDto.Name) ? (values.Name ?? "Müşteri") : createOrderAddressDto.Name;
            createOrderAddressDto.Surname = string.IsNullOrWhiteSpace(createOrderAddressDto.Surname) ? (values.Surname ?? "Soyadı") : createOrderAddressDto.Surname;
            createOrderAddressDto.Email = values.Email;
            
            // Prevent 400 Bad Request from API due to null strings
            createOrderAddressDto.Detail2 = createOrderAddressDto.Detail2 ?? "";
            createOrderAddressDto.ZipCode = createOrderAddressDto.ZipCode ?? "";
            createOrderAddressDto.Description = createOrderAddressDto.Description ?? "";
            createOrderAddressDto.Detail1 = createOrderAddressDto.Detail1 ?? "";
            createOrderAddressDto.District = createOrderAddressDto.District ?? "";
            createOrderAddressDto.City = createOrderAddressDto.City ?? "";
            createOrderAddressDto.Country = createOrderAddressDto.Country ?? "";
            createOrderAddressDto.Phone = createOrderAddressDto.Phone ?? "";

            await _orderAddressService.CreateOrderAddressAsync(createOrderAddressDto);
            return RedirectToAction("MyAddress");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAddress(int id)
        {
            var value = await _orderAddressService.GetOrderAddressByIdAsync(id);
            if (value == null) return RedirectToAction("MyAddress");
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress(UpdateOrderAddressDto updateOrderAddressDto)
        {
            var values = await _userService.GetUserInfo();
            updateOrderAddressDto.UserId = values.Id;
            updateOrderAddressDto.Email = values.Email;
            
            // Prevent 400 Bad Request from API due to null strings
            updateOrderAddressDto.Name = string.IsNullOrWhiteSpace(updateOrderAddressDto.Name) ? (values.Name ?? "Müşteri") : updateOrderAddressDto.Name;
            updateOrderAddressDto.Surname = string.IsNullOrWhiteSpace(updateOrderAddressDto.Surname) ? (values.Surname ?? "Soyadı") : updateOrderAddressDto.Surname;
            updateOrderAddressDto.Detail2 = updateOrderAddressDto.Detail2 ?? "";
            updateOrderAddressDto.ZipCode = updateOrderAddressDto.ZipCode ?? "";
            updateOrderAddressDto.Description = updateOrderAddressDto.Description ?? "";
            updateOrderAddressDto.Detail1 = updateOrderAddressDto.Detail1 ?? "";
            updateOrderAddressDto.District = updateOrderAddressDto.District ?? "";
            updateOrderAddressDto.City = updateOrderAddressDto.City ?? "";
            updateOrderAddressDto.Country = updateOrderAddressDto.Country ?? "";
            updateOrderAddressDto.Phone = updateOrderAddressDto.Phone ?? "";

            await _orderAddressService.UpdateOrderAddressAsync(updateOrderAddressDto);
            return RedirectToAction("MyAddress");
        }

        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _orderAddressService.DeleteOrderAddressAsync(id);
            return RedirectToAction("MyAddress");
        }

        public IActionResult MyPassword()
        {
            return View();
        }

        public IActionResult MyCards()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCard(string CardName, string CardNumber, string ExpiryDate)
        {
            var options = new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) };
            Response.Cookies.Append("MockCardName", string.IsNullOrWhiteSpace(CardName) ? "MÜŞTERİ" : CardName, options);
            Response.Cookies.Append("MockCardNumber", string.IsNullOrWhiteSpace(CardNumber) ? "**** **** **** ****" : CardNumber, options);
            Response.Cookies.Append("MockExpiryDate", string.IsNullOrWhiteSpace(ExpiryDate) ? "AA/YY" : ExpiryDate, options);

            return RedirectToAction("MyCards");
        }
    }
}
