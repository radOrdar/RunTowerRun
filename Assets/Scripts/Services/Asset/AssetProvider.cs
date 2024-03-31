using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Services.Asset
{
    public class AssetProvider : IAssetProvider
    {
        private bool _isReady;

        private Dictionary<string, AsyncOperationHandle<Object>> _cachedLoading = new();
        private HashSet<GameObject> _cachedInstantiated = new();

        public async UniTask<T> InstantiateAsync<T>(string assetId, Transform parent = null) =>
            await InstantiateAsync<T>(assetId, Vector3.zero, Quaternion.identity, parent);

        public async UniTask<T> InstantiateAsync<T>(string assetId, Vector3 pos, Quaternion rotation, Transform parent = null)
        {
            GameObject asset = await Addressables.InstantiateAsync(assetId, pos, rotation, parent);

            if (asset.TryGetComponent(out T component))
            {
                _cachedInstantiated.Add(asset);
            } else
            {
                Addressables.ReleaseInstance(asset);

                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            }

            return component;
        }
        
        public async UniTask<Object> LoadAssetAsync(string assetId)
        {
            if (_cachedLoading.TryGetValue(assetId, out AsyncOperationHandle<Object> handle) == false)
            {
                handle = Addressables.LoadAssetAsync<Object>(assetId);
                
                // handle = await Addressables.LoadAssetAsync<Object>(assetId);
                _cachedLoading.Add(assetId, handle);
            }
        
            return await handle;
        }

        public async UniTask<GameObject> LoadGameObjectAsync(string assetId) => 
            (GameObject)await LoadAssetAsync(assetId);

        public async UniTask<T> LoadComponentAsync<T>(string assetId) where T : Component
        {
            GameObject asset = await LoadGameObjectAsync(assetId);
            if (asset.TryGetComponent(out T component) == false)
            {
                ReleaseAsset(assetId);
                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                               "attempt to load it from addressables");
            }

            return component;
        }

        public void ReleaseAsset(string assetPath)
        {
            if (_cachedLoading.TryGetValue(assetPath, out AsyncOperationHandle<Object> handle))
            {
                _cachedLoading.Remove(assetPath);
                Addressables.Release(handle);
            }
            
            
            // string matchedKey = null;
            // foreach (string key in _cachedLoading.Keys)
            // {
            //     if (_cachedLoading[key] == assetPath)
            //     {
            //         matchedKey = key;
            //     }
            // }
            //
            // if (matchedKey != null)
            // {
            //     _cachedLoading.Remove(matchedKey);
            //     Addressables.Release(assetPath);
            // }
        }

        public void ReleaseInstance(GameObject instance)
        {
            _cachedInstantiated.Remove(instance);
            Addressables.ReleaseInstance(instance);
        }

        public void Dispose()
        {
            foreach (var asset in _cachedLoading.Values)
            {
                Addressables.Release(asset);
            }
        
            foreach (GameObject instance in _cachedInstantiated)
            {
                Addressables.ReleaseInstance(instance);
            }
        
            _cachedLoading.Clear();
            _cachedInstantiated.Clear();
        }

        
    }
}