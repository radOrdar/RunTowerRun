using System;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

namespace Core.Loading
{
    public class ResetProgressOperation : ILoadingOperation
    {
        public string Description => "Reset progress..";
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.15f);
            await UniTask.Delay(500);
            UserContainer userContainer = ProjectContext.I.UserContainer;
            PlayerPrefs.SetInt("Level", 0);
            userContainer.Level = 0;
        }
    }
}
