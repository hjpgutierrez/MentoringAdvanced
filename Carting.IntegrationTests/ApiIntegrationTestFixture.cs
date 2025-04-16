using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carting.IntegrationTests
{
    public abstract class ApiIntegrationTestFixture
    {
        private WebAppFactory _factory = null!;

        protected HttpClient HttpClient = null!;

        [SetUp]
        public virtual void Setup()
        {
            _factory = new WebAppFactory();  
            HttpClient = HttpClientFactory.Create(_factory);
        }

        [TearDown]
        public virtual void TearDown()
        {
            //_todoDbContext.Database.EnsureDeleted();
            //_todoDbContext.Dispose();
            _factory.Dispose();
            HttpClient.Dispose();
        }
    }

    public static class HttpClientFactory
    {
        public static HttpClient Create(WebAppFactory factory)
        {
            var httpClient = factory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost/");

            return httpClient;
        }
    }
}
