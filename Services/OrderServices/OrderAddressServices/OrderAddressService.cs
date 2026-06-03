using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public class OrderAddressService : IOrderAddressService
    {
        private readonly HttpClient _httpClient;
        public OrderAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("addresses", createOrderAddressDto);
            }
            catch
            {
                // Hata durumunda uygulamanın çökmemesi için hatayı yutuyoruz.
            }
        }
        public async Task<List<ResultOrderAddressDto>> GetAddressListByUserIdAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("addresses");
                if (!responseMessage.IsSuccessStatusCode) return new List<ResultOrderAddressDto>();
                var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultOrderAddressDto>>();
                return values?.Where(x => x.UserId == id).ToList() ?? new List<ResultOrderAddressDto>();
            }
            catch
            {
                return new List<ResultOrderAddressDto>();
            }
        }

        public async Task UpdateOrderAddressAsync(UpdateOrderAddressDto updateOrderAddressDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("addresses", updateOrderAddressDto);
            }
            catch { }
        }

        public async Task DeleteOrderAddressAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"addresses?id={id}");
            }
            catch { }
        }

        public async Task<UpdateOrderAddressDto> GetOrderAddressByIdAsync(int id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"addresses/{id}");
                if (!responseMessage.IsSuccessStatusCode) return null;
                var value = await responseMessage.Content.ReadFromJsonAsync<UpdateOrderAddressDto>();
                return value;
            }
            catch
            {
                return null;
            }
        }
    }
}
