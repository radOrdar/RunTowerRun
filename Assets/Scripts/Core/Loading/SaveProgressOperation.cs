using System;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

namespace Core.Loading
{
    public class SaveProgressOperation : ILoadingOperation
    {
        public string Description => "Saving progress..";

        private readonly int level;

        public SaveProgressOperation(int level)
        {
            this.level = level;
        }

        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.2f);
        
            await UniTask.Delay(100);
            ProjectContext.I.UserContainer.Level = level;
            PlayerPrefs.SetInt("Level", level);
        }
    }
}
