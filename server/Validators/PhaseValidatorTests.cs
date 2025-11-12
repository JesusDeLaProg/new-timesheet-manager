using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class PhaseValidatorTests
{
    private readonly PhaseValidator _validator;

    public PhaseValidatorTests()
    {
        var logger = new Mock<ILogger<PhaseValidator>>().Object;
        _validator = new PhaseValidator(logger);
    }

    private static Phase ParsePhase(string path)
    {
        var text = System.IO.File.ReadAllText(path);
        return JsonParser.Default.Parse<Phase>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_Phase()
    {
        // Arrange
        var phase = ParsePhase("Testdata/phase.json");

        // Act
        var result = _validator.ValidateMessage(phase);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_No_Activities()
    {
        // Arrange
        var phase = ParsePhase("Testdata/phase_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(phase);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "activities_in_phase" && v.Error == "At least one activity is required.");
    }

    [Fact]
    public void Should_Fail_For_Duplicate_Activities()
    {
        // Arrange
        var phase = ParsePhase("Testdata/phase_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(phase);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "activities_in_phase" && v.Error == "Activities in phase must be unique.");
    }

    [Fact]
    public void Should_Fail_For_Invalid_Code()
    {
        // Arrange
        var phase = ParsePhase("Testdata/phase_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(phase);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "code" && v.Error == "Code must be 2 letters.");
    }
}
