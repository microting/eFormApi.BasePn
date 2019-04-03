using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using eFormCore;
using Microsoft.Extensions.Logging;
using Microting.eFormApi.BasePn.Services;

namespace Microting.eFormApi.BasePn.Infrastructure.Helpers
{
    public sealed class CoreSingleton
    {
        private static Core _coreInstance;
        private static readonly object LockObj = new object();
        private static string _connectionString;

        public static Core GetCoreInstance(string connectionString, ILogger<EFormCoreService> logger)
        {   
            if (_coreInstance != null && connectionString.Equals(_connectionString)) return _coreInstance;

            lock (LockObj)
            {
                bool isCoreRunning;
                _coreInstance = new Core();

                try
                {
                    isCoreRunning = _coreInstance.StartSqlOnly(connectionString);
                }

                catch (Exception exception)
                {
                    logger.LogError(exception.Message);
                    var adminTools = new AdminTools(connectionString);
                    adminTools.MigrateDb();
                    adminTools.DbSettingsReloadRemote();
                    isCoreRunning = _coreInstance.StartSqlOnly(connectionString);
                }

                if (isCoreRunning)
                {
                    _connectionString = connectionString;
                    return _coreInstance;
                }

                logger.LogError("Core is not running");
                throw new Exception("Core is not running");
            }
        }
    }
}
