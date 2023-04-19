namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient.AsImplementedInterfaces]
public class Automatic_Transient_AsImplementedInterfaces_Service : IService1, IService2
{
}