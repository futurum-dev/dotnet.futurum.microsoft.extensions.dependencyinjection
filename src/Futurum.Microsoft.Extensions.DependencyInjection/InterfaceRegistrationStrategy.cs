namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interface registration strategies
/// </summary>
public enum InterfaceRegistrationStrategy
{
    /// <summary>
    /// Registers the service as itself.
    /// </summary>
    Self = 0,

    /// <summary>
    /// Registers the service as each its implemented interfaces.
    /// </summary>
    ImplementedInterfaces = 1,

    /// <summary>
    /// Registers the service as itself and each its implemented interfaces.
    /// </summary>
    SelfWithInterfaces = 2
}