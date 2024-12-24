namespace Basket.Api.Domain;

public record BasketCreatedDomainEvent(Guid Id)
    : DomainEvent(Id);

