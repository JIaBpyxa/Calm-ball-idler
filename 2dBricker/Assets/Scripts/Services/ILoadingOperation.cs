using System;

namespace Vorval.CalmBall.Service
{
    public interface ILoadingOperation
    {
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }
    }
}