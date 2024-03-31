using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using Services.Asset;
using Services.Factory;

namespace Core.Loading
{
    public class AssetInitializeOperation : ILoadingOperation
    {
        public string Description => "Assets Initialization...";
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(.2f);
            IGameFactory gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
            // var tasks = new List<UniTask>
            // {
            //     assetProvider.LoadAssetAsync(Constants.Assets.OBSTACLE_BLOCK),
            //     assetProvider.LoadAssetAsync(Constants.Assets.OBSTACLE_FRAME),
            //     assetProvider.LoadAssetAsync(Constants.Assets.FRAME_GHOST_TOP),
            //     assetProvider.LoadAssetAsync(Constants.Assets.FRAME_GHOST_SIDE),
            //     assetProvider.LoadAssetAsync(Constants.Assets.TOWER_BLOCK_PF),
            //     assetProvider.LoadAssetAsync(Constants.Assets.SCORE_GAIN_FX_PF)
            // };

            await gameFactory.WarmupAsync();
            // await UniTask.WhenAll(tasks);
        }
        
    }
}
