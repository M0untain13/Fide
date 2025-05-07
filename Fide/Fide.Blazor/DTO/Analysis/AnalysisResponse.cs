namespace Fide.Blazor.DTO.Analysis;

public class AnalysisResponse
{
    public double PredictedResult { get; set; }
    public required string BucketName { get; set; }
    public required string ObjectName { get; set; }
}
