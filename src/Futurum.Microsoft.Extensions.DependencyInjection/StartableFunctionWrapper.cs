namespace Futurum.Microsoft.Extensions.DependencyInjection;

public class StartableFunctionWrapper : IStartable
{
    private readonly Func<Task> _func;

    public StartableFunctionWrapper(Func<Task> func)
    {
        _func = func;
    }
    
    public async Task StartAsync()
    {
        await _func();
    }
}