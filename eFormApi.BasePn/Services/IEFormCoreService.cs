using Rebus.Bus;

namespace Microting.eFormApi.BasePn.Services
{
    public interface IEFormCoreService
    {
        eFormCore.Core GetCore();
        IBus Bus { get; }
    }
}