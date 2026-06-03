using MultiShop.DtoLayer.CommentDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;
        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCommentDto>("comments", createCommentDto);
        }
        public async Task DeleteCommentAsync(string id)
        {
            await _httpClient.DeleteAsync("comments?id=" + id);
        }
        public async Task<UpdateCommentDto> GetByIdCommentAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/" + id);
                if (!responseMessage.IsSuccessStatusCode) return new UpdateCommentDto();
                var values = await responseMessage.Content.ReadFromJsonAsync<UpdateCommentDto>();
                return values ?? new UpdateCommentDto();
            }
            catch
            {
                return new UpdateCommentDto();
            }
        }
        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments");
                if (!responseMessage.IsSuccessStatusCode) return new List<ResultCommentDto>();
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
                return values ?? new List<ResultCommentDto>();
            }
            catch
            {
                return new List<ResultCommentDto>();
            }
        }
        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCommentDto>("comments", updateCommentDto);
        }
        public async Task<List<ResultCommentDto>> CommentListByProductId(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"comments/CommentListByProductId/{id}");
                if (!responseMessage.IsSuccessStatusCode) return new List<ResultCommentDto>();
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
                return values ?? new List<ResultCommentDto>();
            }
            catch
            {
                return new List<ResultCommentDto>();
            }
        }

        public async Task<List<ResultCommentDto>> CommentListByEmail(string email)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"comments/CommentListByEmail?email={email}");
                if (!responseMessage.IsSuccessStatusCode) 
                {
                    var err = await responseMessage.Content.ReadAsStringAsync();
                    return new List<ResultCommentDto> { new ResultCommentDto { CommentDetail = "API Error: " + responseMessage.StatusCode + " - " + err, Rating = 1, CreatedDate = DateTime.Now, Status = true } };
                }
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
                return values ?? new List<ResultCommentDto>();
            }
            catch (Exception ex)
            {
                return new List<ResultCommentDto> { new ResultCommentDto { CommentDetail = "Exception: " + ex.Message, Rating = 1, CreatedDate = DateTime.Now, Status = true } };
            }
        }

        public async Task<int> GetTotalCommentCount()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/GetTotalCommentCount");
                if (!responseMessage.IsSuccessStatusCode) return 0;
                var values = await responseMessage.Content.ReadFromJsonAsync<int>();
                return values;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetActiveCommentCount()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/GetActiveCommentCount");
                if (!responseMessage.IsSuccessStatusCode) return 0;
                var values = await responseMessage.Content.ReadFromJsonAsync<int>();
                return values;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetPAssiveCommentCount()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/GetPassiveCommentCount");
                if (!responseMessage.IsSuccessStatusCode) return 0;
                var values = await responseMessage.Content.ReadFromJsonAsync<int>();
                return values;
            }
            catch
            {
                return 0;
            }
        }
    }
}
