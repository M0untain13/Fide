using Fide.Blazor.Data.Base;

namespace Fide.Blazor.Data;

public class ImageLink : Entity
{
    /// <summary>
    /// Дата загрузки изображения
    /// </summary>
    public virtual DateTime Created { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Владелец изображения
    /// </summary>
    public virtual ApplicationUser? User { get; set; }
    /// <summary>
    /// Название исходника
    /// </summary>
    public virtual string OriginalName { get; set; } = string.Empty;
    /// <summary>
    /// Название анализа
    /// </summary>
    public virtual string? AnalysisName { get; set; }
    /// <summary>
    /// Результат анализа
    /// </summary>
    public virtual double? AnalysisResult { get; set; }
    /// <summary>
    /// Последняя дата анализа
    /// </summary>
    public virtual DateTime? AnalysisRequested { get; set; }
}
