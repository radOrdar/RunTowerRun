using System.Collections.Generic;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class AppStartup : MonoBehaviour
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            Application.targetFrameRate = 60;
            if (SceneManager.GetActiveScene().name != Constants.Scenes.STARTUP)
            {
                await SceneManager.LoadSceneAsync(Constants.Scenes.STARTUP);
            }
            
            ProjectContext.I.Initialize();

            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(ProjectContext.I.AssetProvider);
            loadingOperations.Enqueue(new LoadProgressOperation());
            loadingOperations.Enqueue(new MenuLoadingOperation());

            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(loadingOperations);
        }
    }
}
