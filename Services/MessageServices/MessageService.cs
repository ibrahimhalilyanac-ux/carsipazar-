using MultiShop.DtoLayer.DiscountDtos;
using MultiShop.DtoLayer.MessageDtos;

namespace MultiShop.WebUI.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ResultInboxMessageDto>> GetInboxMessageAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("http://localhost:5000/services/Message/UserMessage/GetMessageInbox?id=" + id);
                if (!responseMessage.IsSuccessStatusCode) return new List<ResultInboxMessageDto>();
                var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultInboxMessageDto>>();
                return values ?? new List<ResultInboxMessageDto>();
            }
            catch
            {
                return new List<ResultInboxMessageDto>();
            }
        }

        public async Task<List<ResultSendboxMessageDto>> GetSendboxMessageAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("http://localhost:5000/services/Message/UserMessage/GetMessageSendbox?id=" + id);
                if (!responseMessage.IsSuccessStatusCode) return new List<ResultSendboxMessageDto>();
                var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultSendboxMessageDto>>();
                return values ?? new List<ResultSendboxMessageDto>();
            }
            catch
            {
                return new List<ResultSendboxMessageDto>();
            }
        }

        public async Task<int> GetTotalMessageCountByReceiverId(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("UserMessage/GetTotalMessageCountByReceiverId?id=" + id);
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
//http://localhost:7078/api/UserMessage/GetMessageSendbox?id=a