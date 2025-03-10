namespace Fide.Blazor.Services;

public class FideEnvironment(bool isDebug)
{
    public bool IsDebug() => isDebug;
}
