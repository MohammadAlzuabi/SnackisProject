using SnackisProject.Models;
using System.Text.Json;

namespace SnackisProject.Gateways
{
    public class PostGateway
    {
        private readonly Uri _uri = new Uri("https://snackisapi1.azurewebsites.net/");

        public  async Task<List<Post>> GetAllPosts() // Vissa alla ämnen  
        {
            List<Post> posts = new List<Post>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;

                HttpResponseMessage responseMessage = await client.GetAsync("api/Posts");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    posts = JsonSerializer.Deserialize<List<Post>>(responseString);
                }
                return posts;

            }
        }

        public  async Task<List<Post>> GetAllPostsBySubcategoryId(string subcategoryId)
        {
            List<Post> post = new List<Post>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/Posts/subcategoryId/" + subcategoryId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    post = JsonSerializer.Deserialize<List<Post>>(responseString);
                }
                return post;
            }
        }


        public async Task<Post> GetPostById(string postId)
        {
            Post post = new Post();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/Posts/" + postId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    post = JsonSerializer.Deserialize<Post>(responseString);
                }
                return post;
            }
        }



        public async Task CreatePost(Post post)
        {
            if (post != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(post);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("api/Posts/", httpConten);
                }
            }
        }
        public async Task DeletePost(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage response = await client.DeleteAsync("api/Posts/" + id);
            }
        }
    }
}
