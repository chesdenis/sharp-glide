using System;
using System.Collections.Generic;
using SharpGlide.Parts;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.FlowSchema
{
    public class Bindings : Dictionary<IRoute, Tuple<IBasePart, ITunnel>>
    {
        
    }
}