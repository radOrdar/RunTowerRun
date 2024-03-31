using Core.Loading;
using Cysharp.Threading.Tasks;
using Obstacle;
using Tower.Components;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Services.Factory
{
    public interface IGameFactory : IService
    {
        UniTask WarmupAsync();
        UniTask<Finish> GetFinishAsync(Vector3 at);
        UniTask<ObstacleBlock> GetObstacleBlockAsync(Vector3 localPos, Transform parent);
        UniTask<ObstacleFrame> GetObstacleFrameAsync(Vector3 localPos, Quaternion globalRotation, Vector3 scale, Transform parent);
        UniTask<ParticleSystem> GetObstacleGhostAsync(Vector3 localPos, Quaternion globalRotation, Transform parent, int count);
        UniTask<TowerBlock> GetTowerBlockAsync(Vector3 localPos, Transform parent);
        UniTask<ScoreGainFx> GetScoreGainFxAsync(Transform parent, int score);
        UniTask<ParticleSystem> GetFireworkAsync(Vector3 localPos, Quaternion localRotation, Transform parent);
        UniTask<UiPopup> GetUiPopupAsync();
        UniTask<LoadingScreen> GetLoadingScreenAsync();
        void ReleaseInstance<T>(T instance) where T : Component;
    }
}