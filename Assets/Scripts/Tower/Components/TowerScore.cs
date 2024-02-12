using System.Collections;
using Core;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerScore : MonoBehaviour
    {
        [SerializeField] private float tickPeriod = 0.2f;
        [SerializeField] private StreakFx streakFx;
        [SerializeField] private TextMeshProUGUI scoreText;

        private EventsProvider _eventsProvider;
        private AssetProvider _assetProvider;

        private ScoreGainFx _gainFxPf;
        private IObjectPool<ScoreGainFx> _poolScoreGainFx;

        private int _streak;
        private float _lastStreakChangeTime;
        private float _lastResetStreakTime;
        
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

        private async void Start()
        {
            _eventsProvider = ProjectContext.I.EventsProvider;
            _assetProvider = ProjectContext.I.AssetProvider;

            _gainFxPf = await _assetProvider.LoadAsync<ScoreGainFx>(Constants.Assets.SCORE_GAIN_FX_PF);
            
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
            
            _eventsProvider.GatePassed += StreakIncrease;
            _eventsProvider.GateCollided += ResetStreak;
            _eventsProvider.FinishPassed += OnFinishPassed;
            _eventsProvider.HasteSwitch += SetActiveGaining;
            Streak = 1;
            StartCoroutine(ScoreTick());
        }

        private void OnDestroy()
        {
            _eventsProvider.GatePassed -= StreakIncrease;
            _eventsProvider.GateCollided -= ResetStreak;
            _eventsProvider.FinishPassed -= OnFinishPassed;
            _eventsProvider.HasteSwitch -= SetActiveGaining;
            
            // _assetProvider.UnloadAsset(_gainFxPf.gameObject);
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

                yield return WaitForSecondsPool.Get(tickPeriod);
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