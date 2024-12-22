﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects;

public sealed record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;
    
    public static CustomerId Of(Guid? value)
    {
        DomainException.ThrowIfNullOrEmpty(value, nameof(CustomerId));
        return new CustomerId(value.Value);
    }
}