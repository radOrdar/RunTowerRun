using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using Services.Factory;
using UnityEngine;

namespace Obstacle
{
    public class AllGates : MonoBehaviour
    {
        private IGameFactory _gameFactory;

        private List<int[,]> _gatePatterns;

        private int _currentGateIndex;

        public async UniTask Init(List<int[,]> gatePatterns, int distanceBtwObstacles)
        {
            _gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
            _gatePatterns = gatePatterns;
            await SpawnGates(gatePatterns, distanceBtwObstacles);
        }

        public bool TryGetNextGatePattern(out int[,] gatePattern)
        {
            gatePattern = null;
            if (_currentGateIndex > _gatePatterns.Count - 1)
                return false;

            gatePattern = _gatePatterns[_currentGateIndex];
            return true;
        }

        private async UniTask SpawnGates(List<int[,]> gatePatterns, int distanceBtwObstacles)
        {
            int height = gatePatterns[0].GetLength(1);
            for (int nom = 0; nom < gatePatterns.Count; nom++)
            {
                int nextGateDistance = (nom + 1) * distanceBtwObstacles;

                var gate = await GenerateGateFrames(nextGateDistance, height);
                gate.OnChecked += () => _currentGateIndex++;
                gate.transform.parent = transform;

                int[,] matrix = gatePatterns[nom];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == 1)
                        {
                            _gameFactory.GetObstacleBlockAsync(new Vector3(i, j, 0), gate.transform);
                        }
                    }
                }
            }
        }

        private async UniTask<Gate> GenerateGateFrames(float distance, int height)
        {
            Transform obstacleParent = new GameObject("Gate").transform;
            obstacleParent.position = new Vector3(-2, 0, distance);

            _gameFactory.GetObstacleFrameAsync(new Vector3(-1, 0, 0), Quaternion.identity, new Vector3(1, height, 1), obstacleParent);
            _gameFactory.GetObstacleFrameAsync(new Vector3(5, 0, 0), Quaternion.identity, new Vector3(1, height, 1), obstacleParent);
            _gameFactory.GetObstacleFrameAsync(new Vector3(-1.5f, height + .5f, 0), Quaternion.LookRotation(Vector3.forward, Vector3.right), new Vector3(1, 7, 1), obstacleParent);

            ParticleSystem ghostLeft = await _gameFactory.GetObstacleGhostAsync(
                new Vector3(-1, height / 2f - 0.5f, 0),
                Quaternion.LookRotation(Vector3.back, Vector3.left),
                obstacleParent, height);
            
            ParticleSystem ghostRight = await _gameFactory.GetObstacleGhostAsync(
                new Vector3(5, height / 2f - 0.5f, 0),
                Quaternion.LookRotation(Vector3.back, Vector3.left),
                obstacleParent, height);
            
            ParticleSystem ghostTop = await _gameFactory.GetObstacleGhostAsync(
                new Vector3(2, height + 0.5f, 0),
                Quaternion.identity,
                obstacleParent, 6);

            Gate gate = obstacleParent.gameObject.AddComponent<Gate>();
            gate.FrameFxs = new List<ParticleSystem> { ghostLeft, ghostRight, ghostTop };
            return await UniTask.FromResult(gate);
        }
    }
}