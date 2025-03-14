﻿using Fide.Blazor.Models.AnalysisModels;

namespace Fide.Blazor.Services.AnalysisProxy;

public interface IAnalysisProxy
{
    Task<AnalysisResponse> SendAsync(AnalysisRequest request);
}
