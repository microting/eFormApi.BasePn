using System.ComponentModel;

namespace Microting.eFormApi.BasePn.Infrastructure.Interfaces;

public interface ICommonSort
{
    [DefaultValue("Id")]
    public string Sort { get; set; }

    [DefaultValue(false)]
    public bool IsSortDsc { get; set; }
}