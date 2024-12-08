using FluentAssertions;
using SettlementService.Domain.Exceptions;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Test.ValueObjects;

[TestFixture]
public class TimeTypeTests
{
    [Test]
    public void MilitaryTime_Create_WithValidTimeOnly_ShouldSucceed()
    {
        // Arrange
        var time = TimeOnly.Parse("09:30");

        // Act
        var militaryTime = MilitaryTime.Create(time);

        // Assert
        militaryTime.Value.Should().Be(time);
    }

    [Test]
    public void MilitaryTime_Create_WithValidString_ShouldSucceed()
    {
        // Arrange
        var timeString = "09:30";

        // Act
        var militaryTime = MilitaryTime.Create(timeString);

        // Assert
        militaryTime.Value.Should().Be(TimeOnly.Parse(timeString));
    }

    [Test]
    public void MilitaryTime_Create_WithInvalidString_ShouldThrowException()
    {
        // Arrange
        var invalidTime = "invalid-time";

        // Act
        Action act = () => MilitaryTime.Create(invalidTime);

        // Assert
        act.Should()
            .Throw<UnsupportedTimeTypeException>()
            .WithMessage("Invalid time format*");
    }

    [Test]
    public void IsDurationDifference_WithValidInputs_ShouldReturnTrue()
    {
        // Arrange
        var time1 = MilitaryTime.Create("09:30");
        var time2 = TimeOnly.Parse("08:30");
        var expectedDuration = TimeSpan.FromHours(1);

        // Act
        var result = time1.IsDurationDifference(time2, expectedDuration);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsDurationDifference_WithInvalidDuration_ShouldReturnFalse()
    {
        // Arrange
        var time1 = MilitaryTime.Create("09:30");
        var time2 = TimeOnly.Parse("08:30");
        var invalidDuration = TimeSpan.FromHours(2);

        // Act
        var result = time1.IsDurationDifference(time2, invalidDuration);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ComparisonOperators_ShouldWorkAsExpected()
    {
        // Arrange
        var time1 = MilitaryTime.Create("08:30");
        var time2 = MilitaryTime.Create("09:30");

        // Act & Assert
        (time1 < time2).Should().BeTrue();
        (time2 > time1).Should().BeTrue();
        (time1 <= time2).Should().BeTrue();
        (time2 >= time1).Should().BeTrue();
    }

    [Test]
    public void ToString_ShouldReturnDefaultFormat()
    {
        // Arrange
        var time = MilitaryTime.Create("09:30");

        // Act
        var result = time.ToString();

        // Assert
        result.Should().Be("09:30");
    }

    [Test]
    public void ToShortForm_ShouldReturnShortFormat()
    {
        // Arrange
        var time = MilitaryTime.Create("09:30");

        // Act
        var result = time.ToShortForm();

        // Assert
        result.Should().Be("9:30");
    }

    [Test]
    public void ImplicitConversionToTimeOnly_ShouldWorkAsExpected()
    {
        // Arrange
        var time = MilitaryTime.Create("09:30");

        // Act
        TimeOnly result = time;

        // Assert
        result.Should().Be(time.Value);
    }

    [Test]
    public void TryCreate_WithNullOrEmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        string nullTime = null;
        var emptyTime = "";

        // Act
        Action act1 = () => MilitaryTime.TryCreate(nullTime);
        Action act2 = () => MilitaryTime.TryCreate(emptyTime);

        // Assert
        act1.Should().Throw<ArgumentException>();
        act2.Should().Throw<ArgumentException>();
    }

    [Test]
    public void TryCreate_WithInvalidFormat_ShouldThrowUnsupportedTimeTypeException()
    {
        // Arrange
        var invalidTime = "invalid-time";

        // Act
        Action act = () => MilitaryTime.TryCreate(invalidTime);

        // Assert
        act.Should()
            .Throw<UnsupportedTimeTypeException>()
            .WithMessage("Invalid time format*");
    }
}