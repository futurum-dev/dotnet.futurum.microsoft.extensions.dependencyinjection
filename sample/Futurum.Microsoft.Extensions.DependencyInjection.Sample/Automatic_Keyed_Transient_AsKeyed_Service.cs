namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient.AsKeyed<IService2>("ServiceKey")]
public class Automatic_Keyed_Transient_AsKeyed_Service : IService1, IService2
{
}