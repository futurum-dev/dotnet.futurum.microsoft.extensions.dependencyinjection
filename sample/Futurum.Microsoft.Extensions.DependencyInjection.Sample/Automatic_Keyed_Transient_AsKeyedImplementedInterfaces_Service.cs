namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient.AsKeyedImplementedInterfaces("ServiceKey")]
public class Automatic_Keyed_Transient_AsKeyedImplementedInterfaces_Service : IService1, IService2
{
}