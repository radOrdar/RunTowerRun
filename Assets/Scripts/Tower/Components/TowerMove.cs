using System.Collections;
using System.Collections.Generic;
using Core;
using Infrastructure;
using Obstacle;
using StaticData;
using UnityEngine;
using Utils;

namespace Tower.Components
{
    public class TowerMove : MonoBehaviour
    {
        [SerializeField] private Transform bodyTransform;

        
        [SerializeField] private float bounceBackSpeed = 3;
        [SerializeField] private float acceleration = 30;
        [SerializeField] private float bounceAcceleration = 3;
        [SerializeField] private float finishAcceleration = 150;
        [SerializeField] private float bounceAccelerationDelay = 1.5f;
        [SerializeField] private float manualRotationSpeed = 3;
        [SerializeField] private float autoRotateSpeed = 90;

        private InputProvider _inputProvider;
        private EventsProvider eventsProvider;
        private AllGates _allGates;

        private Vector3[] _directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back, };
        private Dictionary<Vector3, int[,]> _towerProjections;
        
        private float _moveSpeed;
        private float _hasteMoveSpeed;
        
        private Vector3 _prevMousePos;
        private Quaternion _targetRotation;
        private float _targetSpeed;
        private float _currentSpeed;
        private float _currentAcceleration;
        private bool _slowedDown;
        private bool _stopped;

        public void Init(Dictionary<Vector3, int[,]> towerProjections, ProgressionUnit progressionUnit)
        {
            _moveSpeed = progressionUnit.normalSpeed;
            _hasteMoveSpeed = progressionUnit.hasteSpeed;
            _towerProjections = towerProjections;
        }
        
        private void Start()
        {
            _inputProvider = ProjectContext.I.InputProvider;
            eventsProvider = ProjectContext.I.EventsProvider;
            _allGates = FindAnyObjectByType<AllGates>();
            
            eventsProvider.GateCollided += BounceBack;
            eventsProvider.FinishPassed += Stop;
            
            _targetSpeed = _moveSpeed;
            _currentAcceleration = acceleration;
            StartCoroutine(CheckGateForm());
        }

        private void OnDestroy()
        {
            eventsProvider.GateCollided -= BounceBack;
            eventsProvider.FinishPassed -= Stop;
        }

        void Update()
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _currentAcceleration * Time.deltaTime);
            transform.position += Vector3.forward * (_currentSpeed * Time.deltaTime);

            if(_stopped)
                return;
            
            if (_inputProvider.GetMouseButtonDown(0))
            {
                _prevMousePos = _inputProvider.MousePosition;
            }

            if (_inputProvider.GetMouseButton(0))
            {
                Vector3 mousePosition = _inputProvider.MousePosition;
                bodyTransform.Rotate(Vector3.up, (mousePosition - _prevMousePos).x * manualRotationSpeed, Space.World);
                _prevMousePos = mousePosition;
            } else
            {
                bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, _targetRotation, autoRotateSpeed * Time.deltaTime);
            }

            if (_inputProvider.GetMouseButtonUp(0))
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

        private IEnumerator CheckGateForm()
        {
            while (true)
            {
                if(_stopped)
                    break;
                if (!_slowedDown)
                {
                    
                    if (_towerProjections.TryGetValue(bodyTransform.forward, out int[,] proj) &&
                        _allGates.TryGetNextGatePattern(out int[,] gatePattern) &&
                        EqualityCheck(proj, gatePattern))
                    {
                        _targetSpeed = _hasteMoveSpeed;
                        eventsProvider.OnHasteSwitch(true);
                    } else
                    {
                        _targetSpeed = _moveSpeed;
                        eventsProvider.OnHasteSwitch(false);
                    }
                }

                yield return WaitForSecondsPool.Get(0.1f);
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
            _currentSpeed = -bounceBackSpeed;
            StartCoroutine(BounceAccelerationRoutine());
        }

        private IEnumerator BounceAccelerationRoutine()
        {
            _currentAcceleration = bounceAcceleration;
            _slowedDown = true;
            eventsProvider.OnHasteSwitch(false);
            yield return WaitForSecondsPool.Get(bounceAccelerationDelay);
            _currentAcceleration = acceleration;
            _slowedDown = false;
        }

        private void Stop()
        {
            _stopped = true;
            _targetSpeed = 0;
            _currentAcceleration = finishAcceleration;
        }
    }
}