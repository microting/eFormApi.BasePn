using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;
using Rebus.Bus;

namespace Microting.eFormApi.BasePn.Services;

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

    public async Task<Core> GetCore()
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

        Core coreInstance = await CoreSingleton.GetCoreInstance(connectionStr, _logger);
        while (!coreInstance.Running())
        {
            Thread.Sleep(1000);
            Log.LogEvent("EFormCoreService.GetCore: sleeping for 1 second, waiting for core to startup!");
        }

        return coreInstance;
    }
        
    public void LogEvent(string appendText)
    {
        try
        {                
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DBG] {GetMethodName()}: {appendText}");
            Console.ForegroundColor = oldColor;
        }
        catch
        {
        }
    }

    public void LogException(string appendText)
    {
        try
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {GetMethodName()}: {appendText}");
            Console.ForegroundColor = oldColor;
        }
        catch
        {
        }
    }
        
    private string GetMethodName()
    {
        StackTrace st = new StackTrace();
        StackFrame sf = st.GetFrame(2);

        return sf.GetMethod().ReflectedType.Name + "." + sf.GetMethod().Name;
    }
}