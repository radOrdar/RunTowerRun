using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services.Asset;
using StaticData;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<Type, ScriptableObject> _cached = new();
        private Dictionary<Type, string> _paths;
        private IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;

            _paths = new Dictionary<Type, string>
            {
                [typeof(AppConfigurationData)] = Constants.StaticDataPath.APP_CONFIG,
                [typeof(TowerConfigurationData)] = Constants.StaticDataPath.TOWER_CONFIG,
                [typeof(SoundsData)] = Constants.StaticDataPath.SOUNDS,
                [typeof(LevelProgressionData)] = Constants.StaticDataPath.LEVEL_PROGRESSION
            };
        }
    

        public async UniTask<T> GetData<T>() where T : ScriptableObject
        {
            Type type = typeof(T);

            if (_cached.ContainsKey(type) == false)
                _cached[type] = await _assetProvider.LoadAssetAsync(_paths[type]) as ScriptableObject;

            return await UniTask.FromResult((T)_cached[type]);
        }
    }
}