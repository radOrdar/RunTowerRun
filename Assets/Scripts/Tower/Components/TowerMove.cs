using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Obstacle;
using Services;
using Services.Events;
using Services.StaticData;
using StaticData;
using UnityEngine;
using Utils;

namespace Tower.Components
{
    public class TowerMove : MonoBehaviour
    {
        [SerializeField] private Transform bodyTransform;

        #region Configuration
        
        private Dictionary<Vector3, int[,]> _towerProjections;
        private float _moveSpeed;
        private float _hasteMoveSpeed;
        private TowerConfigurationData _towerConfig;

        #endregion

        #region Dependencies

        private IEventService _eventService;
        private IStaticDataService _staticDataService;
        private AllGates _allGates;

        #endregion

        #region CurrentState

        private float _targetSpeed;
        private float _currentSpeed;
        private float _currentAcceleration;
        private bool _slowedDown;
        private bool _active;

        #endregion

        public async UniTask Init(Dictionary<Vector3, int[,]> towerProjections, ProgressionUnit progressionUnit)
        {
            _eventService = ServiceLocator.Instance.Get<IEventService>();
            _staticDataService = ServiceLocator.Instance.Get<IStaticDataService>();
            _allGates = FindAnyObjectByType<AllGates>();
            _towerConfig = await _staticDataService.GetData<TowerConfigurationData>();

            _moveSpeed = progressionUnit.normalSpeed;
            _hasteMoveSpeed = progressionUnit.hasteSpeed;
            _towerProjections = towerProjections;

            _targetSpeed = _moveSpeed;
            _currentAcceleration = _towerConfig.acceleration;
            
            _eventService.GateCollided += BounceBack;
            _eventService.FinishPassed += Stop;
            
            _active = true;
        }

        private void OnDestroy()
        {
            _eventService.GateCollided -= BounceBack;
            _eventService.FinishPassed -= Stop;
        }

        void Update()
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _currentAcceleration * Time.deltaTime);
            transform.position += Vector3.forward * (_currentSpeed * Time.deltaTime);
            
            if (_active == false)
                return;
            if (!_slowedDown)
            {
                if (_towerProjections.TryGetValue(bodyTransform.forward, out int[,] proj) &&
                    _allGates.TryGetNextGatePattern(out int[,] gatePattern) &&
                    EqualityCheck(proj, gatePattern))
                {
                    _targetSpeed = _hasteMoveSpeed;
                    _eventService.OnHasteSwitch(true);
                } else
                {
                    _targetSpeed = _moveSpeed;
                    _eventService.OnHasteSwitch(false);
                }
            }
        }

        //TODO replace with some sorts of hashcodes
        private bool EqualityCheck(int[,] m1, int[,] m2)
        {
            if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
                return false;

            int rows = m1.GetLength(0);
            int cols = m2.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (m1[i, j] != m2[i, j])
                        return false;
                }
            }

            return true;
        }

        private void BounceBack()
        {
            _currentSpeed = -_towerConfig.bounceBackSpeed;
            StartCoroutine(BounceAccelerationRoutine());
        }

        private IEnumerator BounceAccelerationRoutine()
        {
            _currentAcceleration = _towerConfig.bounceAcceleration;
            _slowedDown = true;
            _eventService.OnHasteSwitch(false);
            yield return WaitForSecondsPool.Get(_towerConfig.bounceAccelerationDelay);
            _currentAcceleration = _towerConfig.acceleration;
            _slowedDown = false;
        }

        private void Stop()
        {
            _active = false;
            _targetSpeed = 0;
            _currentAcceleration = _towerConfig.finishAcceleration;
        }
    }
}