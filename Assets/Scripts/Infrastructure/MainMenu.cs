using System;
using Core;
using Core.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _newGameBtn;
        [SerializeField] private Button _removeAdsBtn;

        private PersistentDataProvider _persistentData;
        private EventsProvider _eventsProvider;
        private IAPProvider _iapProvider;
        private LoadingScreenProvider _loadingScreenProvider;

        private void Start()
        {
            _persistentData = ProjectContext.I.PersistentDataProvider;
            _eventsProvider = ProjectContext.I.EventsProvider;
            _iapProvider = ProjectContext.I.IAPProvider;
            _loadingScreenProvider = ProjectContext.I.LoadingScreenProvider;

            _continueBtn.onClick.AddListener(OnContinueBtnClicked);
            _newGameBtn.onClick.AddListener(OnNewGameBtnClicked);
            _removeAdsBtn.onClick.AddListener(OnRemoveAdsClicked);

            bool notSubscriber = !_persistentData.TryGetSubscriptionExpirationDate(out DateTime expirationDateTime) || expirationDateTime.CompareTo(DateTime.Now) < 0;
            _removeAdsBtn.gameObject.SetActive(notSubscriber);

            _eventsProvider.AdsRemoved += OnAdsRemoved;
        }

        private void OnDestroy()
        {
            _eventsProvider.AdsRemoved -= OnAdsRemoved;
        }

        private void OnAdsRemoved()
        {
            _removeAdsBtn.gameObject.SetActive(false);
        }

        private void OnRemoveAdsClicked()
        {
            _iapProvider.BuyCancelAds();
        }

        private void OnContinueBtnClicked()
        {
            DisableButtons();

            _loadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
        }

        private void OnNewGameBtnClicked()
        {
            DisableButtons();
        
            _persistentData.ResetProgress();

            _loadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
        }

        private void DisableButtons()
        {
            _continueBtn.interactable = false;
            _newGameBtn.interactable = false;
            _removeAdsBtn.interactable = false;
        }
    }
}