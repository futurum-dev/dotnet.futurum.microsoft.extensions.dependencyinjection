namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsScoped.As<IService2>]
public class Automatic_Scoped_As_Service : IService1, IService2
{
}