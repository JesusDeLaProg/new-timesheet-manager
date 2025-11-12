using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class ProjectValidatorTests
{
    private readonly ProjectValidator _validator;

    public ProjectValidatorTests()
    {
        var logger = new Mock<ILogger<ProjectValidator>>().Object;
        _validator = new ProjectValidator(logger);
    }

    private static Project ParseProject(string path)
    {
        var text = System.IO.File.ReadAllText(path);
        return JsonParser.Default.Parse<Project>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_Project()
    {
        // Arrange
        var project = ParseProject("Testdata/project.json");

        // Act
        var result = _validator.ValidateMessage(project);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_Invalid_Code()
    {
        // Arrange
        var project = ParseProject("Testdata/project_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(project);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "code" && v.Error == "Code must match pattern '^[0-9]{2,4}-[0-9]{2,}$'.");
    }

    [Fact]
    public void Should_Fail_For_Missing_ClientId()
    {
        // Arrange
        var project = ParseProject("Testdata/project_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(project);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "client_id" && v.Error == "Client ID is required.");
    }

    [Fact]
    public void Should_Fail_For_Unspecified_Type()
    {
        // Arrange
        var project = ParseProject("Testdata/project_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(project);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "type" && v.Error == "Project type must be specified.");
    }
}
