using System.Linq;
using SharpGlide.Context.Abstractions;
using SharpGlide.Extensions;
using SharpGlide.Model;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context
{
    public abstract class HeartBeatContext : IHeartBeatContext
    {
        private readonly IGroupContext _groupContext;

        protected HeartBeatContext(IGroupContext groupContext)
        {
            _groupContext = groupContext;
        }

        public abstract int IdleTimeoutMs { get; set; }
        public abstract bool Idle { get; }
        public abstract bool Failed { get; }
        public abstract void Collect();
        public abstract void Collect(string key, string value);

        public string ReportAsXml(IBasePart startPart)
        {
            var tree = _groupContext.GetPartTree(startPart).ToList();

            foreach (var leaf in tree)
            {
                leaf.Item2.Context.HeartBeatContext.Collect();
            }

            var statusTree = tree.Select(
                s => new HeartBeat()
                {
                    Level = s.Item1,
                    Exceptions = s.Item2.Context.MetaDataContext.Exceptions.ToArray(),
                    Data = s.Item2.Context.MetaDataContext.Status.Select(ss => new HeartBeatEntry()
                    {
                        Name = ss.Key,
                        Value = ss.Value
                    }).ToArray()
                }).ToArray();

            return statusTree.AsXml();
        }
    }
}