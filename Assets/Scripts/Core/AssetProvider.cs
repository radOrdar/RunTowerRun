using System;
using System.Collections.Generic;
using Core.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class AssetProvider : ILoadingOperation
    {
        private bool _isReady;

        private HashSet<GameObject> _cachedObjects = new ();
        public async UniTask<T> InstantiateAsync<T>(string assetId, Transform parent = null)
        {
            GameObject asset = await Addressables.InstantiateAsync(assetId, parent);
            
            _cachedObjects.Add(asset);
            
            if (asset.TryGetComponent(out T component) == false)
            {
                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            }
            
            return component;
        }

        public void Unload(GameObject asset)
        {
            asset.SetActive(false);
            if(_cachedObjects.Remove(asset))   
                Addressables.ReleaseInstance(asset);
        }

        public string Description => "Assets Initialization...";
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.15f);
            await Addressables.InitializeAsync();
            await UniTask.Delay(500);
            _isReady = true;
        }
    }
}
