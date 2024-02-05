using Core;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class MuteButton : MonoBehaviour
{
    [SerializeField] private Sprite muteIcon;
    [SerializeField] private Sprite onIcon;
    
    private Button _btn;
    private Image _img;
    private AudioProvider _audioProvider;

    void Start()
    {
        _audioProvider = ProjectContext.I.AudioProvider;
        
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(SwitchMute);
        
        _img = GetComponent<Image>();
        _img.sprite = _audioProvider.Muted ? muteIcon : onIcon;
    }

    private void SwitchMute()
    {
        _audioProvider.Muted = !_audioProvider.Muted;

        _img.sprite = _audioProvider.Muted ? muteIcon : onIcon;
    }
}
