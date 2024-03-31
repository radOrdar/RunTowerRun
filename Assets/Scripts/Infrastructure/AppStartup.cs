using System;
using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Services;
using Services.Ads;
using Services.Asset;
using Services.Audio;
using Services.Events;
using Services.Factory;
using Services.Input;
using Services.Save;
using Services.ScreenLoading;
using Services.StaticData;
using StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class AppStartup : MonoBehaviour
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            if (SceneManager.GetActiveScene().name != Constants.Scenes.STARTUP)
            {
                await SceneManager.LoadSceneAsync(Constants.Scenes.STARTUP);
            }
            
            await RegisterServices();

            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new ConfigLoadingOperation());
            loadingOperations.Enqueue(new AssetInitializeOperation());
            loadingOperations.Enqueue(new MenuLoadingOperation());

           ServiceLocator.Instance.Get<ILoadingScreenProvider>().LoadAndDestroy(loadingOperations);
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.Dispose();
        }

        private async UniTask RegisterServices()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            
            AssetProvider assetProvider = new AssetProvider();
            StaticDataService staticDataService = new StaticDataService(assetProvider);
            GameFactory gameFactory = new GameFactory(assetProvider);

            serviceLocator.Register<IEventService>(new EventService());
            serviceLocator.Register<IInputService>(new InputService());
            serviceLocator.Register<IPersistentDataService>(new PersistentDataService());
            serviceLocator.Register<IAssetProvider>(assetProvider);
            serviceLocator.Register<IStaticDataService>(staticDataService);
            serviceLocator.Register<IGameFactory>(gameFactory);
            serviceLocator.Register<ILoadingScreenProvider>(new LoadingScreenProvider(gameFactory));
            
            SoundsData soundsData = await staticDataService.GetData<SoundsData>();
            serviceLocator.Register<IAudioService>(new AudioService(soundsData));

            AppConfigurationData appConfigurationData = await staticDataService.GetData<AppConfigurationData>();
            serviceLocator.Register<IAdsService>(new AdsService(appConfigurationData));
        }
    }
}