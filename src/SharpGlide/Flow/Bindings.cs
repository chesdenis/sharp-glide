using System;
using System.Collections.Generic;
using SharpGlide.Parts;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Flow
{
    public class Bindings : Dictionary<IRoute, Tuple<IBasePart, ITunnel>>
    {
        
    }
}