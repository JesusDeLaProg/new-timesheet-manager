using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class UserValidatorTests
{
    private readonly UserValidator _validator;

    public UserValidatorTests()
    {
        var logger = new Mock<ILogger<UserValidator>>().Object;
        _validator = new UserValidator(logger);
    }

    private static User ParseUser(string path)
    {
        var text = System.IO.File.ReadAllText(path);
        return JsonParser.Default.Parse<User>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_User()
    {
        // Arrange
        var user = ParseUser("Testdata/user.json");

        // Act
        var result = _validator.ValidateMessage(user);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_Short_Username()
    {
        // Arrange
        var user = ParseUser("Testdata/user_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(user);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "username" && v.Error == "Username must be at least 3 characters long.");
    }

    [Fact]
    public void Should_Fail_For_Invalid_Email()
    {
        // Arrange
        var user = ParseUser("Testdata/user_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(user);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "email" && v.Error == "Invalid email format.");
    }

    [Fact]
    public void Should_Fail_For_Unspecified_Role()
    {
        // Arrange
        var user = ParseUser("Testdata/user_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(user);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "role" && v.Error == "User role must be specified.");
    }
}
