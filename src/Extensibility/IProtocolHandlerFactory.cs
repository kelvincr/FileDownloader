using System;
using System.Collections.Generic;
using System.Text;

namespace Extensibility
{
    public interface IProtocolHandlerFactory
    {
        IProtocolHandler GetHandler(Uri uri);
    }
}
