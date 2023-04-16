using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interface for ALL <see cref="Microsoft.Extensions.DependencyInjection"/> startables
/// <para></para>
/// NOTE - You will need to manually add this startable.
/// <para></para>
/// See <see cref="StartableExtensions.AddStartable{T}(IServiceCollection,T)"/> and <see cref="StartableExtensions.AddStartable{T}(IServiceCollection)"/>
/// <remarks>A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.</remarks>
/// </summary>
public interface IStartable
{
    void Start();
}