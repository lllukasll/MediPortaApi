using MediPortaApi.Entities;
using MediPortaApi.Services.Interfaces;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using MediPortaApi.Models;
using Azure.Core;
using MediPortaApi.Repositories.Interfaces;

namespace MediPortaApi.Services
{
    public class StackOverflowAPIService : IStackOverflowAPIService
    {
        private readonly ILogger<StackOverflowAPIService> _logger;
        private readonly HttpClient _httpClient;

        public StackOverflowAPIService(HttpClient httpClient, ILogger<StackOverflowAPIService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            string baseURL = "https://api.stackexchange.com";
            int pageSize = 100;
            int pageCount = 10;
            string site = "stackoverflow";

            _httpClient.BaseAddress = new Uri(baseURL);

            List<Tag> Tags = new List<Tag>();
            double totalCount = 0;

            for (int i = 1; i <= pageCount; i++)
            {
                string url = $"2.3/tags?pagesize={pageSize}&page={i}&order=desc&sort=popular&site={site}";

                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully fetched tags from " + site + " | Page " + i + "/" + pageCount);

                    string json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<StackOverflowResponse>(json);

                    if(result != null && result.Items.Count > 0)
                    {
                        foreach (var item in result.Items)
                        {
                            string tagName = item.Name;
                            Tags.Add(new Tag
                            {
                                Name = item.Name,
                                Count = item.Count
                            });

                            totalCount = totalCount + item.Count;
                        }
                    }

                    foreach (var tag in Tags)
                    {
                        tag.Percentage = (tag.Count / totalCount) * 100;
                    }
                }
                else
                {
                    _logger.LogError("An error occurred while trying to Fetch tags from " + site + " | Page " + i + "/" + pageCount);
                }
            }

            return Tags;
        }
    }
}
