using Services;
using Services.Audio;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class MuteButton : MonoBehaviour
{
    [SerializeField] private Sprite muteIcon;
    [SerializeField] private Sprite onIcon;
    
    private Button _btn;
    private Image _img;
    private IAudioService _audioService;

    void Start()
    {
        _audioService = ServiceLocator.Instance.Get<IAudioService>();
        
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(SwitchMute);
        
        _img = GetComponent<Image>();
        _img.sprite = _audioService.Muted ? muteIcon : onIcon;
    }

    private void SwitchMute()
    {
        _audioService.Muted = !_audioService.Muted;

        _img.sprite = _audioService.Muted ? muteIcon : onIcon;
    }
}
