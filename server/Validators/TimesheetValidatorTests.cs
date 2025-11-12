using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Google.Protobuf;
using System.Linq;

namespace new_timesheet_manager_server.Validators.Tests;

public class TimesheetValidatorTests
{
    private readonly TimesheetValidator _validator;

    public TimesheetValidatorTests()
    {
        var logger = new Mock<ILogger<TimesheetValidator>>().Object;
        var configuration = new Mock<IConfiguration>().Object;
        _validator = new TimesheetValidator(logger, configuration);
    }

    private static Timesheet ParseTimesheet(string path)
    {
        var text = File.ReadAllText(path);
        return JsonParser.Default.Parse<Timesheet>(text);
    }

    [Fact]
    public void Should_Pass_For_Valid_Timesheet()
    {
        // Arrange
        var timesheet = ParseTimesheet("Testdata/timesheet.json");

        // Act
        var result = _validator.ValidateMessage(timesheet);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Should_Fail_For_Missing_Employee_Id()
    {
        // Arrange
        var timesheet = ParseTimesheet("Testdata/timesheet_invalid1.json");

        // Act
        var result = _validator.ValidateMessage(timesheet);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "employee_id" && v.Error == "Employee ID is required.");
    }

    [Fact]
    public void Should_Fail_For_Begin_Date_Not_Sunday()
    {
        // Arrange
        var timesheet = ParseTimesheet("Testdata/timesheet_invalid2.json");

        // Act
        var result = _validator.ValidateMessage(timesheet);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "begin" && v.Error == "Start date must be a Sunday");
    }

    [Fact]
    public void Should_Fail_For_Less_Than_14_Entries()
    {
        // Arrange
        var timesheet = ParseTimesheet("Testdata/timesheet_invalid3.json");

        // Act
        var result = _validator.ValidateMessage(timesheet);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, v => v.Path == "lines.0.entries" && v.Error == "There must be exactly 14 entries.");
    }
}
