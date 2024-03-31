using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Services.Asset
{
    public interface IAssetProvider : IService, IDisposable
    {
        UniTask<T> InstantiateAsync<T>(string assetId, Transform parent = null);
        UniTask<T> InstantiateAsync<T>(string assetId, Vector3 pos, Quaternion rotation, Transform parent = null);
        void ReleaseAsset(string assetPath);
        void ReleaseInstance(GameObject instance);
        
        UniTask<T> LoadComponentAsync<T>(string assetId) where T : Component;
        UniTask<Object> LoadAssetAsync(string assetId);
        UniTask<GameObject> LoadGameObjectAsync(string assetId);
    }
}