using Catalog.Application.Categories.Commands;
using System.Text;
using System.Text.Json;

namespace Catalog.API.IntegrationTests
{
    public class CategoryControllerTests: IClassFixture<CatalogWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private string _controllerPath = "/api/Categories";
        public CategoryControllerTests(CatalogWebApplicationFactory factory) {
            _httpClient = factory.CreateClient();
        }


        [Fact]
        public async Task GetCategoriesWithPagination_HappyPath()
        {
            // Act
            var response = await _httpClient.GetAsync($"{_controllerPath}?PageNumber=1&PageSize=10");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateCategory_HappyPath()
        {
            // Arrange       
            var command = new CreateCategoryCommand() { Name = "TV", Image="www.miimage.com", ParentCategoryId=null};
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

    }
}