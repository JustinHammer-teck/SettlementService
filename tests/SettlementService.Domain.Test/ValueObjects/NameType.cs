using FluentAssertions;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Test.ValueObjects;

[TestFixture]
public class FullNameTests
{
    [Test]
    public void Create_WithValidFullName_ShouldSucceed()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var fullName = FullName.Create(firstName, lastName);

        // Assert
        fullName.FirstName.Should().Be(firstName);
        fullName.LastName.Should().Be(lastName);
    }

    [Test]
    public void Create_WithNullOrWhitespaceFirstName_ShouldThrowArgumentException()
    {
        // Arrange
        string firstName = null;
        var lastName = "Doe";

        // Act
        Action act = () => FullName.Create(firstName, lastName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("'firstName' cannot be null or whitespace. (Parameter 'firstName')");
    }

    [Test]
    public void Create_WithNullOrWhitespaceLastName_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "John";
        string lastName = null;

        // Act
        Action act = () => FullName.Create(firstName, lastName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("'lastName' cannot be null or whitespace. (Parameter 'lastName')");
    }

    [Test]
    public void Create_WithValidSingleStringName_ShouldSucceed()
    {
        // Arrange
        var name = "John Doe";

        // Act
        var fullName = FullName.Create(name);

        // Assert
        fullName.FirstName.Should().Be("John");
        fullName.LastName.Should().Be("Doe");
    }

    [Test]
    public void Create_WithInvalidSingleStringName_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidName = "John";

        // Act
        Action act = () => FullName.Create(invalidName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid full name format: {invalidName}");
    }

    [Test]
    public void TryCreate_WithValidSingleStringName_ShouldReturnTrueAndFullName()
    {
        // Arrange
        var name = "John Doe";

        // Act
        var result = FullName.TryCreate(name, out var fullName);

        // Assert
        result.Should().BeTrue();
        fullName.FirstName.Should().Be("John");
        fullName.LastName.Should().Be("Doe");
    }

    [Test]
    public void TryCreate_WithInvalidSingleStringName_ShouldReturnFalseAndDefault()
    {
        // Arrange
        var invalidName = "John";

        // Act
        var result = FullName.TryCreate(invalidName, out var fullName);

        // Assert
        result.Should().BeFalse();
        fullName.Should().BeNull();
    }

    [Test]
    public void TryCreate_WithNullOrWhitespaceName_ShouldThrowArgumentException()
    {
        // Arrange
        string nullName = null;

        // Act
        Action act = () => FullName.TryCreate(nullName, out _);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("'name' cannot be null or whitespace. (Parameter 'name')");
    }
}