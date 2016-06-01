using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BoC.CmsHelpers.Abstraction;
using BoC.CmsHelpers.Sitecore.JsonConverters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BoC.CmsHelpers.Sitecore.Services
{
    public class SitecoreService : ISitecoreService
    {

        private readonly ILoggerFactory _logger;
        private readonly IOptions<ApiSettings> _apiSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Uri _baseAddress;

        public SitecoreService(ILoggerFactory logger, IOptions<ApiSettings> apiSettings, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiSettings = apiSettings;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            _baseAddress = new Uri(new Uri(apiSettings.Value.BaseUrl), apiSettings.Value.ScApiPath);
        }

        public virtual async Task<T> Get<T>(Guid id)
        {
            if (id == Guid.Empty)
                return default(T);
            return await Get<T>($"item/{id:B}");
        }

        public async Task<IEnumerable<T>> GetChildren<T>(Guid id)
        {
            if (id == Guid.Empty)
                return Enumerable.Empty<T>();
            return await Get<IEnumerable<T>>($"item/{id:B}/children");
        }
        public virtual async Task<T> Get<T>(string requestPath, Uri baseAddress = null)
        {
            try
            {
                var requestContext = _httpContextAccessor.HttpContext;
                var isInCmsMode = requestContext.IsInCmsMode();
                var cacheKey = baseAddress + requestPath + typeof(T).FullName + requestContext?.Request?.Cookies[_apiSettings.Value.SessionCookieName] + requestContext?.Request?.Cookies[_apiSettings.Value.AuthCookieName];
                T result;
                if (!isInCmsMode && _memoryCache.TryGetValue(cacheKey, out result))
                    return result;

                using (var httpClient = new HttpClient {BaseAddress = (baseAddress ?? _baseAddress)})
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    if (requestContext != null)
                    {
                        httpClient.DefaultRequestHeaders.Add("Cookie", (IEnumerable<string>) requestContext?.Request.Headers["Cookie"]);
                    }
                    using (var response = await httpClient.GetAsync(requestPath))
                    {
                        response.EnsureSuccessStatusCode();
                        if (requestContext != null && response.Headers.Contains("Set-Cookie"))
                        {
                            requestContext.Response.Headers["Set-Cookie"] = new StringValues(response.Headers.GetValues("Set-Cookie").ToArray());
                        }

                        var serializer = JsonSerializer.CreateDefault();
                        serializer.Converters.Add(new BoolConverter());
                        serializer.Converters.Add(new MultiListConverter());
                        serializer.Converters.Add(new ImageFieldConverter());
                        serializer.Converters.Add(new LinkFieldConverter());
                        using (var sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            result = serializer.Deserialize<T>(jsonTextReader);
                            return isInCmsMode ? result : _memoryCache.Set(cacheKey, result);
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                var log = _logger.CreateLogger("Error");
                log.LogError(exc.Message);
            }
            return default(T);
        }

    }
}
