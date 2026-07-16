using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Exceptions
{
    internal class SDLException : Exception
    {
        public SDLException(string message) 
            : base(message)
        {}
    }
}
