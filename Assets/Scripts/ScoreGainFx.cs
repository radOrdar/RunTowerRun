using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

[RequireComponent(typeof(CanvasGroup))]
public class ScoreGainFx : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Vector3[] endPoints;
    [SerializeField] private EasingFunctions.EaseType moveEaseType = EasingFunctions.EaseType.OutQuint;
    [SerializeField] private EasingFunctions.EaseType alphaEaseType = EasingFunctions.EaseType.IsInSine;
    private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    public IObjectPool<ScoreGainFx> Origin;

    public void Init(Transform parent, int value)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        text.SetText($"+{value}");
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        canvasGroup.alpha = 1;
        StartCoroutine(AnimateAndRelease());
    }
    private IEnumerator AnimateAndRelease()
    {
        float time = 0;
            
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(endPoints[0], endPoints[1], EasingFunctions.Ease(moveEaseType, time/duration));
            canvasGroup.alpha = EasingFunctions.Ease(alphaEaseType, 1 - time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Origin.Release(this);
    }
}