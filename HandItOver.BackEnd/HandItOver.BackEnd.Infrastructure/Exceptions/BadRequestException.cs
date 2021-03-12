using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;

namespace HandItOver.BackEnd.Infrastructure.Exceptions
{
    public class NotFoundException : InvalidOperationException
    {
        public NotFoundException(string itemName)
            : base($"{itemName} is not found.")
        {
        }
    }

    public class WrongValueException : InvalidOperationException
    {
        public WrongValueException(string parameterName)
            : base($"{parameterName} is invalid.")
        {

        }
    }
}
