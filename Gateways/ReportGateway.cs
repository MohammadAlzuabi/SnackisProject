using Microsoft.Extensions.Hosting;
using SnackisProject.Models;
using System.Text.Json;

namespace SnackisProject.Gateways
{
    public class ReportGateway
    {

        private readonly Uri _uri = new Uri("https://snackisapi1.azurewebsites.net/");

        public async Task<List<Report>> GetAllReports()
        {
            List<Report> categories = new List<Report>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;

                HttpResponseMessage responseMessage = await client.GetAsync("api/Reports");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    categories = JsonSerializer.Deserialize<List<Report>>(responseString);
                }
                return categories;

            }
        }

        public async Task CreateReport(Report report)
        {
            if (report != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = _uri;
                    var json = JsonSerializer.Serialize(report);
                    StringContent httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("api/Reports/", httpConten);
                }
            }
        }

        public async Task<Report> GetReportById(string reportId)
        {
            Report report = new Report();

            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage responseMessage = await client.GetAsync("api/Reports/" + reportId);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    report = JsonSerializer.Deserialize<Report>(responseString);
                }
                return report;
            }
        }

        public async Task DeleteReport(string reportId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _uri;
                HttpResponseMessage response = await client.DeleteAsync("api/Reports/" + reportId);
            }
        }
    }
}
