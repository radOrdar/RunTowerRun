using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using StaticData;
using UnityEngine;

namespace Services.Ads
{
    public class AdsService : IAdsService
    {
        private bool _active;
        private AppConfigurationData _appData;

        public AdsService(AppConfigurationData appData)
        {
            _appData = appData;
        }

        public void Initialize()
        {
            _active = true;
        
            int adTypes = AppodealAdType.Interstitial | AppodealAdType.Banner;

            Appodeal.SetTesting(_appData.isAppodealTestMode);
            Appodeal.SetAutoCache(adTypes, true);
        
            AppodealCallbacks.Sdk.OnInitialized += OnAppodealInitalized;
        
            // AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
            // AppodealCallbacks.Interstitial.OnFailedToLoad += OnInterstitialFailedToLoad;
            // AppodealCallbacks.Interstitial.OnShown += OnInterstitialShown;
        
            Appodeal.Initialize(_appData.appodealAppKey, adTypes);
        }

        public void ShowInterstitial()
        {
            if (!_active) 
                return;
        
            if (Appodeal.IsLoaded(AppodealAdType.Interstitial))
            {
                Appodeal.Show(AppodealAdType.Interstitial);
            }
        }

        public void ShowBanner()
        {
            if (!_active) 
                return;
        
            Appodeal.Show(AppodealShowStyle.BannerBottom);
        }

        public void RemoveAds()
        {
            _active = false;
            HideBanner();
        }

        public void HideBanner()
        {
            Appodeal.Hide(AppodealAdType.Banner);
        }
        
        private void OnAppodealInitalized(object sender, SdkInitializedEventArgs e)
        {
            _active = true;
            Debug.Log("Appodeal Initialized");
        }

        // private void OnInterstitialShown(object sender, EventArgs e)
        // {
        //     Debug.Log("Interstitial shown");
        // }
        //
        // private void OnInterstitialFailedToLoad(object sender, EventArgs e)
        // {
        //     Debug.Log("Interstitial failed to load");
        // }
        //
        // private void OnInterstitialLoaded(object sender, AdLoadedEventArgs e)
        // {
        //     Debug.Log("Interstitial Loaded");
        // }

       
    }
}
