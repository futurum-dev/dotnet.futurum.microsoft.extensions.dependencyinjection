using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

public class ModuleFunctionWrapper : IModule
{
    private readonly Action<IServiceCollection> _func;

    public ModuleFunctionWrapper(Action<IServiceCollection> func)
    {
        _func = func;
    }

    public void Load(IServiceCollection services)
    {
        _func(services);
    }
}