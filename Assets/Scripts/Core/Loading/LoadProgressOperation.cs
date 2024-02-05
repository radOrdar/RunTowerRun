using System;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

namespace Core.Loading
{
    public class LoadProgressOperation : ILoadingOperation
    {
        public string Description => "Loading progress..";
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.15f);
            await UniTask.Delay(500);
            UserContainer userContainer = ProjectContext.I.UserContainer;
            userContainer.IsSubscriber = PlayerPrefs.GetInt("IsSubscriber") == 1;
            userContainer.Level = PlayerPrefs.GetInt("Level");
        }
    }
}
