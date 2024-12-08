using FluentValidation;
using MediatR;
using ValidationException = SettlementService.Application.Exceptions.ValidationException;

namespace SettlementService.Application.Common.BehaviorPipeline;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(v => v.Errors.Any())
            .SelectMany(v => v.Errors)
            .ToList();

        if (failures.Count != 0) throw new ValidationException(failures);

        return await next();
    }
}