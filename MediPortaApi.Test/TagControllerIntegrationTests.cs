using FluentAssertions;
using MediPortaApi.Entities;
using MediPortaApi.Models;
using MediPortaApi.Test.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediPortaApi.Test
{
    public class TagControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        public TagControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public async Task GetTags_WhenCalled_Returns1000Tags()
        {
            var response = await _client.GetAsync("/Tag/GetTags?sortBy=name&sortDesc=true");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<Tag>>(responseString);

            Assert.Equal(1000, result.Count);
        }

        [Fact]
        public async Task GetTags_WhenCalled_ReturnsError()
        {
            var response = await _client.GetAsync("/Tag/GetTags?sortBy=count&sortDesc=true");

            response.ToString().Equals(HttpStatusCode.BadRequest);
        }
    }
}
