using System;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

namespace Core.Loading
{
    public class ConfigLoadingOperation : ILoadingOperation
    {
        public string Description => "Loading Configuration...";
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.1f);
            Application.targetFrameRate = ProjectContext.I.StaticDataProvider.AppConfigurationData.targetFPS;
            await UniTask.Delay(500);
        }
    }
}
