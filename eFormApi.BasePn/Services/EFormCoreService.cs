using System;
using System.Net;
using eFormCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;
using Rebus.Bus;

namespace Microting.eFormApi.BasePn.Services
{
  public class EFormCoreService : IEFormCoreService
    {
        private readonly IOptions<ConnectionStringsSdk> _connectionStrings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EFormCoreService> _logger;
        public IBus Bus { get; set; }

        public EFormCoreService(IOptions<ConnectionStringsSdk> connectionStrings,
            ILogger<EFormCoreService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _connectionStrings = connectionStrings;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Core GetCore()
        {
            string connectionString = _connectionStrings.Value.SdkConnection;

            // Update values
            if (string.IsNullOrEmpty(connectionString))
            {
                _httpContextAccessor.HttpContext.Response.OnStarting(async () =>
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    await _httpContextAccessor.HttpContext.Response.Body.FlushAsync();
                });
            }

            string connectionStr = _connectionStrings.Value.SdkConnection;

            Core coreInstance = CoreSingleton.GetCoreInstance(connectionStr, _logger);

            return coreInstance;
        }
    }
}
