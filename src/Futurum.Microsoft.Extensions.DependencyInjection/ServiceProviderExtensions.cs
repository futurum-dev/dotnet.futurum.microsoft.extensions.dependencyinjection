using Futurum.Core.Result;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Try to get the service object of the specified type.
    /// </summary>
    public static Result<T> TryGetService<T>(this IServiceProvider serviceProvider) =>
        serviceProvider.GetService(typeof(T)) is T service
            ? service.ToResultOk()
            : Result.Fail<T>($"Failed to resolve Type : '{typeof(T).FullName}'");

    /// <summary>
    /// Try to get the service object of the specified type.
    /// </summary>
    public static Result<T> TryGetService<T>(this IServiceProvider serviceProvider, Type type) =>
        serviceProvider.GetService(type) is T service
            ? service.ToResultOk()
            : Result.Fail<T>($"Failed to resolve Type : '{type.FullName}'");

    /// <summary>
    /// Try to get the service object of the specified type.
    /// </summary>
    public static Result<object> TryGetService(this IServiceProvider serviceProvider, Type type) =>
        serviceProvider.GetService(type)?.ToResultOk() ?? Result.Fail<object>($"Failed to resolve Type : '{type.FullName}'");
}