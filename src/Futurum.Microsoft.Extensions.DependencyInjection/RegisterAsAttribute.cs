namespace Futurum.Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RegisterAsAttribute : Attribute
{
    protected RegisterAsAttribute()
    {
        DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try;
        InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.SelfWithInterfaces;
    }

    /// <summary>
    /// The type of the implementation
    /// </summary>
    public Type? ImplementationType { get; set; }

    /// <summary>
    /// The type of the service
    /// </summary>
    public Type? ServiceType { get; set; }

    public DuplicateRegistrationStrategy DuplicateRegistrationStrategy { get; set; }

    public InterfaceRegistrationStrategy InterfaceRegistrationStrategy { get; set; }
}