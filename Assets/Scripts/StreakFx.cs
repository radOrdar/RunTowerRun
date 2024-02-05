using System.Collections;
using TMPro;
using UnityEngine;
using Utils;

public class StreakFx : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI streakText;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float duration;
    [SerializeField] private EasingFunctions.EaseType easeType;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void PlayFx(int streak)
    {
        streakText.SetText($"x {streak}");
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        canvasGroup.alpha = 0;
        float time = 0;
        while (time < duration / 2)
        {
            float normalizedTime = 2 * time / duration;
            canvasGroup.alpha = EasingFunctions.Ease(easeType, normalizedTime);
            yield return null;
            time += Time.deltaTime;
        }

        time = 0;
        while (time < duration)
        {
            float normalizedTime = 2 * time / duration;
            canvasGroup.alpha = EasingFunctions.Ease(easeType,1 - normalizedTime);
            yield return null;
            time += Time.deltaTime;
        }
    }
}