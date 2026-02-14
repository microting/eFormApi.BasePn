using System.Threading.Tasks;

namespace Microting.eFormApi.BasePn.Abstractions;

public interface IEFormCoreService
{
    Task<eFormCore.Core> GetCore();
    void LogEvent(string appendText);
    void LogException(string appendText);
}