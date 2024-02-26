using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;

namespace Services.ScreenLoading
{
    public interface ILoadingScreenProvider : IService
    {
        UniTask LoadAndDestroy(ILoadingOperation loadingOperation);
        UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations);
    }
}