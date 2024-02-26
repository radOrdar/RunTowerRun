using System.Collections;
using Cysharp.Threading.Tasks;
using Services;
using Services.Asset;
using Services.Events;
using Services.StaticData;
using StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerScore : MonoBehaviour
    {
        [SerializeField] private StreakFx streakFx;
        [SerializeField] private TextMeshProUGUI scoreText;

        private TowerConfigurationData _towerConfig;

        #region Dependencies

        private IEventService _eventService;
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;

        #endregion

        #region State

        private float _lastStreakChangeTime;
        private float _lastResetStreakTime;
        private int _streak;

        private int Streak
        {
            get => _streak;
            set
            {
                _streak = value;
                StreakEffect();
            }
        }

        private bool _gaining;
        private int _score;

        #endregion

        private ScoreGainFx _gainFxPf;
        private IObjectPool<ScoreGainFx> _poolScoreGainFx;

        public async UniTask Init()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            _eventService = serviceLocator.Get<IEventService>();
            _assetProvider = serviceLocator.Get<IAssetProvider>();
            _staticDataService = serviceLocator.Get<IStaticDataService>();

            _eventService.GatePassed += StreakIncrease;
            _eventService.GateCollided += ResetStreak;
            _eventService.FinishPassed += OnFinishPassed;
            _eventService.HasteSwitch += SetActiveGaining;
            Streak = 1;

            _gainFxPf = await _assetProvider.LoadComponentAsync<ScoreGainFx>(Constants.Assets.SCORE_GAIN_FX_PF);
            _poolScoreGainFx = new ObjectPool<ScoreGainFx>(
                createFunc: () =>
                {
                    var s = Instantiate(_gainFxPf, transform);
                    s.Origin = _poolScoreGainFx;
                    return s;
                },
                actionOnGet: s =>
                {
                    s.gameObject.SetActive(true);
                },
                actionOnRelease: g => g.gameObject.SetActive(false)
            );
            _towerConfig = await _staticDataService.GetData<TowerConfigurationData>();
            StartCoroutine(ScoreTick());
        }

        private void OnDestroy()
        {
            if (_eventService != null)
            {
                _eventService.GatePassed -= StreakIncrease;
                _eventService.GateCollided -= ResetStreak;
                _eventService.FinishPassed -= OnFinishPassed;
                _eventService.HasteSwitch -= SetActiveGaining;
            }
        }

        private void SetActiveGaining(bool enable)
        {
            _gaining = enable;
        }

        private void OnFinishPassed()
        {
            _gaining = false;
            _score += 300;
            scoreText.SetText(_score.ToString());
            _poolScoreGainFx.Get().SetValue(300);
        }

        private void ResetStreak()
        {
            if (Time.time - _lastResetStreakTime > 0.5f)
            {
                Streak = 0;
                _lastResetStreakTime = Time.time;
            }
        }

        private void StreakIncrease()
        {
            if (Time.time - _lastStreakChangeTime < 0.5f)
                return;
            Streak++;
            _poolScoreGainFx.Get().SetValue(_streak * 10);
            _score += _streak * 10;
            scoreText.SetText(_score.ToString());
            _lastStreakChangeTime = Time.time;
        }

        private IEnumerator ScoreTick()
        {
            while (true)
            {
                if (_gaining && _streak > 0)
                {
                    _score += Streak;
                    scoreText.SetText(_score.ToString());
                    _poolScoreGainFx.Get().SetValue(_streak);
                }

                yield return WaitForSecondsPool.Get(_towerConfig.scoreTickPeriod);
            }
        }

        private void StreakEffect()
        {
            if (_streak > 0)
            {
                streakFx.PlayFx(_streak);
            }
        }
    }
}