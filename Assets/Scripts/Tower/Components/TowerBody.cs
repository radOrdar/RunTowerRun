using UnityEngine;

namespace Tower.Components
{
    public class TowerBody : MonoBehaviour
    {
        [SerializeField] private TowerBlock blockPf;

        public void Init(int[][,] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matrix[i][j, k] == 1)
                        {
                            Transform towerBlock = Instantiate(blockPf).transform;
                            towerBlock.parent = transform;
                            towerBlock.localPosition = new Vector3(j - 2, i, k - 2);
                        }
                    }
                }
            }
        }
    }
}