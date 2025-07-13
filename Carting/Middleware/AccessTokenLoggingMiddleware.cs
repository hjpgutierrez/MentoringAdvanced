namespace Carting.Middleware
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class AccessTokenLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AccessTokenLoggingMiddleware> _logger;

        public AccessTokenLoggingMiddleware(RequestDelegate next, ILogger<AccessTokenLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the Authorization header exists
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length);

                try
                {
                    // Parse the JWT token
                    var jwtHandler = new JwtSecurityTokenHandler();
                    if (jwtHandler.CanReadToken(token))
                    {
                        var jwtToken = jwtHandler.ReadJwtToken(token);

                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (var claim in jwtToken.Claims)
                        {
                            stringBuilder.Append($"Claim Type: {claim.Type}, Claim Value: {claim.Value} \n");
                        }

                        _logger.LogInformation(stringBuilder.ToString());
                    }
                    else
                    {
                        _logger.LogInformation("Invalid JWT token");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error parsing token: {ex.Message}");
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
