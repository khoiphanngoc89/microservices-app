using BuildingBlocks.Domains;

namespace Basket.Api.Domains;

public record BasketCreatedDomainEvent(Guid Id)
    : DomainEvent(Id);

