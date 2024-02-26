using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Services.Asset;

namespace Services.ScreenLoading
{
    public class LoadingScreenProvider : ILoadingScreenProvider
    {
        private IAssetProvider _assetProvider;

        public LoadingScreenProvider(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public async UniTask LoadAndDestroy(ILoadingOperation loadingOperation)
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(loadingOperation);
            await LoadAndDestroy(operations);
        }

        public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
        {
            LoadingScreen loadingScreen = await _assetProvider.InstantiateAsync<LoadingScreen>(Constants.Assets.LOADING_SCREEN);
            
            await loadingScreen.Load(loadingOperations);
            _assetProvider.UnloadInstance(loadingScreen.gameObject);
        }
    }
}
