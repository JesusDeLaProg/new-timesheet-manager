
using Google.Protobuf;
using System.Collections.Generic;
using System;
using System.Linq;

namespace new_timesheet_manager_server.Validators;

public class EmployeeValidator(ILogger<EmployeeValidator> logger) : BaseValidator(logger)
{
    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not Employee pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not an Employee");
        }

        var violations = new List<ValidationError>();

        if (string.IsNullOrEmpty(pb.FirstName))
        {
            violations.Add(new ValidationError { Path = "first_name", Error = "First name is required." });
        }

        if (string.IsNullOrEmpty(pb.LastName))
        {
            violations.Add(new ValidationError { Path = "last_name", Error = "Last name is required." });
        }

        violations.AddRange(ValidateRoles(pb));

        return violations;
    }

    private static IEnumerable<ValidationError> ValidateRoles(Employee employee)
    {
        var violations = new List<ValidationError>();
        var rolesByProjectType = employee.Roles.GroupBy(r => r.ProjectType);

        foreach (var group in rolesByProjectType)
        {
            var roles = group.ToList();

            if (roles.Count(r => r.End is null) != 1)
            {
                violations.Add(new ValidationError { Path = "roles", Error = $"Exactly one active role is required for project type {group.Key}" });
            }

            if (roles.Count(r => r.Start is null) != 1)
            {
                violations.Add(new ValidationError { Path = "roles", Error = $"Exactly one 'first' role is required for project type {group.Key}" });
            }

            for (var i = 0; i < roles.Count; i++)
            {
                var role = roles[i];

                if (string.IsNullOrEmpty(role.Title))
                {
                    violations.Add(new ValidationError { Path = $"roles.{i}.title", Error = "Role title is required." });
                }
                else if (role.Title.Length < 3)
                {
                    violations.Add(new ValidationError { Path = $"roles.{i}.title", Error = "Role title must be at least 3 characters long." });
                }

                if (role.ProjectType == Project.Types.Type.Unspecified)
                {
                    violations.Add(new ValidationError { Path = $"roles.{i}.project_type", Error = "Role project type must be specified." });
                }

                if (role.HourlyRate == null)
                {
                    violations.Add(new ValidationError { Path = $"roles.{i}.hourly_rate", Error = "Hourly rate is required." });
                }
                else
                {
                    if (string.IsNullOrEmpty(role.HourlyRate.CurrencyCode))
                    {
                        violations.Add(new ValidationError { Path = $"roles.{i}.hourly_rate.currency_code", Error = "hourly_rate should have a currency code" });
                    }
                    if (role.HourlyRate.Units <= 0 && role.HourlyRate.Nanos <= 0)
                    {
                        violations.Add(new ValidationError { Path = $"roles.{i}.hourly_rate", Error = "hourly_rate should be a positive amount" });
                    }
                }

                for (var j = i + 1; j < roles.Count; j++)
                {
                    var role2 = roles[j];

                    var start1 = role.Start?.ToDateTime() ?? DateTime.MinValue;
                    var end1 = role.End?.ToDateTime() ?? DateTime.MaxValue;
                    var start2 = role2.Start?.ToDateTime() ?? DateTime.MinValue;
                    var end2 = role2.End?.ToDateTime() ?? DateTime.MaxValue;

                    if (start1 < end2 && start2 < end1)
                    {
                        violations.Add(new ValidationError { Path = "roles", Error = "Employee roles cannot have overlapping dates for the same project type" });
                    }
                }
            }
        }

        return violations;
    }
}
