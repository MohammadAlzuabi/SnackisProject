using SnackisProject.Models;
using System.Text.Json;

namespace SnackisProject.Gateways
{
    public class MessageGateway
    {
        public  Uri _uri = new Uri("https://snackisapi1.azurewebsites.net/");

        public async Task<List<Message>> GetAllMessagesByUserId(string userId) //  /Messages/UserId/
        {
            List<Message> messages = new List<Message>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/Messages/UserId/" + userId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    messages = JsonSerializer.Deserialize<List<Message>>(responseString);
                }
                return messages;
            }
        }

        public  async Task CreateMessage(Message message)
        {
            if (message != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(message);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("api/Messages/", httpConten);
                }
            }
        }
    }
}
