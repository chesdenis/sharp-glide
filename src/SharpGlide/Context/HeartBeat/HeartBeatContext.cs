using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Renders;

namespace SharpGlide.Context.HeartBeat
{
    public abstract class HeartBeatContext : IHeartBeatContext
    {
        private readonly IGroupContext _groupContext;

        private static readonly object _syncRoot = new object();

        protected HeartBeatContext(IGroupContext groupContext)
        {
            _groupContext = groupContext;
        }
        
        public abstract int IdleTimeoutMs { get; set; }
        public abstract bool Idle { get; }
        public abstract bool Failed { get; }
        public abstract void UpdateStatus(int indentation = 0);
        public abstract void UpdateStatus(string key, string value);
        public List<ExpandoObject> GetStatus(IBasePart startPart)
        {
            var partTree = _groupContext.GetPartTree(startPart).ToList();
            
            foreach (var leaf in partTree)
            {
                leaf.Item2.Context.HeartBeatContext.UpdateStatus(leaf.Item1);
            }
            
            var partsStatusKeys = partTree
                .SelectMany(ss => ss.Item2.Status.Keys)
                .Distinct()
                .ToList();
            
            var dataToPlot = new List<ExpandoObject>();
            
            foreach (var child in partTree)
            {
                var partStatus = new ExpandoObject();
                var partStatusAsDict = (IDictionary<string, object>) partStatus;
            
                foreach (var partsStatusKey in partsStatusKeys)
                {
                    partStatusAsDict[partsStatusKey] =
                        child.Item2.Status.ContainsKey(partsStatusKey) ? child.Item2.Status[partsStatusKey] : string.Empty;
                }
            
                dataToPlot.Add(partStatus);
            }
            
            return dataToPlot;
        }

        public string GetStatusTable(IBasePart startPart)
        {
            lock (_syncRoot)
            {
                var stringBuilder = new StringBuilder();

                var statusInfo = startPart.Context.HeartBeatContext.GetStatus(startPart);

                if (!statusInfo.Any()) return string.Empty;

                stringBuilder.AppendLine(startPart.Name);
                var statusTable = ConsoleTable.FromDynamic(statusInfo);

                stringBuilder.AppendLine(statusTable.ToString());
             
                return stringBuilder.ToString();
            }
        }
    }
}