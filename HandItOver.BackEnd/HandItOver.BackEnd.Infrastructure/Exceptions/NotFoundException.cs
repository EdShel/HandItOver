using System;

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

    public class RecordAlreadyExistsException : InvalidOperationException
    {
        public RecordAlreadyExistsException(string recordName)
            :base($"{recordName} already exists.")
        {
        }
    }

    public class ExpiredException : InvalidOperationException
    {
        public ExpiredException(string whatHasExpired)
            : base($"{whatHasExpired} has expired.")
        {
        }
    }

    public class NoRightsException : InvalidOperationException
    {
        public NoRightsException(string action)
            : base($"You're not allowed to {action}.")
        {
        }
    }

    public class NotOwnerException : InvalidOperationException
    {
        public NotOwnerException(string item)
            : base($"{item} doesn't belong to you.")
        {
        }
    }

    public class OperationException : InvalidOperationException
    {
        public OperationException(string message)
            : base(message)
        {
        }
    }
}
