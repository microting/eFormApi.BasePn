using System;
using System.Net;
using eFormCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers.WritableOptions;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;

namespace Microting.eFormApi.BasePn.Services
{
  public class EFormCoreService : IEFormCoreService
    {
        private readonly IWritableOptions<ConnectionStrings> _connectionStrings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Core _core;
        private readonly ILogger<EFormCoreService> _logger;

        public EFormCoreService(IWritableOptions<ConnectionStrings> connectionStrings,
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
            if (string.IsNullOrEmpty(connectionString))
            {
                _httpContextAccessor.HttpContext.Response.OnStarting(async () =>
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    await _httpContextAccessor.HttpContext.Response.Body.FlushAsync();
                });
            }

            _core = new Core();
            string connectionStr = _connectionStrings.Value.SdkConnection;
            bool running;

            try
            {
                running = _core.StartSqlOnly(connectionStr);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                var adminTools = new AdminTools(connectionStr);
                adminTools.MigrateDb();
                adminTools.DbSettingsReloadRemote();
                running = _core.StartSqlOnly(connectionStr);
            }

            if (running)
            {
                return _core;
            }

            _logger.LogError("Core is not running");
            throw new Exception("Core is not running");
        }
    }
}
