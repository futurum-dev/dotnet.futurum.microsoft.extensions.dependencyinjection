using FluentAssertions;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class RegisterAsAttributeTests
{
    [Fact]
    public void check_defaults()
    {
        var testRegisterAsAttribute = new TestRegisterAsAttribute()
        {
            ServiceType = typeof(int),
            ImplementationType = typeof(string)
        };

        testRegisterAsAttribute.DuplicateRegistrationStrategy.Should().Be(DuplicateRegistrationStrategy.Try);
        testRegisterAsAttribute.InterfaceRegistrationStrategy.Should().Be(InterfaceRegistrationStrategy.SelfWithInterfaces);
        testRegisterAsAttribute.InterfaceRegistrationStrategy.Should().Be(InterfaceRegistrationStrategy.SelfWithInterfaces);
        testRegisterAsAttribute.ServiceType.Should().Be(typeof(int));
        testRegisterAsAttribute.ImplementationType.Should().Be(typeof(string));
    }
    
    public class TestRegisterAsAttribute : RegisterAsAttribute
    {
    }
}