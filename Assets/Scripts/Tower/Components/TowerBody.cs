using Core;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

namespace Tower.Components
{
    public class TowerBody : MonoBehaviour
    {
        private AssetProvider _assetProvider;

        private TowerBlock _blockPf;
        public async UniTask Init(int[][,] matrix)
        {
            _assetProvider = ProjectContext.I.AssetProvider;
            _blockPf = await _assetProvider.LoadAsync<TowerBlock>(Constants.Assets.TOWER_BLOCK_PF);
            
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

        private void OnDestroy()
        {
            // _assetProvider.UnloadAsset(_blockPf.gameObject);
        }
    }
}