using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class AssetProvider : IDisposable
    {
        private bool _isReady;

        private Dictionary<string, GameObject> _cachedLoaded = new();
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

        public async UniTask<GameObject> LoadAsync(string assetId)
        {
            if (_cachedLoaded.TryGetValue(assetId, out GameObject asset) == false)
            {
                asset = await Addressables.LoadAssetAsync<GameObject>(assetId);
                _cachedLoaded.Add(assetId, asset);
            }

            return asset;
        }

        public async UniTask<T> LoadAsync<T>(string assetId) where T : Component
        {
            bool notCached = false;
            if (_cachedLoaded.TryGetValue(assetId, out GameObject asset) == false)
            {
                asset = await Addressables.LoadAssetAsync<GameObject>(assetId);
                notCached = true;
            }

            if (asset.TryGetComponent(out T component))
            {
                if (notCached)
                {
                    _cachedLoaded[assetId] = asset;
                }
            } else
            {
                if (notCached)
                {
                    Addressables.Release(asset);
                }

                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            }

            return component;
        }

        public void UnloadAsset(GameObject asset)
        {
            string matchedKey = null;
            foreach (string key in _cachedLoaded.Keys)
            {
                if (_cachedLoaded[key] == asset)
                {
                    matchedKey = key;
                }
            }

            if (matchedKey != null)
            {
                _cachedLoaded.Remove(matchedKey);
                Addressables.Release(asset);
            }
        }

        public void UnloadInstance(GameObject instance)
        {
            _cachedInstantiated.Remove(instance);
            Addressables.ReleaseInstance(instance);
        }

        public void Dispose()
        {
            foreach (var asset in _cachedLoaded.Values)
            {
                Addressables.Release(asset);
            }

            foreach (GameObject instance in _cachedInstantiated)
            {
                Addressables.ReleaseInstance(instance);
            }

            _cachedLoaded.Clear();
            _cachedInstantiated.Clear();
        }
    }
}