using System;

namespace Kin.Sdk
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {}

        public OperationFailedException(string message, Exception e) : base(message, e)
        {}
    }
}
