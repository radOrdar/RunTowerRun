using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _progressFill;
        [SerializeField] private TextMeshProUGUI _loadingInfo;
        [SerializeField] private float _barSpeed;

        private float _targetProgress;

        public async UniTask Load(Queue<ILoadingOperation> loadingOperations)
        {
            _canvas.enabled = true;
            StartCoroutine(UpdateProgressBar());

            foreach (var operation in loadingOperations)
            {
                ResetFill();
                _loadingInfo.text = operation.Description;

                await operation.Load(OnProgress);
                _targetProgress = 1f;
                await WaitForBarFill();
            }
            _canvas.enabled = false;
        }

        private void ResetFill()
        {
            _progressFill.value = 0;
            _targetProgress = 0;
        }

        private async UniTask WaitForBarFill()
        {
            while (_progressFill.value < _targetProgress)
            {
                await UniTask.NextFrame();
            }

            await UniTask.Delay(150);
        }

        private IEnumerator UpdateProgressBar()
        {
            while (_canvas.enabled)
            {
                if (_progressFill.value < _targetProgress) 
                    _progressFill.value += Time.deltaTime * _barSpeed;
                yield return null;
            }
        }

        private void OnProgress(float progress)
        {
            _targetProgress = progress;
        }
    }
}
