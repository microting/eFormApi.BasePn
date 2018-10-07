using Rebus.Bus;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IEFormCoreService
    {
        eFormCore.Core GetCore();
        IBus Bus { get; }
    }
}
