namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

[RegisterAsTransient]
public class TransientService : IService1
{
}

[RegisterAsTransient.AsSelf]
public class TransientService_AsSelf : IService1
{
}

[RegisterAsTransient.As<IService1>]
public class TransientService_As : IService1
{
}

[RegisterAsTransient.AsImplementedInterfaces]
public class TransientService_AsImplementedInterfaces : IService1
{
}

[RegisterAsTransient.AsImplementedInterfacesAndSelf]
public class TransientService_AsImplementedInterfacesAndSelf : IService1
{
}

[RegisterAsScoped]
public class ScopedService : IService2
{
}

[RegisterAsScoped.AsSelf]
public class ScopedService_AsSelf : IService2
{
}

[RegisterAsScoped.As<IService1>]
public class ScopedService_As : IService1
{
}

[RegisterAsScoped.AsImplementedInterfaces]
public class ScopedService_AsImplementedInterfaces : IService2
{
}

[RegisterAsScoped.AsImplementedInterfacesAndSelf]
public class ScopedService_AsImplementedInterfacesAndSelf : IService2
{
}

[RegisterAsSingleton]
public class SingletonService : IService3
{
}

[RegisterAsSingleton.AsSelf]
public class SingletonService_AsSelf : IService3
{
}

[RegisterAsSingleton.As<IService1>]
public class SingletonService_As : IService1
{
}

[RegisterAsSingleton.AsImplementedInterfaces]
public class SingletonService_AsImplementedInterfaces : IService3
{
}

[RegisterAsSingleton.AsImplementedInterfacesAndSelf]
public class SingletonService_AsImplementedInterfacesAndSelf : IService3
{
}

[RegisterAsScoped.AsSelf(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try)]
public class SingletonService_Try : IService1
{
}

[RegisterAsScoped.AsSelf(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Replace)]
public class SingletonService_Replace : IService1
{
}

[RegisterAsScoped.AsSelf(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class SingletonService_Add : IService1
{
}

[RegisterAsScoped.AsImplementedInterfaces(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class Service_AsImplementedInterfaces : IService1, IService2
{
}

[RegisterAsTransient.AsOpenGeneric(ImplementationType = typeof(TransientService_AsOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class TransientService_AsOpenGeneric<T> : IService_OpenGeneric<T>
{
}

[RegisterAsScoped.AsOpenGeneric(ImplementationType = typeof(ScopedService_AsOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class ScopedService_AsOpenGeneric<T> : IService_OpenGeneric<T>
{
}

[RegisterAsSingleton.AsOpenGeneric(ImplementationType = typeof(SingletonService_AsOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class SingletonService_AsOpenGeneric<T> : IService_OpenGeneric<T>
{
}
