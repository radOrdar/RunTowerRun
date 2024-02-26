using System;
using Core.Loading;
using Services;
using Services.Events;
using Services.Save;
using Services.ScreenLoading;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _newGameBtn;
        [SerializeField] private Button _removeAdsBtn;

        private IPersistentDataService _persistentDataService;
        private IEventService _eventService;
        private ILoadingScreenProvider _loadingScreenProvider;

        private void Start()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            _persistentDataService = serviceLocator.Get<IPersistentDataService>();
            _eventService = serviceLocator.Get<IEventService>();
            _loadingScreenProvider = serviceLocator.Get<ILoadingScreenProvider>();

            _continueBtn.onClick.AddListener(OnContinueBtnClicked);
            _newGameBtn.onClick.AddListener(OnNewGameBtnClicked);

            bool notSubscriber = !_persistentDataService.TryGetSubscriptionExpirationDate(out DateTime expirationDateTime) || expirationDateTime.CompareTo(DateTime.Now) < 0;
            _removeAdsBtn.gameObject.SetActive(notSubscriber);

            _eventService.AdsRemoved += OnAdsRemoved;
        }

        private void OnDestroy()
        {
            _eventService.AdsRemoved -= OnAdsRemoved;
        }

        private void OnAdsRemoved()
        {
            _removeAdsBtn.gameObject.SetActive(false);
        }

        private void OnContinueBtnClicked()
        {
            DisableButtons();

            _loadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
        }

        private void OnNewGameBtnClicked()
        {
            DisableButtons();
        
            _persistentDataService.ResetProgress();

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