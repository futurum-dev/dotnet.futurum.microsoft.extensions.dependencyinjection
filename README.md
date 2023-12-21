# Futurum.Microsoft.Extensions.DependencyInjection

![license](https://img.shields.io/github/license/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection/ci.yml?branch=main&style=for-the-badge)
[![Coverage Status](https://img.shields.io/coveralls/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)](https://coveralls.io/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?branch=main)
[![NuGet version](https://img.shields.io/nuget/v/futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)](https://www.nuget.org/packages/futurum.microsoft.extensions.dependencyinjection)

A dotnet library that extends Microsoft.Extensions.DependencyInjection by adding support for [attribute based registration](#attribute-based-registration), [modules](#modules) and [startables](#startables).

- [x] [Easy setup](#easy-setup)
- [x] Autodiscovery of DependencyInjection registrations, based on [attributes](#attribute-based-registration) and Source Generators
- [x] Support for *keyed* registrations
- [x] Autodiscovery of DependencyInjection modules, based on [attributes](#attribute-based-module) and Source Generators
- [x] Autodiscovery of DependencyInjection startables, based on [attributes](#attribute-based-startable) and Source Generators
- [x] [Roslyn Analysers](#roslyn-analysers) to help build your registrations, modules and startables, using best practices
- [x] [Tested solution](https://coveralls.io/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?branch=main)

## Easy setup
1. Install the [NuGet package](https://www.nuget.org/packages/futurum.microsoft.extensions.dependencyinjection)
2. Update *program.cs* as per [here](#programcs)

### Program.cs
```csharp
builder.Services.AddDependencyInjectionFor...();
```

#### AddDependencyInjectionFor... (per project containing registrations)
This will be automatically created by the source generator.

e.g.
```csharp
builder.Services.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionSample();
```

## Attribute based registration
You can register services using attributes.

The attributes have been created is a discoverable way. They take the following form:
- [RegisterAs{Lifetime}](#registeraslifetime-attribute)
- [RegisterAsKeyed{Lifetime}](#registeraskeyedlifetime-attribute)
- [RegisterAs{Lifetime}.AsSelf](#registeraslifetimeasself-attribute)
- [RegisterAs{Lifetime}.AsKeyedSelf](#registeraslifetimeaskeyedself-attribute)
- [RegisterAs{Lifetime}.As{ServiceType}](#registeraslifetimeasservicetype-attribute)
- [RegisterAs{Lifetime}.AsKeyed{ServiceType}](#registeraslifetimeaskeyedservicetype-attribute)
- [RegisterAs{Lifetime}.AsImplementedInterfaces](#registeraslifetimeasimplementedinterfaces-attribute)
- [RegisterAs{Lifetime}.AsKeyedImplementedInterfaces](#registeraslifetimeaskeyedimplementedinterfaces-attribute)
- [RegisterAs{Lifetime}.AsImplementedInterfacesAndSelf](#registeraslifetimeasimplementedinterfacesandself-attribute)
- [RegisterAs{Lifetime}.AsKeyedImplementedInterfacesAndSelf](#registeraslifetimeaskeyedimplementedinterfacesandself-attribute)
- [RegisterAs{Lifetime}.AsOpenGeneric](#registeraslifetimeasopengeneric-attribute)
- [RegisterAs{Lifetime}.AsKeyedOpenGeneric](#registeraslifetimeaskeyedopengeneric-attribute)

There are 3 lifetimes available:
- Singleton
- Scoped
- Transient

**NOTE** - There is a Roslyn Analyser that will provide information on exactly what registration is taking place. Just hover over the class.

### RegisterAs{Lifetime} attribute
This will register the class against the 1 interface the class implements, for the specified lifetime.

**NOTE** - There is a Roslyn Analyser that will warn you if you use this attribute and the class does not implement exactly 1 interface.

e.g. This will register *Service* against *IService* with a *Singleton* lifetime.
```csharp
[RegisterAsSingleton]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Transient* lifetime.
```csharp
[RegisterAsTransient]
public class Service : IService
{
}
```

### RegisterAsKeyed{Lifetime} attribute
This will register the class against the 1 interface the class implements, for the specified lifetime, with the *service key*.

**NOTE** - There is a Roslyn Analyser that will warn you if you use this attribute and the class does not implement exactly 1 interface.

e.g. This will register *Service* against *IService*, with a *Singleton* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsKeyedSingleton("service-key")]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService*, with a *Scoped* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsKeyedScoped("service-key")]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService*, with a *Transient* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsKeyedTransient("service-key")]
public class Service : IService
{
}
```

### RegisterAs{Lifetime}.AsSelf attribute
This will register the class against itself, for the specified lifetime.

e.g. This will register *Service* against *Service* with a *Singleton* lifetime.
```csharp
[RegisterAsSingleton.AsSelf]
public class Service
{
}
```

e.g. This will register *Service* against *Service* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsSelf]
public class Service
{
}
```

e.g. This will register *Service* against *Service* with a *Transient* lifetime.
```csharp
[RegisterAsTransient.AsSelf]
public class Service
{
}
```

### RegisterAs{Lifetime}.AsKeyedSelf attribute
This will register the class against itself, for the specified lifetime, for the specified lifetime, with the *service key*.

e.g. This will register *Service* against *Service*, with a *Singleton* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsSingleton.AsKeyedSelf("service-key")]
public class Service
{
}
```

e.g. This will register *Service* against *Service*, with a *Scoped* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsScoped.AsKeyedSelf("service-key")]
public class Service
{
}
```

e.g. This will register *Service* against *Service*, with a *Transient* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsTransient.AsKeyedSelf("service-key")]
public class Service
{
}
```

### RegisterAs{Lifetime}.As{ServiceType} attribute
This will register the class against the specified interface, for the specified lifetime.

**NOTE** - There is a Roslyn Analyser that will warn you if you use the class does not implement the specified interface.

e.g. This will register *Service* against *IService1* with a *Singleton* lifetime.
```csharp
[RegisterAsSingleton.As<IService2>]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.As<IService2>]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1* with a *Transient* lifetime.
```csharp
[RegisterAsTransient.As<IService2>]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsKeyed{ServiceType} attribute
This will register the class against the specified interface, for the specified lifetime, with the *service key*.

**NOTE** - There is a Roslyn Analyser that will warn you if you use the class does not implement the specified interface.

e.g. This will register *Service* against *IService1*, with a *Singleton* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsSingleton.AsKeyed<IService2>("service-key")]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1*, with a *Scoped* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsScoped.AsKeyed<IService2>("service-key")]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1*, with a *Transient* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsTransient.AsKeyed<IService2>("service-key")]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsImplementedInterfaces attribute
This will register the class against all the interfaces it implements directly, for the specified lifetime.

e.g. This will register *Service* against *IService1* and *IService2* with a *Singleton* lifetime.
```csharp
[RegisterAsSingleton.AsImplementedInterfaces]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1* and *IService2* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsImplementedInterfaces]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1* and *IService2* with a *Transient* lifetime.
```csharp
[RegisterAsTransient.AsImplementedInterfaces]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsKeyedImplementedInterfaces attribute
This will register the class against all the interfaces it implements directly, for the specified lifetime, with the *service key*.

e.g. This will register *Service* against *IService1* and *IService2*, with a *Singleton* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsSingleton.AsKeyedImplementedInterfaces("service-key")]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1* and *IService2*, with a *Scoped* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsScoped.AsKeyedImplementedInterfaces("service-key")]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *IService1* and *IService2*, with a *Transient* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsTransient.AsKeyedImplementedInterfaces("service-key")]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsImplementedInterfacesAndSelf attribute
This will register the class against all the interfaces it implements directly and itself, for the specified lifetime.

e.g. This will register *Service* against *Service*, *IService1* and *IService2* with a *Singleton* lifetime.
```csharp
[RegisterAsSingleton.AsImplementedInterfacesAndSelf]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *Service*, *IService1* and *IService2* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsImplementedInterfacesAndSelf]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *Service*, *IService1* and *IService2* with a *Transient* lifetime.
```csharp
[RegisterAsTransient.AsImplementedInterfacesAndSelf]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsKeyedImplementedInterfacesAndSelf attribute
This will register the class against all the interfaces it implements directly and itself, for the specified lifetime, with the *service key*.

e.g. This will register *Service* against *Service*, *IService1* and *IService2*, with a *Singleton* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsSingleton.AsKeyedImplementedInterfacesAndSelf("service-key")]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *Service*, *IService1* and *IService2*, with a *Scoped* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsScoped.AsKeyedImplementedInterfacesAndSelf("service-key")]
public class Service : IService1, IService2
{
}
```

e.g. This will register *Service* against *Service*, *IService1* and *IService2*, with a *Transient* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsTransient.AsKeyedImplementedInterfacesAndSelf("service-key")]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsOpenGeneric attribute
This will register the an open generic class against an open generic interface, for the specified lifetime.

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;* with a *Singleton* lifetime.
```csharp
[RegisterAsSingleton.AsOpenGeneric(ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsOpenGeneric(ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;* with a *Transient* lifetime.
```csharp
[RegisterAsTransient.AsOpenGeneric(ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

### RegisterAs{Lifetime}.AsKeyedOpenGeneric attribute
This will register the an open generic class against an open generic interface, for the specified lifetime, with the *service key*.

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;*, with a *Singleton* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsSingleton.AsKeyedOpenGeneric("service-key", ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;*, with a *Scoped* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsScoped.AsKeyedOpenGeneric("service-key", ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;*, with a *Transient* lifetime, with a *"service-key"* service key.
```csharp
[RegisterAsTransient.AsKeyedOpenGeneric("service-key", ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

### DuplicateRegistrationStrategy
**All** registration attributes allow you to specify how to handle duplicate registrations.

- Try - Adds the new registration, if the service hasn't already been registered
- Replace - Removes any existing registration and then adds the new registration
- Add - Adds the new registration, irrespective of if its previously been registered

**NOTE** - This defaults to *Try*

#### Try
**NOTE - This is the default behaviour.**

e.g. This will register *Service* against *IService* with a *Singleton* lifetime via *Try*.
```csharp
[RegisterAsSingleton(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try)]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Scoped* lifetime via *Try*.
```csharp
[RegisterAsScoped(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try)]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Transient* lifetime via *Try*.
```csharp
[RegisterAsTransient(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try)]
public class Service : IService
{
}
```

#### Replace
e.g. This will register *Service* against *IService* with a *Singleton* lifetime via *Replace*.
```csharp
[RegisterAsSingleton(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Replace)]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Scoped* lifetime via *Replace*.
```csharp
[RegisterAsScoped(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Replace)]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Transient* lifetime via *Replace*.
```csharp
[RegisterAsTransient(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Replace)]
public class Service : IService
{
}
```

#### Add
e.g. This will register *Service* against *IService* with a *Singleton* lifetime via *Add*.
```csharp
[RegisterAsSingleton(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Scoped* lifetime via *Add*.
```csharp
[RegisterAsScoped(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class Service : IService
{
}
```

e.g. This will register *Service* against *IService* with a *Transient* lifetime via *Add*.
```csharp
[RegisterAsTransient(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class Service : IService
{
}
```

## Modules
A module allows you to break up registration into logical units.

Module can either be registered using *IModule* interface and *AddModule* extension method, or by using the [*RegisterAsDependencyInjectionModule*](#attribute-based-module) attribute.

### IModule interface and AddModule extension method
If you use the *IModule* interface, you must manually register the module using the *AddModule* extension method.

#### IModule interface
Implements this interface to create a module.

```csharp
public class TestModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton<ITestService, TestService>();
    }
}
```

#### AddModule extension method
Allows you to register a module.

```csharp
services.AddModule<TestModule>();
```

```csharp
services.AddModule(new TestModule());
```

### Attribute based module
You can also register modules using attributes.

- RegisterAsDependencyInjectionModule attribute

```csharp
public class Module
{
    [RegisterAsDependencyInjectionModule]
    public void Load(IServiceCollection services)
    {
    }
}
```

```csharp
public static class Module
{
    [RegisterAsDependencyInjectionModule]
    public static void Load(IServiceCollection services)
    {
    }
}
```

## Startables
A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.

Startable can either be registered using *IStartable* interface and *AddStartable* extension method, or by using the [*RegisterAsDependencyInjectionStartable*](#attribute-based-startable) attribute.

**NOTE** - The main difference is that using *IStartable* interface allows you to inject dependencies into the startable, whereas the attribute based approach does not.

### IStartable interface and AddStartable extension method
If you use the *IStartable* interface, you must manually register the startable using the *AddStartable* extension method.

#### IStartable interface
Implements this interface to create a startable.

```csharp
public class TestStartable : IStartable
{
    public Task Start()
    {
        // Do something
    }
}
```

#### AddStartable extension method
Allows you to register a startable.

```csharp
services.AddStartable<TestStartable>();
```

```csharp
services.AddStartable(new TestStartable());
```

### Attribute based startable
You can also register modules using attributes.

- RegisterAsDependencyInjectionStartable attribute

```csharp
public class Startable
{
    [RegisterAsDependencyInjectionStartable]
    public Task StartAsync()
    {
        // Do something
    }
}
```

```csharp
public static class Startable
{
    [RegisterAsDependencyInjectionStartable]
    public static Task StartAsync()
    {
        // Do something
    }
}
```

## Roslyn Analysers
- FMEDI0001 - Invalid Module Parameter
- FMEDI0002 - Missing Module Parameter
- FMEDI0003 - Non empty constructor found on Module
- FMEDI0004 - Non empty constructor found on Startable
- FMEDI0005 - Non async method found on Startable
- FMEDI0006 - Non void method found on Module
- FMEDI0007 - Register ServiceType not implemented by class
- FMEDI0008 - Registration, must only have 1 interface
- FMEDI1000 - Registration information
