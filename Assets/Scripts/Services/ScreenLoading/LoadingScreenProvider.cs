using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Services.Factory;

namespace Services.ScreenLoading
{
    public class LoadingScreenProvider : ILoadingScreenProvider
    {
        private IGameFactory _gameFactory;

        public LoadingScreenProvider(IGameFactory gameFactory) => 
            _gameFactory = gameFactory;

        public async UniTask LoadAndDestroy(ILoadingOperation loadingOperation)
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(loadingOperation);
            await LoadAndDestroy(operations);
        }

        public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
        {
            // LoadingScreen loadingScreen = await _gameFactory.InstantiateAsync<LoadingScreen>(Constants.Assets.LOADING_SCREEN);
            LoadingScreen loadingScreen = await _gameFactory.GetLoadingScreenAsync();
            await loadingScreen.Load(loadingOperations);
            _gameFactory.ReleaseInstance(loadingScreen);
        }
    }
}
