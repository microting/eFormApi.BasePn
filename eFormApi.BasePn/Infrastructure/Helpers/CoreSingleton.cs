using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        public static async Task<Core> GetCoreInstance(string connectionString, ILogger<EFormCoreService> logger)
        {   
            if (_coreInstance != null && connectionString.Equals(_connectionString)) return _coreInstance;

            lock (LockObj)
            {
                bool isCoreRunning;
                _coreInstance = new Core();

                try
                {
                    isCoreRunning = _coreInstance.StartSqlOnly(connectionString).Result;
                }

                catch (Exception exception)
                {
                    Log.LogException($"CoreSingleton.GetCoreInstance: Got exception {exception.Message}");
                    var adminTools = new AdminTools(connectionString);
                    var result = adminTools.DbSettingsReloadRemote().Result;
                    isCoreRunning = _coreInstance.StartSqlOnly(connectionString).Result;
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
