using System.Text;
using Google.Protobuf;
using Grpc.Core;
using new_timesheet_manager_server;

namespace new_timesheet_manager_server.Services;

public class ActivityServiceImpl(ILogger<ActivityServiceImpl> logger, ProtoValidate.IValidator validator) : ActivityService.ActivityServiceBase
{
    private readonly ILogger<ActivityServiceImpl> _logger = logger;
    private readonly ProtoValidate.IValidator _validator = validator;

    private ProtoValidate.ValidationResult ValidateInternal(Activity activity)
    {
        return _validator.Validate(activity, false);
    }

    public override Task<ValidateActivityResponse> Validate(ValidateActivityRequest request, ServerCallContext context)
    {
        var violations = ValidateInternal(request.Activity);

        if (violations.Violations.Count > 0)
        {
            var result = new ValidateActivityResponse { };
            result.Errors.AddRange(violations.Violations.Select(v => new ValidationError { Path = v.FieldPath, Error = v.Message }));
            return Task.FromResult(result);
        }
        else
        {
            return Task.FromResult(new ValidateActivityResponse { });
        }
    }


    public override Task<FetchActivitiesResponse> Fetch(FetchActivitiesRequest request, ServerCallContext context)
    {
        using (_logger.BeginScope("Processing request: {Request}", request))
        {
            var response = new FetchActivitiesResponse { };
            response.Activities.Add(new Activity
            {
                Id = ByteString.CopyFromUtf8("abc123"),
                Code = "GE",
                Name = "Général"
            });
            return Task.FromResult(response);
        }
    }
}
