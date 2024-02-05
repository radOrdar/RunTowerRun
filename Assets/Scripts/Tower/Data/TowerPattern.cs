using System.Collections.Generic;
using UnityEngine;

namespace Tower.Data
{
    public struct TowerPattern
    {
        public int[][,] matrix;
        public Dictionary<Vector3, int[,]> towerProjections;
    }
}
