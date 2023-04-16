using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interface for <see cref="Microsoft.Extensions.DependencyInjection"/> modules
/// <para></para>
/// NOTE - You will need to manually add this module.
/// <para></para>
/// See <see cref="ModuleExtensions.AddModule{T}(IServiceCollection, T)"/> and <see cref="ModuleExtensions.AddModule{T}(IServiceCollection)"/>
/// <remarks>A module allows you to break up registration into logical units i.e. modules</remarks>
/// </summary>
public interface IModule
{
    void Load(IServiceCollection services);
}