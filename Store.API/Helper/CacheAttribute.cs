using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Services.Services.CacheService;
using System.Text;

namespace Store.API.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSeconds;

        public CacheAttribute(int _timeToLiveInSeconds)
        {
            timeToLiveInSeconds = _timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cacheService.GetCacheAsyn(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult response)
                await cacheService.SetCacheAsyn(cacheKey, response.Value, TimeSpan.FromSeconds(timeToLiveInSeconds));

        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                cacheKey.Append($"|{key}-{value}");
            return cacheKey.ToString();
        }
    }
}
