namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IEFormCoreService
    {
        eFormCore.Core GetCore();
        void LogEvent(string appendText);
        void LogException(string appendText);
    }
}
