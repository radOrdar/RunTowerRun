using Cysharp.Threading.Tasks;
using Services;
using Services.Asset;
using UnityEngine;

namespace Tower.Components
{
    public class TowerBody : MonoBehaviour
    {
        private IAssetProvider _assetProvider;

        private TowerBlock _blockPf;
        public async UniTask Init(int[][,] matrix)
        {
            _assetProvider = ServiceLocator.Instance.Get<IAssetProvider>();
            _blockPf = await _assetProvider.LoadComponentAsync<TowerBlock>(Constants.Assets.TOWER_BLOCK_PF);
            
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matrix[i][j, k] == 1)
                        {
                            Transform towerBlock = Instantiate(_blockPf).transform;
                            towerBlock.parent = transform;
                            towerBlock.localPosition = new Vector3(j - 2, i, k - 2);
                        }
                    }
                }
            }
        }
    }
}