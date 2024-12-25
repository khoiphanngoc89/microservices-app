using FluentValidation;
using MediatR;

namespace BuildingBlocks.Application.MediatR.Behaviors;

/// <summary>
/// All commands will have be validated and get error messages
/// query does not have as normal. <see cref="IPipelineBehavior{TRequest, TResponse}"> work like middleware
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="validators"></param>
/// <marks>
/// <see cref="ICommand{TRequest}"/>
/// it will cause a bug when the CreateProduct request invoke
/// the FluentValidator would be not called
/// see back <see cref="ICommand"/>
/// </marks>
public sealed class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.Where(x => x.Errors.Any()).SelectMany(x => x.Errors).ToList();
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
