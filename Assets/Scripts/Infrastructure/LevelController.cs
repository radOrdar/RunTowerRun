using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Obstacle;
using Services;
using Services.Ads;
using Services.Asset;
using Services.Audio;
using Services.Events;
using Services.Factory;
using Services.Save;
using Services.ScreenLoading;
using Services.StaticData;
using StaticData;
using TMPro;
using Tower;
using Tower.Components;
using Tower.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Infrastructure
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button nextLevelBtn;

        private IAudioService _audioService;
        private IEventService _eventService;
        private IAdsService _adsService;
        private IPersistentDataService _persistentData;
        private ILoadingScreenProvider _loadingScreenProvider;
        private IGameFactory _gameFactory;

        private LevelProgressionData _levelProgressionData;
        private Finish _finish;
        private IStaticDataService _staticDataService;

        private async void Awake()
        {
            var serviceLocator = ServiceLocator.Instance;
            _audioService = serviceLocator.Get<IAudioService>();
            _eventService = serviceLocator.Get<IEventService>();
            _adsService = serviceLocator.Get<IAdsService>();
            _persistentData = serviceLocator.Get<IPersistentDataService>();
            _loadingScreenProvider = serviceLocator.Get<ILoadingScreenProvider>();
            _gameFactory = serviceLocator.Get<IGameFactory>();
            _staticDataService = serviceLocator.Get<IStaticDataService>();

            if (!_persistentData.TryGetSubscriptionExpirationDate(out DateTime expirationDateTime) || expirationDateTime.CompareTo(DateTime.Now) < 0)
            {
                _adsService.Initialize();
                _adsService.ShowBanner();
            }

            _audioService.PlayMusic();

            int currentLevel = _persistentData.GetLevel();
            levelText.SetText($"Level {currentLevel + 1}");
            nextLevelBtn.onClick.AddListener(NextLevel);

            _levelProgressionData = await _staticDataService.GetData<LevelProgressionData>();
            ProgressionUnit progressionUnit = _levelProgressionData.GetProgression(currentLevel);
            TowerPattern towerPattern = new TowerGenerator().GeneratePattern(progressionUnit);
            List<int[,]> gatePatterns = Enumerable.Range(0, progressionUnit.numOfGates).Select(_ => towerPattern.towerProjections[RandomDirection()]).ToList();
            await FindAnyObjectByType<AllGates>().Init(gatePatterns, progressionUnit.distanceBtwGates);
            await FindAnyObjectByType<TowerMove>().Init(towerPattern.towerProjections, progressionUnit);
            await FindAnyObjectByType<TowerBody>().Init(towerPattern.matrix);
            await FindAnyObjectByType<TowerScore>().Init();
            await FindAnyObjectByType<TowerRotation>().Init();
            FindAnyObjectByType<TowerEffects>().Init(progressionUnit);
            FindAnyObjectByType<TowerCollision>().Init(towerPattern.matrix);
            _eventService.GateCollided += OnGateCollided;
            _eventService.GatePassed += OnGatePassed;
            _eventService.FinishPassed += OnFinishPassed;


            _finish = await _gameFactory.GetFinishAsync(new Vector3(0, 0.1f, progressionUnit.numOfGates * progressionUnit.distanceBtwGates + 40));
        }

        private void OnDestroy()
        {
            _eventService.GateCollided -= OnGateCollided;
            _eventService.GatePassed -= OnGatePassed;
            _eventService.FinishPassed -= OnFinishPassed;

            if (_finish != null)
            {
                _gameFactory.ReleaseInstance(_finish);
            }
        }

        private void NextLevel()
        {
            _loadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
        }

        private void OnGatePassed()
        {
            _audioService.PlayDing();
        }

        private void OnGateCollided()
        {
            _audioService.PlayBump();
        }

        private async void OnFinishPassed()
        {
            _audioService.PlayFinish();
            _persistentData.SaveLevel(_persistentData.GetLevel() + 1);
            await UniTask.Delay(3000);
            _adsService.ShowInterstitial();
            await UniTask.Delay(2000);
            nextLevelBtn.gameObject.SetActive(true);
        }

        private Vector3 RandomDirection()
        {
            return Random.Range(0, 4) switch
            {
                0 => Vector3.forward,
                1 => Vector3.back,
                2 => Vector3.right,
                3 => Vector3.left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}