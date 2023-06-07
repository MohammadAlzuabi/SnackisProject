using SnackisProject.Models;
using System.Text.Json;

namespace SnackisProject.Gateways
{
    public class CategoryGateway
    {
        private  Uri _uri = new Uri("https://snackisapi1.azurewebsites.net/");


        public  async Task<List<Category>> GetAllCategories() // Vissa alla ämnen  
        {
            List<Category> categories = new List<Category>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;

                HttpResponseMessage responseMessage = await client.GetAsync("api/Categories");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    categories = JsonSerializer.Deserialize<List<Category>>(responseString);
                }
                return categories;

            }
        }



        //CreateCategory
        public  async Task CreateCategory(Category category)
        {
            if (category != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(category);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("api/Categories/", httpConten);
                }
            }

        }
        public async Task EditCategory(Category category)
        {
            var categor = (await GetAllCategories()).Where(p => p.Id == category.Id).FirstOrDefault();
            if (categor != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(category);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PutAsync("api/Categories/" + categor.Id, httpConten);
                }
            }

        }



        public  async Task<Category> GetCategoryById(string categoryId)
        {
            Category category = new Category();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/Categories/" + categoryId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    category = JsonSerializer.Deserialize<Category>(responseString);
                }
                return category;
            }
        }

        // DeleteCategory
        public  async Task DeleteCategory(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage response = await client.DeleteAsync("api/Categories/" + id);
            }
        }
    }
}
