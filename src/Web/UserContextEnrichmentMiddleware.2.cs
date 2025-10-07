#if NETCOREAPP2_0

using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web
{
    public sealed class UserContextEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserContextEnrichmentMiddleware> _logger;

        public UserContextEnrichmentMiddleware(RequestDelegate next, ILogger<UserContextEnrichmentMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context != null && context.User != null
                ? context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            if (userId != null)
            {
                Activity.Current?.SetTag("user.id", userId);

                var data = new Dictionary<string, object>();
                data["UserId"] = userId;

                using (_logger.BeginScope(data))
                {
                    await _next(context);
                }

                return;
            }

            await _next(context);
        }
    }
}

#endif
