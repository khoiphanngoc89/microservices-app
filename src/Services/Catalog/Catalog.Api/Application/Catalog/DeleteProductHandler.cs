﻿using BuildingBlocks.Application.MediatR;

namespace Catalog.Api.Application.Catalog;

public sealed record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;
public sealed record DeleteProductResult(bool IsSuccess);

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Product Id is required");
    }
}

internal sealed class DeleteProductHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}
