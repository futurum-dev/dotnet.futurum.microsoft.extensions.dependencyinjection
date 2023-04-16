namespace Futurum.Microsoft.Extensions.DependencyInjection;

public class StartableFunctionWrapper : IStartable
{
    private readonly Action _func;

    public StartableFunctionWrapper(Action func)
    {
        _func = func;
    }
    
    public void Start()
    {
        _func();
    }
}