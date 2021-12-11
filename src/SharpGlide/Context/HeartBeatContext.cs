using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using SharpGlide.Context.Abstractions;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Renders;

namespace SharpGlide.Context
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
        public abstract void UpdateStatus();
        public abstract void UpdateStatus(string key, string value);
        public List<ExpandoObject> GetStatus(IBasePart startPart)
        {
            var partTree = _groupContext.GetPartTree(startPart).ToList();
            
            foreach (var leaf in partTree)
            {
                leaf.Item2.Context.HeartBeatContext.UpdateStatus();
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

        public string GetException(IBasePart startPart)
        {
            var result = new List<string>();
            
            startPart.EnumerateChildren(part => result.AddRange(
                part.Errors
                    .ToList()), true);
             
            return string.Join("\n", result);
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

        public string GetExceptionList(IBasePart startPart)
        {
            lock (_syncRoot)
            {
                var stringBuilder = new StringBuilder();

                var exceptionList = startPart.Context.HeartBeatContext.GetException(startPart);

                if (!exceptionList.Any()) return string.Empty;

                stringBuilder.AppendLine(startPart.Name);

                stringBuilder.AppendLine(exceptionList);
             
                return stringBuilder.ToString();
            }
        }
    }
}