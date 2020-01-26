using System;

namespace Core.Lib.Backend.Exceptions
{
    public class MappingException : Exception
    {
        public MappingException() : base() { }

        public MappingException(string message) : base(message) { }

        public MappingException(string message, System.Exception inner) : base(message, inner) { }

    }
}
