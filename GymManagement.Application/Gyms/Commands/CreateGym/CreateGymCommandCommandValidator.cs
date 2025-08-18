using FluentValidation;
using FluentValidation.Validators;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}