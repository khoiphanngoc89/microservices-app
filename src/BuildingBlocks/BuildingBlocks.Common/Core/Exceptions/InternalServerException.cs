using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Common.Core.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException()
    {
    }

    public InternalServerException(string message)
        : base(message)
    {
    }

    public InternalServerException(string message, string details)
        : base(message)
    {
        this.Details = details;
    }

    public InternalServerException(string message, string details, Exception innerException)
        : base(message, innerException)
    {
        this.Details = details;
    }

    public string? Details { get; private set; }
}
