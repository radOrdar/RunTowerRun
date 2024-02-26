using Cysharp.Threading.Tasks;
using Services;
using Services.Events;
using Services.Input;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Tower.Components
{
    public class TowerRotation : MonoBehaviour
    {
        [SerializeField] private Transform bodyTransform;

        private TowerConfigurationData _towerConfig;

        #region Dependencies

        private IInputService _inputService;
        private IEventService _eventService;
        private Camera _mainCamera;

        #endregion

        #region State

        private int currDirIndex;
        private Vector3 _prevMousePos;
        private Quaternion _targetRotation;
        private bool _active;

        #endregion

        private Vector3[] _directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        public async UniTask Init()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            _inputService = serviceLocator.Get<IInputService>();
            _eventService = serviceLocator.Get<IEventService>();
            _mainCamera = Camera.main;
            _eventService.FinishPassed += Stop;
            _towerConfig = await serviceLocator.Get<IStaticDataService>().GetData<TowerConfigurationData>();

            _active = true;
        }

        private void OnDestroy()
        {
            if (_eventService != null)
            {
                _eventService.FinishPassed -= Stop;
            }
        }

        private void Update()
        {
            if (_active == false)
                return;

            if (_towerConfig.rotationControlType == RotationControlType.Manual)
            {
                ManualRotation();
            } else if (_towerConfig.rotationControlType == RotationControlType.Auto)
            {
                AutoRotation();
            }
        }

        private void AutoRotation()
        {
            if (_inputService.GetMouseButtonDown(0))
            {
                Vector3 tapViewportCoord = _mainCamera.ScreenToViewportPoint(_inputService.MousePosition);
                if (tapViewportCoord.x > 0.5f)
                {
                    currDirIndex++;
                } else
                {
                    currDirIndex--;
                }

                if (currDirIndex < 0)
                {
                    currDirIndex = _directions.Length - 1;
                }

                if (currDirIndex > _directions.Length - 1)
                {
                    currDirIndex = 0;
                }

                _targetRotation = Quaternion.LookRotation(_directions[currDirIndex]);
            }

            bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, _targetRotation, _towerConfig.autoRotateSpeed * Time.deltaTime);
        }

        private void ManualRotation()
        {
            if (_inputService.GetMouseButtonDown(0))
            {
                _prevMousePos = _inputService.MousePosition;
            }

            if (_inputService.GetMouseButton(0))
            {
                Vector3 mousePosition = _inputService.MousePosition;
                bodyTransform.Rotate(Vector3.up, (mousePosition - _prevMousePos).x * _towerConfig.manualRotationSpeed, Space.World);
                _prevMousePos = mousePosition;
            } else
            {
                bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, _targetRotation, _towerConfig.autoRotateSpeed * Time.deltaTime);
            }

            if (_inputService.GetMouseButtonUp(0))
            {
                Vector3 currentDir = bodyTransform.forward;
                Vector3 max = _directions[0];
                float maxDot = Vector3.Dot(currentDir, max);
                for (int i = 1; i < _directions.Length; i++)
                {
                    float dot = Vector3.Dot(currentDir, _directions[i]);
                    if (dot > maxDot)
                    {
                        maxDot = dot;
                        max = _directions[i];
                    }
                }

                _targetRotation = Quaternion.LookRotation(max, Vector3.up);
            }
        }

        private void Stop()
        {
            _active = false;
        }
    }
}