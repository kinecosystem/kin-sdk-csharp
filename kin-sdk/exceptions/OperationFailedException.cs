using System;

namespace kin_sdk
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {}

        public OperationFailedException(string message, Exception e) : base(message, e)
        {}
    }
}
