using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        Task CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto);
        Task<List<ResultOrderAddressDto>> GetAddressListByUserIdAsync(string id);
        Task UpdateOrderAddressAsync(UpdateOrderAddressDto updateOrderAddressDto);
        Task DeleteOrderAddressAsync(int id);
        Task<UpdateOrderAddressDto> GetOrderAddressByIdAsync(int id);
    }
}
