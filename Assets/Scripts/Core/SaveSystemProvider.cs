using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

public class SaveSystemProvider
{
    public async UniTask SaveProgress()
    {
        PlayerPrefs.SetInt("Level", ProjectContext.I.UserContainer.Level);
    }
}
