using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button OkBtn;
    
    private Canvas _canvas;
    private UniTaskCompletionSource _completionSource;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        OkBtn.onClick.AddListener(OnOkBtnClicked);
    }

    public async UniTask AwaitForCompletion(string info)
    {
        _canvas.enabled = true;
        text.SetText(info);
        _completionSource = new UniTaskCompletionSource();
        await _completionSource.Task;
        _canvas.enabled = false;
    }
    
    private void OnOkBtnClicked()
    {
        _completionSource.TrySetResult();
    }

}