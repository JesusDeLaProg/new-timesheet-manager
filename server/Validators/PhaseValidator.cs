

using System.Text.RegularExpressions;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;

namespace new_timesheet_manager_server.Validators;

public class PhaseValidator(ILogger<PhaseValidator> logger) : BaseValidator(logger)
{
    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not Phase pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not a Phase");
        }

        var violations = new List<ValidationError>();

        if (string.IsNullOrEmpty(pb.Code))
        {
            violations.Add(new ValidationError { Path = "code", Error = "Code is required." });
        }
        else if (!Regex.IsMatch(pb.Code, "^[a-zA-Z]{2}$"))
        {
            violations.Add(new ValidationError { Path = "code", Error = "Code must be 2 letters." });
        }

        if (string.IsNullOrEmpty(pb.Name))
        {
            violations.Add(new ValidationError { Path = "name", Error = "Name is required." });
        }
        else if (pb.Name.Length < 2)
        {
            violations.Add(new ValidationError { Path = "name", Error = "Name must be at least 2 characters long." });
        }

        if (pb.ActivitiesInPhase.Count == 0)
        {
            violations.Add(new ValidationError { Path = "activities_in_phase", Error = "At least one activity is required." });
        }
        else if (pb.ActivitiesInPhase.Distinct().Count() != pb.ActivitiesInPhase.Count)
        {
            violations.Add(new ValidationError { Path = "activities_in_phase", Error = "Activities in phase must be unique." });
        }

        return violations;
    }
}
