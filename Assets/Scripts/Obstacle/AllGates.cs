using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class AllGates : MonoBehaviour
    {
        [SerializeField] private ObstacleBlock obstaclePf;
        [SerializeField] private ObstacleFrame frameObstaclePf;
        [SerializeField] private ParticleSystem frameFxTop;
        [SerializeField] private ParticleSystem frameFxSide;

        private List<int[,]> _gatePatterns;

        private int _currentGateIndex;

        public void Init(List<int[,]> gatePatterns, int distanceBtwObstacles)
        {
            _gatePatterns = gatePatterns;
            SpawnGates(gatePatterns, distanceBtwObstacles);
        }

        public bool TryGetNextGatePattern(out int[,] gatePattern)
        {
            gatePattern = null;
            if (_currentGateIndex > _gatePatterns.Count - 1) 
                return false;

            gatePattern = _gatePatterns[_currentGateIndex];
            return true;
        }

        private void SpawnGates(List<int[,]> gatePatterns, int distanceBtwObstacles)
        {
            int height = gatePatterns[0].GetLength(1);
            for (int nom = 0; nom < gatePatterns.Count; nom++)
            {
                int nextGateDistance = (nom + 1) * distanceBtwObstacles;

                var gate = GenerateGateFrames(nextGateDistance, height);
                gate.OnChecked += () => _currentGateIndex++;
                gate.transform.parent = transform;

                int[,] matrix = gatePatterns[nom];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == 1)
                        {
                            Transform spawnObs = SpawnObs(gate.transform);
                            spawnObs.localPosition = new Vector3(i, j, 0);
                        }
                    }
                }
            }

            Transform SpawnObs(Transform obstacleParent)
            {
                Transform obstacleBlock = Instantiate(obstaclePf).transform;
                obstacleBlock.parent = obstacleParent;
                return obstacleBlock;
            }
        }

        private Gate GenerateGateFrames(float distance, int height)
        {
            Transform obstacleParent = new GameObject("Gate").transform;
            Gate gate = obstacleParent.gameObject.AddComponent<Gate>();
            obstacleParent.position = new Vector3(-2, 0, distance);

            var obstacleFrameLeft = Instantiate(frameObstaclePf, obstacleParent).transform;
            obstacleFrameLeft.localPosition = new Vector3(-1, 0, 0);
            obstacleFrameLeft.localScale = new Vector3(1, height, 1);

            ParticleSystem sideFxLeft = Instantiate(frameFxSide, obstacleParent);
            sideFxLeft.transform.localPosition = new Vector3(-1, height / 2f - 0.5f, 0);
            var sideFxLeftEmission = sideFxLeft.emission;
            sideFxLeftEmission.SetBurst(0, new ParticleSystem.Burst(0, (short)height, (short)height, int.MaxValue, 1));
            var sideFxShapeLeft = sideFxLeft.shape;
            sideFxShapeLeft.radius = height / 2f;

            var obstacleFrameRight = Instantiate(frameObstaclePf, obstacleParent).transform;
            obstacleFrameRight.localPosition = new Vector3(5, 0, 0);
            obstacleFrameRight.localScale = new Vector3(1, height, 1);

            ParticleSystem sideFxRight = Instantiate(frameFxSide, obstacleParent);
            sideFxRight.transform.localPosition = new Vector3(5, height / 2f - 0.5f, 0);
            var sideFxRightEmission = sideFxRight.emission;
            sideFxRightEmission.SetBurst(0, new ParticleSystem.Burst(0, (short)height, (short)height, int.MaxValue, 1));
            var sideFxShapeRight = sideFxRight.shape;
            sideFxShapeRight.radius = height / 2f;

            var obstacleFrameTop = Instantiate(frameObstaclePf, obstacleParent).transform;
            obstacleFrameTop.up = obstacleParent.TransformDirection(Vector3.right);
            obstacleFrameTop.localPosition = new Vector3(-1.5f, height + .5f, 0);
            obstacleFrameTop.localScale = new Vector3(1, 7, 1);

            ParticleSystem sideFxTop = Instantiate(frameFxTop, obstacleParent);
            sideFxTop.transform.localPosition = new Vector3(2, height + 0.5f, 0);

            gate.FrameFxs = new List<ParticleSystem> { sideFxLeft, sideFxRight, sideFxTop };

            return gate;
        }
    }
}