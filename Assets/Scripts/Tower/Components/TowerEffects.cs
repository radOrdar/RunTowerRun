using Cysharp.Threading.Tasks;
using Services;
using Services.Asset;
using Services.Events;
using StaticData;
using UnityEngine;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem towerSpeedFx;

        private IEventService _eventService;
        private IAssetProvider _assetProvider;

        private ParticleSystem _fireWork;
        private int _towerHeight;

        public void Init(ProgressionUnit progressionUnit)
        {
            _eventService = ServiceLocator.Instance.Get<IEventService>();
            _assetProvider = ServiceLocator.Instance.Get<IAssetProvider>();
            _eventService.HasteSwitch += SetEnabledSpeedFx;
            _eventService.FinishPassed += OnFinishPassed;

            _towerHeight = progressionUnit.height;
            ParticleSystem.ShapeModule shapeModule = towerSpeedFx.shape;
            shapeModule.scale = new Vector3(4, _towerHeight, 1);
            shapeModule.position = Vector3.up * (_towerHeight / 2f);
        }

        private void OnDestroy()
        {
            if (_eventService != null)
            {
                _eventService.HasteSwitch -= SetEnabledSpeedFx;
                _eventService.FinishPassed -= OnFinishPassed;
            }

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