using System.ComponentModel;

namespace Microting.eFormApi.BasePn.Infrastructure.Interfaces;

public interface ICommonPagination
{
    [DefaultValue(10)]
    public int PageSize { get; set; }

    [DefaultValue(0)]
    public int Offset { get; set; }
}