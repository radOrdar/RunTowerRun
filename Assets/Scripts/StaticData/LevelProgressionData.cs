using System;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelProgression", menuName = "StaticData/LevelProgression", order = 0)]
    public class LevelProgressionData : ScriptableObject
    {
        [SerializeField] private ProgressionUnit[] progressions;

        public ProgressionUnit GetProgression(int level)
        {
            return progressions[Mathf.Clamp(level, 0, progressions.Length - 1)];
        }
    }

    [Serializable]
    public struct ProgressionUnit
    {
        public int height, ledges, normalSpeed, hasteSpeed, numOfGates, distanceBtwGates;
    }
}