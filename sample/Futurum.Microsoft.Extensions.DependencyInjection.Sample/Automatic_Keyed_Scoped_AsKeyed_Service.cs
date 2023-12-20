namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsScoped.AsKeyed<IService2>("ServiceKey")]
public class Automatic_Keyed_Scoped_AsKeyed_Service : IService1, IService2
{
}