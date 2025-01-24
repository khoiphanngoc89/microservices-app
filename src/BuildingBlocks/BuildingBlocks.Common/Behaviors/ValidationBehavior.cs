using BuildingBlocks.Common.Core;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TReponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TReponse>
    where TRequest : ICommand<TReponse>
{
    public async Task<TReponse> Handle(TRequest request, RequestHandlerDelegate<TReponse> next, CancellationToken cancellationToken)
    {
        var cxt = new ValidationContext<TRequest>(request);
        var validatedResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(cxt, cancellationToken)));
        var failures =
            validatedResults.Where(x => x.Errors.Count != 0)
            .SelectMany(x => x.Errors);

        if(failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
