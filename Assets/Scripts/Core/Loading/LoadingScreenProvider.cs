using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Core.Loading
{
    public class LoadingScreenProvider
    {
        public async UniTask LoadAndDestroy(ILoadingOperation loadingOperation)
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(loadingOperation);
            await LoadAndDestroy(operations);
        }

        public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
        {
            AssetProvider assetProvider = ProjectContext.I.AssetProvider;
            LoadingScreen loadingScreen = await assetProvider.InstantiateAsync<LoadingScreen>(Constants.Assets.LOADING_SCREEN);
            
            await loadingScreen.Load(loadingOperations);
            assetProvider.Unload(loadingScreen.gameObject);
        }
    }
}
