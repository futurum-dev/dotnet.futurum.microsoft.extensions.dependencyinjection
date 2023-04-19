namespace Futurum.Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RegisterAsAttribute : Attribute
{
    public DuplicateRegistrationStrategy DuplicateRegistrationStrategy { get; set; }
}