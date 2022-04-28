using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Flow
{
    public class FlowModelBuilder : IFlowModelBuilder
    {
        public FlowModel Configure<T>(
            IEnumerable<string> partsFiles,
            IEnumerable<string> tunnelsFiles,
            IEnumerable<string> settingsFiles) where T : IFlowModelEntryProvider, new()
        {
            throw new NotImplementedException();
            // var model = new Model();
            //
            // var configEntryProvider = new T();
            //
            // var configurationEntries = partsFiles.Union(tunnelsFiles)
            //     .Select(s => configEntryProvider.Read(s))
            //     .Select(s => configEntryProvider.Parse(s)).ToList();
            //
            // var availableTypes = DetectTypes(configurationEntries).ToArray();
            // //var instances = ComputeInstances(availableTypes);
            //
            // AddTunnels(model, availableTypes);
            //
            // return model;
        }
 
        private void AddTunnels(FlowModel flowModel, IEnumerable<Tuple<Type, string>> availableTypes)
        {
            var tunnels = availableTypes
                .Where(w => w.Item1.IsInterfaceOf(typeof(ITunnel)))
                .ToList();

            foreach (var tunnel in tunnels)
            {
                var defaultConstructor = tunnel.Item1.GetConstructor(
                    Type.EmptyTypes);

                if (defaultConstructor != null)
                {
                    var configure = Activator.CreateInstance(tunnel.Item1) as ITunnel;

                    flowModel.AddTunnel(configure, tunnel.Item2);
                }
                else
                {
                    var availableConstructor = tunnel.Item1.GetConstructors();
                }
            }
        }

        private IEnumerable<Tuple<Type, string>> DetectTypes(IEnumerable<ConfigurationEntry> configurationEntries)
        {
            var retVal = new List<Tuple<Type, string>>();
            var assemblies = new Dictionary<string, Assembly>();
            foreach (var configurationEntry in configurationEntries)
            {
                try
                {
                    if (!assemblies.ContainsKey(configurationEntry.AssemblyLocation))
                    {
                        assemblies[configurationEntry.AssemblyLocation] =
                            Assembly.LoadFrom(configurationEntry.AssemblyLocation);
                    }

                    var assembly = assemblies[configurationEntry.AssemblyLocation];
                    var type = assembly.GetType(configurationEntry.FullName);
                    var instanceName = configurationEntry.InstanceName;
                    retVal.Add(new Tuple<Type, string>(type, instanceName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return retVal;
        }
    }
}