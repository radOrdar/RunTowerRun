using Core;
using Cysharp.Threading.Tasks;
using Infrastructure;
using StaticData;
using UnityEngine;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem towerSpeedFx;

        private EventsProvider _eventsProvider;
        private AssetProvider _assetProvider;

        private ParticleSystem _fireWork;
        private int _towerHeight;

        public void Init(ProgressionUnit progressionUnit)
        {
            _eventsProvider = ProjectContext.I.EventsProvider;
            _assetProvider = ProjectContext.I.AssetProvider;
            _eventsProvider.HasteSwitch += SetEnabledSpeedFx;
            _eventsProvider.FinishPassed += OnFinishPassed;

            _towerHeight = progressionUnit.height;
            ParticleSystem.ShapeModule shapeModule = towerSpeedFx.shape;
            shapeModule.scale = new Vector3(4, _towerHeight, 1);
            shapeModule.position = Vector3.up * (_towerHeight / 2f);
        }

        private void OnDestroy()
        {
            _eventsProvider.HasteSwitch -= SetEnabledSpeedFx;
            _eventsProvider.FinishPassed -= OnFinishPassed;

            if (_fireWork != null)
            {
                _assetProvider.UnloadInstance(_fireWork.gameObject);
            }
        }

        public void SetEnabledSpeedFx(bool enable)
        {
            if (enable)
            {
                towerSpeedFx.Play();
            } else
            {
                towerSpeedFx.Stop();
            }
        }

        private async void OnFinishPassed()
        {
            SetEnabledSpeedFx(false);
            await UniTask.Delay(200);
            _fireWork = await _assetProvider.InstantiateAsync<ParticleSystem>(Constants.Assets.FIREWORK, transform);
            _fireWork.transform.localPosition = Vector3.up * _towerHeight;
            _fireWork.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }
}