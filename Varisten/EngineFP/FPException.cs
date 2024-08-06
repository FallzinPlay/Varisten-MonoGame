using System;

namespace EngineFP
{
    internal class FPException : ApplicationException
    {
        public FPException(string message) : base(message) { }
    }
}
