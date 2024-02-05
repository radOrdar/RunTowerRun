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
        [SerializeField] private ParticleSystem fireWorkPf;

        private EventsProvider _eventsProvider;

        private int _towerHeight;

        public void Init(ProgressionUnit progressionUnit)
        {
            _towerHeight = progressionUnit.height;
            ParticleSystem.ShapeModule shapeModule = towerSpeedFx.shape;
            shapeModule.scale = new Vector3(4, _towerHeight, 1);
            shapeModule.position = Vector3.up * (_towerHeight / 2f);
        }
        
        private void Start()
        {
            _eventsProvider = ProjectContext.I.EventsProvider;
            _eventsProvider.HasteSwitch += SetEnabledSpeedFx;
            _eventsProvider.FinishPassed += OnFinishPassed;
        }

        private void OnDestroy()
        {
            _eventsProvider.HasteSwitch -= SetEnabledSpeedFx;
            _eventsProvider.FinishPassed -= OnFinishPassed;
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
            var firework = Instantiate(fireWorkPf, transform);
            firework.transform.localPosition = Vector3.up * _towerHeight;
        }
    }
}
