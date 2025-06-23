using System.Text;

namespace Catalog.API.Helpers
{
    public static class UrlHelper
    {
        public static string GetUrl(HttpContext httpContext, params string[] extraValues) => $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{BuildExtraValues(extraValues)}";

        private static string BuildExtraValues(params string[] extraValues)
        {
            if (extraValues == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new();
            foreach (var item in extraValues)
            {
                stringBuilder.Append($"/{item}");
            }

            return stringBuilder.ToString();
        }
    }
}
