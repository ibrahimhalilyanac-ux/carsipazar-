using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDetailViewModel> GetUserInfo()
        {
            return await _httpClient.GetFromJsonAsync<UserDetailViewModel>("/api/users/getuser");
        }

        public async Task<bool> UpdateUserAsync(UserDetailViewModel model)
        {
            if (model.BirthYear.HasValue && model.BirthMonth.HasValue && model.BirthDay.HasValue)
            {
                try
                {
                    model.DateOfBirth = new DateTime(model.BirthYear.Value, model.BirthMonth.Value, model.BirthDay.Value);
                }
                catch
                {
                    // Invalid date provided, ignore
                }
            }

            var response = await _httpClient.PutAsJsonAsync("/api/users/updateuser", model);
            return response.IsSuccessStatusCode;
        }
    }
}
