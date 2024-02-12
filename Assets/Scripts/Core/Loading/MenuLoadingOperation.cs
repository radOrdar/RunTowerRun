using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Loading
{
    public class MenuLoadingOperation : ILoadingOperation
    {
        public string Description => "Main menu loading...";
        
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress?.Invoke(0.5f);
            await SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, LoadSceneMode.Additive);
        }
    }
}
