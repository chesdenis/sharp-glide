﻿using XDataFlow.Parts;

namespace XDataFlow.Abstractions
{
    public interface IExecutable
    {
        void Execute(IRestartablePart part);
    }
}