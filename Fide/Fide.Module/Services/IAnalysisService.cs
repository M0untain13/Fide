using Fide.Module.NonPersistentObjects;

namespace Fide.Module.Services;

public interface IAnalysisService
{
    void StartAnalysis(SelectImageForAnalysis selectImageForAnalysis);
}
