using FluentValidation;

namespace SettlementService.Application.Settlement.Commands.CreateSettlement;

public class CreateSettlementCommandValidator : AbstractValidator<CreateSettlementCommand>
{
    public CreateSettlementCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(v => v.BookingTime)
            .NotEmpty()
            .Custom((time, context) =>
            { 
                if (!TimeOnly.TryParseExact(time, "HH:mm", out var militaryTime)) 
                    context.AddFailure("Invalid time value");
            });
    }
    
}

