using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class ActivityValidatorTests
{
    private readonly ActivityValidator _validator;

    public ActivityValidatorTests()
    {
        var logger = new Mock<ILogger<ActivityValidator>>().Object;
        _validator = new ActivityValidator(logger);
    }

    private static Activity ParseActivity(string path)
    {
        var text = System.IO.File.ReadAllText(path);
        return JsonParser.Default.Parse<Activity>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_Activity()
    {
        // Arrange
        var activity = ParseActivity("Testdata/activity.json");

        // Act
        var result = _validator.ValidateMessage(activity);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_Missing_Code()
    {
        // Arrange
        var activity = ParseActivity("Testdata/activity_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(activity);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "code" && v.Error == "Code is required.");
    }

    [Fact]
    public void Should_Fail_For_Invalid_Code_Pattern()
    {
        // Arrange
        var activity = ParseActivity("Testdata/activity_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(activity);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "code" && v.Error == "Code must be 2 letters.");
    }

    [Fact]
    public void Should_Fail_For_Short_Name()
    {
        // Arrange
        var activity = ParseActivity("Testdata/activity_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(activity);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "name" && v.Error == "Name must be at least 2 characters long.");
    }
}
