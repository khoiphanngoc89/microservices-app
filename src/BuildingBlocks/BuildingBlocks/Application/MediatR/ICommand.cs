﻿using MediatR;

namespace BuildingBlocks.Application.MediatR;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}