using System.Text.Json;
using Fide.Blazor.DTO.Analysis;
using Microsoft.Extensions.Options;

namespace Fide.Blazor.Services.AnalysisProxy;

public class AomacaProxy(IOptions<AomacaOptions> options, ILogger<AomacaProxy> logger) : IAnalysisProxy
{
    public readonly AomacaOptions _aomacaOptions = options.Value;

    public async Task<AnalysisResponse> SendAsync(AnalysisRequest request)
    {
        try
        {
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_aomacaOptions.ServiceUrl)
            };
            var json = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(json);
            var response = await httpClient.PostAsync("/analysis", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<AnalysisResponse>(responseContent)
                ?? throw new NullReferenceException("Был получен пустой ответ от сервиса анализа");
            return obj;
        }
        catch (Exception ex)
        {
            logger.LogError(new EventId(), ex, "Ошибка при отправке запроса на сервис анализа");
            throw;
        }
    }
}
