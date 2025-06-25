using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Catalog.Application.Categories.Commands;
using Microsoft.Extensions.Configuration;

namespace Catalog.API.IntegrationTests
{
    public class CategoryControllerTests : IClassFixture<CatalogWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private string _controllerPath = "/api/Categories";
        private readonly Uri _authTokenEndpoint;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public CategoryControllerTests(CatalogWebApplicationFactory factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            _httpClient = factory.CreateClient();

            // Load configuration from appsettings.json
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<CategoryControllerTests>()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .Build();

            // Get the Auth0 token endpoint from appsettings.json
            _authTokenEndpoint = new Uri(_configuration["Auth0:TokenEndpoint"]!);

            // Fetch the access token and set the Authorization header
            _accessToken = GetAccessTokenAsync().GetAwaiter().GetResult();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }


        [Fact]
        public async Task GetCategoriesWithPagination_HappyPath()
        {
            // Act
            var response = await _httpClient.GetAsync($"{_controllerPath}?PageNumber=1&PageSize=10");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var pageIndex = tokenResponse.GetProperty("pageIndex").GetInt32()!;
            Assert.Equal(1, pageIndex);
        }

        [Fact]
        public async Task CreateCategory_HappyPath()
        {
            // Arrange       
            var command = new CreateCategoryCommand() { Name = "TV", Image = "www.miimage.com", ParentCategoryId = null };
            var json = JsonSerializer.Serialize(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // Act
            var response = await _httpClient.PostAsync($"{_controllerPath}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = int.Parse(responseBody);
            Assert.True(result > 0, $"Expected result to be greater than 0, but got {result}.");

        }

        private async Task<string> GetAccessTokenAsync()
        {
            using var authClient = new HttpClient();       

            // Prepare the request payload
            var requestBody = new
            {
                grant_type = "password",
                username = _configuration["Auth0:Username"],
                password = _configuration["Auth0:Password"],
                audience = _configuration["Auth0:TestAudience"],
                client_id = _configuration["Auth0:ClientId"],
                client_secret = _configuration["Auth0:ClientSecret"],
                scope = "openid profile offline_access"
            };

            using (var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"))
            {
                var response = await authClient.PostAsync(_authTokenEndpoint!, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch access token. Status code: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                return tokenResponse.GetProperty("access_token").GetString()!;
            }
        }
    }
}
