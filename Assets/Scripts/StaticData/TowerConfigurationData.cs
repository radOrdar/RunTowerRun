using Tower;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "StaticData/TowerConfig", order = 0)]
    public class TowerConfigurationData : ScriptableObject
    {
        [Header("Movement")]
        public float bounceBackSpeed = 3;
        public float acceleration = 15;
        public float bounceAcceleration = 4.5f;
        public float finishAcceleration = 100;
        public float bounceAccelerationDelay = 1.5f;

        [Header("Score")]
        public float scoreTickPeriod = 0.2f;

        [Header("Rotation")]
        public float manualRotationSpeed = 1f;
        public float autoRotateSpeed = 360;
        public RotationControlType rotationControlType = RotationControlType.Auto;
    }
}