using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Context
{
    public class GroupContext : IGroupContext
    {
        public IDictionary<string, IBasePart> Children { get; } = new Dictionary<string, IBasePart>();

        public void AddChild(IBasePart child)
        {
            if (string.IsNullOrWhiteSpace(child.Name))
            {
                child.Name = child.GetType().Name;
            }

            if (Children.ContainsKey(child.Name))
            {
                child.Name = "Copy of " + child.Name;
            }
            
            Children.Add(child.Name, child);
        }

        public IBasePart GetChild(string name, bool recursive = false)
        {
            if (Children.ContainsKey(name))
            {
                return Children[name];
            }

            if (!recursive)
            {
                throw new KeyNotFoundException(name);
            }

            IBasePart deepChildren = null;

            foreach (var child in Children)
            {
                try
                {
                    deepChildren = child.Value.GetChild(name, true);
                }
                catch (KeyNotFoundException)
                {
                }
            }

            if (deepChildren == null)
            {
                throw new KeyNotFoundException(name);
            }

            return deepChildren;
        }

        private IEnumerable<IBasePart> GetChildren() => Children.Select(s => s.Value);
        
        public IEnumerable<Tuple<int, IBasePart>> GetPartTree(IBasePart parentPart, int parentLevel = 0)
        {
            var total = new List<Tuple<int, IBasePart>>();
            
            total.Add(new Tuple<int, IBasePart>(parentLevel, parentPart));

            parentLevel++;

            parentPart.EnumerateChildren(child =>
            {
                var childrenTree = child.Context.GroupContext.GetPartTree(child, parentLevel);
                total.AddRange(childrenTree);
            });

            return total;
        }
        
        public void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false)
        {
            foreach (var part in GetChildren())
            {
                partAction(part);
                
                if (recursive)
                {
                    part.Context.GroupContext.EnumerateChildren(partAction, true);
                }
            }
        }
    }
}