using SnackisProject.Models;
using System.Text.Json;

namespace SnackisProject.Gateways
{
    public class SubCategoryGateway
    {
        private readonly Uri _uri = new Uri("https://snackisapi1.azurewebsites.net/");

        public async Task<List<SubCategory>> GetAllCategories() // Vissa alla ämnen  
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;

                HttpResponseMessage responseMessage = await client.GetAsync("api/SubCategories");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    subCategories = JsonSerializer.Deserialize<List<SubCategory>>(responseString);
                }
                return subCategories;

            }
        }


        public  async Task<SubCategory> GetSubCategoryById(string subCategoryId)
        {
            SubCategory subCategory = new SubCategory();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/SubCategories/" + subCategoryId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    subCategory = JsonSerializer.Deserialize<SubCategory>(responseString);
                }
                return subCategory;
            }
        }



        public  async Task CreateSubCategory(SubCategory subCategory)
        {
            var subCat = (await GetAllCategories()).Where(p => p.Id == subCategory.CategoryId).FirstOrDefault();

            if (subCat == null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(subCategory);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("api/SubCategories/", httpConten);
                }
            }

        }

        public  async Task DeleteSubCategory(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage response = await client.DeleteAsync("api/SubCategories/" + id);
            }
        }
    }
}
