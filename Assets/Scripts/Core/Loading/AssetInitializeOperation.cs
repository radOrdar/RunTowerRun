using System;
using System.Collections.Generic;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Infrastructure;

public class AssetInitializeOperation : ILoadingOperation
{
    public string Description => "Assets Initialization...";
    public async UniTask Load(Action<float> onProgress)
    {
        onProgress(.2f);
        var assetProvider = ProjectContext.I.AssetProvider;
        var tasks = new List<UniTask>();
        tasks.Add(assetProvider.LoadAsync(Constants.Assets.OBSTACLE_BLOCK));
        tasks.Add(assetProvider.LoadAsync(Constants.Assets.OBSTACLE_FRAME));
        tasks.Add(assetProvider.LoadAsync(Constants.Assets.FRAME_GHOST_TOP));
        tasks.Add(assetProvider.LoadAsync(Constants.Assets.FRAME_GHOST_SIDE));
        tasks.Add(assetProvider.LoadAsync(Constants.Assets.TOWER_BLOCK_PF));
        tasks.Add(assetProvider.LoadAsync(Constants.Assets.SCORE_GAIN_FX_PF));

        await UniTask.WhenAll(tasks);
    }
}
