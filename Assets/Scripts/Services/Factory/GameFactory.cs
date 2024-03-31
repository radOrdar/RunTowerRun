using System;
using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Obstacle;
using Services.Asset;
using Tower.Components;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

namespace Services.Factory
{
    class GameFactory : IGameFactory, IDisposable
    {
        private IAssetProvider _assetProvider;
        private ObjectPool<ScoreGainFx> _poolScoreGainFx;
        
        private ObstacleBlock _obstaclePf;
        private ObstacleFrame _frameObstaclePf;
        private ParticleSystem _frameGhost;
        private TowerBlock _blockPf;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask WarmupAsync()
        {
            // var tasks = new List<UniTask>
            // {
            //     _assetProvider.LoadAssetAsync(Constants.Assets.OBSTACLE_BLOCK),
            //     _assetProvider.LoadAssetAsync(Constants.Assets.OBSTACLE_FRAME),
            //     _assetProvider.LoadAssetAsync(Constants.Assets.FRAME_GHOST),
            //     _assetProvider.LoadAssetAsync(Constants.Assets.TOWER_BLOCK_PF),
            //     _assetProvider.LoadAssetAsync(Constants.Assets.SCORE_GAIN_FX_PF)
            // };
            
            ScoreGainFx gainFxPf = await _assetProvider.LoadComponentAsync<ScoreGainFx>(Constants.Assets.SCORE_GAIN_FX_PF);
            _poolScoreGainFx = new ObjectPool<ScoreGainFx>(
                createFunc: () =>
                {
                    ScoreGainFx s = Object.Instantiate(gainFxPf);
                    s.Origin = _poolScoreGainFx;
                    return s;
                },
                actionOnGet: s =>
                {
                    s.gameObject.SetActive(true);
                },
                actionOnRelease: g => g.gameObject.SetActive(false)
            );

            // await UniTask.WhenAll(tasks);
            
            _obstaclePf = await _assetProvider.LoadComponentAsync<ObstacleBlock>(Constants.Assets.OBSTACLE_BLOCK);
            _frameObstaclePf = await _assetProvider.LoadComponentAsync<ObstacleFrame>(Constants.Assets.OBSTACLE_FRAME);
            _frameGhost = await _assetProvider.LoadComponentAsync<ParticleSystem>(Constants.Assets.FRAME_GHOST);
            _blockPf = await _assetProvider.LoadComponentAsync<TowerBlock>(Constants.Assets.TOWER_BLOCK_PF);
        }

        public async UniTask<Finish> GetFinishAsync(Vector3 at)
        {
            return await _assetProvider.InstantiateAsync<Finish>(Constants.Assets.FINISH_LINE, at , Quaternion.identity);
        }

        public async UniTask<ObstacleBlock> GetObstacleBlockAsync(Vector3 localPos, Transform parent)
        {
            ObstacleBlock obsBlock = Object.Instantiate(_obstaclePf, parent);
            obsBlock.transform.localPosition = localPos;
            return await UniTask.FromResult(obsBlock);
        }

        public async UniTask<ObstacleFrame> GetObstacleFrameAsync(Vector3 localPos, Quaternion globalRotation, Vector3 scale, Transform parent)
        {
            ObstacleFrame obstacleFrame = Object.Instantiate(_frameObstaclePf, parent);
            Transform transform = obstacleFrame.transform;
            transform.localPosition = localPos;
            transform.localScale = scale;
            transform.rotation = globalRotation;
            return await UniTask.FromResult(obstacleFrame);
        }

        public UniTask<ParticleSystem> GetObstacleGhostAsync(Vector3 localPos, Quaternion globalRotation, Transform parent, int count)
        {
            ParticleSystem ghostFx = Object.Instantiate(_frameGhost, parent);
            ghostFx.transform.localPosition = localPos;
            ghostFx.transform.rotation = globalRotation;
            var sideFxLeftEmission = ghostFx.emission;
            sideFxLeftEmission.SetBurst(0, new ParticleSystem.Burst(0, (short)count, (short)count, int.MaxValue, 1));
            var sideFxShapeLeft = ghostFx.shape;
            sideFxShapeLeft.radius = count / 2f;
            return UniTask.FromResult(ghostFx);
        }

        public UniTask<TowerBlock> GetTowerBlockAsync(Vector3 localPos, Transform parent)
        {
            TowerBlock obstacleBlock = Object.Instantiate(_blockPf, parent);
            obstacleBlock.transform.localPosition = localPos;
            return UniTask.FromResult(obstacleBlock);
        }

        public async UniTask<ScoreGainFx> GetScoreGainFxAsync(Transform parent, int score)
        {
            ScoreGainFx scoreGainFx = _poolScoreGainFx.Get();
            scoreGainFx.Init(parent, score);
            return await UniTask.FromResult(scoreGainFx);
        }

        public async UniTask<ParticleSystem> GetFireworkAsync(Vector3 localPos, Quaternion localRotation, Transform parent)
        {
            ParticleSystem _fireWork = await _assetProvider.InstantiateAsync<ParticleSystem>(Constants.Assets.FIREWORK, parent);
            _fireWork.transform.localPosition = localPos;
            _fireWork.transform.localRotation = localRotation;
            return await UniTask.FromResult(_fireWork);
        }

        public async UniTask<UiPopup> GetUiPopupAsync()
        {
            return await _assetProvider.InstantiateAsync<UiPopup>(Constants.Assets.UI_POPUP);
        }

        public void ReleaseInstance<T>(T instance) where T : Component
        {
            _assetProvider.ReleaseInstance(instance.gameObject);
        }

        public async UniTask<LoadingScreen> GetLoadingScreenAsync()
        {
            return await _assetProvider.InstantiateAsync<LoadingScreen>(Constants.Assets.LOADING_SCREEN);
        }


        public void Dispose()
        {
            _assetProvider.Dispose();
        }
    }
}