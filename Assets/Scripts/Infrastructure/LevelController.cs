using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using Obstacle;
using StaticData;
using TMPro;
using Tower.Components;
using Tower.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Infrastructure
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelProgressionData levelProgressionData;
        [SerializeField] private Finish finishPf;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button nextLevelBtn;

        private AudioProvider _audioProvider;
        private EventsProvider _eventsProvider;
        private AdsProvider _adsProvider;
        private PersistentDataProvider _persistentData;
        private LoadingScreenProvider _loadingScreenProvider;

        private void Awake()
        {
            _audioProvider = ProjectContext.I.AudioProvider;
            _eventsProvider = ProjectContext.I.EventsProvider;
            _adsProvider = ProjectContext.I.AdsProvider;
            _persistentData = ProjectContext.I.PersistentDataProvider;
            _loadingScreenProvider = ProjectContext.I.LoadingScreenProvider;

            if (!_persistentData.TryGetSubscriptionExpirationDate(out DateTime expirationDateTime) || expirationDateTime.CompareTo(DateTime.Now) < 0)
            {
                _adsProvider.Initialize();
                _adsProvider.ShowBanner();
            }
            
            _audioProvider.PlayMusic();

            int currentLevel = ProjectContext.I.PersistentDataProvider.GetLevel();
            levelText.SetText($"Level {currentLevel + 1}");
            nextLevelBtn.onClick.AddListener(NextLevel);

            ProgressionUnit progressionUnit = levelProgressionData.GetProgression(currentLevel);
            TowerPattern towerPattern = new TowerGenerator().GeneratePattern(progressionUnit);
            FindAnyObjectByType<TowerMove>().Init(towerPattern.towerProjections, progressionUnit);
            FindAnyObjectByType<TowerBody>().Init(towerPattern.matrix);
            FindAnyObjectByType<TowerEffects>().Init(progressionUnit);
            FindAnyObjectByType<TowerCollision>().Init(towerPattern.matrix);
            _eventsProvider.GateCollided += OnGateCollided;
            _eventsProvider.GatePassed += OnGatePassed;
            _eventsProvider.FinishPassed += OnFinishPassed;

            List<int[,]> gatePatterns = Enumerable.Range(0, progressionUnit.numOfGates).Select(_ => towerPattern.towerProjections[RandomDirection()]).ToList();
            FindAnyObjectByType<AllGates>().Init(gatePatterns, progressionUnit.distanceBtwGates);

            Instantiate(finishPf, new Vector3(0, 0.1f, progressionUnit.numOfGates * progressionUnit.distanceBtwGates + 40), Quaternion.identity);
        }

        private void OnDestroy()
        {
            _eventsProvider.GateCollided -= OnGateCollided;
            _eventsProvider.GatePassed -= OnGatePassed;
            _eventsProvider.FinishPassed -= OnFinishPassed;
        }

        private void NextLevel()
        {
            _loadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
        }
        
        private void OnGatePassed()
        {
            _audioProvider.PlayDing();
        }

        private void OnGateCollided()
        {
            _audioProvider.PlayBump();
        }

        private async void OnFinishPassed()
        {
            _audioProvider.PlayFinish();
            _persistentData.SaveLevel(_persistentData.GetLevel() + 1);
            await UniTask.Delay(3000);
            _adsProvider.ShowInterstitial();
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