using System;

namespace Vorval.CalmBall.Service
{
    public interface ILoadingOperation
    {
        public Action<ILoadingOperation> OnOperationFinished { get; set; }
    }
}