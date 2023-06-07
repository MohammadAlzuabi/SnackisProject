using Microsoft.Extensions.Hosting;
using SnackisProject.Models;
using System;
using System.Text.Json;

namespace SnackisProject.Gateways
{
    public class CommentGateway
    {
        private readonly Uri _uri = new Uri("https://snackisapi1.azurewebsites.net/");
        public  async Task<List<Comment>> GetCommentByPostId(string postId) //"/Posts/subcategoryId/"
        {
            List<Comment> comments = new List<Comment>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/Comments/PostId/" + postId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    comments = JsonSerializer.Deserialize<List<Comment>>(responseString);
                }
                return comments;
            }
        }


        public async Task CreateComment(Comment comment)
        {
            if (comment != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(comment);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("api/Comments/", httpConten);
                }
            }
        }

    }
}
