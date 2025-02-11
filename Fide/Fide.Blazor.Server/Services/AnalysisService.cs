using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects;
using Fide.Module.Enums;
using Fide.Module.NonPersistentObjects;
using Fide.Module.Services;

namespace Fide.Blazor.Server.Services;

public class AnalysisService : IAnalysisService
{
    private readonly Dictionary<AnalysisEnum, Func<FileData, IEnumerable<AnalysisResult>>> _analyzers;

    public AnalysisService()
    {
        _analyzers = new()
        {
            {AnalysisEnum.Metadata, AnalyzeExif },
            {AnalysisEnum.ELA, AnalyzeEla },
        };
    }

    public void StartAnalysis(SelectImageForAnalysis selectImageForAnalysis)
    {
        var analysisTypes = selectImageForAnalysis.SelectedAnalysisTypes.Select(a => a.AnalysisType).Distinct();

        foreach (var analysisType in analysisTypes)
        {
            if (_analyzers.TryGetValue(analysisType, out var analyzer))
            {
                if (!selectImageForAnalysis.SelectedImage.Results.Select(r => r.AnalysisType).Contains(analysisType))
                {
                    var results = analyzer.Invoke(selectImageForAnalysis.SelectedImage.Image);
                    foreach (var result in results)
                    {
                        selectImageForAnalysis.SelectedImage.Results.Add(result);
                    }
                }
            }
            else
            {
                throw new NotImplementedException($"Не реализован алгоритм анализа '{analysisType}'");
            }
        }
    }

    private IEnumerable<AnalysisResult> AnalyzeExif(FileData fileData)
    {
        var result = new AnalysisResult()
        {
            Information = "EXIF Test",
            AnalysisType = AnalysisEnum.Metadata,
        };
        result.OnCreated();
        return [result];
    }

    private IEnumerable<AnalysisResult> AnalyzeEla(FileData fileData)
    {
        var result = new AnalysisResult()
        {
            Information = "ELA Test",
            AnalysisType = AnalysisEnum.ELA,
        };
        result.Files.Add(fileData);
        result.OnCreated();
        return [result];
    }
}
