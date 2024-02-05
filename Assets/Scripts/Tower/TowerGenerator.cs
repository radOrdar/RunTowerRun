using System;
using System.Collections.Generic;
using System.Linq;
using StaticData;
using Tower.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class TowerGenerator
    {
        public TowerPattern GeneratePattern(ProgressionUnit progressionUnit)
        {
            int height = progressionUnit.height;
            int numLedge = progressionUnit.ledges;

            int[][,] yxz = new int[height][,];

            for (int i = 0; i < height; i++)
            {
                yxz[i] = new int[5, 5];
                yxz[i][2, 2] = 1;
            }

            List<int> levels = Enumerable.Range(0, height).ToList();

            for (int i = 0; i < numLedge; i++)
            {
                int levelIndex = Random.Range(0, levels.Count);
                int y = levels[levelIndex];
                levels.RemoveAt(levelIndex);

        
                if (Random.value < 0.5f)
                {
                    if (Random.value < 0.5f)
                    {
                        yxz[y][3, 2] = 1;
                        if (Random.value < 0.5f)
                        {
                            yxz[y][4, 2] = 1;
                        }
                    } else
                    {
                        yxz[y][1, 2] = 1;
                        if (Random.value < 0.5f)
                        {
                            yxz[y][0, 2] = 1;
                        }
                    }
                } else
                {
                    if (Random.value < 0.5f)
                    {
                        yxz[y][2, 3] = 1;
                        if (Random.value < 0.5f)
                        {
                            yxz[y][2, 4] = 1;
                        }
                    } else
                    {
                        yxz[y][2, 1] = 1;
                        if (Random.value < 0.5f)
                        {
                            yxz[y][2, 0] = 1;
                        }
                    }
                }
            }
            
            return new TowerPattern
            {
                matrix = yxz,
                towerProjections = GenerateGatesPatterns(yxz)
            };
        }

        private Dictionary<Vector3, int[,]> GenerateGatesPatterns(int[][,] towerPattern)
        {
            int patternWidth = towerPattern[0].GetLength(0);
            int patternHeight = towerPattern.Length;
            
            Dictionary<Vector3, int[,]> gatePatterns = new(new Vector3Comparer())
            {
                [Vector3.forward] = new int[patternWidth, patternHeight],
                [Vector3.back] = new int[patternWidth, patternHeight],
                [Vector3.right] = new int[patternWidth, patternHeight],
                [Vector3.left] = new int[patternWidth, patternHeight],
            };
            
            for (int level = 0; level < patternHeight; level++)
            {
                int[,] xz = towerPattern[level];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < patternWidth; j++)
                    {
                        if (xz[j,2] != 1) 
                            gatePatterns[Vector3.forward][j, level] = 1;
                        if (xz[4 - j, 2] != 1)
                            gatePatterns[Vector3.back][j, level] = 1;
                        if (xz[2,j] != 1) 
                            gatePatterns[Vector3.right][j, level] = 1;
                        if (xz[2, 4 -j] != 1) 
                            gatePatterns[Vector3.left][j, level] = 1;
                    }
                }
                
            }

            return gatePatterns;
        }
        
        class Vector3Comparer : IEqualityComparer<Vector3>{
            private const float Eps = 3.40282347E-3f;

            public bool Equals(Vector3 v1, Vector3 v2)
            {
                return Math.Abs(v1.x - v2.x) < Eps && Math.Abs(v1.y - v2.y) < Eps && Math.Abs(v1.z - v2.z) < Eps;
            }
 
            public int GetHashCode (Vector3 v){
                return Mathf.RoundToInt (v.x) ^ Mathf.RoundToInt (v.y) << 2 ^ Mathf.RoundToInt (v.z) >> 2;
            }
        }
    }

    
}
