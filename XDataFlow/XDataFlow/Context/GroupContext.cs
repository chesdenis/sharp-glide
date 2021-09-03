using System;
using System.Collections.Generic;
using System.Linq;
using XDataFlow.Parts.Abstractions;

namespace XDataFlow.Context
{
    public class GroupContext : IGroupContext
    {
        public IDictionary<string, IBasePart> Children { get; } = new Dictionary<string, IBasePart>();
        public void AddChild(IBasePart part)
        {
            Children.Add(part.Name, part);
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
 
        public IEnumerable<IBasePart> GetChildren() => this.Children.Select(s => s.Value);
        public IEnumerable<Tuple<int, IBasePart>> GetChildrenTree(int parentLevel = 0)
        {
            var total = new List<Tuple<int, IBasePart>>();
            var currentLevel = parentLevel;

            foreach (var nameAndPart in Children)
            {
                var groupContext = ((BasePart)nameAndPart.Value).Context.GroupContext;

                var childrenTree = groupContext.GetChildrenTree(++currentLevel);
                
                total.AddRange(childrenTree);
                
                total.Add(new Tuple<int, IBasePart>(currentLevel, nameAndPart.Value));
            }

            return total;
        }


        public void EnumerateParts(Action<IBasePart> partAction, bool recursive)
        {
            foreach (var part in this.GetChildren())
            {
                partAction(part);
                
                if (recursive)
                {
                    part.Context.GroupContext.EnumerateParts(partAction, true);
                }
            }
        }
    }
}