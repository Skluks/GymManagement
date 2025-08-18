using GymManagement.Domain.Gyms;
using MediatR;
using ErrorOr;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandBehavior : IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand command, RequestHandlerDelegate<ErrorOr<Gym>> next,
        CancellationToken cancellationToken)
    {
        var validator = new CreateGymCommandCommandValidator();
        ValidationResult? result = await validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            return result.Errors.Select(x => Error.Validation(x.PropertyName, x.ErrorMessage)).ToList();
        }

        return await next();
    }
}
