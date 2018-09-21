﻿using System;
using System.Net;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using eFormCore;
using eFormCore.Installers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Helpers.WritableOptions;
using Microting.eFormApi.BasePn.Models.Application;
using Microting.eFormApi.BasePn.Resources;
using Rebus.Bus;

namespace Microting.eFormApi.BasePn.Services
{
    public class EFormCoreService : IEFormCoreService
    {
        private readonly IWritableOptions<ConnectionStrings> _connectionStrings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private Core _core;
        private readonly ILogger<EFormCoreService> _logger;
        private IWindsorContainer _container;
        public IBus Bus { get; private set; }

        public EFormCoreService(IWritableOptions<ConnectionStrings> connectionStrings,
            ILogger<EFormCoreService> logger,
            IHttpContextAccessor httpContextAccessor, 
            IStringLocalizer<SharedResource> localizer)
        {
            _connectionStrings = connectionStrings;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public Core GetCore()
        {
            var connectionString = _connectionStrings.Value.SdkConnection;
            if (string.IsNullOrEmpty(connectionString))
            {
                _httpContextAccessor.HttpContext.Response.OnStarting(async () =>
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    await _httpContextAccessor.HttpContext.Response.Body.FlushAsync();
                });
            }

            _core = new Core();
            var connectionStr = _connectionStrings.Value.SdkConnection;
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
                _container = new WindsorContainer();
                _container.Register(Component.For<Core>().Instance(_core));
                _container.Install(new RebusHandlerInstaller(),
                    new RebusInstaller(_connectionStrings.Value.DefaultConnection, 5, 5)); // TODO 1,1 parameters
                Bus = _container.Resolve<IBus>();
                return _core;
            }

            _logger.LogError("Core is not running");
            throw new Exception("Core is not running");
        }
    }
}