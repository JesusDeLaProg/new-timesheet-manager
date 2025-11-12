using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class EmployeeValidatorTests
{
    private readonly EmployeeValidator _validator;

    public EmployeeValidatorTests()
    {
        var logger = new Mock<ILogger<EmployeeValidator>>().Object;
        _validator = new EmployeeValidator(logger);
    }

    private static Employee ParseEmployee(string path)
    {
        var text = System.IO.File.ReadAllText(path);
        return JsonParser.Default.Parse<Employee>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_Employee()
    {
        // Arrange
        var employee = ParseEmployee("Testdata/employee.json");

        // Act
        var result = _validator.ValidateMessage(employee);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_Missing_FirstName()
    {
        // Arrange
        var employee = ParseEmployee("Testdata/employee_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(employee);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "first_name" && v.Error == "First name is required.");
    }

    [Fact]
    public void Should_Fail_For_Two_Active_Roles()
    {
        // Arrange
        var employee = ParseEmployee("Testdata/employee_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(employee);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "roles" && v.Error.Contains("Exactly one active role is required"));
    }

    [Fact]
    public void Should_Fail_For_Short_Role_Title()
    {
        // Arrange
        var employee = ParseEmployee("Testdata/employee_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(employee);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "roles.0.title" && v.Error == "Role title must be at least 3 characters long.");
    }
}
