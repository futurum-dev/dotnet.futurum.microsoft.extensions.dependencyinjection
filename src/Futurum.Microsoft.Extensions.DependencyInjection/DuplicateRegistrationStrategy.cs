namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Duplicate registration strategies
/// </summary>
public enum DuplicateRegistrationStrategy
{
    /// <summary>
    /// Adds the new registration, if the service hasn't already been registered.
    /// </summary>
    Try = 0,

    /// <summary>
    /// Removes any existing registration and then adds the new registration.
    /// </summary>
    Replace = 1,

    /// <summary>
    /// Adds the new registration, irrespective of if its previously been registered.
    /// </summary>
    Add = 2
}