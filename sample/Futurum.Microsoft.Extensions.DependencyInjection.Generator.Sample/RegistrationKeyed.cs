namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

[RegisterAsKeyedTransient("service-key")]
public class KeyedTransientService : IService1
{
}

[RegisterAsTransient.AsKeyedSelf("service-key")]
public class TransientService_AsKeyedSelf : IService1
{
}

[RegisterAsTransient.AsKeyed<IService1>("service-key")]
public class TransientService_AsKeyed : IService1
{
}

[RegisterAsTransient.AsKeyedImplementedInterfaces("service-key")]
public class TransientService_AsKeyedImplementedInterfaces : IService1
{
}

[RegisterAsTransient.AsKeyedImplementedInterfacesAndSelf("service-key")]
public class TransientService_AsKeyedImplementedInterfacesAndSelf : IService1
{
}

[RegisterAsKeyedScoped("service-key")]
public class KeyedScopedService : IService1
{
}

[RegisterAsScoped.AsKeyedSelf("service-key")]
public class ScopedService_AsKeyedSelf : IService2
{
}

[RegisterAsScoped.AsKeyed<IService1>("service-key")]
public class ScopedService_AsKeyed : IService1
{
}

[RegisterAsScoped.AsKeyedImplementedInterfaces("service-key")]
public class ScopedService_AsKeyedImplementedInterfaces : IService2
{
}

[RegisterAsScoped.AsKeyedImplementedInterfacesAndSelf("service-key")]
public class ScopedService_AsKeyedImplementedInterfacesAndSelf : IService2
{
}

[RegisterAsKeyedSingleton("service-key")]
public class KeyedSingletonService : IService1
{
}

[RegisterAsSingleton.AsKeyedSelf("service-key")]
public class SingletonService_AsKeyedSelf : IService3
{
}

[RegisterAsSingleton.AsKeyed<IService1>("service-key")]
public class SingletonService_AsKeyed : IService1
{
}

[RegisterAsSingleton.AsKeyedImplementedInterfaces("service-key")]
public class SingletonService_AsKeyedImplementedInterfaces : IService3
{
}

[RegisterAsSingleton.AsKeyedImplementedInterfacesAndSelf("service-key")]
public class SingletonService_AsKeyedImplementedInterfacesAndSelf : IService3
{
}

[RegisterAsScoped.AsKeyedSelf("service-key", DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try)]
public class SingletonService_Keyed_Try : IService1
{
}

[RegisterAsScoped.AsKeyedSelf("service-key", DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Replace)]
public class SingletonService_Keyed_Replace : IService1
{
}

[RegisterAsScoped.AsKeyedSelf("service-key", DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class SingletonService_Keyed_Add : IService1
{
}

[RegisterAsScoped.AsKeyedImplementedInterfaces("service-key", DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class Service_AsKeyedImplementedInterfaces : IService1, IService2
{
}

[RegisterAsTransient.AsKeyedOpenGeneric("service-key", ImplementationType = typeof(TransientService_AsOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class TransientService_AsKeyedOpenGeneric<T> : IService_OpenGeneric<T>
{
}

[RegisterAsScoped.AsKeyedOpenGeneric("service-key", ImplementationType = typeof(ScopedService_AsOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class ScopedService_AsKeyedOpenGeneric<T> : IService_OpenGeneric<T>
{
}

[RegisterAsSingleton.AsKeyedOpenGeneric("service-key", ImplementationType = typeof(SingletonService_AsOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class SingletonService_AsKeyedOpenGeneric<T> : IService_OpenGeneric<T>
{
}
