using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Loading;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   [SerializeField] private Button _continueBtn;
   [SerializeField] private Button _newGameBtn;
   [SerializeField] private Button _removeAdsBtn;

   private void Start()
   {
      _continueBtn.onClick.AddListener(OnContinueBtnClicked);
      _newGameBtn.onClick.AddListener(OnNewGameBtnClicked);
      _removeAdsBtn.onClick.AddListener(OnRemoveAdsClicked);
      
      bool adsRemoved = PlayerPrefs.GetInt(Constants.PrefsKeys.REMOVE_ADS) == 1;
      _removeAdsBtn.gameObject.SetActive(!adsRemoved);

      ProjectContext.I.EventsProvider.AdsRemoved += OnAdsRemoved;
   }

   private void OnDestroy()
   {
      ProjectContext.I.EventsProvider.AdsRemoved -= OnAdsRemoved;
   }

   private void OnAdsRemoved()
   {
      _removeAdsBtn.gameObject.SetActive(false);
   }

   private void OnRemoveAdsClicked()
   {
      ProjectContext.I.IAPProvider.BuyCancelAds();
   }

   private void OnContinueBtnClicked()
   {
      DisableButtons();

      ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
   }

   private void OnNewGameBtnClicked()
   {
      DisableButtons();
      
      Queue<ILoadingOperation> operations = new();
      operations.Enqueue(new ResetProgressOperation());
      operations.Enqueue(new GameLoadingOperation());
      
      ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(operations);
   }

   private void DisableButtons()
   {
      _continueBtn.interactable = false;
      _newGameBtn.interactable = false;
      _removeAdsBtn.interactable = false;
   }
}
