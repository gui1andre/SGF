using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ItensFatura.Exceptions
{
    public class InvalidJustificativaException : Exception
    {
        public InvalidJustificativaException(string? message) : base(message)
        {
        }
    }
}
