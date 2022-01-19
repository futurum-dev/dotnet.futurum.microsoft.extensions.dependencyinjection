using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

public interface IModule
{
    void Load(IServiceCollection services);
}