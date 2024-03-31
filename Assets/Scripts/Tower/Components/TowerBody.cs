using Cysharp.Threading.Tasks;
using Services;
using Services.Asset;
using Services.Factory;
using UnityEngine;

namespace Tower.Components
{
    public class TowerBody : MonoBehaviour
    {
        private IGameFactory _gameFactory;

        private TowerBlock _blockPf;
        public async UniTask Init(int[][,] matrix)
        {
            _gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
            // _blockPf = await _gameFactory.LoadComponentAsync<TowerBlock>(Constants.Assets.TOWER_BLOCK_PF);
            
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matrix[i][j, k] == 1)
                        {
                            _gameFactory.GetTowerBlockAsync(new Vector3(j - 2, i, k - 2), transform);
                            // Transform towerBlock = Instantiate(_blockPf).transform;
                            // towerBlock.parent = transform;
                            // towerBlock.localPosition = new Vector3(j - 2, i, k - 2);
                        }
                    }
                }
            }
        }
    }
}