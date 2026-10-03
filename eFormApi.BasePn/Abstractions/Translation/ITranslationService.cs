using System.Threading.Tasks;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Microting.eFormApi.BasePn.Abstractions.Translation;

/// <summary>
/// Machine translation of short user texts (task titles, descriptions). The API host
/// owns the implementation and registers it in DI; plugins depend on this interface only
/// and must treat it as optional, because a host may not register it.
/// </summary>
/// <remarks>
/// It lives in its own namespace so a host that still declares an
/// <c>ITranslationService</c> of its own can take this package without an ambiguous
/// reference.
/// </remarks>
public interface ITranslationService
{
    /// <summary>True when the host has translation configured (an API key is set).</summary>
    bool IsConfigured { get; }

    /// <summary>
    /// Translates <paramref name="sourceText"/>. Language codes may be bare ISO codes
    /// ("da") or locale codes ("en-US"). An unconfigured service, or a failed call,
    /// returns an unsuccessful result instead of throwing.
    /// </summary>
    Task<OperationDataResult<string>> TranslateText(string sourceText, string sourceLanguageCode,
        string targetLanguageCode);
}
