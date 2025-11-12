using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class ClientValidatorTests
{
    private readonly ClientValidator _validator;

    public ClientValidatorTests()
    {
        var logger = new Mock<ILogger<ClientValidator>>().Object;
        _validator = new ClientValidator(logger);
    }

    private static Client ParseClient(string path)
    {
        var text = System.IO.File.ReadAllText(path);
        return JsonParser.Default.Parse<Client>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_Client()
    {
        // Arrange
        var client = ParseClient("Testdata/client.json");

        // Act
        var result = _validator.ValidateMessage(client);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_Missing_Name()
    {
        // Arrange
        var client = ParseClient("Testdata/client_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(client);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "name" && v.Error == "Name is required.");
    }

    [Fact]
    public void Should_Fail_For_Unspecified_Project_Type()
    {
        // Arrange
        var client = ParseClient("Testdata/client_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(client);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "default_project_type" && v.Error == "Default project type must be specified.");
    }

    [Fact]
    public void Should_Fail_For_Empty_Name()
    {
        // Arrange
        var client = ParseClient("Testdata/client_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(client);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "name" && v.Error == "Name is required.");
    }
}
