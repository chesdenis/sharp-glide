using System;

namespace SharpGlide.Parts
{
    public interface IPartChildrenSupport
    {   
        IBasePart AddChild(IBasePart part);
        IBasePart GetChild(string name, bool recursive = false);
        void EnumerateChildren(Action<IBasePart> partAction, bool recursive = false);
    }
}