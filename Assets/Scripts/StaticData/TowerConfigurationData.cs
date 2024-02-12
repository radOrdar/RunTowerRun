using Tower.Components;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "StaticData/TowerConfig", order = 0)]
    public class TowerConfigurationData : ScriptableObject
    {
        public TowerBlock towerBlockPf;
    }
}