using System;
using Cysharp.Threading.Tasks;
using Services;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Core.Loading
{
    public class ConfigLoadingOperation : ILoadingOperation
    {
        public string Description => "Loading Configuration...";
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.1f);
            IStaticDataService staticDataService = ServiceLocator.Instance.Get<IStaticDataService>();
            AppConfigurationData appData = await staticDataService.GetData<AppConfigurationData>();
            Application.targetFrameRate = appData.targetFPS;
            await UniTask.Delay(500);
        }
    }
}
